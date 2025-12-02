
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels;

[TestClass]
public sealed class TestProjectPageViewModel
{
    // Et mock objekt som skal efterligne vores CompanyRepository
    private Mock<ICompanyRepository> companyRepositoryMock;
    private Mock<IProjectRepository> projectRepositoryMock;

    [TestInitialize]
    public void Init()
    {
        // Opretter et nyt mock objekt før hver test
        companyRepositoryMock = new Mock<ICompanyRepository>();
        projectRepositoryMock = new Mock<IProjectRepository>();
    }

    [TestMethod]
    public void GetProjectsByCompanyId_SelectingCompany_LoadsProjects()
    {
        // Arrange
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
            new Company { CompanyId = 100, CompanyName = "MyCompany" }
            });

        projectRepositoryMock
            .Setup(x => x.GetProjectsByCompanyId(100))
            .Returns(new List<Project>
            {
            new Project { ProjectId = 1, Title = "P1" },
            new Project { ProjectId = 2, Title = "P2" }
            });

        var vm = new TestableProjectPageViewModel(companyRepositoryMock.Object,
                                                  projectRepositoryMock.Object);

        // Act
        vm.SelectedCompany = vm.Companies[0];

        // Assert
        Assert.AreEqual(2, vm.Projects.Count);
        Assert.AreEqual("P1", vm.Projects[0].Title);
    }

    [TestMethod]
    public void CreateProject_ValidInput_SavesProjectToRepositoryAndAddsToObservableCollection()
    {
        // Arrange
        companyRepositoryMock
            .Setup(x => x.GetAllCompanies())
            .Returns(new List<Company>
            {
            new Company { CompanyId = 200, CompanyName = "AnotherCompany" }
            });

        projectRepositoryMock
            .Setup(x => x.SaveNewProject(It.IsAny<Project>()))
            .Returns(55); // Nyt projekt ID
        var vm = new TestableProjectPageViewModel(companyRepositoryMock.Object,
                                                  projectRepositoryMock.Object);
        vm.SelectedCompany = vm.Companies[0];
        vm.Title = "New Project";
        vm.Description = "Project Description";

        // Act
        vm.CreateProject();

        // Assert
        projectRepositoryMock.Verify(x => x.SaveNewProject(It.Is<Project>(p =>
            p.CompanyId == 200 &&
            p.Title == "New Project" &&
            p.Description == "Project Description"
        )), Times.Once);
        Assert.AreEqual(1, vm.Projects.Count);
        Assert.AreEqual(55, vm.Projects[0].ProjectId);
    } 

    [TestMethod]
    public void DeleteSelectedProject_ProjectIsSelected_DeletesProjectFromRepositoryAndRemovesFromObservableCollection()
    {
        // Arrange
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
        var vm = new TestableProjectPageViewModel(companyRepositoryMock.Object,
                                                  projectRepositoryMock.Object);
        vm.SelectedCompany = vm.Companies[0];
        vm.SelectedProject = vm.Projects[0];
        // Act
        vm.DeleteSelectedProject();
        // Assert
        projectRepositoryMock.Verify(x => x.DeleteProject(10), Times.Once);
        Assert.AreEqual(0, vm.Projects.Count);
    }


}
