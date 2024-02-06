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
        public List<KanbanCardDO> KanbanCards { get; set; }
        public List<ProjectDefinitionDO> ProjectDefinitions { get; set; }
        public List<TagDefinitionDO> TagDefinitions { get; set; }
        public DataDO() { Tasks = new List<TaskItemDO>(); Dates = new List<DateDO>(); KanbanCards = new List<KanbanCardDO>(); ProjectDefinitions = new List<ProjectDefinitionDO>(); TagDefinitions = new List<TagDefinitionDO>(); }
    }
}
