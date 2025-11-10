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
        // Fields
        private readonly CompanyRepository _companyRepo = new();
        private readonly ProjectRepository _projectRepo = new();

        private Company? _selectedCompany;
        private Project? _selectedProject;
        private string? _title;
        private string? _description;
        private ProjectStatus _projectStatus = ProjectStatus.Created;

        // Properties
        public ObservableCollection<Company> Companies { get; } = new();
        public ObservableCollection<Project> Projects { get; } = new();
        public ProjectStatus[] StatusOptions { get; } =
            new[] { ProjectStatus.Created, ProjectStatus.InProgress, ProjectStatus.Done };

        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (SetProperty(ref _selectedCompany, value))
                {
                    SelectedProject = null;

                    Projects.Clear();
                    if (value != null)
                    {
                        foreach (var p in _projectRepo.GetProjectsByCompanyId(value.CompanyId))
                            Projects.Add(p);
                    }
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
        public ProjectStatus ProjectStatus
        {
            get => _projectStatus;
            set => SetProperty(ref _projectStatus, value);
        }

        // Properties - commands
        public RelayCommand CreateProjectCommand { get; }
        public RelayCommand DeleteProjectCommand { get; }

        // Mangler en editprojectcommand

        // Constructor
        public ProjectPageViewModel()
        {
            CreateProjectCommand = new RelayCommand(_ => CreateProject());
            // Mangler en editprojectcommand
            DeleteProjectCommand = new RelayCommand(_ => DeleteProject());

            foreach (var c in _companyRepo.GetAllCompanies())
                Companies.Add(c);
        }
        // Metoder

        private void CreateProject()
        {
            if (SelectedCompany is null)
            {
                MessageBox.Show("Vælg en virksomhed først.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Skriv en projekttitel.");
                return;
            }

            var p = new Project(SelectedCompany.CompanyId, Title!, Description, ProjectStatus);
            var newId = _projectRepo.SaveNewProject(p);

            p.ProjectId = newId;

            Projects.Add(p);

            Title = string.Empty;
            Description = string.Empty;
            ProjectStatus = ProjectStatus.Created;
        }
        private void DeleteProject()
        {
            if (SelectedProject == null) return;
            _projectRepo.DeleteProject(SelectedProject.ProjectId);

            Projects.Remove(SelectedProject);
            SelectedProject = null;
        }
    }
}
