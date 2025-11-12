using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicDescription { get; set; }
        public int? ProjectId { get; set; }

        public Topic(string topicdescription, int? projectId)
        {
            
            TopicDescription = topicdescription;
            ProjectId = projectId;
        }
    }
}
