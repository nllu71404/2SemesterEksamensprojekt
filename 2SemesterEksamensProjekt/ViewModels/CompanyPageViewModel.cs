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
    public class CompanyPageViewModel
    {
        
        public CompanyRepository _companyRepository;

        //Inputs til virksomhed
        public string CompanyName { get; set; } = "";

        //Bekræftigelsebeskeder
        public string AddedCompanyMessage { get; private set; } = "";
        public string DeletedCompanyMessage { get; private set; } = "";
        public string EdittedCompanyMessage { get; private set; } = "";

        //Data til UI
        public ObservableCollection<Company> Companies { get; } = new ObservableCollection<Company>();
        public Company? SelectedCompany { get; set; }


        public AdminCompanyViewModel(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            foreach(var company in _companyRepository.GetAllCompanies())
            {
                Companies.Add(company);
            }
            SelectedCompany = Companies.FirstOrDefault();
        }

    }
}
