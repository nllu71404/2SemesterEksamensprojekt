using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Repository
{
    public class TimeRecordRepository : BaseRepository, ITimeRecordRepository
    {
        public List<TimeRecord> GetAllTimeRecords()
        {
            return ExecuteSafe(conn =>
            {
                var timers = new List<TimeRecord>();

                using var cmd = new SqlCommand(
                        "SELECT * FROM dbo.vwSelectAllTimeRecord", conn);

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
        public List<TimeRecord> GetTimeRecordByFilter(int? companyId, int? projectId, int? topicId, int? month, int? year)
        {
            List<TimeRecord> result = new List<TimeRecord>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("uspFilterTimeRecords", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CompanyId", (object?)companyId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProjectId", (object?)projectId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TopicId", (object?)topicId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Month", (object?)month ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Year", (object?)year ?? DBNull.Value);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TimeRecord tr = new TimeRecord();
                        tr.TimerId = reader.GetInt32(0);
                        tr.TimerName = reader.GetString(1);
                        tr.ElapsedTime = reader.GetTimeSpan(2);
                        tr.StartTime = reader.GetDateTime(3);
                        tr.CompanyId = reader.IsDBNull(4) ? null : reader.GetInt32(4);
                        tr.ProjectId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        tr.TopicId = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                        tr.Note = reader.IsDBNull(7) ? null : reader.GetString(7);

                        result.Add(tr);
                    }
                }
            }

            return result;
        }
        public int SaveNewTimeRecord(TimeRecord timer)
        {
            return ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspCreateTimeRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TimerName", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrEmpty(timer.TimerName) ? throw new ArgumentException("TimerName må ikke være tom") : timer.TimerName;

                cmd.Parameters.Add("@ElapsedTime", SqlDbType.Time).Value = timer.ElapsedTime;

                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value =
                    timer.StartTime != default ? timer.StartTime : DateTime.Now;

                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value =
                    timer.CompanyId.HasValue ? timer.CompanyId.Value : DBNull.Value;

                cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value =
                    timer.ProjectId.HasValue ? timer.ProjectId.Value : DBNull.Value;

                cmd.Parameters.Add("@TopicId", SqlDbType.Int).Value =
                    timer.TopicId.HasValue ? timer.TopicId.Value : DBNull.Value;

                cmd.Parameters.Add("@Note", SqlDbType.NVarChar).Value =
                     string.IsNullOrEmpty(timer.Note) ? DBNull.Value : timer.Note;
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);

            });
        }
    }
}
