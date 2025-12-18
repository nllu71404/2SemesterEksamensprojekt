using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using Moq;
using System.Collections.ObjectModel;
using System.Windows;
using Test2SemesterEksamensProjekt.ViewModels.TestableViewModels;

namespace Test2SemesterEksamensProjekt.ViewModels.Unittests;

[TestClass]
public class TestTimeRecordViewModel
{
    // I denne klasse laver vi unit tests for TimeRecordViewModel
    // Vi tester: constructor, validering, gemning og brugerbekræftelse

    private TestableTimeRecordViewModel testingViewModel;
    private Mock<ITimeRecordRepository> timeRecordRepositoryMock;
    private Mock<ICompanyRepository> companyRepositoryMock;
    private Mock<IProjectRepository> projectRepositoryMock;
    private Mock<ITopicRepository> topicRepositoryMock;

    private TimeRecord timeRecord;

    [TestInitialize]
    public void Init()
    {
        // Arrange 
        // Opretter mock-repositories
        timeRecordRepositoryMock = new Mock<ITimeRecordRepository>();
        companyRepositoryMock = new Mock<ICompanyRepository>();
        projectRepositoryMock = new Mock<IProjectRepository>();
        topicRepositoryMock = new Mock<ITopicRepository>();

        // Opretter observable collection til eksisterende tidsregistreringer
        var timeRecords = new ObservableCollection<TimeRecord>();

        // Opretter et TimeRecord som skal gemmes
        timeRecord = new TimeRecord
        {
            TimerName = "TestTimer"
        };

        // Repository returnerer én virksomhed
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 1, CompanyName = "TestCompany" }
            });

        // Repository returnerer projekter
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
                new Topic { TopicId = 100, TopicDescription = "TestTopic" }
            });

        // Opretter ViewModel med mocked dependencies
        testingViewModel = new TestableTimeRecordViewModel(
            timeRecord,
            timeRecordRepositoryMock.Object,
            timeRecords,                        
            companyRepositoryMock.Object,
            projectRepositoryMock.Object,
            topicRepositoryMock.Object
        );
    }

    [TestMethod]
    public void Constructor_LoadsCompaniesAndTopicsToObservableCollection()
    {
        // Assert
        // ViewModel skal indlæse virksomheder og emner ved oprettelse
        Assert.AreEqual(1, testingViewModel.Companies.Count);
        Assert.AreEqual(1, testingViewModel.Topics.Count);
    }

    [TestMethod]
    public void SaveTimeRecord_MissingSelections_ShowsValidationMessage()
    {
        // Act
        // Brugeren forsøger at gemme uden at vælge virksomhed, projekt og emne
        testingViewModel.SaveTimeRecordCommand.Execute(null);

        // Assert
        // Der skal vises en valideringsbesked til brugeren
        Assert.AreEqual(
            "Udfyld venligst Virksomhed, Projekt og Emne",
            testingViewModel.LastShownMessage
        );

        // TimeRecord må ikke gemmes i repository
        timeRecordRepositoryMock.Verify(
            x => x.SaveNewTimeRecord(It.IsAny<TimeRecord>()),
            Times.Never
        );
    }

    [TestMethod]
    public void SaveTimeRecord_ConfirmationNo_DoesNotSave()
    {
        // Arrange
        // Brugeren har valgt virksomhed, projekt og emne
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.SelectedProject = new Project { ProjectId = 10, CompanyId = 1 };
        testingViewModel.SelectedTopic = testingViewModel.Topics[0];

        // Brugeren afviser bekræftelsesdialogen
        testingViewModel.ConfirmationResult = MessageBoxResult.No;

        // Act
        // Brugeren forsøger at gemme tidsregistreringen
        testingViewModel.SaveTimeRecordCommand.Execute(null);

        // Assert
        // Tidsregistreringen må ikke gemmes i repository
        timeRecordRepositoryMock.Verify(
            x => x.SaveNewTimeRecord(It.IsAny<TimeRecord>()),
            Times.Never
        );
    }

    [TestMethod]
    public void SaveTimeRecord_ValidInput_SavesTimeRecord()
    {
        // Arrange
        // Brugeren har valgt virksomhed, projekt og emne
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.SelectedProject = new Project { ProjectId = 10, CompanyId = 1 };
        testingViewModel.SelectedTopic = testingViewModel.Topics[0];

        // Brugeren har tilføjet en note
        testingViewModel.Note = "Test note";

        // Brugeren bekræfter gemning
        testingViewModel.ConfirmationResult = MessageBoxResult.Yes;

        // Repository returnerer ID for den gemte tidsregistrering
        timeRecordRepositoryMock
            .Setup(x => x.SaveNewTimeRecord(It.IsAny<TimeRecord>()))
            .Returns(42);

        // Act
        // Brugeren gemmer tidsregistreringen
        testingViewModel.SaveTimeRecordCommand.Execute(null);

        // Assert
        // Repository skal modtage korrekt udfyldt TimeRecord
        timeRecordRepositoryMock.Verify(
            x => x.SaveNewTimeRecord(It.Is<TimeRecord>(t =>
                t.CompanyId == 1 &&
                t.ProjectId == 10 &&
                t.TopicId == 100 &&
                t.Note == "Test note"
            )),
            Times.Once
        );

        // Brugeren skal have en bekræftelsesbesked
        Assert.AreEqual(
            "Tidsregistrering gemt!",
            testingViewModel.LastShownMessage
        );
    }
}

