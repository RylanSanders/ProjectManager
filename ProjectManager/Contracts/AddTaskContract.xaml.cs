using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static ProjectManager.TimerPage;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for AddTaskContract.xaml
    /// </summary>
    public partial class AddTaskContract : Window
    {
        public TaskItemDO ToAddTaskItem;
        public AddTaskContract()
        {
            InitializeComponent();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            ToAddTaskItem = new TaskItemDO() {Name=NameTextBox.Text, Description=DescriptionTextBox.Text, Type=TypeTextBox.Text };
            this.DialogResult = true;
        }
    }
}
