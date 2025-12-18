using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Test2SemesterEksamensProjekt.ViewModels.TestableViewModels
{
    public class TestableTimeRecordViewModel : TimeRecordViewModel
    {
        public string? LastShownMessage { get; private set; }
        public MessageBoxResult ConfirmationResult { get; set; } = MessageBoxResult.Yes;
        public bool SaveWasCalled { get; private set; }

        public TestableTimeRecordViewModel(
        TimeRecord timeRecord,
        ITimeRecordRepository timeRecordRepository,
        ObservableCollection<TimeRecord> timeRecords,
        ICompanyRepository companyRepository,
        IProjectRepository projectRepository,
        ITopicRepository topicRepository)
        : base(
            timeRecord,
            timeRecordRepository,
            timeRecords,
            companyRepository,
            projectRepository,
            topicRepository)
        {
        }

        // ---------------------------------------------------------
        //                 OVERRIDES TIL UNIT TESTS
        //     Fjerner UI-dialoger så tests ikke viser popups
        // ---------------------------------------------------------
        protected override void ShowMessage(string msg)
        {
            // Gem beskeden i stedet for at vise popup
            LastShownMessage = msg;
        }

        protected override MessageBoxResult ShowConfirmation(string message)
        {
            return ConfirmationResult;
        }
    }
}
