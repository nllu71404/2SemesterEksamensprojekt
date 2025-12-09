using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
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
        public ObservableCollection<string> Months { get; }
        public ObservableCollection<int> Years { get; set; }

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
        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }
        private Topic? _selectedTopic;
        public Topic? SelectedTopic
        {
            get => _selectedTopic;
            set => SetProperty(ref _selectedTopic, value);
        }


        //Commands
        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand CsvCommand { get; }

        //Constructor
        public OverViewPageViewModel(ITimeRecordRepository timeRecordRepository, 
            ICompanyRepository companyRepository, IProjectRepository projectRepository, ITopicRepository topicRepository)
        {
            //Initialisere collection
            TimeRecords = new ObservableCollection<TimeRecord>();
            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();
            Months = new ObservableCollection<string>();
            Years = new ObservableCollection<int>();

            _timeRecordRepo = timeRecordRepository;
            _companyRepo = companyRepository;
            _projectRepo = projectRepository;
            _topicRepo = topicRepository;

            // Tilføjer metode til at loade all data
            LoadAllTimeRecords();
            LoadCompanies();
            LoadProjectsForSelectedCompany();
            LoadAllTopics();
            LoadMonths();
            LoadYears();

            //Commands
            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            CsvCommand = new RelayCommand(_ => ConvertToCSV());
        }

        //Methode
        private void LoadAllTimeRecords()
        {
            foreach (var timeRecord in _timeRecordRepo.GetAllTimeRecords())
            {
                TimeRecords.Add(timeRecord);
            }
        }
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
        private void LoadMonths()
        {
            Months.Clear();

            //Default så vi kan hente danske måneder
            var danishCulture = new CultureInfo("da-DK");
            //Liste fra databasen databasen
            var allTimeRecords = _timeRecordRepo.GetAllTimeRecords() ?? new List<TimeRecord>();

            //Henter kun måneder med data tilknyttet
            var allMonths = allTimeRecords
                .Select(tr => tr.StartTime.Month)
                .Distinct()
                .OrderBy(month => month); //sorterer i årlig rækkefølge

            foreach (var monthNumber in allMonths)
            {
                string monthName = danishCulture.DateTimeFormat.GetMonthName(monthNumber);
                Months.Add(monthName);
            }
        }
        private void LoadYears()
        {
            Years.Clear();

            var allTimeRecords = _timeRecordRepo.GetAllTimeRecords() ?? new List<TimeRecord>();

            var allYears = allTimeRecords
                .Select(tr => tr.StartTime.Year)
                .Distinct()
                .OrderByDescending(Year => Year);

            foreach (var year in allYears)
            {
                Years.Add(year);
            }

        }

        



        private void ApplyFilter()
        {
            TimeRecords.Clear();

            //Hent objekter via ID

        }

        private void ConvertToCSV()
        {
            
        }

        //Hjælpe metoder
        private int? GetMonthNumber(string? monthName)
        {
            if (string.IsNullOrEmpty(monthName))
                return null;

            var danishMonths = new Dictionary<string, int>
    {
        {"januar", 1},
        {"februar", 2},
        {"marts", 3},
        {"april", 4},
        {"maj", 5},
        {"juni", 6},
        {"juli", 7},
        {"august", 8},
        {"september", 9},
        {"oktober", 10},
        {"november", 11},
        {"december", 12}
    };

            // Case-insensitive lookup
            string lowerMonth = monthName.ToLower();

            return danishMonths.ContainsKey(lowerMonth)
                ? danishMonths[lowerMonth]
                : null;
        }




    }
}
