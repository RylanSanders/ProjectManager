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

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for AddDateContract.xaml
    /// </summary>
    public partial class AddDateContract : Window
    {
        public DateDO ToAddDateDO { get; set; }
        public AddDateContract()
        {
            InitializeComponent();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            ToAddDateDO = new DateDO() { Name = NameTextBox.Text, Description = DescriptionTextBox.Text, Interval = new IntervalDO() {StartTime= DateTime.Parse(IntervalStartTextBox.Text), EndTime= DateTime.Parse(IntervalStartTextBox.Text) } };
            this.DialogResult = true;
        }
    }
}
