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

        // Properties
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
            set
            {
                if (SetProperty(ref _selectedProject, value))
                {
                    if (_selectedProject != null)
                    {
                        //Combobox viser virksomhedsnavnet
                        SelectedCompany = Companies.FirstOrDefault(c => c.CompanyId == _selectedProject.CompanyId);
                        // Sæt Title og Description
                        Title = _selectedProject.Title;
                        Description = _selectedProject.Description;
                    }
                    else
                    {
                        // Hvis intet projekt er valgt, nulstil felter
                        SelectedCompany = null;
                        Title = string.Empty;
                        Description = string.Empty;
                    }
                }

            }
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

        // Properties - commands
        public RelayCommand CreateProjectCommand { get; }
        public RelayCommand DeleteSelectedProjectCommand { get; }
        public RelayCommand EditSelectedProjectCommand { get; } // klargør redigering af valgt projekt

        public RelayCommand SaveSelectedProjectCommand { get; } // gemmer ændringer til valgt projekt

        // Constructor
        public ProjectPageViewModel()
        {
            CreateProjectCommand = new RelayCommand(_ => CreateProject());
            EditSelectedProjectCommand = new RelayCommand(_ => EditSelectedProject());
            DeleteSelectedProjectCommand = new RelayCommand(_ => DeleteSelectedProject());
            SaveSelectedProjectCommand = new RelayCommand(_ => SaveSelectedProject());

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

            var p = new Project(SelectedCompany.CompanyId, Title!, Description);
            var newId = _projectRepo.SaveNewProject(p);

            p.ProjectId = newId;

            Projects.Add(p);

            Title = string.Empty;
            Description = string.Empty;
        }

        private void DeleteSelectedProject()
        {
            if (SelectedProject == null) return;
            _projectRepo.DeleteProject(SelectedProject.ProjectId);

            Projects.Remove(SelectedProject);
            SelectedProject = null;
        }
        private void EditSelectedProject()
        {
            if (SelectedProject == null)
            {
                MessageBox.Show("Vælg et projekt, der skal redigeres.");
                return;
            }

            SelectedCompany = Companies.FirstOrDefault(c => c.CompanyId == SelectedProject.CompanyId);
            Title = SelectedProject.Title;
            Description = SelectedProject.Description;

        }
        private void SaveSelectedProject()
        {
            if (SelectedProject == null)
            {
                MessageBox.Show("Vælg et projekt, der skal gemmes.");
                return;
            }

            if (SelectedCompany == null)
            {
                MessageBox.Show("Vælg en virksomhed.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Skriv en projekttitel.");
                return;
            }

            //SelectedProject.CompanyId = SelectedCompany.CompanyId;
            SelectedProject.Title = Title!;
            SelectedProject.Description = Description;

            _projectRepo.UpdateProject(SelectedProject);

            // Reload så listen opdateres
            LoadProjectsForSelectedCompany();

            Title = string.Empty;
            Description = string.Empty;
        }

        private void LoadProjectsForSelectedCompany()
        {
            if (SelectedCompany == null) return;

            Projects.Clear();
           

            foreach (var p in _projectRepo.GetProjectsByCompanyId(SelectedCompany!.CompanyId))
                Projects.Add(p);
        }
    }
}
