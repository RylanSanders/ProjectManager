using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Entities
{
    public class TodoEntity
    {
        public TodoDO Todo { get; set; }
        //Keep in mind that this will not update the item in the DataUtil - that has to be done manually
        public ObservableCollection<SubTodoDO> SubTodos { get; set; }

        public Guid ParentProjectID { get { return Todo.ParentProjectID; } set { Todo.ParentProjectID = value; } }
        public string Name { get { return Todo.Name; } set { Todo.Name = value; } }
        public string Description { get { return Todo.Description; } set { Todo.Description = value; } }
        public string CompletedNote { get { return Todo.CompletedNote; } set { Todo.CompletedNote = value; } }
        public bool IsCompleted { get { return Todo.IsCompleted; } set { Todo.IsCompleted = value; } }
        public Guid ID { get { return Todo.ID; } set { Todo.ID = value; } }

        public DateTime EndDate { get { return Todo.EndDate;} set { Todo.EndDate = value; } }

        public TodoEntity(TodoDO dataobject)
        {
            Todo = dataobject;
            SubTodos = new ObservableCollection<SubTodoDO>(dataobject.SubTodos);
        }
    }
}
