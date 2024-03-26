using ProjectManager.Contracts;
using ProjectManager.DataObjects;
using ProjectManager.Entities;
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

            DataUtil.Load();
            DataUtil.GetInstance().TaskItems.ForEach(item => TaskItems.Add(new TaskItemEntity(TasksListView, item)));
            SetDailyTime();
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

                    ;
                    MainTimerCircle.Dispatcher.Invoke(() => {
                        MainTimerCircle.CurrentTime = sumIntervals;
                    });
                } catch { }};
            _timer.AutoReset = true;
            _timer.Enabled = false;
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
            //Only change the end time if it wasn't set by the pause button
            if(_timerIntervals.Last().EndTime==DateTime.MinValue)
                _timerIntervals.Last().EndTime = DateTime.Now;
            _timer.Stop();
            _timer.Enabled = false;
            MainTimerCircle.CurrentTime = TimeSpan.Zero;

            SessionDO completedSession = new SessionDO();
            completedSession.Intervals.AddRange(_timerIntervals);
            ActiveTask.AddSession(completedSession);
            _timerIntervals.Clear();

            SetDailyTime();
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
                DataUtil.GetInstance().TaskItems.Add(addTaskContract.ToAddTaskItem);
                TaskItems.Add(new TaskItemEntity(TasksListView, addTaskContract.ToAddTaskItem));
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            DataUtil.GetInstance().TaskItems.RemoveAll(task => task.ID == ActiveTask.TaskItem.ID);
            TaskItems.Remove(ActiveTask);
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            TaskItemEntity task = ((Button)sender).DataContext as TaskItemEntity;
            if (task != null) {
                TaskItems.Remove(task);
                DataUtil.GetInstance().TaskItems.Remove(task.TaskItem);
                DataUtil.GetInstance().ArchivedData.Tasks.Add(task.TaskItem);
            }
            
        }

        private void SetDailyTime()
        {
            TimeSpan sumDailyTasks = TimeSpan.Zero;
            DataUtil.GetInstance().TaskItems.ForEach((task) =>
            {
                task.Sessions.ForEach(s =>
                {
                    s.Intervals.Where(i => i.StartTime.Date == DateTime.Today).ToList().ForEach(i => sumDailyTasks += i.EndTime - i.StartTime);
                });
            });
            MainTimerCircle.CurrentDailyTime = sumDailyTasks;
        }

        public void SwitchActiveTask(TaskItemEntity newTaskItem)
        {
            if (_timerIntervals.Count != 0)
            {
                StopButton_Click(null, null);
            }
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = true;
            StopButton.IsEnabled = true;
            ActiveTask = newTaskItem;
            PlayButton_Click(null, null);
        }
    }
}
