using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class CompanyPageViewModel : BaseViewModel
    {

        public readonly CompanyRepository _companyRepository;

        //Data til UI
        public ObservableCollection<Company> Companies { get; } = new ObservableCollection<Company>();

        //Inputs til virksomhed
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
                SetProperty(ref _selectedCompany, value); //Notificerer kun hvis SelectedCompany rent faktisk ændres
                {
                    CompanyName = value?.CompanyName ?? "";
                }
            }
        }
        //Bekræftigelsebeskeder
        public string AddedCompanyMessage { get; private set; } = "";
        public string DeletedCompanyMessage { get; private set; } = "";
        public string EdittedCompanyMessage { get; private set; } = "";

        public CompanyPageViewModel(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            foreach(var company in _companyRepository.GetAllCompanies())
            {
                Companies.Add(company);
            }
            SelectedCompany = Companies.FirstOrDefault();
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
            //Hvis ingen er valg, så retuner
            if (SelectedCompany == null) return;

            //Kalder metode i repository der slette fra databasen
            _companyRepository.DeleteCompany(SelectedCompany);

            //Sletter fra ObservableCollection
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
