using ProjectManager.Contracts;
using ProjectManager.DataObjects;
using ProjectManager.Utils;
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
        public ObservableCollection<TaskItemEntity> TaskItems { get; set; }

        private TaskItemEntity _activeTask;
        public TaskItemEntity ActiveTask { get
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
            TaskItems = new ObservableCollection<TaskItemEntity>();
            InitializeComponent();



            TasksListView.ItemsSource = TaskItems;
            TasksListView.SelectionChanged += TasksListView_SelectionChanged;
            SetTimer();

            TaskUtil.Load();
            TaskUtil.GetInstance().TaskItems.ForEach(item => TaskItems.Add(new TaskItemEntity(this, item)));
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

        //TODO change this to TaskItemEntity - DOs will be for persistance and simple data - Entitites for UI operations
        public class TaskItemEntity
        {
            public TaskItemDO TaskItem { get; set; }
            public string? Name { get { return TaskItem.Name; } set { TaskItem.Name = value; } }
            public string? Description { get { return TaskItem.Description; } set { TaskItem.Description = value; } }
            public string? Type { get { return TaskItem.Type; } set { TaskItem.Type = value; } }
            public TimeSpan Duration { get
                {
                    //TODO for some reason this is wrong
                    TimeSpan sumSessions = TimeSpan.Zero;
                    TaskItem.Sessions.ForEach(session =>
                    {
                        TimeSpan sumIntervals = TimeSpan.Zero;
                        session.Intervals.ForEach(interval => sumIntervals += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                        sumSessions += sumIntervals;
                    });
                    return sumSessions;
                }
            }

            private TimerPage mainPage;

            public void AddSession(SessionDO session)
            {
                TaskItem.Sessions.Add(session);
                var collectionView = CollectionViewSource.GetDefaultView(mainPage.TasksListView.ItemsSource);
                collectionView.Refresh();
            }

            public void UpdateSessions()
            {
                var collectionView = CollectionViewSource.GetDefaultView(mainPage.TasksListView.ItemsSource);
                collectionView.Refresh();
            }

            public TaskItemEntity(TimerPage page, TaskItemDO dataObject) 
            {
                TaskItem = dataObject;
                mainPage = page;
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
            if (_timerIntervals.Count !=0)
            {
                _timerIntervals.Last().EndTime = DateTime.Now;
                _timer.Stop();
            }
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
            var selectedTask = TasksListView.SelectedItem as TaskItemEntity;
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = true;
            StopButton.IsEnabled = true;
            if (selectedTask != null)
            {
                ActiveTask = selectedTask;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskContract addTaskContract = new AddTaskContract();
            addTaskContract.ShowDialog();
            if (addTaskContract.DialogResult == true)
            {
                TaskUtil.GetInstance().TaskItems.Add(addTaskContract.ToAddTaskItem);
                TaskItems.Add(new TaskItemEntity(this, addTaskContract.ToAddTaskItem));
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskUtil.GetInstance().TaskItems.RemoveAll(task => task.ID == ActiveTask.TaskItem.ID);
            TaskItems.Remove(ActiveTask);
        }


    }
}
