using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;

namespace _2SemesterEksamensProjekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : Page
    {
        private CompanyPageViewModel companyViewModel;
        public CompanyRepository _companyRepository;

        public CompanyPage()
        {
            InitializeComponent();

            companyViewModel = new CompanyPageViewModel(_companyRepository);
            DataContext = companyViewModel;
        }

        // Knapper
        /*
        MainMenuButtom_Click
        AddCompany_Click
        DeleteCompany_Click
        EditCompany_Click

        ItemSource
        - {Binding Companies}

        BeskedBox
        - {Binding SelectedMessageCompany}
        - {Binding MessageCompany}
         */

        private void MainMenuButtom_Click(object sender, RoutedEventArgs e)
        {
            //Naviger til hovedmenuen
            companyViewModel.OpenMainMenuPage();
        }

        private void SaveNewCompany_Click(object sender, RoutedEventArgs e)
        {
            //Logik tl at oprette et Virksomhed

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}

            companyViewModel.AddCompany();
        }

        private void DeleteCompany_Click(Object sender, RoutedEventArgs e)
        {
            //Logik tl at slette et Virksomhed

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}

            if (companyViewModel.SelectedCompany != null)
            {
                companyViewModel.DeleteSelectedCompany();
            }
           
        }

        private void EditCompany_Click(Object obj, RoutedEventArgs e)
        {
            //Logik tl at redigerer et Virksomhed

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}

            if (companyViewModel.SelectedCompany != null)
            {
                companyViewModel.LoadFromSelected();
            }
        }
    }
}
