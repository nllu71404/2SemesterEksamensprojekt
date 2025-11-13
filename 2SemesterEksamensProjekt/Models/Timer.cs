using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Timer
    {
        public string Name;
        public TimeSpan ElapsedTime;
        public bool IsRunning;
        public DateTime StartTime;

        public Timer(string name, TimeSpan elapsedTime, bool isRunning, DateTime startTime  )
        {
            Name = name;
            ElapsedTime = elapsedTime;
            IsRunning = isRunning;
            StartTime = startTime;
        }
        public Timer() //Parameterløs
        {

        }
    }
}
