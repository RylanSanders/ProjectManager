using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class TagDefinitionDO
    {
        public TagDefinitionDO() { ID = Guid.NewGuid(); }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ID { get; set; }
    }
}
