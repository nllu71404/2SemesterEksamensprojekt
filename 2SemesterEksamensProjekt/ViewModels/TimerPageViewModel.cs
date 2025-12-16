using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.Services;
using _2SemesterEksamensProjekt.Views.Pages;
using Timer = _2SemesterEksamensProjekt.Models.TimeRecord;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class TimerPageViewModel : BaseViewModel
    {
        //--Fields--
        
        //Viser tiden der tæller ned
        private readonly DispatcherTimer _dispatcherTimer;

        //--Properties--

        //Auto-properties

        //Midlertidig liste med kørende timers 
        public ObservableCollection<Timer> Timers { get; set; }
        public string TimerName { get; set; }
        public TimeSpan ElapsedTime { get; set; }

        //Command properties
        public RelayCommand CreateTimerCommand { get; }
        public RelayCommand DeleteTimerCommand { get; }
        public RelayCommand StartTimerCommand { get; }
        public RelayCommand StopTimerCommand { get; }
        public RelayCommand SaveTimerCommand { get; }
        public RelayCommand AddManualTimeCommand { get; }
        public RelayCommand SubtractManualTimeCommand { get; }


        //--Constructor--
        public TimerPageViewModel()
        {
            Timers = new ObservableCollection<Timer>();
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

            CreateTimerCommand = new RelayCommand(CreateTimer, CanCreateTimer);
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            DeleteTimerCommand = new RelayCommand(DeleteTimer);
            SaveTimerCommand = new RelayCommand(SaveTimer);
            AddManualTimeCommand = new RelayCommand(AddManualTime);
            SubtractManualTimeCommand = new RelayCommand(SubtractManualTime);
        }

        //--Metoder--
        public bool CanCreateTimer(object parameter)
        {
            return !string.IsNullOrWhiteSpace(TimerName);
        }

        public void CreateTimer(object parameter)
        {
            if (string.IsNullOrWhiteSpace(TimerName))
                return;

            var newTimer = new Timer
            {
                TimerName = TimerName,
                ElapsedTime = TimeSpan.Zero,
                IsRunning = false
            };

            Timers.Add(newTimer);
            TimerName = string.Empty;
            OnPropertyChanged(nameof(TimerName));
        }

        public void StartTimer(object parameter)
        {
            if (parameter is Timer timer && !timer.IsRunning)
            {
                timer.StartTime = DateTime.Now;
                timer.IsRunning = true;
            }
        }

        public void StopTimer(object parameter)
        {
            if (parameter is Timer timer && timer.IsRunning)
            {
                timer.IsRunning = false;
            }
        }
        public void AddManualTime(object parameter)
        {
            if (parameter is Timer timer)
            {
                timer.ElapsedTime += TimeSpan.FromHours(timer.ManualHours)
                    + TimeSpan.FromMinutes(timer.ManualMinutes);

                timer.ManualHours = 0;
                timer.ManualMinutes = 0;
            }
        }

        public void SubtractManualTime(object parameter)
        {
            if (parameter is Timer timer)
            {
                var timeToSubtract = TimeSpan.FromHours(timer.ManualHours)
                    + TimeSpan.FromMinutes(timer.ManualMinutes);
                if (timer.ElapsedTime > timeToSubtract)
                {
                    timer.ElapsedTime -= timeToSubtract;
                }
                else
                {
                    timer.ElapsedTime = TimeSpan.Zero;
                }
                timer.ManualHours = 0;
                timer.ManualMinutes = 0;
            }
        }

        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach (var timer in Timers)
            {
                if (timer.IsRunning)
                {
                    timer.ElapsedTime += TimeSpan.FromSeconds(1);
                }
            }
        }

        public virtual void DeleteTimer(object parameter)
        {
            if (parameter is Timer timerToDelete)
            {
                var result = MessageBox.Show(
                    $"Er du sikker på, at du vil slette stopuret '{timerToDelete.TimerName}'?",
                    "Bekræft sletning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    Timers.Remove(timerToDelete);
                    TimerName = string.Empty;
                }
            }
        }

        public virtual void SaveTimer(object parameter)
        {

            if (parameter is Timer timer)
            {
                AppNavigationService.Navigate(new TimeRecordPage(
                timer.TimerName,
                timer.ElapsedTime,
                Timers
                ));
                
            }

        }
    }
}