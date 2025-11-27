
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels
{
    [TestClass]
    public sealed class TestCompanyPageViewModel
    {
        private Mock<ICompanyRepository> companyRepositoryMock;

        [TestInitialize]
        public void Init()
        { 
            companyRepositoryMock = new Mock<ICompanyRepository>();
        }

        [TestMethod]
        public void Constructor_LoadsCompaniesToObservableCollection()
        {
            // Arrange
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>
                {
                                new _2SemesterEksamensProjekt.Models.Company { CompanyId = 1, CompanyName = "Test Company 1" },
                                new _2SemesterEksamensProjekt.Models.Company { CompanyId = 2, CompanyName = "Test Company 2" }
                });
            
            // Act
            var testingViewModel = new CompanyPageViewModel(companyRepositoryMock.Object);

            // Assert
            Assert.AreEqual(2, testingViewModel.Companies.Count);
            Assert.AreEqual("Test Company 1", testingViewModel.Companies[0].CompanyName);
        }
    }
}
