using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class NoteDO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ID { get; set; }
        public NoteDO() { ID = Guid.NewGuid(); }
    }
}
