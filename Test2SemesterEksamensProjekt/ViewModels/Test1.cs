using System.Collections.ObjectModel;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test2SemesterEksamensProjekt;

[TestClass]
public class OverViewPageViewModelTests
{
    [TestMethod]
    public void ExportToCSV_ShouldCallCsvExportService_WhenDialogReturnsTrue()
    {

        // Arrange
        var mockTimeRecordRepo = new Mock<ITimeRecordRepository>();
        var mockCompanyRepo = new Mock<ICompanyRepository>();
        var mockProjectRepo = new Mock<IProjectRepository>();
        var mockTopicRepo = new Mock<ITopicRepository>();
        var mockCsvService = new Mock<ICsvExportService>();


        var vm = new OverViewPageViewModel(
             mockTimeRecordRepo.Object,
             mockCompanyRepo.Object,
             mockProjectRepo.Object,
             mockTopicRepo.Object,
             mockCsvService.Object);

        vm.TimeRecords = new ObservableCollection<TimeRecord>
        {
            new TimeRecord { TimerId = 1, TimerName = "TestTimer" }
        };

        string filePath = "C:\\Test\\export.csv";

        // Act
        vm.ExportToCSV();

        // Assert
        mockCsvService.Verify(
           s => s.ExportTimeRecords(vm.TimeRecords, filePath),
           Times.Once,
           "CSV export service should be called once with the correct parameters.");
    }
}
