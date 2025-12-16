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
        
        //--Fields--
        private readonly TimeRecord _timeRecord;
        private readonly ITimeRecordRepository _timeRecordRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly ITopicRepository _topicRepo;
        private readonly ObservableCollection<TimeRecord> _timers;
        private Company? _selectedCompany;
        private Project? _selectedproject;
        private Topic? _selectedTopic;
        private string? _note;


        //--Properties--

        //Auto-properties
        public ObservableCollection<Company> Companies { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Topic> Topics { get; set; }
      

        
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

        
        public Project? SelectedProject
        {
            get => _selectedproject;
            set => SetProperty(ref _selectedproject, value);
        }

       
        public Topic? SelectedTopic
        {
            get => _selectedTopic;
            set => SetProperty(ref _selectedTopic, value);
        }

        public string TimerName => _timeRecord.TimerName;
        public string ElapsedTimeDisplay => _timeRecord.DisplayTime;

        
        
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

        //--Command properties--
        public RelayCommand SaveTimeRecordCommand { get; }
      

        //--Constructor--
        public TimeRecordViewModel(TimeRecord timeRecord, ITimeRecordRepository timeRecordRepo, ObservableCollection<TimeRecord> timers,
            ICompanyRepository companyRepo, IProjectRepository projectRepo, ITopicRepository topicRepo)
        {

            _timeRecord = timeRecord ?? throw new ArgumentNullException(nameof(timeRecord));

            // Når modellen ændrer sig -> opdater viewmodel
            _timeRecord.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TimeRecord.ElapsedTime))
                    OnPropertyChanged(nameof(ElapsedTimeDisplay));

                if (e.PropertyName == nameof(TimeRecord.TimerName))
                    OnPropertyChanged(nameof(TimerName));
               
            };

            _timeRecordRepo = timeRecordRepo;
            _timers = timers;
            _companyRepo = companyRepo;
            _projectRepo = projectRepo;
            _topicRepo = topicRepo;

            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();

            LoadCompanies();
            LoadAllTopics();

            SaveTimeRecordCommand = new RelayCommand(_ => SaveTimeRecord());
           
        }

        //--Metoder--
        public void LoadCompanies()
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
        

        public void LoadProjectsForSelectedCompany()
        {
            if (SelectedCompany == null)
                return;

            var projekter = _projectRepo.GetProjectsByCompanyId(SelectedCompany.CompanyId)
                            ?? new List<Project>();

            Projects.Clear();

            foreach (var p in projekter)
                Projects.Add(p);
        }

        public void LoadAllTopics()
        {
            Topics.Clear();

            foreach (var topic in _topicRepo.GetAllTopics())
            {
                Topics.Add(topic);
            }
        }



        public void SaveTimeRecord()
        {
            // 1. Valider input
            if (SelectedCompany == null || SelectedProject == null || SelectedTopic == null)
            {
                ShowMessage("Udfyld venligst Virksomhed, Projekt og Emne");
                return;
            }

            // 2. Bekræft gemning
            var result = ShowConfirmation("Vil du gemme denne tidsregistrering?");
            if (result != MessageBoxResult.Yes)
                return;

            // 3. Sæt værdier
            _timeRecord.CompanyId = SelectedCompany.CompanyId;
            _timeRecord.ProjectId = SelectedProject.ProjectId;
            _timeRecord.TopicId = SelectedTopic.TopicId;
            _timeRecord.Note = this.Note;

            // 4. Gem og håndter fejl
            try
            {
                int newId = _timeRecordRepo.SaveNewTimeRecord(_timeRecord);

                // Fjern den tilhørende timer fra ObservableCollection
                var timerToRemove = _timers.FirstOrDefault(t => t.TimerId == _timeRecord.TimerId);
                if (timerToRemove != null)
                    _timers.Remove(timerToRemove);

                ShowMessage("Tidsregistrering gemt!");
                AppNavigationService.GoBack();
            }
            catch (Exception ex)
            {
                ShowMessage($"Fejl ved gemning: {ex.Message}");
            }

            
        }
        protected virtual void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        protected virtual MessageBoxResult ShowConfirmation(string message)
        {
            return MessageBox.Show(
                message,
                "Bekræft sletning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
        }
    }
}

