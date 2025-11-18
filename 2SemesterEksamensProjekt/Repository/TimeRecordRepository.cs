using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt.Repository
{
    public class TimeRecordRepository : BaseRepository
    {
        public List<TimeRecord> GetAllTimers()
        {
            return ExecuteSafe(conn =>
            {
                var timers = new List<TimeRecord>();

                using var cmd = new SqlCommand(
                        "SKRIV VIEW QUERY IND", conn);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    timers.Add(new TimeRecord
                    {
                        TimerId = reader.GetInt32(0),
                        TimerName = reader.GetString(1),
                        ElapsedTime = TimeSpan.FromSeconds(reader.GetInt32(2)),
                        StartTime = reader.GetDateTime(3),
                        CompanyId = reader.GetInt32(4),
                        ProjectId = reader.GetInt32(5),
                        TopicId = reader.GetInt32(6),
                    });
                }
                return timers;
            });
        }
        public List<TimeRecord> GetTimersByTimerId(int timerId)
        {
            return ExecuteSafe(conn =>
            {
                var timers = new List<TimeRecord>();
                using var cmd = new SqlCommand("SKRIV STORED PROCEDURES", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SKRIV STORED PROCEDURES", timerId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    timers.Add(new TimeRecord
                    {
                        TimerId = reader.GetInt32(0),
                        TimerName = reader.GetString(1),
                        ElapsedTime = TimeSpan.FromSeconds(reader.GetInt32(2)),
                        StartTime = reader.GetDateTime(3),
                        CompanyId = reader.GetInt32(4),
                        ProjectId = reader.GetInt32(5),
                        TopicId = reader.GetInt32(6),
                    });
                }

                return timers;
            });
        }
        public int SaveNewProject(TimeRecord timer)
        {
            return ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("INDSÆT STORED PROCEDURE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TimerId", timer.TimerId);
                cmd.Parameters.AddWithValue("@TimerName", timer.TimerName);
                cmd.Parameters.AddWithValue("@ElapsedTime", timer.ElapsedTime);
                cmd.Parameters.AddWithValue("@StartTime", timer.StartTime);
                cmd.Parameters.AddWithValue("@CompanyId", timer.CompanyId);
                cmd.Parameters.AddWithValue("@ProjectId", timer.ProjectId);
                cmd.Parameters.AddWithValue("@TopicId", timer.TopicId);

                var newId = cmd.ExecuteScalar();
                return Convert.ToInt32(newId);
            });
        }
    }
}
