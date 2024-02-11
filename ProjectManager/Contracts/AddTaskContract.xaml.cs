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
using static ProjectManager.TimerPage;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for AddTaskContract.xaml
    /// </summary>
    public partial class AddTaskContract : Window
    {
        public TaskItemDO ToAddTaskItem;
        public ObservableCollection<ProjectDefinitionDO> ProjectDefinitions { get; set; }
        public ObservableCollection<KanbanCardDO> KanbanCards { get; set; }
        private KanbanCardDO BlankCard;
        public AddTaskContract()
        {
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);

            ProjectDefinitions = new ObservableCollection<ProjectDefinitionDO>(DataUtil.GetInstance().ProjectDefinitions);
            ParentProjectComboBox.ItemsSource = ProjectDefinitions;
            ParentProjectComboBox.SelectedIndex = 0;

            KanbanCards = new ObservableCollection<KanbanCardDO>(DataUtil.GetInstance().KanbanCards);
            BlankCard = new KanbanCardDO();
            KanbanCards.Insert(0,BlankCard);
            AssociatedCardComoBox.ItemsSource = KanbanCards;
            AssociatedCardComoBox.SelectedIndex = 0;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var associatedCard = AssociatedCardComoBox.SelectedValue as KanbanCardDO;
            if (associatedCard == BlankCard)
            {
                ToAddTaskItem = new TaskItemDO() { Name = NameTextBox.Text, Description = DescriptionTextBox.Text, ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID };
            }
            else
            {
                ToAddTaskItem = new TaskItemDO() { Name = NameTextBox.Text, Description = DescriptionTextBox.Text, ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID, AssociatedKanbanCardID=associatedCard.ID };
            }
            
            this.DialogResult = true;
        }
    }
}
