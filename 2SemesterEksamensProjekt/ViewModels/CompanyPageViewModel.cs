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
            //--Fields--
            private readonly ICompanyRepository _companyRepo;
            private string _companyName;
            private Company? _selectedCompany;
            
            //--Properties--
            public ObservableCollection<Company> Companies { get; } = new ObservableCollection<Company>();


            // Command properties
            public RelayCommand CreateCompanyCommand { get; }
            public RelayCommand DeleteSelectedCompanyCommand { get; }
            public RelayCommand EditSelectedCompanyCommand { get; }
            public RelayCommand SaveSelectedCompanyCommand { get; } 
           
            public string CompanyName
            {
                get => _companyName;
                set => SetProperty(ref _companyName, value);
            }

            
            public Company? SelectedCompany
            {
                get => _selectedCompany;
                set => SetProperty(ref _selectedCompany, value);

            }

            //--Constructor--
            public CompanyPageViewModel(ICompanyRepository companyRepo)
            {
                this._companyRepo = companyRepo;
                foreach (var company in companyRepo.GetAllCompanies())
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
            
            //--Metoder--
            public void CreateCompany()
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    ShowMessage("Udfyld venligst virksomhedsnavn");
                    return;
                }
            
                var newCompany = new Company
                {
                    CompanyName = _companyName
                };

                int newCompanyId = _companyRepo.SaveNewCompany(newCompany);
                newCompany.CompanyId = newCompanyId;

                Companies.Add(newCompany);
                CompanyName = "";
                ShowMessage("Virksomhed oprettet");
            }
            public void DeleteSelectedCompany()
            {
                if (SelectedCompany == null) return;

                var result = ShowConfirmation(
                    $"Er du sikker på, at du vil slette virksomheden '{SelectedCompany.CompanyName}' og alle tilknyttede projekter?"
                );

                if (result == MessageBoxResult.Yes)
                {
                    _companyRepo.DeleteCompany(SelectedCompany.CompanyId);
                    Companies.Remove(SelectedCompany);
                    SelectedCompany = null;
                }
            }

            public void EditSelectedCompany()
            {
                if (SelectedCompany == null) 
                {
                    ShowMessage("Vælg en virksomhed, der skal redigeres.");
                    return;
                }
                CompanyName = SelectedCompany.CompanyName;
            }

            public void SaveSelectedCompany()
            {
                if (SelectedCompany == null)
                {
                    ShowMessage("Vælg en virksomhed, der skal gemmes.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    ShowMessage("Skriv et virksomhedsnavn");
                    return;
                }

                SelectedCompany.CompanyName = CompanyName!;

                _companyRepo.UpdateCompany(SelectedCompany);

                // Reload så listen opdateres
                Companies.Clear();
                foreach (var c in _companyRepo.GetAllCompanies())
                    Companies.Add(c);

                CompanyName = string.Empty;
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
