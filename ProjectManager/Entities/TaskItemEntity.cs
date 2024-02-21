using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProjectManager.Entities
{
    public class TaskItemEntity
    {
        public TaskItemDO TaskItem { get; set; }

        public Guid ID { get { return TaskItem.ID; } }
        public string? Name { get { return TaskItem.Name; } set { TaskItem.Name = value; } }
        public string? Description { get { return TaskItem.Description; } set { TaskItem.Description = value; } }
        public Guid ParentProjectID { get { return TaskItem.ParentProjectID; } set { TaskItem.ParentProjectID = value; } }
        public TimeSpan Duration
        {
            get
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

        private DataGrid listView;

        public void AddSession(SessionDO session)
        {
            TaskItem.Sessions.Add(session);
            var collectionView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            collectionView.Refresh();
        }

        public void UpdateSessions()
        {
            var collectionView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            collectionView.Refresh();
        }

        public TaskItemEntity(DataGrid listViewControl, TaskItemDO dataObject)
        {
            TaskItem = dataObject;
            listView = listViewControl;
        }
    }
}
