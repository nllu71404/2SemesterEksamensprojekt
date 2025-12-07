using _2SemesterEksamensProjekt.ViewModels;
using _2SemesterEksamensProjekt.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Timer = _2SemesterEksamensProjekt.Models.TimeRecord;

namespace Test2SemesterEksamensProjekt.ViewModels
{
    public class TestableTimerPageViewModel : TimerPageViewModel
    {
        public TestableTimerPageViewModel()
            : base()
        {
            DisableDispatcherTimer();
        }

        // ---------------------------------------------------------
        // Stop DispatcherTimer så tiden ikke tæller under tests
        // ---------------------------------------------------------
        private void DisableDispatcherTimer()
        {
            var timerField = typeof(TimerPageViewModel)
                .GetField("_dispatcherTimer",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            if (timerField?.GetValue(this) is DispatcherTimer dt)
            {
                dt.Stop(); // vigtigt for deterministiske tests
            }
        }

        public override void DeleteTimer(object parameter)
        {
            if (parameter is Timer timer)
            {
                Timers.Remove(timer);
                TimerName = string.Empty;
            }
        }

        public override void SaveTimer(object parameter)
        {
            if (parameter is Timer timer)
            {
                Timers.Remove(timer);
            }
        }
    }
}
