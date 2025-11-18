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
using _2SemesterEksamensProjekt.Views.Pages;
using Timer = _2SemesterEksamensProjekt.Models.TimeRecord;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class TimerPageViewModel : BaseViewModel
    {
        
        private readonly TimeRecordRepository _timeRecordRepository;
        private readonly DispatcherTimer _dispatcherTimer;
        private string _newTimerName;
        private string _timerName;
        private string _elapsedTime;
        private string _isRunning;
        private string _startTime;

        public ObservableCollection<TimeRecord> Timers { get; } = new ObservableCollection<TimeRecord>();

        
        //Command properties
        public RelayCommand CreateTimerCommand { get; }
        public RelayCommand DeleteTimerCommand { get; }
        public RelayCommand StartTimerCommand { get; }
        public RelayCommand StopTimerCommand { get; }
        public RelayCommand SaveTimerCommand { get; }

        
        //Inputs til Timer

        public string NewTimerName
        {
            get => _newTimerName;
            set
            {
                _newTimerName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public string TimerName
        {
            get => _timerName;
            set
            {
                _timerName = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan ElapsedTime
        {
            get => ElapsedTime;
            set
            {
                ElapsedTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayTime));
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }
        public bool IsRunning
        {
            get => IsRunning;
            set
            {
                IsRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }

        public DateTime StartTime
        {
            get => StartTime;
            set
            {
                StartTime = value;
                OnPropertyChanged();
            }
        }

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
        //Defulat properties til visning
        public string DisplayTime =>
            $"{(int)ElapsedTime.TotalHours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}";

        public string DisplayInfo =>
            IsRunning ? "Kører ..." : $"Total id: {DisplayTime}";

        private bool CanCreateTimer(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewTimerName);
        }

        private void CreateTimer(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewTimerName))
                return;

            var newTimer = new Timer
            {
                TimerName = NewTimerName,
                ElapsedTime = TimeSpan.Zero,
                IsRunning = false
            };

            Timers.Add(newTimer);
            NewTimerName = string.Empty;
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
                    NewTimerName = string.Empty;
                }
            }
        }
        public void SaveTimer(object parameter)
        {
            //Kald den nye side
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
