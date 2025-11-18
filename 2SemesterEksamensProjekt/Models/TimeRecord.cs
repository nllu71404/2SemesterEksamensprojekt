using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class TimeRecord
    {
        public string TimerName { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public bool IsRunning { get; set; }
        public DateTime StartTime { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int TopicId { get; set; }
        public int TimerId { get; set; }

        public TimeRecord(string timername, TimeSpan elapsedTime, bool isRunning, DateTime startTime, int companyId, int projectId, int topicId, int timerId)
        {
            TimerId = timerId;
            TimerName = timername;
            ElapsedTime = elapsedTime;
            IsRunning = isRunning;
            StartTime = startTime;
            CompanyId = companyId;
            ProjectId = projectId;
            TopicId = topicId;

        }
        public TimeRecord()
        {

        }
    }
}
