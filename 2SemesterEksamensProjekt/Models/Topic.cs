using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Topic
    {
        private string _topicDescription;
        private int _topicId;

        public string TopicDescription { get { return _topicDescription; } set { _topicDescription = value; } }
        public int TopicId { get { return _topicId; } set { _topicId = value; } }

        public Topic (string topicDescription, int topicId)
        {
            _topicDescription = topicDescription;
            _topicId = topicId;
        }
        public Topic()
        {

        }
    }
}
