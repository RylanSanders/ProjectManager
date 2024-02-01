using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class DateDO
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IntervalDO? Interval { get; set; }

        public DateDO()
        {
            ID = Guid.NewGuid();
        }
    }
}
