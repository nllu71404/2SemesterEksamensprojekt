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

namespace _2SemesterEksamensProjekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            InitializeComponent();
        }


        /*
        
        Knapper
        - MainMenuButtom_Click
        - AddProject_Click
        - DeleteProkject_Click
        - EditProject_Click

        ItemSource
        - {Binding Projects}
        - {Binding Companies}

        BeskedBox
        - {Binding SelectedMessageProject}
        - {Binding MessageProject}
        */

        private void MainMenuButtom_Click(object sender, RoutedEventArgs e)
        {
            //Naviger til hovedmenuen
        }

        private void SaveNewProject_Click(object sender, RoutedEventArgs e)
        {
            //Logik tl at oprette et projekt

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}
        }

        private void DeleteProject_Click(Object sender, RoutedEventArgs e)
        {
            //Logik tl at slette et projekt

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}
        }

        private void EditProject_Click(Object obj, RoutedEventArgs e)
        {
            //Logik tl at redigerer et projekt

            //Beskedtekst til bekræftigelse besked
            //- { Binding SelectedMessageProject}
            //- { Binding MessageProject}
        }

    }
    }
