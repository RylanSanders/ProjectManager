using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class SessionDO
    {
        public List<IntervalDO> Intervals { get; set; }
        public SessionDO()
        {
            Intervals = new List<IntervalDO>();
        }
    }
}
