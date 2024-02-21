using ProjectManager.DataObjects;
using ProjectManager.Entities;
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
    /// Interaction logic for KanbanCardEditContract.xaml
    /// </summary>
    public partial class EditKanbanCardContract : Window
    {
        public ObservableCollection<ProjectDefinitionDO> Projects { get; set; }
        public ObservableCollection<TaskItemEntity> TaskItems { get; set; }
        public KanbanCardDO EditedKanbanCard { get; set; }
        private KanbanCardDO ExistingKanbanCard;
        public EditKanbanCardContract(KanbanCardDO kanbanCard)
        {
            ExistingKanbanCard = kanbanCard;
            TaskItems = new ObservableCollection<TaskItemEntity>();
            Projects = new ObservableCollection<ProjectDefinitionDO>();
            DataUtil.GetInstance().ProjectDefinitions.ForEach(d => Projects.Add(d));
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);

            ProjectComboBox.ItemsSource = Projects;
            NameTextBox.Text = kanbanCard.Name;
            var currentProject = Projects.Where(p=>p.ID== kanbanCard.ParentProjectID).FirstOrDefault();
            ProjectComboBox.SelectedIndex = currentProject!=null?Projects.IndexOf(currentProject):0;
            DescriptionTextbox.AppendText(kanbanCard.Description);

            TasksListView.ItemsSource = TaskItems;
            
            DataUtil.GetInstance().TaskItems
                .Where(t=>t.AssociatedKanbanCardID==kanbanCard.ID)
                .ToList()
                .ForEach(item => TaskItems.Add(new TaskItemEntity(TasksListView, item)));
            DataUtil.GetInstance().ArchivedData.Tasks
                .Where(t => t.AssociatedKanbanCardID == kanbanCard.ID)
                .ToList()
                .ForEach(item => TaskItems.Add(new TaskItemEntity(TasksListView, item)));

        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            EditedKanbanCard = new KanbanCardDO() {ID=ExistingKanbanCard.ID, 
                ColumnNumber=ExistingKanbanCard.ColumnNumber,
             Description=new TextRange(DescriptionTextbox.Document.ContentStart, DescriptionTextbox.Document.ContentEnd).Text,
            Name = NameTextBox.Text,
            ParentProjectID = ((ProjectDefinitionDO)ProjectComboBox.SelectedValue).ID};
            DialogResult = true;
        }
    }
}
