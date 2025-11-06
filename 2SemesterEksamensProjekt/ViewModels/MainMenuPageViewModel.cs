using _2SemesterEksamensProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _2SemesterEksamensProjekt.Services;
using _2SemesterEksamensProjekt.Views.Pages;
namespace _2SemesterEksamensProjekt.ViewModels
{
    public class MainMenuPageViewModel : BaseViewModel
    {
        public ICommand OpenCompanyPageCommand { get; }                  // bindes i XAML

        public MainMenuPageViewModel()
        {
            OpenCompanyPageCommand = new RelayCommand(_ =>               // ved klik på knap:
            AppNavigationService.Navigate(new CompanyPage()));          // navigér til CompanyPage
        }
    }       
}
