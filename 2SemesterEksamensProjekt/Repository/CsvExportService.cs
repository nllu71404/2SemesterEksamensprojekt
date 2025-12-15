using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using System.Printing;

namespace _2SemesterEksamensProjekt.Repository
{
    public class CsvExportService : ICsvExportService
    {

        private readonly ITimeRecordRepository _timeRecordRepo;


        public CsvExportService(ITimeRecordRepository timeRecordRepo)
        {
            _timeRecordRepo = timeRecordRepo;
            
        }

        public void ExportTimeRecords<T>(IEnumerable<T> data, string filePath, params string[] selectedProperties)
        {
            if (!data.Any())
                return;

            var type = typeof(T);
            var hoursProp = type.GetProperty("ElapsedTime");
            var startTimeProp = type.GetProperty("StartTime");
            var timerNameProp = type.GetProperty("TimerName");
            var companyNameProp = type.GetProperty("CompanyName");
            var projectTitleProp = type.GetProperty("ProjectTitle");
            var topicDescProp = type.GetProperty("TopicDescription");
            var noteProp = type.GetProperty("Note");


            // Helper til at konvertere timer
            TimeSpan ToTimeSpan(object v)
            {
                if (v is TimeSpan ts) return ts;
                if (v is double d) return TimeSpan.FromHours(d);
                if (v is string s) return TimeSpan.Parse(s);
                throw new Exception("Unknown time format");
            }

            // --- TOTALS (unikke summer) ---
            var byCompany = data
                .GroupBy(x => companyNameProp.GetValue(x)?.ToString())
                .Select(g => new
                {
                    Company = g.Key ?? "",
                    Total = g.Aggregate(TimeSpan.Zero, (sum, item) => sum + ToTimeSpan(hoursProp.GetValue(item)))
                })
                .ToList();

            var byProject = data
                .GroupBy(x => projectTitleProp.GetValue(x)?.ToString())
                .Select(g => new
                {
                    Project = g.Key ?? "",
                    Total = g.Aggregate(TimeSpan.Zero, (sum, item) => sum + ToTimeSpan(hoursProp.GetValue(item)))
                })
                .ToList();

            var byTopic = data
                .GroupBy(x => topicDescProp.GetValue(x)?.ToString())
                .Select(g => new
                {
                    Topic = g.Key ?? "",
                    Total = g.Aggregate(TimeSpan.Zero, (sum, item) => sum + ToTimeSpan(hoursProp.GetValue(item)))
                })
                .ToList();

            // --- CSV ---
            var sb = new StringBuilder();

            // 1) Original data
            sb.AppendLine("DATA");
            sb.AppendLine("Date;Timer Name;Company;Project;Topic;Note;Hours");

            foreach (var item in data)
            {
                var hours = ToTimeSpan(hoursProp.GetValue(item));
                var company = companyNameProp.GetValue(item)?.ToString() ?? "";
                var project = projectTitleProp.GetValue(item)?.ToString() ?? "";
                var topic = topicDescProp.GetValue(item)?.ToString() ?? "";
                var startTime = startTimeProp.GetValue(item)?.ToString() ?? "";
                var timerName = timerNameProp.GetValue(item)?.ToString() ?? "";
                var note = noteProp.GetValue(item)?.ToString() ?? "";


                sb.AppendLine($"{startTime};{timerName};{company};{project};{topic};{note};{hours:hh\\:mm}");
            }

            // Company totals
            sb.AppendLine();
            sb.AppendLine("COMPANY TOTALS");
            sb.AppendLine("Company;Total Hours");
            foreach (var c in byCompany)
                sb.AppendLine($"{c.Company};{c.Total:hh\\:mm}");

            // Project totals
            sb.AppendLine();
            sb.AppendLine("PROJECT TOTALS");
            sb.AppendLine("Project;Total Hours");
            foreach (var p in byProject)
                sb.AppendLine($"{p.Project};{p.Total:hh\\:mm}");

            // Topic totals
            sb.AppendLine();
            sb.AppendLine("TOPIC TOTALS");
            sb.AppendLine("Topic;Total Hours");
            foreach (var t in byTopic)
                sb.AppendLine($"{t.Topic};{t.Total:hh\\:mm}");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

    }
}
