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
        List<TimeRecord> GetAllTimers();
        List<TimeRecord> GetTimersByTimerId(int timerId);
        int SaveNewTimeRecord(TimeRecord timer);
        List<TimeRecord> GetTimeRecordByCompanyId(int companyId);
        List<TimeRecord> GetProjectsByCompanyId(int companyId);
    }
}
