using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class TimeRecord : INotifyPropertyChanged
    {


        //Fields
        public int TimerId { get; set; }

        private string _timerName;
        public string TimerName
        {
            get => _timerName;
            set 
            {
                _timerName = value;
                OnPropertyChanged();
                    
            }
        }


        private TimeSpan _elapsedTime;
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

        private bool _isRunning;
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

        public DateTime StartTime { get; set; }
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }
        public int? TopicId { get; set; }

        private string? _note;
        public string? Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }
        private int _manualHours;
        public int ManualHours
        {
            get => _manualHours;
            set
            {
                _manualHours = value;
                OnPropertyChanged();
            }
        }
        private int _manualMinutes;
        public int ManualMinutes
        {
            get => _manualMinutes;
            set
            {
                _manualMinutes = value;
                OnPropertyChanged();
            }
        }

        //Defulat properties til visning
        public string DisplayTime =>
            $"{(int)ElapsedTime.TotalHours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}";

        public string DisplayInfo =>
            IsRunning ? "Kører ..." : $"Total id: {DisplayTime}";

        public TimeRecord(string timername)
        {
            TimerName = timername;
            ElapsedTime = TimeSpan.Zero;
            IsRunning = false;

        }
        public TimeRecord() //parameterløs constructor
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

