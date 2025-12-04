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
    public class TimeRecordRepository : BaseRepository, ITimeRecordRepository
    {
        public List<TimeRecord> GetAllTimers()
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
        public List<TimeRecord> GetTimeRecordByFilter(string month)
        {
            return ExecuteSafe(conn =>
            {
                var timers = new List<TimeRecord>();
                using var cmd = new SqlCommand("SKRIV STORED PROCEDURES", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SKRIV STORED PROCEDURES", );

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

        public List<TimeRecord> GetTimeRecordByCompanyId(int companyId) //Bruges ikke endnu 
        {
            return ExecuteSafe(conn =>
            {
                var projects = new List<TimeRecord>();
                using var cmd = new SqlCommand("STOREDPROCEDURE FOR COMPANYID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", companyId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    projects.Add(new TimeRecord
                    {
                        TimerId = reader.GetInt32(0),
                        CompanyId = reader.GetInt32(1)
                    });
                }

                return projects;
            });
        }
        public List<TimeRecord> GetProjectsByCompanyId(int companyId) //Bruges ikke endnu
        {
            return ExecuteSafe(conn =>
            {
                var projects = new List<TimeRecord>();
                using var cmd = new SqlCommand("STOREDPROCEDURE FOR COMPANYID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", companyId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    projects.Add(new TimeRecord
                    {
                        TimerId = reader.GetInt32(0),
                        CompanyId = reader.GetInt32(1)
                    });
                }

                return projects;
            });
        }
    }
}
