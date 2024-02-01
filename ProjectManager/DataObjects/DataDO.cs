using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class DataDO
    {
        public List<TaskItemDO> Tasks { get; set; }
        public List<DateDO> Dates { get; set; }

        public DataDO() { Tasks = new List<TaskItemDO>(); Dates = new List<DateDO>(); }
    }
}
