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
        public NoteDO DataObject { get; set; }
        public ObservableCollection<NoteEntity> ChildrenNotes { get; set; }

        public string Name { get { return DataObject.Name; } set { DataObject.Name = value; } }

        public string Description { get { return DataObject.Description; } set { DataObject.Description = value; } }
        public NoteEntity(NoteDO note) 
        {
            DataObject = note;
            ChildrenNotes = new ObservableCollection<NoteEntity>();
            foreach (NoteDO child in note.ChildrenNotes)
            {
                ChildrenNotes.Add(new NoteEntity(child));
            }
        }
    }
}
