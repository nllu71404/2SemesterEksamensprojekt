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

        private Project? _selectedproject;
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

        
        private string? _note;
        public string? Note
        {
            get => _note;
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        //Commands
        public RelayCommand SaveTimeRecordCommand { get; }
        public RelayCommand CancelTimeRecordCommand { get; }

        //Constructor
        public TimeRecordViewModel(TimeRecord timeRecord)
        {

            _timeRecord = timeRecord ?? throw new ArgumentNullException(nameof(timeRecord));

            // Når modellen ændrer sig -> opdater viewmodel
            _timeRecord.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TimeRecord.ElapsedTime))
                    OnPropertyChanged(nameof(ElapsedTimeDisplay));

                if (e.PropertyName == nameof(TimeRecord.TimerName))
                    OnPropertyChanged(nameof(TimerName));
                if (e.PropertyName == nameof(TimeRecord.Note))
                    OnPropertyChanged(nameof(Note));    
            };

            _timeRecordRepo = new TimeRecordRepository();
            _companyRepo = new CompanyRepository();
            _projectRepo = new ProjectRepository();
            _topicRepo = new TopicRepository();

            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();

            LoadCompanies();
            LoadAllTopics();

            SaveTimeRecordCommand = new RelayCommand(_ => SaveTimeRecord());
            CancelTimeRecordCommand = new RelayCommand(_ => CancelTimeRecord());
        }

        //Metoder
        private void LoadCompanies()
        {
            
            Companies.Clear();

            var allCompanies = _companyRepo.GetAllCompanies() ?? new List<Company>();
            var allProjects = _projectRepo.GetAllProjects() ?? new List<Project>();

            // Kun de virksomheder, der har mindst ét projekt
            var companiesWithProjects = allCompanies
                .Where(c => allProjects.Any(p => p.CompanyId == c.CompanyId));

            foreach (var company in companiesWithProjects)
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
            if (SelectedCompany == null || SelectedProject == null || SelectedTopic == null)
            {
                MessageBox.Show("Udfyld venligst alle felter", "Mangler information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _timeRecord.CompanyId = SelectedCompany.CompanyId;
            _timeRecord.ProjectId = SelectedProject.ProjectId;
            _timeRecord.TopicId = SelectedTopic.TopicId;
            _timeRecord.Note = this.Note;

            try
            {
                int newId = _timeRecordRepo.SaveNewTimeRecord(_timeRecord);
                

                MessageBox.Show("Tidsregistrering gemt!", "Succes",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                AppNavigationService.GoBack();
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

