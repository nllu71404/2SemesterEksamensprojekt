using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _2SemesterEksamensProjekt.ViewModels
    {
        public class CompanyPageViewModel : BaseViewModel
    {
        public readonly CompanyRepository _companyRepository;

        // Command properties
        public ICommand SaveNewCompanyCommand { get; }
        public ICommand DeleteSelectedCompanyCommand { get; }
        public ICommand EditSelectedCompanyCommand { get; }

        // Data til UI
        public ObservableCollection<Company> Companies { get; } = new ObservableCollection<Company>();

        // Inputs til virksomhed
        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                SetProperty(ref _selectedCompany, value);
                CompanyName = value?.CompanyName ?? "";
            }
        }

        // Bekræftigelsesbeskeder
        public string AddedCompanyMessage { get; private set; } = "";
        public string DeletedCompanyMessage { get; private set; } = "";
        public string EdittedCompanyMessage { get; private set; } = "";

        public CompanyPageViewModel(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            foreach (var company in _companyRepository.GetAllCompanies())
            {
                Companies.Add(company);
            }
            SelectedCompany = Companies.FirstOrDefault();

            // Commands initialiseres og kalder de eksisterende metoder
            SaveNewCompanyCommand = new RelayCommand(_ => SaveNewCompany());
            DeleteSelectedCompanyCommand = new RelayCommand(_ => DeleteSelectedCompany());
            EditSelectedCompanyCommand = new RelayCommand(_ => EditSelectedCompany());
        }

        public void SaveNewCompany()
        {
            if (string.IsNullOrWhiteSpace(CompanyName)) return;

            var newCompany = new Company
            {
                CompanyName = _companyName
            };

            int newCompanyId = _companyRepository.SaveNewCompany(newCompany);
            newCompany.CompanyId = newCompanyId;

            Companies.Add(newCompany);
            CompanyName = "";
        }

        public void DeleteSelectedCompany()
        {
            if (SelectedCompany == null) return;

            _companyRepository.DeleteCompany(SelectedCompany);
            Companies.Remove(SelectedCompany);
        }

        public void EditSelectedCompany()
        {
            if (SelectedCompany == null) return;

            SelectedCompany.CompanyName = CompanyName;
            _companyRepository.EditCompany(SelectedCompany);
            OnPropertyChanged(nameof(SelectedCompany));
        }
    }


}
