using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectManager.Utils
{
    public class PageUtil
    {
        private static PageUtil instance;
        public Dictionary<string, Page> Pages;
        public MainWindow MainWindow;
        public static void Init(Dictionary<string, Page> pages, MainWindow mainWindow)
        {
            instance = new PageUtil();
            instance.Pages = pages;
            instance.MainWindow = mainWindow;
        }

        public static Dictionary<string, Page> GetPages()
        {
            return instance.Pages;
        }

        public static MainWindow GetMainWindow()
        {
            return instance.MainWindow;
        }
    }
}
