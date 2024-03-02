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
        public static void Init(Dictionary<string, Page> pages)
        {
            instance = new PageUtil();
            instance.Pages = pages;
        }

        public static Dictionary<string, Page> GetPages()
        {
            return instance.Pages;
        }
    }
}
