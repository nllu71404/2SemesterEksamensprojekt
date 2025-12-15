using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using Test2SemesterEksamensProjekt.ViewModels.TestableViewModels;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels.Unittests;

[TestClass]
public sealed class TestProjectPageViewModel
{
    // I denne klasse laver vi unit tests for ProjectPageViewModel
    // Vi tester: hentning af projekter, oprettelse, redigering, gemning og sletning

    // Mock-objekter der efterligner repositories
    private Mock<ICompanyRepository> companyRepositoryMock;
    private Mock<IProjectRepository> projectRepositoryMock;

    [TestInitialize]
    public void Init()
    {
        // Arrange (fælles setup)
        // Opretter nye mock-objekter før hver test
        companyRepositoryMock = new Mock<ICompanyRepository>();
        projectRepositoryMock = new Mock<IProjectRepository>();
    }

    [TestMethod]
    public void GetProjectsByCompanyId_SelectingCompany_LoadsProjects()
    {
        // Arrange
        // Repository returnerer én virksomhed
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 100, CompanyName = "MyCompany" }
            });

        // Når virksomheden vælges, returneres to projekter
        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(100))
            .Returns(new List<Project>
            {
                new Project { ProjectId = 1, Title = "P1" },
                new Project { ProjectId = 2, Title = "P2" }
            });

        var testingViewModel = new TestableProjectPageViewModel(
            companyRepositoryMock.Object,
            projectRepositoryMock.Object
        );

        // Act
        // Brugeren vælger en virksomhed i UI
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];

        // Assert
        // Projekter skal nu være indlæst i ViewModel
        Assert.AreEqual(2, testingViewModel.Projects.Count);
        Assert.AreEqual("P1", testingViewModel.Projects[0].Title);
    }

    [TestMethod]
    public void CreateProject_ValidInput_SavesProjectToRepositoryAndAddsToObservableCollection()
    {
        // Arrange
        // Én virksomhed er indlæst
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 200, CompanyName = "AnotherCompany" }
            });

        // Repository returnerer nyt projekt-ID ved gem
        projectRepositoryMock
            .Setup(x => x.SaveNewProject(It.IsAny<Project>()))
            .Returns(55);

        var testingViewModel = new TestableProjectPageViewModel(
            companyRepositoryMock.Object,
            projectRepositoryMock.Object
        );

        // Brugeren udfylder felter i UI
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.Title = "New Project";
        testingViewModel.Description = "Project Description";

        // Act
        // Brugeren opretter et projekt
        testingViewModel.CreateProject();

        // Assert
        // Repository skal være kaldt med korrekt data
        projectRepositoryMock.Verify(
            x => x.SaveNewProject(It.Is<Project>(p =>
                p.CompanyId == 200 &&
                p.Title == "New Project" &&
                p.Description == "Project Description"
            )),
            Times.Once
        );

        // Projektet skal være tilføjet ViewModel
        Assert.AreEqual(1, testingViewModel.Projects.Count);
        Assert.AreEqual(55, testingViewModel.Projects[0].ProjectId);
    }

    [TestMethod]
    public void EditSelectedProject_ProjectIsSelected_SetsProjectNameToSelected()
    {
        // Arrange
        // Virksomhed og projekt er indlæst
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 10, CompanyName = "Virksomhed X" }
            });

        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(10))
            .Returns(new List<Project>
            {
                new Project
                {
                    ProjectId = 5,
                    CompanyId = 10,
                    Title = "Original Titel",
                    Description = "Original Desc"
                }
            });

        var testingViewModel = new TestableProjectPageViewModel(
            companyRepositoryMock.Object,
            projectRepositoryMock.Object
        );

        // Brugeren vælger virksomhed og projekt
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.SelectedProject = testingViewModel.Projects[0];

        // Act
        // Brugeren klikker "Rediger"
        testingViewModel.EditSelectedProject();

        // Assert
        // UI-felter skal nu afspejle projektets data
        Assert.AreEqual("Original Titel", testingViewModel.Title);
        Assert.AreEqual("Original Desc", testingViewModel.Description);
        Assert.AreEqual(10, testingViewModel.SelectedCompany!.CompanyId);
    }

    [TestMethod]
    public void SaveSelectedProject_UpdatesRepositoryAndReloadsProjects()
    {
        // Arrange
        // Virksomhed og oprindeligt projekt
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 20, CompanyName = "Virksomhed Y" }
            });

        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(20))
            .Returns(new List<Project>
            {
                new Project
                {
                    ProjectId = 7,
                    CompanyId = 20,
                    Title = "Gammel Titel",
                    Description = "Gammel Desc"
                }
            });

        var testingViewModel = new TestableProjectPageViewModel(
            companyRepositoryMock.Object,
            projectRepositoryMock.Object
        );

        // Brugeren vælger virksomhed og projekt
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.SelectedProject = testingViewModel.Projects[0];

        // Brugeren ændrer værdier i UI
        testingViewModel.Title = "Ny Titel";
        testingViewModel.Description = "Ny Desc";

        // Act
        // Brugeren gemmer ændringer
        testingViewModel.SaveSelectedProject();

        // Assert
        // Repository skal opdateres med nye værdier
        projectRepositoryMock.Verify(
            x => x.UpdateProject(It.Is<Project>(p =>
                p.ProjectId == 7 &&
                p.Title == "Ny Titel" &&
                p.Description == "Ny Desc"
            )),
            Times.Once
        );

        // Projekter reloades korrekt
        Assert.AreEqual(1, testingViewModel.Projects.Count);

        // Det redigerede projekt forbliver valgt
        Assert.AreEqual(7, testingViewModel.SelectedProject.ProjectId);

        // Inputfelter nulstilles i UI
        Assert.AreEqual(string.Empty, testingViewModel.Title);
        Assert.AreEqual(string.Empty, testingViewModel.Description);
    }

    [TestMethod]
    public void DeleteSelectedProject_ProjectIsSelected_DeletesProjectFromRepositoryAndRemovesFromObservableCollection()
    {
        // Arrange
        // Virksomhed og projekt er indlæst
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
                new Company { CompanyId = 300, CompanyName = "DeleteCompany" }
            });

        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(300))
            .Returns(new List<Project>
            {
                new Project { ProjectId = 10, Title = "ToBeDeleted" }
            });

        var testingViewModel = new TestableProjectPageViewModel(
            companyRepositoryMock.Object,
            projectRepositoryMock.Object
        );

        // Brugeren vælger virksomhed og projekt
        testingViewModel.SelectedCompany = testingViewModel.Companies[0];
        testingViewModel.SelectedProject = testingViewModel.Projects[0];

        // Act
        // Brugeren sletter projektet
        testingViewModel.DeleteSelectedProject();

        // Assert
        // Repository skal kaldes med korrekt ID
        projectRepositoryMock.Verify(x => x.DeleteProject(10), Times.Once);

        // Projektet skal være fjernet fra ViewModel
        Assert.AreEqual(0, testingViewModel.Projects.Count);
    }
}