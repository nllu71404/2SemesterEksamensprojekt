using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;

namespace _2SemesterEksamensProjekt.Repository
{
    public class TopicRepository : BaseRepository
    {
        public List<Topic> GetAllTopics()
        {
            return ExecuteSafe(conn =>
            {
                var topics = new List<Topic>();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT TopicId, TopicDescription FROM Topic", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var topic = new Topic
                    {
                        TopicId = reader.GetInt32(0),
                        TopicDescription = reader.IsDBNull(1) ? "" : reader.GetString(1)
                    };
                }
                return topics;
            }) ?? new List<Topic>();
        }
    }
}
