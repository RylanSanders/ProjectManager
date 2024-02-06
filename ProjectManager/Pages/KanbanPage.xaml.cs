using MaterialDesignThemes.Wpf;
using ProjectManager.Contracts;
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
        public KanbanPage()
        {
            Column1Cards = new ObservableCollection<KanbanCardDO>();
            Column2Cards = new ObservableCollection<KanbanCardDO>();
            Column3Cards = new ObservableCollection<KanbanCardDO>();
            Column4Cards = new ObservableCollection<KanbanCardDO>();
            InitializeComponent();

            Column1ListView.ItemsSource = Column1Cards;
            Column2ListView.ItemsSource = Column2Cards;
            Column3ListView.ItemsSource = Column3Cards;
            Column4ListView.ItemsSource = Column4Cards;

            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 0).ToList().ForEach(c => Column1Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 1).ToList().ForEach(c => Column2Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 2).ToList().ForEach(c => Column3Cards.Add(c));
            DataUtil.GetInstance().KanbanCards.Where(c => c.ColumnNumber == 3).ToList().ForEach(c => Column4Cards.Add(c));
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectContract addProjectContract = new AddProjectContract();
            addProjectContract.ShowDialog();
            if (addProjectContract.DialogResult == true)
            {
                DataUtil.GetInstance().ProjectDefinitions.Add(addProjectContract.NewProjectDefinition);
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
    }
}
