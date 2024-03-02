using ProjectManager.Pages;
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
        private Dictionary<string, Page> Pages = new Dictionary<string, Page>();
        public MainWindow()
        {
            InitializeComponent();

            Pages.Add("Pages/CalendarPage.xaml", new CalendarPage());
            Pages.Add("Pages/TimerPage.xaml", new TimerPage());
            Pages.Add("Pages/KanbanPage.xaml", new KanbanPage());
            Pages.Add("Pages/NotesPage.xaml", new NotesPage()) ;

            ContentFrame.Content = Pages["Pages/KanbanPage.xaml"];

            WindowUtil.ApplyDarkWindowStyle(this);
            PageUtil.Init(Pages);
        }

        public static RoutedCommand SwitchPageCommand = new RoutedCommand();


        protected void SwitchPageExecuted(object target, ExecutedRoutedEventArgs e)
        {
            string pageID = e.Parameter as string;
            ContentFrame.Content = Pages[pageID];
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DataUtil.Save();
        }
    }
}
