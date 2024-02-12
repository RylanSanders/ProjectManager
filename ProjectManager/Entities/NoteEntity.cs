using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Entities
{
    public class NoteEntity
    {
        private NoteDO Data;
        public ObservableCollection<NoteEntity> ChildrenNotes { get; set; }

        public string Name { get { return Data.Name.Substring(Data.Name.LastIndexOf("/")+1); } set { Data.Description = value; } }

        public string Description { get { return Data.Description; } set { Data.Description = value; } }
        public NoteEntity(NoteDO note) 
        {
            Data = note;
            ChildrenNotes = new ObservableCollection<NoteEntity>();
        }
    }
}
