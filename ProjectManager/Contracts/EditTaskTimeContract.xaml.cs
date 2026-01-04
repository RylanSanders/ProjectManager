using ProjectManager.DataObjects;
using ProjectManager.Entities;
using ProjectManager.Utils;
using System.Collections.ObjectModel;
using System.Windows;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for EditTaskTimeContract.xaml
    /// </summary>
    public partial class EditTaskTimeContract : Window
    {
        public TaskItemEntity taskItemEntity {get;set;}

        public ObservableCollection<TaskIntervalsEntity> TaskIntervals {get;set;}
        public TaskItemDO CompletedTaskItemEntity {get;set;}
        public EditTaskTimeContract(TaskItemEntity task)
        {
            taskItemEntity = task;
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);

            TaskNameTextBlock.Text = taskItemEntity.TaskItem.Name;
            var intervals = new List<TaskIntervalsEntity>();
            taskItemEntity.TaskItem.Sessions.ForEach(session =>
            {
                session.Intervals.ForEach(interval => intervals.Add(new TaskIntervalsEntity(interval, session)));
            });
            TaskIntervals = new ObservableCollection<TaskIntervalsEntity>(intervals);
            TaskSessionsDataGrid.ItemsSource = TaskIntervals;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            TaskItemDO taskItemDO = new TaskItemDO()
            {
                ID = taskItemEntity.ID,
                Name = taskItemEntity.Name,
                Description = taskItemEntity.Description,
                ParentProjectID = taskItemEntity.ParentProjectID,
                AssociatedKanbanCardID = taskItemEntity.TaskItem.AssociatedKanbanCardID,
                Sessions = taskItemEntity.TaskItem.Sessions
            };
            DialogResult = true;
        }

        public void UpdateSessions()
        {
            foreach (var interval in TaskIntervals)
            {
                SessionDO sessionDO = taskItemEntity.TaskItem.Sessions.Find(s => s == interval.Session);
                
            }
        }
    }
}
