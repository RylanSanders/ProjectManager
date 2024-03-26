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

        private BitmapImage DefaultIcon;
        private BitmapImage TimerIcon;
        private BitmapImage PauseIcon;
        public MainWindow()
        {
            InitializeComponent();

            Pages.Add("Pages/CalendarPage.xaml", new CalendarPage());
            Pages.Add("Pages/TimerPage.xaml", new TimerPage());
            Pages.Add("Pages/KanbanPage.xaml", new KanbanPage());
            Pages.Add("Pages/NotesPage.xaml", new NotesPage()) ;


            ContentFrame.Content = Pages["Pages/KanbanPage.xaml"];
            WindowUtil.ApplyDarkWindowStyle(this);
            PageUtil.Init(Pages, this);

            var resourceDict = new ResourceDictionary();
            resourceDict.Source = new Uri(@"pack://application:,,,/ResourceDictionary.xaml", UriKind.RelativeOrAbsolute);
            DefaultIcon = (BitmapImage)resourceDict["DefaultAppIcon"];
            TimerIcon = (BitmapImage)resourceDict["TimerAppIcon"];
            PauseIcon = (BitmapImage)resourceDict["PauseAppIcon"];
        }

        public static RoutedCommand SwitchPageCommand = new RoutedCommand();


        protected void SwitchPageExecuted(object target, ExecutedRoutedEventArgs e)
        {
            string pageID = e.Parameter as string;
            ContentFrame.Content = Pages[pageID];
        }

        public void ChangeIcon(AppIcons icon)
        {
            if (icon == AppIcons.Default)
            {
                Icon = DefaultIcon;
            }
            else if (icon == AppIcons.TimerPlay)
            {
                Icon = TimerIcon;
            }
            else if (icon == AppIcons.Pause)
            {
                Icon = PauseIcon;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DataUtil.Save();
        }

        public enum AppIcons
        {
            Default, TimerPlay, Pause
        }
    }
}
