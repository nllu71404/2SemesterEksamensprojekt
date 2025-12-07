
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels
{
    [TestClass]
    public sealed class TestCompanyPageViewModel
    {

        // I denne klasse laver vi unit tests for CompanyPageViewModel
        // VI har: Test af constructor, CreateCompany, DeleteSelectedCompany, EditSelectedCompany, SaveSelectedCompany


        // Et mock objekt som skal efterligne vores CompanyRepository
        private Mock<ICompanyRepository> companyRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            // Opretter et nyt mock objekt før hver test
            companyRepositoryMock = new Mock<ICompanyRepository>();
        }

        [TestMethod]
        public void Constructor_LoadsCompaniesToObservableCollection()
        {
            // Arrange
            // Fortæller mocken hvad den skal returnere, når GetAllCompanies() bliver kaldt
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>
                {
                    new _2SemesterEksamensProjekt.Models.Company { CompanyId = 1, CompanyName = "Test Company 1" },
                    new _2SemesterEksamensProjekt.Models.Company { CompanyId = 2, CompanyName = "Test Company 2" }
                });

            // Act
            // Opretter et nyt ViewModel objekt med den mocked repository
            var testingViewModel = new TestableCompanyPageViewModel(companyRepositoryMock.Object);

            // Assert
            // Tjekker at Companies-listen i ViewModel rent faktisk blev fyldt
            Assert.AreEqual(2, testingViewModel.Companies.Count);
            // Tjekker at den første virksomheds navn blev indlæst korrekt
            Assert.AreEqual("Test Company 1", testingViewModel.Companies[0].CompanyName);
        }

        [TestMethod]
        public void CreateCompany_SavesNewCompany_SavesToRepositoryAndAddsCompanyToObservableCollection()
        {
            // Arrange
            // Tom liste når ViewModel loades
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>());

            // Repository skal returnere nyt ID ved gem
            companyRepositoryMock
                .Setup(x => x.SaveNewCompany(It.IsAny<_2SemesterEksamensProjekt.Models.Company>()))
                .Returns(99);

            var testingViewModel = new TestableCompanyPageViewModel(companyRepositoryMock.Object);

            // Sætter CompanyName, så CreateCompany kan bruge det
            testingViewModel.CompanyName = "Ny Virksomhed";

            // Act
            // Kalder den rigtige metode vi skal teste
            testingViewModel.CreateCompany();

            // Assert
            // Tjekker at der er blevet tilføjet et nyt element
            Assert.AreEqual(1, testingViewModel.Companies.Count);
            Assert.AreEqual(99, testingViewModel.Companies[0].CompanyId);
            Assert.AreEqual("Ny Virksomhed", testingViewModel.Companies[0].CompanyName);

            // Tjekker at repository blev kaldt korrekt
            companyRepositoryMock.Verify(
                x => x.SaveNewCompany(
                    It.Is<_2SemesterEksamensProjekt.Models.Company>(c => c.CompanyName == "Ny Virksomhed")
                ),
                Times.Once);
        }

        [TestMethod]
        public void DeleteSelectedCompany_CompanyIsSelected_RemovesCompanyFromObservableCollectionAndRepository()
        {
            // Arrange
            // En liste med én virksomhed ved load
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>
                {
                    new _2SemesterEksamensProjekt.Models.Company { CompanyId = 1, CompanyName = "Firma A" }
                });

            // Repository skal kunne slette
            companyRepositoryMock
                .Setup(x => x.DeleteCompany(1));

            var testingViewModel = new TestableCompanyPageViewModel(companyRepositoryMock.Object);

            // Vælger elementet som SelectedCompany
            testingViewModel.SelectedCompany = testingViewModel.Companies[0];

            // Act
            testingViewModel.DeleteSelectedCompany();

            // Assert
            // Der skal ikke være nogen virksomheder tilbage
            Assert.AreEqual(0, testingViewModel.Companies.Count);

            // Repository skal være kaldt med korrekt id
            companyRepositoryMock.Verify(x => x.DeleteCompany(1), Times.Once);
        }

        [TestMethod]
        public void EditSelectedCompany_CompanyIsSelected_SetsCompanyNameToSelected()
        {
            // Arrange
            // Mock repo returnerer én virksomhed
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>
                {
                    new _2SemesterEksamensProjekt.Models.Company { CompanyId = 1, CompanyName = "Firma ABC" }
                });

            var testingViewModel = new TestableCompanyPageViewModel(companyRepositoryMock.Object);

            // Sæt SelectedCompany til den indlæste virksomhed
            testingViewModel.SelectedCompany = testingViewModel.Companies[0];

            // Act
            testingViewModel.EditSelectedCompany();

            // Assert
            // CompanyName i ViewModel skal nu matche virksomhedens navn
            Assert.AreEqual("Firma ABC", testingViewModel.CompanyName);
        }

        [TestMethod]
        public void SaveSelectedCompany_CompanyIsSelected_UpdatesRepositoryAndReloadsCompanies()
        {
            // Arrange
            // Mock repo returnerer én virksomhed oprindeligt
            companyRepositoryMock
                .Setup(x => x.GetAllCompanies())
                .Returns(new List<_2SemesterEksamensProjekt.Models.Company>
                {
                    new _2SemesterEksamensProjekt.Models.Company { CompanyId = 1, CompanyName = "Gammelt Navn" }
                });

            var testingViewModel = new TestableCompanyPageViewModel(companyRepositoryMock.Object);

            // Vi ændrer navnet via ViewModel
            testingViewModel.SelectedCompany = testingViewModel.Companies[0];
            testingViewModel.CompanyName = "Nyt Navn";

            // Act

            testingViewModel.SaveSelectedCompany();

            // Assert
            // Repository skal have modtaget rigtige data
            companyRepositoryMock.Verify(
                x => x.UpdateCompany(
                    It.Is<_2SemesterEksamensProjekt.Models.Company>(
                        c => c.CompanyId == 1 && c.CompanyName == "Nyt Navn"
                    )
                ),
                Times.Once);
        }
    }
}