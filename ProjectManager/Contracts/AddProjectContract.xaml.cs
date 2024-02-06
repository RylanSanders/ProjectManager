using ProjectManager.DataObjects;
using ProjectManager.Utils;
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
using System.Windows.Shapes;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for AddProjectContract.xaml
    /// </summary>
    public partial class AddProjectContract : Window
    {
        public ProjectDefinitionDO NewProjectDefinition { get; set; }
        public AddProjectContract()
        {
            InitializeComponent();
            WindowUtil.ApplyDarkWindowStyle(this);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            NewProjectDefinition = new ProjectDefinitionDO() {Name=NameTextBox.Text, Description=DescriptionTextBox.Text};
            DialogResult = true;
        }
    }
}
