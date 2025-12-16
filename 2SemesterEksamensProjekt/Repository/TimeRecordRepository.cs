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
        //--Metoder--
        public List<TimeRecord> GetAllTimeRecords()
        {
            return ExecuteSafe(conn =>
            {
                var timeRecords = new List<TimeRecord>();

                using var cmd = new SqlCommand(
                        "SELECT * FROM dbo.vwSelectAllTimeRecords", conn);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    timeRecords.Add(new TimeRecord
                    {
                        TimerId = reader.GetInt32(0),
                        TimerName = reader.GetString(1),
                        ElapsedTime = reader.GetTimeSpan(2),
                        StartTime = reader.GetDateTime(3),
                        CompanyId = reader.IsDBNull(4) ? null : reader.GetInt32(4),    
                        ProjectId = reader.IsDBNull(5) ? null : reader.GetInt32(5),    
                        TopicId = reader.IsDBNull(6) ? null : reader.GetInt32(6),      
                        Note = reader.IsDBNull(7) ? null : reader.GetString(7),
                        CompanyName = reader.IsDBNull(8) ? null : reader.GetString(8),
                        ProjectTitle = reader.IsDBNull(9) ? null : reader.GetString(9),
                        TopicDescription = reader.IsDBNull(10) ? null : reader.GetString(10)
                    });
                }
                return timeRecords;
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
                        TimeRecord timeRecord = new TimeRecord();
                        timeRecord.TimerId = reader.GetInt32(0);
                        timeRecord.TimerName = reader.GetString(1);
                        timeRecord.ElapsedTime = reader.GetTimeSpan(2);
                        timeRecord.StartTime = reader.GetDateTime(3);
                        timeRecord.CompanyId = reader.IsDBNull(4) ? null : reader.GetInt32(4);
                        timeRecord.ProjectId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        timeRecord.TopicId = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                        timeRecord.Note = reader.IsDBNull(7) ? null : reader.GetString(7);
                        timeRecord.CompanyName = reader.IsDBNull(8) ? null : reader.GetString(8);
                        timeRecord.ProjectTitle = reader.IsDBNull(9) ? null : reader.GetString(9);
                        timeRecord.TopicDescription = reader.IsDBNull(10) ? null : reader.GetString(10);

                        result.Add(timeRecord);
                    }
                }
            }

            return result;
        }
        public int SaveNewTimeRecord(TimeRecord timeRecord)
        {
            return ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspCreateTimeRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TimerName", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(timeRecord.TimerName) ? throw new ArgumentException("TimerName må ikke være tom") : timeRecord.TimerName;

                cmd.Parameters.Add("@ElapsedTime", SqlDbType.Time).Value = timeRecord.ElapsedTime;

                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = timeRecord.StartTime != default ? timeRecord.StartTime : DateTime.Now;

                cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = timeRecord.ProjectId.HasValue ? timeRecord.ProjectId.Value : DBNull.Value;

                cmd.Parameters.Add("@TopicId", SqlDbType.Int).Value = timeRecord.TopicId.HasValue ? timeRecord.TopicId.Value : DBNull.Value;

                cmd.Parameters.Add("@Note", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(timeRecord.Note) ? DBNull.Value : timeRecord.Note;
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);

            });
        }
    }
}
