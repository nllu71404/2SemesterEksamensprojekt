using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.Services;
using _2SemesterEksamensProjekt.Views.Pages;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class TimeRecordViewModel : BaseViewModel
    {
        //Underliggende model
        private readonly TimeRecord _timeRecord;

        //Fields
        private readonly TimeRecordRepository _timeRecordRepo;
        private readonly CompanyRepository _companyRepo;
        private readonly ProjectRepository _projectRepo;
        private readonly TopicRepository _topicRepo;

        //Tager den gemte tid med over i ny Page
        //private TimerPageViewModel _timerPageViewModel;
        

        //Properties
        public ObservableCollection<Company> Companies { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Topic> Topics { get; set; }

        private Company? _selectedCompany;
        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (SetProperty(ref _selectedCompany, value))
                {
                    LoadProjectsForSelectedCompany();
                    SelectedProject = null;
                }
            }
        }

        public Project? _selectedproject;
        public Project? SelectedProject
        {
            get => _selectedproject;
            set => SetProperty(ref _selectedproject, value);
        }

        private Topic? _selectedTopic;
        public Topic? SelectedTopic
        {
            get => _selectedTopic;
            set => SetProperty(ref _selectedTopic, value);
        }

        public string TimerName => _timeRecord.TimerName;
        public string ElapsedTimeDisplay => _timeRecord.DisplayTime;

        //Commands
        public RelayCommand SaveTimeRecordCommand { get; }
        public RelayCommand CancelTimeRecordCommand { get; }

        //Constructor
        public TimeRecordViewModel(TimeRecord timeRecord, string timerName, TimeSpan elapsedTime)
        {

            _timeRecord = timeRecord ?? throw new ArgumentNullException(nameof(timeRecord));

            // Når modellen ændrer sig -> opdater viewmodel
            _timeRecord.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(elapsedTime))
                    OnPropertyChanged(nameof(ElapsedTimeDisplay));

                if (e.PropertyName == nameof(timerName))
                    OnPropertyChanged(nameof(TimerName));
            };

            _timeRecordRepo = new TimeRecordRepository();
            _companyRepo = new CompanyRepository();
            _projectRepo = new ProjectRepository();
            _topicRepo = new TopicRepository();

            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();

            //LoadCompanies();
            //LoadAllTopics();

            SaveTimeRecordCommand = new RelayCommand(_ => SaveTimeRecord());
            CancelTimeRecordCommand = new RelayCommand(_ => CancelTimeRecord());
        }

        //Metoder
        private void LoadCompanies()
        {
            Companies.Clear();

            foreach (var company in _companyRepo.GetAllCompanies()
                                        .Where(c => c.ProjectId != null))
            {
                Companies.Add(company);
            }
        }

        private void LoadProjectsForSelectedCompany()
        {
            if (SelectedCompany == null)
                return;

            var projekter = _projectRepo.GetProjectsByCompanyId(SelectedCompany.CompanyId)
                            ?? new List<Project>();

            Projects.Clear();

            foreach (var p in projekter)
                Projects.Add(p);
        }

        private void LoadAllTopics()
        {
            Topics.Clear();

            foreach (var topic in _topicRepo.GetAllTopics())
            {
                Topics.Add(topic);
            }
        }

        private void SaveTimeRecord()
        {
            if (SelectedCompany == null)
            {
                MessageBox.Show("Vælg en virksomhed", "Mangler information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedProject == null)
            {
                MessageBox.Show("Vælg et projekt", "Mangler information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedTopic == null)
            {
                MessageBox.Show("Vælg et emne", "Mangler information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _timeRecord.CompanyId = SelectedCompany.CompanyId;
            _timeRecord.ProjectId = SelectedProject.ProjectId;
            _timeRecord.TopicId = SelectedTopic.TopicId;

            try
            {
                int newId = _timeRecordRepo.SaveNewTimeRecord(_timeRecord);
                _timeRecord.TimerId = newId;

                MessageBox.Show("Tidsregistrering gemt!", "Succes",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                AppNavigationService.Navigate(new MainMenuPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning: {ex.Message}", "Fejl",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelTimeRecord()
        {
            var result = MessageBox.Show(
                "Er du sikker på, at du vil annullere? Tidsregistreringen gemmes ikke.",
                "Bekræft annullering",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                AppNavigationService.Navigate(new TimerPage());
            }
        }
    }
}

