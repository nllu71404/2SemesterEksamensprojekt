using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using Test2SemesterEksamensProjekt.ViewModels.TestableViewModels;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels.Unittests;

[TestClass]
public class TestTopicPageViewModel
{
    // I denne klasse laver vi unit tests for TopicPageViewModel
    // Vi tester: constructor, oprettelse, redigering, gemning og sletning af emner

    // Mock-objekt der efterligner TopicRepository
    private Mock<ITopicRepository> topicRepositoryMock;

    [TestInitialize]
    public void Init()
    {
        // Arrange (fælles setup)
        // Opretter et nyt mock repository før hver test
        topicRepositoryMock = new Mock<ITopicRepository>();
    }

    [TestMethod]
    public void Constructor_LoadsTopicsToObservableCollection()
    {
        // Arrange
        // Repository returnerer to emner ved load
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                new Topic { TopicId = 1, TopicDescription = "A" },
                new Topic { TopicId = 2, TopicDescription = "B" }
            });

        // Act
        // ViewModel oprettes, hvilket indlæser emnerne
        var testingViewModel = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Assert
        // Emnerne skal være indlæst korrekt i ViewModel
        Assert.AreEqual(2, testingViewModel.Topics.Count);
        Assert.AreEqual("A", testingViewModel.Topics[0].TopicDescription);
    }

    [TestMethod]
    public void CreateTopic_SavesNewTopic_SavesToRepositoryAndAddsToObservableCollection()
    {
        // Arrange
        // Ingen emner eksisterer ved load
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>());

        // Repository returnerer nyt ID ved oprettelse
        topicRepositoryMock
            .Setup(x => x.SaveNewTopic(It.IsAny<Topic>()))
            .Returns(50);

        var testingViewModel = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Brugeren indtaster emnebeskrivelse i UI
        testingViewModel.TopicDescription = "Nyt emne";

        // Act
        // Brugeren opretter et nyt emne
        testingViewModel.CreateTopic();

        // Assert
        // Emnet skal være tilføjet ViewModel
        Assert.AreEqual(1, testingViewModel.Topics.Count);
        Assert.AreEqual(50, testingViewModel.Topics[0].TopicId);
        Assert.AreEqual("Nyt emne", testingViewModel.Topics[0].TopicDescription);

        // Repository skal være kaldt med korrekt data
        topicRepositoryMock.Verify(
            x => x.SaveNewTopic(It.Is<Topic>(t =>
                t.TopicDescription == "Nyt emne"
            )),
            Times.Once
        );
    }

    [TestMethod]
    public void DeleteSelectedTopic_TopicIsSelected_DeletesFromRepositoryAndCollection()
    {
        // Arrange
        // Repository returnerer ét eksisterende emne
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                new Topic { TopicId = 10, TopicDescription = "To delete" }
            });

        var testingViewModel = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Brugeren vælger emnet i UI
        testingViewModel.SelectedTopic = testingViewModel.Topics[0];

        // Act
        // Brugeren sletter det valgte emne
        testingViewModel.DeleteSelectedTopic();

        // Assert
        // Repository skal kaldes med korrekt ID
        topicRepositoryMock.Verify(x => x.DeleteTopic(10), Times.Once);

        // Emnet skal være fjernet fra ViewModel
        Assert.AreEqual(0, testingViewModel.Topics.Count);

        // Det valgte emne skal nulstilles
        Assert.IsNull(testingViewModel.SelectedTopic);
    }

    [TestMethod]
    public void EditSelectedTopic_TopicIsSelected_SetsTopicDescriptionToSelected()
    {
        // Arrange
        // Repository returnerer ét emne
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                new Topic { TopicId = 20, TopicDescription = "Original" }
            });

        var testingViewModel = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Brugeren vælger emnet
        testingViewModel.SelectedTopic = testingViewModel.Topics[0];

        // Act
        // Brugeren klikker "Rediger"
        testingViewModel.EditSelectedTopic();

        // Assert
        // UI-feltet skal vise emnets nuværende beskrivelse
        Assert.AreEqual("Original", testingViewModel.TopicDescription);
    }

    [TestMethod]
    public void SaveSelectedTopic_TopicIsSelected_UpdatesRepositoryAndReloadsTopics()
    {
        // Arrange
        // Repository returnerer ét eksisterende emne
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                new Topic { TopicId = 30, TopicDescription = "Olddesc" }
            });

        var testingViewModel = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Brugeren vælger emnet og ændrer beskrivelsen
        testingViewModel.SelectedTopic = testingViewModel.Topics[0];
        testingViewModel.TopicDescription = "Newdesc";

        // Act
        // Brugeren gemmer ændringerne
        testingViewModel.SaveSelectedTopic();

        // Assert
        // Repository skal opdateres med nye værdier
        topicRepositoryMock.Verify(
            x => x.UpdateTopic(It.Is<Topic>(t =>
                t.TopicId == 30 &&
                t.TopicDescription == "Newdesc"
            )),
            Times.Once
        );

        // Emnerne reloades korrekt
        Assert.AreEqual(1, testingViewModel.Topics.Count);
        Assert.AreEqual("Newdesc", testingViewModel.Topics[0].TopicDescription);

        // Inputfeltet skal nulstilles i UI
        Assert.AreEqual(string.Empty, testingViewModel.TopicDescription);
    }
}