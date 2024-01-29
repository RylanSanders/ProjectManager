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
    /// Interaction logic for TimerPage.xaml
    /// </summary>
    public partial class TimerPage : Page
    {
        public ObservableCollection<TaskItemDO> TaskItems { get; set; }

        private TimeSpan _startTime;
        private System.Timers.Timer _timer;
        public TimerPage()
        {
            TaskItems = new ObservableCollection<TaskItemDO>();
            InitializeComponent();

            TaskItems.Add(new TaskItemDO() { Name="First", Description="Hello", Type="adsf", Duration=TimeSpan.Zero});
            TaskItems.Add(new TaskItemDO() { Name = "Second", Description = "World", Type = "bxvcb", Duration = TimeSpan.Zero });
            TaskItems.Add(new TaskItemDO() { Name = "Third", Description = "Plc", Type = "kh", Duration = TimeSpan.Zero });

            TasksListView.ItemsSource = TaskItems;
            _startTime = DateTime.Now.TimeOfDay;

            SetTimer();
            
        }

        private void SetTimer()
        {
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += (sender, args) => MainTimerCircle.Dispatcher.Invoke(() => MainTimerCircle.CurrentTime = DateTime.Now.TimeOfDay - _startTime);
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public class TaskItemDO
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Type { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}
