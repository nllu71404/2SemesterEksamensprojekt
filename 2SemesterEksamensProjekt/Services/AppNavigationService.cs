using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace _2SemesterEksamensProjekt.Services
{
    public static class AppNavigationService
    {
        //--Fields--
        private static Frame? _frame;


        //--Metoder--

        // Kaldes én gang fra MainWindow, efter InitializeComponent
        public static void Initialize(Frame frame)
        {
            _frame = frame;
        }

        // Bruges fra ViewModels og MainWindow til at navigere til en side
        public static void Navigate(Page page)
        {
            _frame?.Navigate(page);
        }

        // Tager dig tilbage til den side, du lige har været på
        public static void GoBack()
        {
            if (_frame != null && _frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
