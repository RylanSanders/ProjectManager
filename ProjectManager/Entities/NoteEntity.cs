using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace ProjectManager.Entities
{
    public class NoteEntity
    {
        public NoteDO DataObject { get; set; }
        public ObservableCollection<NoteEntity> ChildrenNotes { get; set; }

        public string Name { get { return DataObject.Name; } set { DataObject.Name = value; } }
        public NoteDO.NoteType Type {get{return DataObject.Type;}}

        public string Description { get { return DataObject.Description; } set { DataObject.Description = value; } }

        private bool _isSelected = false;
        public bool IsSelected { get { return _isSelected; }set { _isSelected = value; } }
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
