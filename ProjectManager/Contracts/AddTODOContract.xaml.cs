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
    /// Interaction logic for AddTODOContract.xaml
    /// </summary>
    public partial class AddTODOContract : Window
    {
        public ObservableCollection<ProjectDefinitionDO> ProjectDefinitions { get; set; }

        public ObservableCollection<string> IntervalSelections = new ObservableCollection<string>(["Weekly", "Monthly", "Every 2 Months"]);

        public List<TodoDO> Result = new List<TodoDO>();

        public ObservableCollection<SubTodoDO> subTodoDOs = new ObservableCollection<SubTodoDO>();
        public AddTODOContract()
        {
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);

            ProjectDefinitions = new ObservableCollection<ProjectDefinitionDO>(DataUtil.GetInstance().ProjectDefinitions);
            ParentProjectComboBox.ItemsSource = ProjectDefinitions;
            ParentProjectComboBox.SelectedIndex = 0;

            IntervalComboBox.ItemsSource = IntervalSelections;
            SubTodoDataGrid.ItemsSource = subTodoDOs;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (IntervalComboBox.SelectedValue == null)
            {
                Result.Add(new TodoDO() { 
                    ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID, 
                    Name = NameTextBox.Text, 
                    Description = DescriptionTextBox.Text, 
                    ID = Guid.NewGuid(),
                    IsCompleted = false, 
                    EndDate= DueDatePicker.SelectedDate.Value });

            }else if (IntervalComboBox.SelectedValue.ToString()== "Weekly")
            {
                foreach(DateTime saturday in GetSaturdays(StartDateTimePicker.SelectedDate.Value, EndDateTimePicker.SelectedDate.Value))
                {
                    Result.Add(new TodoDO() { 
                        ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID, 
                        Name = NameTextBox.Text, 
                        Description = DescriptionTextBox.Text, 
                        ID = Guid.NewGuid(), 
                        IsCompleted = false, 
                        EndDate = saturday 
                    });
                }
            }
            else if (IntervalComboBox.SelectedValue.ToString() == "Monthly")
            {
                foreach (DateTime day in GetEndOfMonths(StartDateTimePicker.SelectedDate.Value, EndDateTimePicker.SelectedDate.Value))
                {
                    Result.Add(new TodoDO() { 
                        ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID, 
                        Name = NameTextBox.Text, 
                        Description = DescriptionTextBox.Text, 
                        ID = Guid.NewGuid(), 
                        IsCompleted = false, 
                        EndDate = day });
                }
            }
            else if (IntervalComboBox.SelectedValue.ToString() == "Every 2 Months")
            {
                foreach (DateTime day in GetLastDaysOfEveryOtherMonth(StartDateTimePicker.SelectedDate.Value, EndDateTimePicker.SelectedDate.Value))
                {
                    Result.Add(new TodoDO() { 
                        ParentProjectID = ((ProjectDefinitionDO)ParentProjectComboBox.SelectedValue).ID, 
                        Name = NameTextBox.Text, 
                        Description = DescriptionTextBox.Text, 
                        ID = Guid.NewGuid(), 
                        IsCompleted = false, 
                        EndDate = day });
                }
            }
            AssignSubTodos();
            DialogResult = true;
        }

        void AssignSubTodos()
        {
            foreach(TodoDO todo in Result)
            {
                foreach(SubTodoDO sub in subTodoDOs)
                {
                    SubTodoDO subTODO = sub.Clone();
                    subTODO.ParentTodoID = todo.ID;
                    todo.SubTodos.Add(subTODO);
                }
            }
        }

        IEnumerable<DateTime> GetSaturdays(DateTime start, DateTime end)
        {
            if (start > end)
                (start, end) = (end, start);

            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    yield return date;
            }
        }

        IEnumerable<DateTime> GetEndOfMonths(DateTime start, DateTime end)
        {
            if (start > end)
                (start, end) = (end, start);

            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (DateTime.DaysInMonth(DateTime.UtcNow.Year, date.Month) == date.Day) 
                    yield return date;
            }
        }

        IEnumerable<DateTime> GetLastDaysOfEveryOtherMonth(DateTime start, DateTime end)
        {
            if (start > end)
                (start, end) = (end, start);

            var current = new DateTime(start.Year, start.Month, 1);

            while (current <= end)
            {
                var lastDay = current.AddMonths(1).AddDays(-1);

                if (lastDay >= start && lastDay <= end)
                    yield return lastDay;

                current = current.AddMonths(2);
            }
        }

        private void DeleteSubTODOButton_Click(object sender, RoutedEventArgs e)
        {
            SubTodoDO sub = ((Button)sender).DataContext as SubTodoDO;
            if(sub != null)
            {
                subTodoDOs.Remove(sub);
            }
        }
    }
}
