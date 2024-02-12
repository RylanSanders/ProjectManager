using ProjectManager.DataObjects;
using ProjectManager.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManager.Pages
{
    /// <summary>
    /// Interaction logic for ToDoPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
        public ObservableCollection<NoteEntity> Notes;
        public ObservableCollection<NoteEntity> OpenNotes;
        public NotesPage()
        {
            InitializeComponent();
            Notes = new ObservableCollection<NoteEntity>();
            OpenNotes = new ObservableCollection<NoteEntity>();
            //I have to make an entity because I also want folder 
            //todo the folders I will add to the name something like Random/ThingsIWant
            //This would put it at folder random with the name Things I want

            BuildTree();
            NoteSelectionTreeView.ItemsSource = Notes;
            NoteTabPanel.ItemsSource = OpenNotes;
        }

        private void BuildTree()
        {
            //TODO get this from the DataUtil instead of building here
            NoteDO note = new NoteDO() {Name="Test1", Description="dfbfdhgjn" };
            NoteDO note2 = new NoteDO() { Name = "TestDir/Test2", Description = "kljxcv" };
            NoteDO note3 = new NoteDO() { Name = "TestDir/Woot/Test2", Description = "n,mfg" };
            List<NoteDO> NoteDOs = new List<NoteDO>();
            NoteDOs.Add(note);
            NoteDOs.Add(note2);
            NoteDOs.Add(note3);
            foreach (NoteDO n in NoteDOs)
            {
                //Split the Name for the dir
               string[] dirs = n.Name.Split('/');
                NoteEntity parent = null;
                for (int i = 0; i < dirs.Count() - 1; i++)
                {
                    //Create all of the dirs
                    var name = dirs[i];
                    NoteEntity existingDir = null;
                    if (parent == null)
                    {
                        existingDir = Notes.Where(no => no.Name == name).FirstOrDefault();
                    }
                    else
                    {
                        existingDir = parent.ChildrenNotes.Where(no => no.Name == name).FirstOrDefault();
                    }

                    if (existingDir != null)
                    {
                        parent = existingDir;
                    }
                    else
                    {
                        if (parent == null)
                        {
                            parent = new NoteEntity(new NoteDO() { Name = name });
                            Notes.Add(parent);
                        }
                        else
                        {
                            NoteEntity newNote = new NoteEntity(new NoteDO() { Name = name });
                            parent.ChildrenNotes.Add(newNote);
                            parent = newNote;
                        }
                    }
                }
                string realFileName = dirs[dirs.Count() - 1];
                if (parent != null)
                {
                    parent.ChildrenNotes.Add(new NoteEntity(n));
                }
                else
                {
                    Notes.Add(new NoteEntity(n));
                }
            }
        }

        private void NoteSelectionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NoteEntity newNote = (NoteEntity)e.NewValue;
            TabItem newTab = new TabItem();
            newTab.Name = newNote.Name; 
            OpenNotes.Add(newNote);

        }
    }
}
