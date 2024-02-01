using ProjectManager.Utils;
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

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        public static RoutedCommand SwitchPageCommand = new RoutedCommand();


        protected void SwitchPageExecuted(object target, ExecutedRoutedEventArgs e)
        {
            string a = e.Parameter as string;
            ContentFrame.Source = new Uri(a, UriKind.Relative);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DataUtil.Save();
        }
    }
}
