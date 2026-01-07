using ProjectManager.Contracts;
using ProjectManager.DataObjects;
using ProjectManager.Entities;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for TODOPage.xaml
    /// </summary>
    public partial class TODOPage : Page
    {
        public ObservableCollection<string> IntervalSelections = new ObservableCollection<string>(["All", "This Week", "2 Weeks", "Month", "2 Months", "Year"]);
        public ObservableCollection<TodoEntity> TodoDataObjects { get; set; }
        public TODOPage()
        {
            InitializeComponent();
            TodoDataObjects = new ObservableCollection<TodoEntity>();

            IntervalListBox.ItemsSource = IntervalSelections;
            TodoTreeView.ItemsSource = TodoDataObjects;

            DataUtil.GetInstance().Todos.Select(f=>new TodoEntity(f)).ToList().ForEach(f => TodoDataObjects.Add(f));
            
            TodoDataObjects.CollectionChanged += TodoDataObjects_CollectionChanged;
        }

        private void TodoDataObjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SortByColumn("EndDate", ListSortDirection.Ascending);
        }

        void SortByColumn(string propertyName, ListSortDirection direction)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(TodoTreeView.ItemsSource);

            if (view == null) return;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(
                new SortDescription(propertyName, direction)
            );

            view.Refresh();
        }

        private void AddTODOButton_Click(object sender, RoutedEventArgs e)
        {
            AddTODOContract contract = new AddTODOContract();
            if (contract.ShowDialog().GetValueOrDefault())
            {
                contract.Result.ForEach(f => DataUtil.GetInstance().Todos.Add(f));
                contract.Result.Select(f => new TodoEntity(f)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
        }

        private void DeleteTODOButton_Click(object sender, RoutedEventArgs e)
        {
            TodoEntity task = ((Button)sender).DataContext as TodoEntity;
            if (task != null)
            {
                TodoDataObjects.Remove(task);
                DataUtil.GetInstance().Todos.Remove(task.Todo);
            }

            SubTodoDO sub = ((Button)sender).DataContext as SubTodoDO;
            if(sub != null)
            {
                var localParentTODO = TodoDataObjects.FirstOrDefault(f => f.ID == sub.ParentTodoID);
                if (localParentTODO != null)
                {
                    localParentTODO.SubTodos.Remove(sub);
                }
                var parentTODO = DataUtil.GetInstance().Todos.FirstOrDefault(f => f.ID == sub.ParentTodoID);
                if (parentTODO != null)
                {
                    parentTODO.SubTodos.Remove(sub);
                }
            }
        }

        private void IntervalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0]?.ToString() == "All")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }else if(e.AddedItems[0]?.ToString() == "This Week")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().Where(f=>f.EndDate-DateTime.Today<TimeSpan.FromDays(7)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
            else if (e.AddedItems[0]?.ToString() == "2 Weeks")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().Where(f => f.EndDate - DateTime.Today < TimeSpan.FromDays(14)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
            else if (e.AddedItems[0]?.ToString() == "Month")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().Where(f => f.EndDate - DateTime.Today < TimeSpan.FromDays(30)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
            else if (e.AddedItems[0]?.ToString() == "2 Months")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().Where(f => f.EndDate - DateTime.Today < TimeSpan.FromDays(60)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
            else if (e.AddedItems[0]?.ToString() == "Year")
            {
                TodoDataObjects.Clear();
                DataUtil.GetInstance().Todos.Select(f => new TodoEntity(f)).ToList().Where(f => f.EndDate - DateTime.Today < TimeSpan.FromDays(365)).ToList().ForEach(f => TodoDataObjects.Add(f));
            }
        }
    }
}
