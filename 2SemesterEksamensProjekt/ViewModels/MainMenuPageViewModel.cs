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

        //--Command properties--
        public ICommand OpenCompanyPageCommand { get; }                  
        public ICommand OpenProjectPageCommand { get; }                  
        public ICommand OpenTopicPageCommand { get; }
        public ICommand OpenTimerPageCommand { get; }
        public ICommand OpenOverViewPageCommand { get; }


        //--Constructor--
        public MainMenuPageViewModel()
        {
            OpenCompanyPageCommand = new RelayCommand(_ =>               
            AppNavigationService.Navigate(new CompanyPage()));          
            OpenProjectPageCommand = new RelayCommand(_ =>
            AppNavigationService.Navigate(new ProjectPage()));
            OpenTopicPageCommand = new RelayCommand(_ => 
            AppNavigationService.Navigate(new TopicPage()));
            OpenTimerPageCommand = new RelayCommand(_ =>
            AppNavigationService.Navigate(new TimerPage()));
            OpenOverViewPageCommand = new RelayCommand(_ =>
            AppNavigationService.Navigate(new OverViewPage()));
        }
    }       
}
