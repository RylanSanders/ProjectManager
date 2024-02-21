using MaterialDesignThemes.Wpf;
using ProjectManager.Contracts;
using ProjectManager.Controls;
using ProjectManager.DataObjects;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for KanbanPage.xaml
    /// </summary>
    public partial class KanbanPage : Page
    {
        public ObservableCollection<KanbanCardDO> Column1Cards { get; set; }
        public ObservableCollection<KanbanCardDO> Column2Cards { get; set; }
        public ObservableCollection<KanbanCardDO> Column3Cards { get; set; }
        public ObservableCollection<KanbanCardDO> Column4Cards { get; set; }

        public ObservableCollection<KanbanCardDO>[] ColumnCollections;
        public ObservableCollection<ProjectDefinitionDO> ProjectDefinitions { get; set; }
        private ProjectDefinitionDO BlankProject;
        private TimeSpan LastLeftMouseDown = DateTime.Now.TimeOfDay;
        public KanbanPage()
        {
            Column1Cards = new ObservableCollection<KanbanCardDO>();
            Column2Cards = new ObservableCollection<KanbanCardDO>();
            Column3Cards = new ObservableCollection<KanbanCardDO>();
            Column4Cards = new ObservableCollection<KanbanCardDO>();
            ColumnCollections = [Column1Cards, Column2Cards, Column3Cards, Column4Cards];
            InitializeComponent();

            Column1ListView.ItemsSource = Column1Cards;
            Column2ListView.ItemsSource = Column2Cards;
            Column3ListView.ItemsSource = Column3Cards;
            Column4ListView.ItemsSource = Column4Cards;

            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 0).ToList().ForEach(c => Column1Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 1).ToList().ForEach(c => Column2Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 2).ToList().ForEach(c => Column3Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 3).ToList().ForEach(c => Column4Cards.Add(c));

            ProjectDefinitions = new ObservableCollection<ProjectDefinitionDO>(DataUtil.GetInstance().ProjectDefinitions);
            BlankProject = new ProjectDefinitionDO();
            ProjectDefinitions.Insert(0,BlankProject);
            ProjectFilterComboBox.ItemsSource = ProjectDefinitions;
            ProjectFilterComboBox.SelectedIndex = 0;
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectContract addProjectContract = new AddProjectContract();
            addProjectContract.ShowDialog();
            if (addProjectContract.DialogResult == true)
            {
                DataUtil.GetInstance().ProjectDefinitions.Add(addProjectContract.NewProjectDefinition);
                ProjectDefinitions.Add(addProjectContract.NewProjectDefinition);
            }
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddKanbanCardContract addCardContract = new AddKanbanCardContract();
            addCardContract.ShowDialog();
            if (addCardContract.DialogResult == true)
            {
                DataUtil.GetInstance().KanbanCards.Add(addCardContract.NewKanbanCard);
                Column1Cards.Add(addCardContract.NewKanbanCard);
            }
        }

        private void ColumnListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is KanbanCard)
            {
                LastLeftMouseDown = DateTime.Now.TimeOfDay;
                KanbanCard draggedItem = sender as KanbanCard;
                var listViewItem = ((Border)((ContentPresenter)draggedItem.TemplatedParent).Parent).TemplatedParent as ListViewItem;
                DragDrop.DoDragDrop(draggedItem, listViewItem.DataContext, DragDropEffects.Move);
                listViewItem.IsSelected = true;
            }
        }

        private void ColumnListView_Drop(object sender, DragEventArgs e)
        {
            if (DateTime.Now.TimeOfDay-LastLeftMouseDown > TimeSpan.FromMilliseconds(250))
            {
                var data = e.Data.GetData(typeof(KanbanCardDO)) as KanbanCardDO;
                var newIndex = int.Parse(((FrameworkElement)sender).Tag.ToString());
                ColumnCollections[data.ColumnNumber].Remove(data);
                data.ColumnNumber = newIndex;
                ColumnCollections[data.ColumnNumber].Add(data);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            if (senderButton != null)
            {
                var card = senderButton.DataContext as KanbanCardDO;
                if (card != null)
                {
                    ConfirmContract confirm = new ConfirmContract($"Are you sure you want to delete {card.Name}?", ConfirmContract.ConfirmCategories.ConfirmCancel);
                    confirm.ShowDialog();
                    if (confirm.DialogResult==true)
                    {
                        ColumnCollections[card.ColumnNumber].Remove(card);
                        DataUtil.GetInstance().KanbanCards.Remove(card);
                    }
                }
            }
        }

        private void ProjectFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var allCards = DataUtil.GetInstance().KanbanCards;
            Guid parentProjectID = ((ProjectDefinitionDO)ProjectFilterComboBox.SelectedValue).ID;
            if (parentProjectID == BlankProject.ID) { AddAllCards();return; }
            Column1Cards.Clear();
            Column2Cards.Clear();
            Column3Cards.Clear();
            Column4Cards.Clear();

            allCards.Where(card => card.ColumnNumber == 0 && card.ParentProjectID == parentProjectID).ToList().ForEach(c=>Column1Cards.Add(c));
            allCards.Where(card => card.ColumnNumber == 1 && card.ParentProjectID == parentProjectID).ToList().ForEach(c => Column2Cards.Add(c));
            allCards.Where(card => card.ColumnNumber == 2 && card.ParentProjectID == parentProjectID).ToList().ForEach(c => Column3Cards.Add(c));
            allCards.Where(card => card.ColumnNumber == 3 && card.ParentProjectID == parentProjectID).ToList().ForEach(c => Column4Cards.Add(c));
        }

        private void AddAllCards()
        {
            Column1Cards.Clear();
            Column2Cards.Clear();
            Column3Cards.Clear();
            Column4Cards.Clear();

            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 0).ToList().ForEach(c => Column1Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 1).ToList().ForEach(c => Column2Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 2).ToList().ForEach(c => Column3Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 3).ToList().ForEach(c => Column4Cards.Add(c));
        }
    }
}
