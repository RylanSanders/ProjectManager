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
using static ProjectManager.TimerPage;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        public ObservableCollection<DateEntity> CurrentDateDOs = new ObservableCollection<DateEntity>();
        public CalendarPage()
        {
            InitializeComponent();

            DateListView.ItemsSource = CurrentDateDOs;

            DataUtil.Load();
            DataUtil.GetInstance().Dates.Where(d => d.Interval.StartTime.Date == MainCalendar.SelectedDate).ToList().ForEach(dateDO => CurrentDateDOs.Add(new DateEntity(dateDO)));
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
            }
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentDateDOs.Clear();
            DataUtil.GetInstance().Dates.Where(d=>d.Interval.StartTime.Date == MainCalendar.SelectedDate).ToList().ForEach(dateDO => CurrentDateDOs.Add(new DateEntity(dateDO)));
        }
    }
}
