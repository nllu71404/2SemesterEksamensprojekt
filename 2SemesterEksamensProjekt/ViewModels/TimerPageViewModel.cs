using System;
using _2SemesterEksamensProjekt.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Repository;
using Timer = _2SemesterEksamensProjekt.Models.Timer;

 namespace _2SemesterEksamensProjekt.ViewModels
{
    public class TimerPageViewModel : BaseViewModel
    {
        //private readonly TimerRepository _timerRepository;

        private readonly DispatcherTimer _dispatcherTimer;
        private string _newTimerName;

        public ObservableCollection<Timer> Timers { get; set; }

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

        public ICommand CreateTimerCommand { get; }
        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public TimerPageViewModel()
        {
            Timers = new ObservableCollection<Timer>();

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
        }

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
                Name = NewTimerName,
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
                    var elapsed = DateTime.Now - timer.StartTime;
                    timer.ElapsedTime += TimeSpan.FromSeconds(1);
                }
            }
        }

    }
}
