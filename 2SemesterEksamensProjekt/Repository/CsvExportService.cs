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
        //private readonly ICompanyRepository _companyRepo;
        //private readonly IProjectRepository _projectRepo;
        //private readonly ITopicRepository _topicRepo;


        public CsvExportService(ITimeRecordRepository timeRecordRepo/*, ICompanyRepository companyRepo, IProjectRepository projectRepo, ITopicRepository topicRepo*/)
        {
            _timeRecordRepo = timeRecordRepo;
            //_companyRepo = companyRepo;
            //_projectRepo = projectRepo;
            //_topicRepo = topicRepo;
        }

        //public void ExportTimeRecords<T>(IEnumerable<T> data, IEnumerable<T> companydata, string filePath, params string[] selectedProperties)
        //{
        //    if (data == null || !data.Any())
        //        return;

        //    var type = typeof(T);

        //    var properties = type.GetProperties()
        //        .Where(p => selectedProperties.Contains(p.Name))
        //        .ToList();

        //    var elapsedTimeProp = type.GetProperty("ElapsedTime");
        //    var companyProp = type.GetProperty("CompanyId");
        //    var projectProp = type.GetProperty("ProjectId");
        //    var subjectProp = type.GetProperty("TopicId");


        //    //if (elapsedTimeProp == null || companyProp == null || projectProp == null || subjectProp == null)
        //    //    throw new Exception("Properties Hours, Project, Subject must exist.");

        //    // --------------------------------------------------------
        //    // 1) Udregn total time pr. virksomhed
        //    // --------------------------------------------------------
        //    var byCompany = data
        //        .GroupBy(x => companyProp.GetValue(x))
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
        //                sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
        //        );

        //    // --------------------------------------------------------
        //    // 2) Udregn total time pr. projekt
        //    // --------------------------------------------------------
        //    var byProject = data
        //        .GroupBy(x => projectProp.GetValue(x))
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
        //                sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
        //        );

        //    // --------------------------------------------------------
        //    // 2) Udregn total time pr. emne
        //    // --------------------------------------------------------
        //    var bySubject = data
        //        .GroupBy(x => subjectProp.GetValue(x))
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
        //                sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
        //        );


        //    var sb = new StringBuilder();


        //    // Header1
        //    sb.AppendLine(string.Join(";", properties.Select(p => p.Name)));


        //    // Rækker
        //    foreach (var item in data)
        //    {
        //        var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
        //        sb.AppendLine(string.Join(";", values));
        //    }


        //    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);


        //    // Header2
        //    sb.AppendLine(string.Join(";", "TotalHoursForCompany"/*, "TotalHoursForProject", "TotalHoursForTopic"*/));


        //    // Rækker
        //    foreach (var item in companydata)
        //    {
        //        var company = companyProp.GetValue(item);
        //        //var project = projectProp.GetValue(item);
        //        //var subject = subjectProp.GetValue(item);



        //        var row = new[]
        //        {
        //            byCompany[company].ToString(@"hh\:mm"),
        //            //byProject[project].ToString(@"hh\:mm"),
        //            //bySubject[subject].ToString(@"hh\:mm")

        //            };
        //        sb.AppendLine(string.Join(";", row));
        //    }


        //    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        //}

        public void ExportTimeRecords<T>(IEnumerable<T> data, string filePath, params string[] selectedProperties)
        {
            if (!data.Any())
                return;

            var type = typeof(T);
            var hoursProp = type.GetProperty("ElapsedTime");
            var companyNameProp = type.GetProperty("CompanyName");
            var projectTitleProp = type.GetProperty("ProjectTitle");
            var topicDescProp = type.GetProperty("TopicDescription");

            if (hoursProp == null || companyNameProp == null || projectTitleProp == null || topicDescProp == null)
                throw new Exception("ElapsedTime, CompanyName, ProjectTitle, TopicDescription must exist.");

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
            sb.AppendLine("CompanyName;ProjectTitle;TopicDescription;ElapsedTime");

            foreach (var item in data)
            {
                var hours = ToTimeSpan(hoursProp.GetValue(item));
                var company = companyNameProp.GetValue(item)?.ToString() ?? "";
                var project = projectTitleProp.GetValue(item)?.ToString() ?? "";
                var topic = topicDescProp.GetValue(item)?.ToString() ?? "";

                sb.AppendLine($"{company};{project};{topic};{hours:hh\\:mm}");
            }

            // Company totals
            sb.AppendLine();
            sb.AppendLine("COMPANY TOTALS");
            sb.AppendLine("CompanyName;TotalHours");
            foreach (var c in byCompany)
                sb.AppendLine($"{c.Company};{c.Total:hh\\:mm}");

            // Project totals
            sb.AppendLine();
            sb.AppendLine("PROJECT TOTALS");
            sb.AppendLine("ProjectTitle;TotalHours");
            foreach (var p in byProject)
                sb.AppendLine($"{p.Project};{p.Total:hh\\:mm}");

            // Topic totals
            sb.AppendLine();
            sb.AppendLine("TOPIC TOTALS");
            sb.AppendLine("TopicDescription;TotalHours");
            foreach (var t in byTopic)
                sb.AppendLine($"{t.Topic};{t.Total:hh\\:mm}");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }


        private TimeSpan GetHoursAsTimeSpan(object value)
        {
            if (value is TimeSpan ts)
                return ts;

            if (value is double d)
                return TimeSpan.FromHours(d);

            if (value is string s)
                return TimeSpan.Parse(s);

            throw new InvalidCastException("Unknown Hours format.");
        }
    }
}
