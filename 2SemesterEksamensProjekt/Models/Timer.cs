using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Timer : INotifyPropertyChanged
    {
        private string _timerName;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        private DateTime _startTime;


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
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayTime));
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        //Defulat properties til visning
        public string DisplayTime =>
            $"{(int)ElapsedTime.TotalHours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}";

        public string DisplayInfo =>
            IsRunning ? "Kører ..." : $"Total id: {DisplayTime}";

        public Timer(string timername, TimeSpan elapsedTime, bool isRunning, DateTime startTime  )
        {
            TimerName = timername;
            ElapsedTime = elapsedTime;
            IsRunning = isRunning;
            StartTime = startTime;
        }
        public Timer() //Parameterløs
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
