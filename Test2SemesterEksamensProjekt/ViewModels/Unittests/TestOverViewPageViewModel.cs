using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using Moq;
using Test2SemesterEksamensProjekt.ViewModels.TestableViewModels;

namespace Test2SemesterEksamensProjekt.ViewModels.Unittests;

[TestClass]
public class TestOverViewPageViewModel
{
    // I denne klasse laver vi unit tests for OverViewPageViewModel
    // Vi tester: constructor, filtrering, nulstilling af filter og CSV-export

    private TestableOverViewPageViewModel testingViewModel;

    private Mock<ITimeRecordRepository> timeRecordRepositoryMock;
    private Mock<ICompanyRepository> companyRepositoryMock;
    private Mock<IProjectRepository> projectRepositoryMock;
    private Mock<ITopicRepository> topicRepositoryMock;
    private Mock<ICsvExportService> csvExportServiceMock;

    private List<TimeRecord> timeRecords;

    [TestInitialize]
    public void Init()
    {
        // Arrange (fælles setup)
        // Opretter mock-repositories og services
        timeRecordRepositoryMock = new Mock<ITimeRecordRepository>();
        companyRepositoryMock = new Mock<ICompanyRepository>();
        projectRepositoryMock = new Mock<IProjectRepository>();
        topicRepositoryMock = new Mock<ITopicRepository>();
        csvExportServiceMock = new Mock<ICsvExportService>();

        // Opretter testdata med to tidsregistreringer
        timeRecords = new List<TimeRecord>
        {
            new TimeRecord { StartTime = new DateTime(2024, 5, 1) },
            new TimeRecord { StartTime = new DateTime(2024, 5, 2) }
        };

        // Repository returnerer alle tidsregistreringer
        timeRecordRepositoryMock
            .Setup(x => x.GetAllTimeRecords())
            .Returns(timeRecords);

        // Repository returnerer én virksomhed
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 1 }
            });

        // Repository returnerer ét projekt
        projectRepositoryMock
            .Setup(x => x.GetAllProjects())
            .Returns(new List<Project>
            {
                new Project { ProjectId = 10, CompanyId = 1 }
            });

        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(1))
            .Returns(new List<Project>
            {
                new Project { ProjectId = 10, CompanyId = 1 }
            });

        // Repository returnerer ét emne
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                new Topic { TopicId = 100 }
            });

        // Opretter ViewModel med mocked dependencies
        testingViewModel = new TestableOverViewPageViewModel(
            timeRecordRepositoryMock.Object,
            companyRepositoryMock.Object,
            projectRepositoryMock.Object,
            topicRepositoryMock.Object,
            csvExportServiceMock.Object
        );
    }

    [TestMethod]
    public void Constructor_LoadsInitialData()
    {
        // Assert
        // ViewModel skal indlæse alle data korrekt ved oprettelse
        Assert.AreEqual(2, testingViewModel.TimeRecords.Count);
        Assert.AreEqual(2, testingViewModel.FilteredTimeRecords.Count);

        // ObservableCollections til UI-binding skal være udfyldt
        Assert.AreEqual(1, testingViewModel.Companies.Count);
        Assert.AreEqual(1, testingViewModel.Topics.Count);
    }

    [TestMethod]
    public void ApplyFilter_WithActiveFilter_LoadsFilteredTimeRecords()
    {
        // Arrange
        // Repository returnerer kun én tidsregistrering ved filtrering
        timeRecordRepositoryMock
            .Setup(x => x.GetTimeRecordByFilter(
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .Returns(timeRecords.Take(1).ToList());

        // Act
        // Brugeren anvender et filter i UI
        testingViewModel.ApplyFilterCommand.Execute(null);

        // Assert
        // Den filtrerede liste skal kun indeholde ét element
        Assert.AreEqual(1, testingViewModel.FilteredTimeRecords.Count);
    }

    [TestMethod]
    public void ClearFilter_FilterApplied_ReloadsAllTimeRecords()
    {
        // Arrange
        // Filtreret liste er tom (simulerer aktivt filter)
        testingViewModel.FilteredTimeRecords.Clear();

        // Act
        // Brugeren nulstiller filteret
        testingViewModel.ClearFilterCommand.Execute(null);

        // Assert
        // Alle tidsregistreringer skal genindlæses
        Assert.AreEqual(2, testingViewModel.FilteredTimeRecords.Count);
    }

    [TestMethod]
    public void ExportToCSV_WhenDialogAccepted_ExportsCsv()
    {
        // Arrange
        // Brugeren accepterer gem-dialogen
        testingViewModel.DialogResult = true;

        // Act
        // Brugeren eksporterer til CSV
        testingViewModel.CsvCommand.Execute(null);

        // Assert
        // CSV-export-servicen skal kaldes én gang med filtrerede data
        csvExportServiceMock.Verify(
            x => x.ExportTimeRecords(
                testingViewModel.FilteredTimeRecords,
                "test.csv",
                It.IsAny<string[]>()),
            Times.Once
        );
    }

    [TestMethod]
    public void ExportToCSV_WhenDialogCancelled_DoesNotExport()
    {
        // Arrange
        // Brugeren annullerer gem-dialogen
        testingViewModel.DialogResult = false;

        // Act
        // Brugeren forsøger at eksportere til CSV
        testingViewModel.CsvCommand.Execute(null);

        // Assert
        // CSV-export må ikke blive kaldt
        csvExportServiceMock.Verify(
            x => x.ExportTimeRecords(
                It.IsAny<IEnumerable<TimeRecord>>(),
                It.IsAny<string>(),
                It.IsAny<string[]>()),
            Times.Never
        );
    }
}