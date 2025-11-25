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
        private static Frame? _frame;

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

        // Valgfrit: hvis du nogensinde vil bruge historik-tilbage
        public static void GoBack()
        {
            if (_frame != null && _frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
