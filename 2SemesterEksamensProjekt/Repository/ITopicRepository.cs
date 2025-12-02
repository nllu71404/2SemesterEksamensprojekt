using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;

namespace _2SemesterEksamensProjekt.Repository
{
    public interface ITopicRepository
    {
        List<Topic> GetAllTopics();
        int SaveNewTopic(Topic topic);
        void DeleteTopic(int topicId);
        void UpdateTopic(Topic topic);


    }
}
