using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataObjects
{
    public class TodoDO
    {
       public Guid ParentProjectID { get; set; }
       public Guid ID { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletedNote { get; set; }
        public List<SubTodoDO> SubTodos { get; set; }

        public TodoDO()
        {
            ID = Guid.NewGuid();
            SubTodos = new List<SubTodoDO>();
        }
    }

    public class SubTodoDO
    {
        public Guid ID { get; set; }
        public Guid ParentTodoID { get; set; }
        public bool IsCompleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletedNote { get; set; }

        /// <summary>
        /// Note this does not assign the same parent as the original
        /// </summary>
        /// <returns></returns>
        public SubTodoDO Clone()
        {
            return new SubTodoDO() { ID = Guid.NewGuid(), IsCompleted = IsCompleted, CompletedNote=CompletedNote, Description=Description, Name=Name };
        }
    }
}
