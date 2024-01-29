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

        private System.Timers.Timer _timer;
        private List<IntervalDO> _timerIntervals = new List<IntervalDO>();
        public TimerPage()
        {
            TaskItems = new ObservableCollection<TaskItemDO>();
            InitializeComponent();

            TaskItems.Add(new TaskItemDO() { Name="First", Description="Hello", Type="adsf", Duration=TimeSpan.Zero});
            TaskItems.Add(new TaskItemDO() { Name = "Second", Description = "World", Type = "bxvcb", Duration = TimeSpan.Zero });
            TaskItems.Add(new TaskItemDO() { Name = "Third", Description = "Plc", Type = "kh", Duration = TimeSpan.Zero });

            TasksListView.ItemsSource = TaskItems;

            SetTimer();
            
        }

        private void SetTimer()
        {
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += (sender, args) => {
                //Try Catch prevents an error on close because the task is canceled.
                try
                {
                    _timerIntervals.Last().EndTime = DateTime.Now;
                    TimeSpan sumIntervals = TimeSpan.Zero;
                    _timerIntervals.ForEach(interval => sumIntervals += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                    MainTimerCircle.Dispatcher.Invoke(() => MainTimerCircle.CurrentTime = sumIntervals);
                } catch { }};
            _timer.AutoReset = true;
            _timer.Enabled = false;
        }

        public class TaskItemDO
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Type { get; set; }
            public TimeSpan Duration { get; set; }
        }

        public class IntervalDO
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _timerIntervals.Add(new IntervalDO() { StartTime = DateTime.Now });
            _timer.Enabled = true;
            _timer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _timerIntervals.Last().EndTime = DateTime.Now;
            _timer.Stop();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _timerIntervals.Last().EndTime = DateTime.Now;
            _timer.Stop();
            _timer.Enabled = false;
            MainTimerCircle.CurrentTime = TimeSpan.Zero;

            //TODO: this is where we would cache the completed time results
            _timerIntervals.Clear();
        }
    }
}
