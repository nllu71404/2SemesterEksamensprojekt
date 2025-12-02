
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


}
