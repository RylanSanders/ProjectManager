using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Entities
{
    public class DailyTaskEntity
    {
        private DateTime _date;
        private TaskItemDO TaskItemDO { get; set; }

        public Guid ParentProjectID { get { return TaskItemDO.ParentProjectID; } }

        public string Name { get { return TaskItemDO.Name; } }

        public TimeSpan Duration
        {
            get
            {
                TimeSpan toRet = new TimeSpan();
                foreach (SessionDO session in TaskItemDO.Sessions)
                {
                    foreach (IntervalDO interval in session.Intervals)
                    {
                        if (interval.StartTime.Date == _date)
                        {
                            toRet += interval.EndTime - interval.StartTime;
                        }
                    }
                }
                return toRet;
            }
        }

        public DailyTaskEntity(TaskItemDO taskItem, DateTime date) 
        {
            TaskItemDO = taskItem;
            _date = date;
        }
    }
}
