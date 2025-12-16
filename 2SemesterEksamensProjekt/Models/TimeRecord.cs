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


        //--Fields--
        private string _timerName;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        private string? _note;
        private int _manualHours;
        private int _manualMinutes;

        //--Auto-Properties--
        public int TimerId { get; set; }
        public DateTime StartTime { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectTitle { get; set; }
        public int? TopicId { get; set; }
        public string? TopicDescription { get; set; }

        
        //--Properties--
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

        
        public string? Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }
       
        public int ManualHours
        {
            get => _manualHours;
            set
            {
                _manualHours = value;
                OnPropertyChanged();
            }
        }
       
        public int ManualMinutes
        {
            get => _manualMinutes;
            set
            {
                _manualMinutes = value;
                OnPropertyChanged();
            }
        }

        public string DisplayTime =>
            $"{(int)ElapsedTime.TotalHours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}";

        public string DisplayInfo =>
            IsRunning ? "Kører ..." : $"Total id: {DisplayTime}";


        //--Events--
        public event PropertyChangedEventHandler PropertyChanged;

        //--Parameterløs constructor--
        public TimeRecord() 
        {

        }


        //--Metoder--
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

