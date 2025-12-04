using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class OverViewPageViewModel : BaseViewModel
    {
        //Fields
        private readonly ITimeRecordRepository _timeRecordRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly ITopicRepository _topicRepo;

        //Observablecollection
        public ObservableCollection<TimeRecord> TimeRecords {get; set;}
        public ObservableCollection<Company> Companies { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Topic> Topics { get; set; }
        public ObservableCollection<string> Month { get; set; }
        public ObservableCollection<int> Year { get; set; }

        //Properties til søgekriterier
        private string? _selectedMonth;
        public string? SelectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }
        private int? _selectedYear;
        public int? SelectedYear
        {
            get => _selectedYear;
            set => SetProperty(ref _selectedYear, value);
        }
        private string? _selectedCompany;
        public string? SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }
        private string? _selectedProject;
        public string? SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }
        private string? _selectedTopic;
        public string? SelectedTopic
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }


        //Commands
        public RelayCommand ApplyFilterCommand { get; }

        //Constructor
        public OverViewPageViewModel(TimeRecord timeRecord, ITimeRecordRepository timeRecordRepository, 
            ICompanyRepository companyRepository, IProjectRepository projectRepository, ITopicRepository topicRepository)
        {
            this._timeRecordRepo = timeRecordRepository;
            foreach (var timeRecords in timeRecordRepository.GetAllTimeRecords())
            {
                TimeRecords.Add(timeRecord);
            }

            _timeRecordRepo = timeRecordRepository;
            _companyRepo = companyRepository;
            _projectRepo = projectRepository;
            _topicRepo = topicRepository;


            TimeRecords = new ObservableCollection<TimeRecord>();
            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();

            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
        }

        //Methode

        private void ApplyFilter()
        {

        }



    }
}
