using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Entities
{
    public class TaskIntervalsEntity
    {

        public TaskIntervalsEntity(IntervalDO interval, SessionDO session) 
        {
            StartTime = interval.StartTime;
            EndTime = interval.EndTime;
            Interval = interval;
            Session = session;
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IntervalDO Interval { get; set; }

        public SessionDO Session { get; set; }
    }
}
