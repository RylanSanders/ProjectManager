using ProjectManager.Contracts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ProjectManager.TimerPage;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        public ObservableCollection<DateEntity> CurrentDateDOs = new ObservableCollection<DateEntity>();
        public ObservableCollection<DailyTaskEntity> CurrentDateTasks = new ObservableCollection<DailyTaskEntity>();
        public CalendarPage()
        {
            InitializeComponent();

            DateListView.ItemsSource = CurrentDateDOs;
            DailyTaskDataGrid.ItemsSource = CurrentDateTasks;
            MainCalendar.SelectedDate = DateTime.Today;
            DataUtil.Load();
            DataUtil.GetInstance().Dates.Where(d => d.Interval.StartTime.Date == MainCalendar.SelectedDate).ToList().ForEach(dateDO => CurrentDateDOs.Add(new DateEntity(dateDO)));

            RefreshDailyTasks();
        }

        public class DateEntity
        {
            public DateEntity(DateDO dateDO)
            {
                DateDataObject = dateDO;
            }
            public DateDO DateDataObject { get; set; }
            public string? Name { get { return DateDataObject.Name; } set { DateDataObject.Name = value; } }
            public string? Description { get { return DateDataObject.Description; } set { DateDataObject.Description = value; } }
        }

        private void AddDateButton_Click(object sender, RoutedEventArgs e)
        {
            AddDateContract addDateContract = new AddDateContract();
            addDateContract.ShowDialog();
            if (addDateContract.DialogResult == true)
            {
                var newDateDO = addDateContract.ToAddDateDO;
                DateTime date = MainCalendar.SelectedDate ?? DateTime.Today;
                newDateDO.Interval.StartTime = date + newDateDO.Interval.StartTime.TimeOfDay;
                newDateDO.Interval.EndTime = date + newDateDO.Interval.EndTime.TimeOfDay;
                DataUtil.GetInstance().Dates.Add(newDateDO);
                CurrentDateDOs.Add(new DateEntity(newDateDO));
                MainCalendar.InvalidateVisual();
            }
        }

        private void RefreshDailyTasks()
        {
            CurrentDateTasks.Clear();
            DataUtil.GetInstance().TaskItems.Select(ti => new DailyTaskEntity(ti, MainCalendar.SelectedDate.Value)).Where(dt => dt.Duration.TotalSeconds > 0).ToList().ForEach(dt => CurrentDateTasks.Add(dt));
            DataUtil.GetInstance().ArchivedData.Tasks.Select(ti => new DailyTaskEntity(ti, MainCalendar.SelectedDate.Value)).Where(dt => dt.Duration.TotalSeconds > 0).ToList().ForEach(dt => CurrentDateTasks.Add(dt));
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentDateDOs.Clear();
            DataUtil.GetInstance().Dates.Where(d=>d.Interval.StartTime.Date == MainCalendar.SelectedDate).ToList().ForEach(dateDO => CurrentDateDOs.Add(new DateEntity(dateDO)));
            RefreshDailyTasks();
        }
    }
}
