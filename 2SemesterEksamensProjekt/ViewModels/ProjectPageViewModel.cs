using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class ProjectPageViewModel : BaseViewModel
    {
        // --Fields--
        private readonly IProjectRepository _projectRepo;
        private readonly ICompanyRepository _companyRepo;

        private Company? _selectedCompany;
        private Project? _selectedProject;
        private string? _title;
        private string? _description;

        // --Properties--
        public ObservableCollection<Company> Companies { get; } = new();
        public ObservableCollection<Project> Projects { get; } = new();

        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (SetProperty(ref _selectedCompany, value))
                {
                    LoadProjectsForSelectedCompany();
                }
            }
        }
        public Project? SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        // Command Properties 
        public RelayCommand CreateProjectCommand { get; }
        public RelayCommand DeleteSelectedProjectCommand { get; }
        public RelayCommand EditSelectedProjectCommand { get; } 

        public RelayCommand SaveSelectedProjectCommand { get; } 

        // --Constructor--
        public ProjectPageViewModel(ICompanyRepository companyRepo, IProjectRepository projectRepo)
        {
            _companyRepo = companyRepo;   
            _projectRepo = projectRepo;   

            CreateProjectCommand = new RelayCommand(_ => CreateProject());
            EditSelectedProjectCommand = new RelayCommand(_ => EditSelectedProject());
            DeleteSelectedProjectCommand = new RelayCommand(_ => DeleteSelectedProject());
            SaveSelectedProjectCommand = new RelayCommand(_ => SaveSelectedProject());

            foreach (var c in _companyRepo.GetAllCompanies())
                Companies.Add(c);
        }
        
        // --Metoder--
        public void CreateProject()
        {
            if (SelectedCompany is null)
            {
                ShowMessage("Vælg en virksomhed først.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Title))
            {
                ShowMessage("Skriv en projekttitel.");
                return;
            }

            var p = new Project(SelectedCompany.CompanyId, Title!, Description);
            var newId = _projectRepo.SaveNewProject(p);

            p.ProjectId = newId;

            Projects.Add(p);

            Title = string.Empty;
            Description = string.Empty;
        }

        public void DeleteSelectedProject()
        {
            if (SelectedProject == null) return;
            _projectRepo.DeleteProject(SelectedProject.ProjectId);

            Projects.Remove(SelectedProject);
            SelectedProject = null;
        }
        public void EditSelectedProject()
        {
            if (SelectedProject == null)
            {
                ShowMessage("Vælg et projekt, der skal redigeres.");
                return;
            }

            SelectedCompany = Companies.FirstOrDefault(c => c.CompanyId == SelectedProject.CompanyId);
            Title = SelectedProject.Title;
            Description = SelectedProject.Description;

        }
        public void SaveSelectedProject()
        {
            if (SelectedProject == null)
            {
                ShowMessage("Vælg et projekt, der skal gemmes.");
                return;
            }

            if (SelectedCompany == null)
            {
                ShowMessage("Vælg en virksomhed.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Title))
            {
                ShowMessage("Skriv en projekttitel.");
                return;
            }

            SelectedProject.Title = Title!;
            SelectedProject.Description = Description;

            var keepId = SelectedProject.ProjectId;   
            _projectRepo.UpdateProject(SelectedProject);

            LoadProjectsForSelectedCompany();         
            SelectedProject = Projects.FirstOrDefault(p => p.ProjectId == keepId);

            Title = string.Empty;
            Description = string.Empty;
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
