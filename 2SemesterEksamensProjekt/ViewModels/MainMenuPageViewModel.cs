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
        public ICommand OpenProjectPageCommand { get; }                  // bindes i XAML
        public ICommand OpenTopicPageCommand { get; }
        public ICommand OpenTimerPageCommand { get; }

        public MainMenuPageViewModel()
        {
            OpenCompanyPageCommand = new RelayCommand(_ =>               // ved klik på knap:
            AppNavigationService.Navigate(new CompanyPage()));          // navigér til CompanyPage
            OpenProjectPageCommand = new RelayCommand(_ =>
            AppNavigationService.Navigate(new ProjectPage()));
            OpenTopicPageCommand = new RelayCommand(_ => 
            AppNavigationService.Navigate(new TopicPage()));
            OpenTimerPageCommand = new RelayCommand(_ =>
            AppNavigationService.Navigate(new TimerPage()));
        }
    }       
}
