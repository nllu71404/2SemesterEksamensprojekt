using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _2SemesterEksamensProjekt.Services
{
    public static class AppNavigationService
    {
        private static Frame? _frame;                          // holder reference til MainWindow’s Frame
        public static void Initialize(Frame frame) => _frame = frame; // kaldes én gang i MainWindow
        public static void Navigate(Page page) => _frame?.Navigate(page); // bruges fra ViewModels
        // Back/Forward-knapperne virker automatisk når NavigationUIVisibility=Visible
    }
}
