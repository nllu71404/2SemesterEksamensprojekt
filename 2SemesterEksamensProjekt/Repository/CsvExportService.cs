using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;

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
            if (data == null || !data.Any())
                return;

            var type = typeof(T);

            var properties = type.GetProperties()
                .Where(p => selectedProperties.Contains(p.Name))
                .ToList();

            var elapsedTimeProp = type.GetProperty("ElapsedTime");
            var companyProp = type.GetProperty("CompanyId");
            var projectProp = type.GetProperty("ProjectId");
            var subjectProp = type.GetProperty("TopicId");


            //if (elapsedTimeProp == null || companyProp == null || projectProp == null || subjectProp == null)
            //    throw new Exception("Properties Hours, Project, Subject must exist.");

            // --------------------------------------------------------
            // 1) Udregn total time pr. virksomhed
            // --------------------------------------------------------
            var byCompany = data
                .GroupBy(x => companyProp.GetValue(x))
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
                        sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
                );

            // --------------------------------------------------------
            // 2) Udregn total time pr. projekt
            // --------------------------------------------------------
            var byProject = data
                .GroupBy(x => projectProp.GetValue(x))
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
                        sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
                );

            // --------------------------------------------------------
            // 2) Udregn total time pr. emne
            // --------------------------------------------------------
            var bySubject = data
                .GroupBy(x => subjectProp.GetValue(x))
                .ToDictionary(
                    g => g.Key,
                    g => g.Aggregate(TimeSpan.Zero, (sum, item) =>
                        sum + GetHoursAsTimeSpan(elapsedTimeProp.GetValue(item)))
                );


            var sb = new StringBuilder();


            // Header1
            sb.AppendLine(string.Join(";", properties.Select(p => p.Name)));


            // Rækker
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
                sb.AppendLine(string.Join(";", values));
            }


            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);


            // Header2
            sb.AppendLine(string.Join(";", "TotalHoursForCompany"/*, "TotalHoursForProject", "TotalHoursForTopic"*/));


            // Rækker
            foreach (var item in data)
            {
                var company = companyProp.GetValue(item);
                var project = projectProp.GetValue(item);
                var subject = subjectProp.GetValue(item);

               

                var row = new[]
                {
                    byCompany[company].ToString(@"hh\:mm"),
                    //byProject[project].ToString(@"hh\:mm"),
                    //bySubject[subject].ToString(@"hh\:mm")

                    };
                sb.AppendLine(string.Join(";", row));
            }


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
