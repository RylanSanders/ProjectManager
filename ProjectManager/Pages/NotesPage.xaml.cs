using ProjectManager.Contracts;
using ProjectManager.DataObjects;
using ProjectManager.Entities;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Net;
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
            foreach (NoteDO n in DataUtil.GetInstance().Notes)
            {
                Notes.Add(new NoteEntity(n));
            }
        }

        private void NoteSelectionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NoteEntity newNote = (NoteEntity)e.NewValue;
            TabItem newTab = new TabItem();
            newTab.Name = newNote.Name; 
            OpenNotes.Add(newNote);

        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedValue = (NoteEntity)NoteSelectionTreeView.SelectedValue;
            string parentFolder = selectedValue != null ? selectedValue.Name : string.Empty;
            AddNoteContract addNoteContract = new AddNoteContract();
            addNoteContract.ShowDialog();
            if (addNoteContract.DialogResult == true)
            {
                if (selectedValue == null)
                {
                    Notes.Add(new NoteEntity(addNoteContract.NewNoteDO));
                    DataUtil.GetInstance().Notes.Add(addNoteContract.NewNoteDO);
                }
                else
                {
                    selectedValue.ChildrenNotes.Add(new NoteEntity(addNoteContract.NewNoteDO));
                    selectedValue.DataObject.ChildrenNotes.Add(addNoteContract.NewNoteDO);
                }
            }
        }
    }
}
