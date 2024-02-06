using ProjectManager.DataObjects;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddNewKanbanCardContract.xaml
    /// </summary>
    public partial class AddKanbanCardContract : Window
    {
        public KanbanCardDO NewKanbanCard { get; set; }

        public ObservableCollection<ProjectDefinitionDO> Projects;
        public AddKanbanCardContract()
        {
            Projects = new ObservableCollection<ProjectDefinitionDO>(DataUtil.GetInstance().ProjectDefinitions);
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);
            ParentProjectComboBox.ItemsSource = Projects;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProjectDef = ParentProjectComboBox.SelectedItem as ProjectDefinitionDO;
            NewKanbanCard = new KanbanCardDO() { Name=NameTextBox.Text, Description=DescriptionTextBox.Text, ParentProjectID=selectedProjectDef.ID };
            DialogResult = true;
        }
    }
}
