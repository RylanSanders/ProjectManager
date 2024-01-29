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
        public TimerPage()
        {
            TaskItems = new ObservableCollection<TaskItemDO>();
            InitializeComponent();

            TaskItems.Add(new TaskItemDO() { Name="First", Description="Hello", Type="adsf", Duration=TimeSpan.Zero});
            TaskItems.Add(new TaskItemDO() { Name = "Second", Description = "World", Type = "bxvcb", Duration = TimeSpan.Zero });
            TaskItems.Add(new TaskItemDO() { Name = "Third", Description = "Plc", Type = "kh", Duration = TimeSpan.Zero });

            TasksListView.ItemsSource = TaskItems;
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
