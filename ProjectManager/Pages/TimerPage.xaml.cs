using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private TaskItemDO _activeTask;
        public TaskItemDO ActiveTask { get
            {
                return _activeTask;
            }
            set
            {
                _activeTask = value;
                MainTimerCircle.Dispatcher.Invoke(() => MainTimerCircle.ActiveTaskTextBox.Text = _activeTask.Name);
            }
        }

        private System.Timers.Timer _timer;
        private List<IntervalDO> _timerIntervals = new List<IntervalDO>();
        public TimerPage()
        {
            TaskItems = new ObservableCollection<TaskItemDO>();
            InitializeComponent();

            TaskItems.Add(new TaskItemDO(this) { Name="First", Description="Hello", Type="adsf"});
            TaskItems.Add(new TaskItemDO(this) { Name = "Second", Description = "World", Type = "bxvcb" });
            TaskItems.Add(new TaskItemDO(this) { Name = "Third", Description = "Plc", Type = "kh" });

            TasksListView.ItemsSource = TaskItems;
            TasksListView.SelectionChanged += TasksListView_SelectionChanged;
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
            public TimeSpan Duration { get
                {
                    TimeSpan sumSessions = TimeSpan.Zero;
                    _sessions.ForEach(session =>
                    {
                        TimeSpan sumIntervals = TimeSpan.Zero;
                        session.Intervals.ForEach(interval => sumIntervals += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                        sumSessions += sumIntervals;
                    });
                    return sumSessions;
                }
            }

            private TimerPage mainPage;

            //Private so that we can do things on add everytime
            private List<SessionDO> _sessions { get; set; }

            public List<SessionDO> Sessions { get { return _sessions; } }

            public void AddSession(SessionDO session)
            {
                _sessions.Add(session);
                var collectionView = CollectionViewSource.GetDefaultView(mainPage.TasksListView.ItemsSource);
                collectionView.Refresh();
            }

            //The right way to do this would be for the session and interval to also be dependency models then make the change event bubble up to the listview
            //But that is annoying
            public void UpdateSessions()
            {
                var collectionView = CollectionViewSource.GetDefaultView(mainPage.TasksListView.ItemsSource);
                collectionView.Refresh();
            }

            public TaskItemDO(TimerPage page) 
            {
                _sessions = new List<SessionDO>();
                mainPage = page;
            }
        }

        public class IntervalDO
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        public class SessionDO
        {
            public List<IntervalDO> Intervals { get; set; }

            public SessionDO()
            {
                Intervals = new List<IntervalDO>();
            }
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

            SessionDO completedSession = new SessionDO();
            completedSession.Intervals.AddRange(_timerIntervals);
            ActiveTask.AddSession(completedSession);
            _timerIntervals.Clear();
        }

        private void TasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = TasksListView.SelectedItem as TaskItemDO;
            if (selectedTask != null)
            {
                ActiveTask = selectedTask;
            }
        }
    }
}
