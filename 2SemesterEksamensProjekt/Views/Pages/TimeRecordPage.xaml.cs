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
using _2SemesterEksamensProjekt.ViewModels;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;

namespace _2SemesterEksamensProjekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for TimeRecordPage.xaml
    /// </summary>
    public partial class TimeRecordPage : Page
    {

        public TimeRecordPage(string timerName, TimeSpan elapsedTime)
        {
            InitializeComponent();

            var timeRecord = new TimeRecord
            {
                TimerName = timerName,
                ElapsedTime = elapsedTime,
                IsRunning = false
            };

            var timeRecordRepository = new TimeRecordRepository();
            var companyRepository = new CompanyRepository();
            var projectRepository = new ProjectRepository();
            var topicRepository = new TopicRepository();
            DataContext = new TimeRecordViewModel(timeRecord, timeRecordRepository, companyRepository, projectRepository, topicRepository);
           
        }
    }
}
