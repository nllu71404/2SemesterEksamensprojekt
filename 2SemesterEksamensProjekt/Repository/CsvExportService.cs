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

        public void ExportTimeRecords(string filePath)
        {
            var timeRecords = _timeRecordRepo.GetAllTimeRecords();

            var sb = new StringBuilder();
            sb.AppendLine("TimerId,TimerName,ElapsedTime,StartTime,CompanyId,ProjectId,TopicId,Note");

            foreach (var t in timeRecords)
            {
                sb.AppendLine($"{t.TimerId},{t.TimerName},{t.ElapsedTime},{t.StartTime},{t.CompanyId},{t.ProjectId},{t.TopicId},{t.Note}");
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
