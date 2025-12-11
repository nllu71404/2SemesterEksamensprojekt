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
        private readonly ICsvExportService _csvExportService;


        //Observablecollection
        public ObservableCollection<TimeRecord> TimeRecords { get; set; }
        public ObservableCollection<Company> Companies { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Topic> Topics { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<int> Years { get; set; }
        public ObservableCollection<TimeRecord> FilteredTimeRecords { get; set; }

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
        private TimeRecord? _selectedTimeRecord;
        public TimeRecord? SelectedTimeRecord
        {
            get => _selectedTimeRecord;
            set => SetProperty(ref _selectedTimeRecord, value);
        }


        //Commands
        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand CsvCommand { get; }
        public RelayCommand ClearFilterCommand { get; }

        //Constructor
        public OverViewPageViewModel(ITimeRecordRepository timeRecordRepo,
            ICompanyRepository companyRepo, IProjectRepository projectRepo, ITopicRepository topicRepo, ICsvExportService csvExportService)
        {
            
            
            _timeRecordRepo = timeRecordRepo;
            _companyRepo = companyRepo;
            _projectRepo = projectRepo;
            _topicRepo = topicRepo;
            _csvExportService = csvExportService;

            //Initialisere collection
            TimeRecords = new ObservableCollection<TimeRecord>();
            FilteredTimeRecords = new ObservableCollection<TimeRecord>();
            Companies = new ObservableCollection<Company>();
            Projects = new ObservableCollection<Project>();
            Topics = new ObservableCollection<Topic>();
            Months = new ObservableCollection<string>();
            Years = new ObservableCollection<int>();


            // Tilføjer metode til at loade all data
            LoadAllTimeRecords();
            Console.WriteLine($"TimeRecords count: {TimeRecords.Count}");
            Console.WriteLine($"FilteredTimeRecords count: {FilteredTimeRecords.Count}");

            LoadCompanies();
            LoadProjectsForSelectedCompany();
            LoadAllTopics();
            LoadMonths();
            Console.WriteLine($"Months count: {Months.Count}");
            foreach (var month in Months)
                Console.WriteLine($"  - {month}");

            LoadYears();
            Console.WriteLine($"Years count: {Years.Count}");
            CurrentMonthAndYear();


            //Commands
            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            CsvCommand = new RelayCommand(_ => ExportToCSV());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
        }

        //Methode
        private void LoadAllTimeRecords()
        {
            TimeRecords.Clear();

            var allTimeRecords = _timeRecordRepo.GetAllTimeRecords() ?? new List<TimeRecord>();

            var alltheTimeRecords = allTimeRecords
                .OrderBy(tr => tr.StartTime);

            foreach (var timeRecord in alltheTimeRecords)
            {
                TimeRecords.Add(timeRecord);
            }

            FilteredTimeRecords.Clear();

            if (TimeRecords.Any())
            {
                // Find den seneste måned i data
                var latestDate = TimeRecords.Max(tr => tr.StartTime);
                var latestMonth = latestDate.Month;
                var latestYear = latestDate.Year;

                // Filtrer records fra seneste måned og sorter efter dato (stigende)
                var latestMonthRecords = TimeRecords
                    .Where(tr => tr.StartTime.Month == latestMonth && tr.StartTime.Year == latestYear)
                    .OrderBy(tr => tr.StartTime)  // Første dag øverst
                    .ToList();

                foreach (var record in latestMonthRecords)
                {
                    FilteredTimeRecords.Add(record);
                }
            }

                //Vise alt på listen til at starte med
                //FilteredTimeRecords.Clear();
                //foreach (var timerecord in TimeRecords)
                //{
                //    FilteredTimeRecords.Add(timerecord);
                //}
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
            Projects.Clear();
            
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
            var allTopics = _topicRepo.GetAllTopics() ?? new List<Topic>();
            foreach (var topic in _topicRepo.GetAllTopics())
            {
                Topics.Add(topic);
            }
        }
        private void LoadMonths()
        {
            if(TimeRecords == null ||  TimeRecords.Count == 0) return;
            
            //Default så vi kan hente danske måneder
            var danishCulture = new CultureInfo("da-DK");

            //Liste fra databasen databasen
            //var allTimeRecords = _timeRecordRepo.GetAllTimeRecords() ?? new List<TimeRecord>();

            //Henter kun måneder med data tilknyttet
            var allMonths = TimeRecords
                .Select(tr => tr.StartTime.Month)
                .Distinct()
                .OrderByDescending(month => month); //sorterer i årlig rækkefølge

            foreach (var monthNumber in allMonths)
            {
                string monthName = danishCulture.DateTimeFormat.GetMonthName(monthNumber);
                //Gør første bogstav stort
                monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);
                Months.Add(monthName);
            }
        }
        private void LoadYears()
        {
            if (TimeRecords == null || TimeRecords.Count == 0)
                return;

            //var allTimeRecords = _timeRecordRepo.GetAllTimeRecords() ?? new List<TimeRecord>();

            var allYears = TimeRecords
                .Select(tr => tr.StartTime.Year)
                .Distinct()
                .OrderByDescending(Year => Year);

            foreach (var year in allYears)
            {
                Years.Add(year);
            }
        }

        private void CurrentMonthAndYear()
        {
            var now = DateTime.Now;

            var danishCulture = new CultureInfo("da-DK");

            string currentMonthName = danishCulture.DateTimeFormat.GetMonthName(now.Month);
            currentMonthName = char.ToUpper(currentMonthName[0]) + currentMonthName.Substring(1);

            if (Months.Contains(currentMonthName))
            {
                SelectedMonth = currentMonthName;
            }
            else if (Months.Any())
            {
                SelectedMonth = Months.First();
            }

            // Sæt nuværende år
            if (Years.Contains(now.Year))
            {
                SelectedYear = now.Year;
            }
            else if (Years.Any())
            {
                SelectedYear = Years.First();
            }

        }
        private void ApplyFilter()
        {
            //Hent objekter via ID
            int? companyId = SelectedCompany?.CompanyId;
            int? projectId = SelectedProject?.ProjectId;
            int? topicId = SelectedTopic?.TopicId;
            int? month = GetMonthNumber(SelectedMonth);
            int? year = SelectedYear;

            // Kald repo for at kalde database
            var filteredRecords = _timeRecordRepo.GetTimeRecordByFilter(
                companyId,
                projectId,
                topicId,
                month,
                year);

            FilteredTimeRecords.Clear();
            //Opdater Observablecollection
            if(filteredRecords != null)
            {
                foreach (var records in filteredRecords)
                {
                    FilteredTimeRecords.Add(records);
                }
            }
            //OnPropertyChanged(nameof(TotalHours));
        }

        public void ClearFilter()
        {
            LoadAllTimeRecords();
            LoadCompanies();
            LoadProjectsForSelectedCompany();
            LoadAllTopics();
            CurrentMonthAndYear();
        }

        public void ExportToCSV()
        {

            if (ShowDialog(out string fileName) == true)
            {
                
                _csvExportService.ExportTimeRecords(TimeRecords, fileName, "TimerName", "CompanyId", "ProjectId", "TopicId",
    "ElapsedTime",
    "StartTime",
    "Note");
            }
        }

        //Hjælpe metoder
        private int? GetMonthNumber(string? monthName)
        {
            if (string.IsNullOrEmpty(monthName))
                return null;

            var danishMonths = new Dictionary<string, int>
            {
                {"Januar", 1},
                {"Februar", 2},
                {"Marts", 3},
                {"April", 4},
                {"Maj", 5},
                {"Juni", 6},
                {"Juli", 7},
                {"August", 8},
                {"September", 9},
                {"Oktober", 10},
                {"November", 11},
                {"December", 12}
            };

            return danishMonths.ContainsKey(monthName)
                ? danishMonths[monthName]
                : null;
        }

        public bool? ShowDialog(out string fileName) //Til ExportToCsv metoden, så den kan unit testes 
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = "TimeRecords.csv"
            };

            var result = dialog.ShowDialog();
            fileName = dialog.FileName;
            return result;
        }



    }
}
