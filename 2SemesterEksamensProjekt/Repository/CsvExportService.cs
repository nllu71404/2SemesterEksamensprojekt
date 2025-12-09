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

            var properties = typeof(T).GetProperties()
                .Where(p => selectedProperties.Contains(p.Name))
                .ToList();

            var sb = new StringBuilder();

            // Header
            sb.AppendLine(string.Join(";", properties.Select(p => p.Name)));

            // Rækker
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
                sb.AppendLine(string.Join(";", values));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
