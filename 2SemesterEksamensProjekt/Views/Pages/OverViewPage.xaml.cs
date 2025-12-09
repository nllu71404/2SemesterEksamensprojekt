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
    /// Interaction logic for OverViewPage.xaml
    /// </summary>
    public partial class OverViewPage : Page
    {
        public OverViewPage()
        {
            var TimeRecordRepository = new TimeRecordRepository();
            var CompanyRepository = new CompanyRepository();
            var ProjectRepository = new ProjectRepository();
            var TopicRepository = new TopicRepository();

            DataContext = new OverViewPageViewModel(
                TimeRecordRepository,
                CompanyRepository,
                ProjectRepository,
                TopicRepository
                );

        }
    }
}
