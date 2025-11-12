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
using System.Windows.Input;

namespace _2SemesterEksamensProjekt.ViewModels
    {
        public class CompanyPageViewModel : BaseViewModel
    {
        public readonly CompanyRepository _companyRepository;
        public ObservableCollection<Company> Companies { get; } = new ObservableCollection<Company>();


        // Command properties
        public RelayCommand CreateCompanyCommand { get; }
        public RelayCommand DeleteSelectedCompanyCommand { get; }
        public RelayCommand EditSelectedCompanyCommand { get; }

        public RelayCommand SaveSelectedCompanyCommand { get; } 

        
        

        // Inputs til virksomhed
        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        private Company? _selectedCompany;
        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);

        }

        public CompanyPageViewModel(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            foreach (var company in _companyRepository.GetAllCompanies())
            {
                Companies.Add(company);
            }
            SelectedCompany = Companies.FirstOrDefault();

            // Commands initialiseres og kalder de eksisterende metoder
            CreateCompanyCommand = new RelayCommand(_ => CreateCompany());
            DeleteSelectedCompanyCommand = new RelayCommand(_ => DeleteSelectedCompany());
            EditSelectedCompanyCommand = new RelayCommand(_ => EditSelectedCompany());
            SaveSelectedCompanyCommand = new RelayCommand(_ => SaveSelectedCompany());
        }

        public void CreateCompany()
        {
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                MessageBox.Show("Udfyld venligst virksomhedsnavn");
                return;
            }
            
            var newCompany = new Company
            {
                CompanyName = _companyName
            };

            int newCompanyId = _companyRepository.SaveNewCompany(newCompany);
            newCompany.CompanyId = newCompanyId;

            Companies.Add(newCompany);
            CompanyName = "";
            MessageBox.Show("Virksomhed oprettet");
        }

        public void DeleteSelectedCompany()
        {
            if (SelectedCompany == null) return;

            // Vis bekræftelsesdialog
            var result = MessageBox.Show(
                $"Er du sikker på, at du vil slette virksomheden '{SelectedCompany.CompanyName}' og alle tilknyttede projekter?",
                "Bekræft sletning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // Brugeren trykkede "Ja"
            if (result == MessageBoxResult.Yes)
            {
                _companyRepository.DeleteCompany(SelectedCompany.CompanyId);
                Companies.Remove(SelectedCompany);

                SelectedCompany = null;

            }
            
        }

        public void EditSelectedCompany()
        {
            if (SelectedCompany == null) 
            {
                MessageBox.Show("Vælg en virksomhed, der skal redigeres.");
                return;
            }
            CompanyName = SelectedCompany.CompanyName;
        }

        private void SaveSelectedCompany()
        {
            if (SelectedCompany == null)
            {
                MessageBox.Show("Vælg en virksomhed, der skal gemmes.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                MessageBox.Show("Skriv et virksomhedsnavn");
                return;
            }

            SelectedCompany.CompanyName = CompanyName!;

            _companyRepository.UpdateCompany(SelectedCompany);

            // Reload så listen opdateres
            Companies.Clear();
            foreach (var c in _companyRepository.GetAllCompanies())
                Companies.Add(c);

            CompanyName = string.Empty;
        }
    }


}
