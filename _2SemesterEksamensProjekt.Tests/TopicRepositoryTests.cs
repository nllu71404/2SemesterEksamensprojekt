using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;

namespace _2SemesterEksamensProjekt.Tests
{
    [TestClass]
    public class TopicRepositoryTests
    {
        private TopicRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new TopicRepository();
        }

        [TestMethod]
        public void SaveNewTopic_ShouldInsertAndReturnNewId()
        {
            // Arrange
            var newTopic = new Topic { TopicDescription = "MSTest topic" };

            // Act
            int newId = _repo.SaveNewTopic(newTopic);

            // Assert
            Assert.IsTrue(newId > 0, "SaveNewTopic should return a valid new ID");

            // Cleanup
            //_repo.DeleteTopic(newId);
        }
        
        [TestMethod]
        public void GetAllTopics_ShouldReturnListOfTopics()
        {
            // Act
            var result = _repo.GetAllTopics();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Count >= 0, "Result should return a list (possibly empty)");
        }

        

        [TestMethod]
        public void UpdateTopic_ShouldChangeDescription()
        {
            // Arrange
            var topic = new Topic { TopicDescription = "Initial topic" };
            int id = _repo.SaveNewTopic(topic);

            topic.TopicId = id;
            topic.TopicDescription = "Updated topic";

            // Act
            _repo.UpdateTopic(topic);

            // Assert
            var topics = _repo.GetAllTopics();
            var updated = topics.Find(t => t.TopicId == id);

            Assert.IsNotNull(updated, "Updated topic should exist in database");
            Assert.AreEqual("Updated topic", updated.TopicDescription, "Description should be updated");

            // Cleanup
            _repo.DeleteTopic(id);
        }

        [TestMethod]
        public void DeleteTopic_ShouldRemoveTopicFromDatabase()
        {
            // Arrange
            var topic = new Topic { TopicDescription = "Topic to delete" };
            int id = _repo.SaveNewTopic(topic);

            // Act
            _repo.DeleteTopic(id);
            var topics = _repo.GetAllTopics();

            // Assert
            Assert.IsFalse(topics.Exists(t => t.TopicId == id), "Deleted topic should not exist in database");
        }
    }
}

