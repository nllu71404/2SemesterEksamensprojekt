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
        //Viser tiden der tæller ned
        private readonly DispatcherTimer _dispatcherTimer;

        private readonly TimeRecord _timeRecord;

        public string TimerName
        {
            get => _timeRecord.TimerName;
            set
            {
                _timeRecord.TimerName = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan ElapsedTime
        {
            get => _timeRecord.ElapsedTime;
            set
            {
                _timeRecord.ElapsedTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayTime));
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }

        public bool IsRunning
        {
            get => _timeRecord.IsRunning;
            set
            {
                _timeRecord.IsRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }

        public string DisplayTime =>
            $"{(int)_timeRecord.ElapsedTime.TotalHours:00}:{_timeRecord.ElapsedTime.Minutes:00}:{_timeRecord.ElapsedTime.Seconds:00}";

        public string DisplayInfo =>
            IsRunning ? "Kører ..." : $"Total tid: {DisplayTime}";

        //ObservableCollection som midlertidig liste med kørende timers 
        public ObservableCollection<TimeRecord> Timers { get; set; }

        //Command properties
        public RelayCommand CreateTimerCommand { get; }
        public RelayCommand DeleteTimerCommand { get; }
        public RelayCommand StartTimerCommand { get; }
        public RelayCommand StopTimerCommand { get; }
        public RelayCommand SaveTimerCommand { get; }

        public TimerPageViewModel()
        {
            //TimerPage
            Timers = new ObservableCollection<TimeRecord>();

            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

            //Commands
            CreateTimerCommand = new RelayCommand(CreateTimer, CanCreateTimer);
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            DeleteTimerCommand = new RelayCommand(DeleteTimer);
            SaveTimerCommand = new RelayCommand(SaveTimer);
        }
        private bool CanCreateTimer(object parameter)
        {
            return !string.IsNullOrWhiteSpace(TimerName);
        }

        private void CreateTimer(object parameter)
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
        }


        private void StartTimer(object parameter)
        {
            if(parameter is Timer timer && !timer.IsRunning){
                timer.StartTime = DateTime.Now;
                timer.IsRunning = true;
            }
        }

        private void StopTimer(object parameter)
        {
            if(parameter is Timer timer && timer.IsRunning)
            {
                timer.IsRunning = false;
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach(var timer in Timers)
            {
                if (timer.IsRunning)
                {
                    //var elapsed = DateTime.Now - timer.StartTime;
                    timer.ElapsedTime += TimeSpan.FromSeconds(1);
                }
            }
        }
        public void DeleteTimer(object parameter)
        {
            if (parameter is Timer timerToDelete)
            {
                var result = MessageBox.Show(
                    $"Er du sikker på, at du vil slette timeren '{timerToDelete.TimerName}'?",
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
        public void SaveTimer(object parameter)
        {
            if(parameter is TimeRecord timer)
            {
                AppNavigationService.Navigate(new TimeRecordPage(timer));
            }
        }
        
    }
}
