using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt.Repository
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        public List<Topic> GetAllTopics()
        {
            return ExecuteSafe(conn =>
            {
                var topics = new List<Topic>();

                using var cmd = new SqlCommand(
                    "SELECT TopicId, TopicDescription FROM dbo.vwSelectAllTopics;", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var topic = new Topic
                    {
                        TopicId = reader.GetInt32(0),
                        TopicDescription = reader.GetString(1)
                    };
                    topics.Add(topic);
                }
                return topics;
            });
        }

        public int SaveNewTopic(Topic topic)
        {
            return ExecuteSafe(conn =>
            {
                using (SqlCommand cmd = new SqlCommand("uspCreateTopic", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TopicDescription", topic.TopicDescription);
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            });
        }

        public void DeleteTopic(int topicId)
        {
            ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspDeleteTopic", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TopicId", topicId);
                cmd.ExecuteNonQuery();

                return true;
            });

        }
        public void UpdateTopic(Topic topic)
        {
            ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspUpdateTopic", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TopicId", topic.TopicId);
                cmd.Parameters.AddWithValue("@TopicDescription", topic.TopicDescription);

                cmd.ExecuteNonQuery();
                return true;
            });
        }
    }
}
