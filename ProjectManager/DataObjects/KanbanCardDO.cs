using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class KanbanCardDO
    {
        public KanbanCardDO()
        {
            ID = Guid.NewGuid();
            TagIDs = new List<Guid>();
        }

        public Guid ID { get; set; }
        public Guid ParentProjectID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> TagIDs { get; set; }
        public int ColumnNumber { get; set; }
    } 
}
