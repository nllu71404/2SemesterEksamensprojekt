using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class OverViewPageViewModel : BaseViewModel
    {
        //Fields
        private readonly TimeRecordRepository _timeRecordRepo;

        //Observablecollection
        public ObservableCollection<TimeRecord> GetAllTimers {get; set;}
        public ObservableCollection<TimeRecord> GetTimeRecordByMonth { get; set; }
        public ObservableCollection<string> Month { get; set; }

        //Properties til søgekriterier
        private string? _selectedMonth;
        public string? SelectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }



    }
}
