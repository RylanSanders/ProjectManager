using ProjectManager.DataObjects;
using ProjectManager.Entities;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for AddNoteContract.xaml
    /// </summary>
    public partial class AddNoteContract : Window
    {
        private const int FOLDER_INDEX = 0;
        private const int NOTE_INDEX = 1;
        public NoteDO NewNoteDO { get; set; }
        public AddNoteContract()
        {
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            NewNoteDO = new NoteDO() { Name = NewNoteNameTextBox.Text, Type=(TypeComboBox.SelectedIndex==FOLDER_INDEX)?NoteDO.NoteType.Folder:NoteDO.NoteType.Note };  
            DialogResult = true;
        }
    }
}
