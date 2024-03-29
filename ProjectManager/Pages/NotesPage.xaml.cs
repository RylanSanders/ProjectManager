﻿using Microsoft.Xaml.Behaviors;
using ProjectManager.Contracts;
using ProjectManager.DataObjects;
using ProjectManager.Entities;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            if (newNote != null && OpenNotes.Contains(newNote))
            {
                NoteTabPanel.SelectedItem = newNote;
            }
            else if (e.NewValue != null && newNote.Type==NoteDO.NoteType.Note)
            {
                TabItem newTab = new TabItem();
                newTab.Name = newNote.Name;
                OpenNotes.Add(newNote);
                NoteTabPanel.SelectedItem = newNote;
            }
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

        private Point _lastMouseDown;
        private NoteEntity DraggedNote;
        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(NoteSelectionTreeView);
            }
        }

        public void TreeViewItem_MouseDown(object sender, MouseButtonEventArgs e) {
            _lastMouseDown = e.GetPosition(NoteSelectionTreeView);
            DraggedNote = (NoteEntity)((FrameworkElement)sender).DataContext;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        public void TreeViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var note = (NoteEntity)((FrameworkElement)sender).DataContext;
            if (DraggedNote == note)
            {
                Mouse.OverrideCursor = null;
                return;
            }
            if (DraggedNote != null)
            {
                if (!ContainsChild(DraggedNote, note))
                {
                    RemoveParent(DraggedNote);
                    note.ChildrenNotes.Add(DraggedNote);
                    note.DataObject.ChildrenNotes.Add(DraggedNote.DataObject);
                    DraggedNote = null;
                }
                else
                {
                    ConfirmContract confirm = new ConfirmContract("Cannot add a Parent Note to one of its children", ConfirmContract.ConfirmCategories.Cancel);
                    confirm.ShowDialog();
                }
            }
            Mouse.OverrideCursor = null;
        }

        private bool ContainsChild(NoteEntity parentNote, NoteEntity searchingNote)
        {
            if(parentNote == searchingNote) return true;
            return parentNote.ChildrenNotes.Any(c => ContainsChild(c, searchingNote));
        }

        public void TreeView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;
            var hitTreeViewItem = NoteSelectionTreeView.FindTreeViewItems().Any(t => t.InputHitTest(e.GetPosition(t)) != null);
            if(!hitTreeViewItem)
            {
                NoteSelectionTreeView.FindTreeViewItems().ForEach(item => item.IsSelected = false);
            }
        }

        public void RemoveParent(NoteEntity note) {

            for (int i=0;i<Notes.Count;i++)
            {
                NoteEntity child = Notes[i];
                if (child == note)
                {
                    Notes.Remove(child);
                    DataUtil.GetInstance().Notes.Remove(child.DataObject);
                }
                else
                {
                    RemoveParent_Rec(child, note);
                }
                
            }
        }

        private void RemoveParent_Rec(NoteEntity parent, NoteEntity toRemove) {
            foreach (NoteEntity child in parent.ChildrenNotes)
            {
                if (child == toRemove)
                {
                    parent.ChildrenNotes.Remove(child);
                    parent.DataObject.ChildrenNotes.Remove(toRemove.DataObject);
                    return;
                }
                else
                {
                    RemoveParent_Rec(child, toRemove);
                }
            }
        }

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmContract confirmContract = new ConfirmContract("Are you sure you want to delete this Note?");
            confirmContract.ShowDialog();
            if (confirmContract.DialogResult==true)
            {
                RemoveParent((NoteEntity)((FrameworkElement)sender).DataContext);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NoteEntity newTextBox = (NoteEntity)NoteTabPanel.SelectedContent;
            newTextBox.Description = ((TextBox)e.Source).Text;

        }

        private void NoteTabPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoteSelectionTreeView.FindTreeViewItems().ForEach(item=>item.IsSelected=false);
            NoteSelectionTreeView.FindTreeViewItems().Where(item=>item.DataContext==NoteTabPanel.SelectedContent).First().IsSelected=true;
        }

    }
}
