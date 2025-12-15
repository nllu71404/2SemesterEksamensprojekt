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

namespace Test2SemesterEksamensProjekt.ViewModels.TestableViewModels
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
            // Finder det private felt _dispatcherTimer via reflection,
            // da det ikke er direkte tilgængeligt fra ViewModel
            var timerField = typeof(TimerPageViewModel)
                .GetField("_dispatcherTimer",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            // Tjekker om feltet findes, og om værdien er en DispatcherTimer
            if (timerField?.GetValue(this) is DispatcherTimer dt)
            {
                // Stopper timeren, så der ikke kører baggrundslogik under unit tests
                // Dette sikrer, at tests er deterministiske  og ikke afhænger af tid
                dt.Stop();
            }
        }

        // ---------------------------------------------------------
        //                 OVERRIDES TIL UNIT TESTS
        //     Fjerner UI-dialoger så tests ikke viser popups
        // ---------------------------------------------------------
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
