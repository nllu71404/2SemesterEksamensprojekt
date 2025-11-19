using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class TimeRecord
    {

        public int TimerId { get; set; }
        public string TimerName { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public bool IsRunning { get; set; }
        public DateTime StartTime { get; set; }

        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }
        public int? TopicId { get; set; }
        //    //Fields
        //    public int TimerId { get; set; }
        //    public string TimerName { get; set; }


        //    private TimeSpan _elapsedTime;
        //    public TimeSpan ElapsedTime
        //    {
        //        get => ElapsedTime;
        //        set
        //        {
        //            _elapsedTime = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(DisplayTime));
        //            OnPropertyChanged(nameof(DisplayInfo));
        //        }
        //    }

        //    private bool _isRunning;
        //    public bool IsRunning
        //    {
        //        get => IsRunning;
        //        set
        //        {
        //            IsRunning = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(DisplayInfo));
        //        }
        //    }

        //    public DateTime StartTime { get; set; }
        //    public int? CompanyId { get; set; }
        //    public int? ProjectId { get; set; }
        //    public int? TopicId { get; set; }


        //    //Defulat properties til visning
        //    public string DisplayTime =>
        //        $"{(int)ElapsedTime.TotalHours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}";

        //    public string DisplayInfo =>
        //        IsRunning ? "Kører ..." : $"Total id: {DisplayTime}";

        //    public TimeRecord(string timername)
        //    {
        //        TimerName = timername;
        //        ElapsedTime = TimeSpan.Zero;
        //        IsRunning = false;

        //    }
        //    public TimeRecord() //parameterløs constructor
        //    {

        //    }

        //    public event PropertyChangedEventHandler PropertyChanged;
        //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
