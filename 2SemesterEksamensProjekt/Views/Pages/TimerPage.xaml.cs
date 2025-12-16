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

namespace _2SemesterEksamensProjekt.Views.Pages
{
  
    public partial class TimerPage : Page
    {
        //--Constructor--
        public TimerPage()
        {
            InitializeComponent();
            DataContext = new TimerPageViewModel();
        }
    }
}
