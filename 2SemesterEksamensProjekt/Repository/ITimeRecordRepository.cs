using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;

namespace _2SemesterEksamensProjekt.Repository
{
    public interface ITimeRecordRepository
    {
        List<TimeRecord> GetAllTimeRecords();
        List<TimeRecord> GetTimeRecordByFilter(int? companyId, int? projectId, int? topicId, int? month, int? year);
        int SaveNewTimeRecord(TimeRecord timer);
    }
}
