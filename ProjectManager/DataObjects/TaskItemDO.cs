using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class TaskItemDO
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid ParentProjectID { get; set; }
        public List<SessionDO> Sessions { get; set; }

        public TaskItemDO() { ID = Guid.NewGuid(); Sessions = new List<SessionDO>(); }
    }
}
