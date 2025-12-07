using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using Moq;

namespace Test2SemesterEksamensProjekt.ViewModels;

[TestClass]
public class TestTopicPageViewModel
{
    // I denne klasse laver vi unit tests for TopicPageViewModel
    // Vi har: Test af constructor, CreateTopic, DeleteSelectedTopic, EditSelectedTopic, SaveSelectedTopic



    private Mock<ITopicRepository> topicRepositoryMock;

    [TestInitialize]
    public void Init()
    {
        topicRepositoryMock = new Mock<ITopicRepository>();
    }

    [TestMethod]
    public void Constructor_LoadsTopicsToObservableCollection()
    {
        // Arrange
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                    new Topic { TopicId = 1, TopicDescription = "A" },
                    new Topic { TopicId = 2, TopicDescription = "B" }
            });

        // Act
        var vm = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        // Assert
        Assert.AreEqual(2, vm.Topics.Count);
        Assert.AreEqual("A", vm.Topics[0].TopicDescription);
    }

    [TestMethod]
    public void CreateTopic_SavesNewTopic_SavesToRepositoryAndAddsToObservableCollection()
    {
        // Arrange
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>());

        topicRepositoryMock
            .Setup(x => x.SaveNewTopic(It.IsAny<Topic>()))
            .Returns(50);

        var vm = new TestableTopicPageViewModel(topicRepositoryMock.Object);
        vm.TopicDescription = "Nyt emne";

        // Act
        vm.CreateTopic();

        // Assert
        Assert.AreEqual(1, vm.Topics.Count);
        Assert.AreEqual(50, vm.Topics[0].TopicId);
        Assert.AreEqual("Nyt emne", vm.Topics[0].TopicDescription);

        topicRepositoryMock.Verify(
            x => x.SaveNewTopic(It.Is<Topic>(t =>
                t.TopicDescription == "Nyt emne"
            )),
            Times.Once);
    }

    [TestMethod]
    public void DeleteSelectedTopic_TopicIsSelected_DeletesFromRepositoryAndCollection()
    {
        // Arrange
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                    new Topic { TopicId = 10, TopicDescription = "To delete" }
            });

        var vm = new TestableTopicPageViewModel(topicRepositoryMock.Object);
        vm.SelectedTopic = vm.Topics[0];

        // Act
        vm.DeleteSelectedTopic();

        // Assert
        topicRepositoryMock.Verify(x => x.DeleteTopic(10), Times.Once);
        Assert.AreEqual(0, vm.Topics.Count);
        Assert.IsNull(vm.SelectedTopic);
    }

    [TestMethod]
    public void EditSelectedTopic_TopicIsSelected_SetsTopicDescriptionToSelected()
    {
        // Arrange
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                    new Topic { TopicId = 20, TopicDescription = "Original" }
            });

        var vm = new TestableTopicPageViewModel(topicRepositoryMock.Object);
        vm.SelectedTopic = vm.Topics[0];

        // Act
        vm.EditSelectedTopic();

        // Assert
        Assert.AreEqual("Original", vm.TopicDescription);
    }

    [TestMethod]
    public void SaveSelectedTopic_TopicIsSelected_UpdatesRepositoryAndReloadsTopics()
    {
        // Arrange
        topicRepositoryMock
            .Setup(x => x.GetAllTopics())
            .Returns(new List<Topic>
            {
                    new Topic { TopicId = 30, TopicDescription = "Olddesc" }
            });

        var vm = new TestableTopicPageViewModel(topicRepositoryMock.Object);

        vm.SelectedTopic = vm.Topics[0];
        vm.TopicDescription = "Newdesc";

        // Act
        vm.SaveSelectedTopic();

        // Assert
        topicRepositoryMock.Verify(x => x.UpdateTopic(
            It.Is<Topic>(t =>
                t.TopicId == 30 &&
                t.TopicDescription == "Newdesc"
            )
        ), Times.Once);

        Assert.AreEqual(1, vm.Topics.Count);
        Assert.AreEqual("Newdesc", vm.Topics[0].TopicDescription);

        Assert.AreEqual(string.Empty, vm.TopicDescription);
    }
}