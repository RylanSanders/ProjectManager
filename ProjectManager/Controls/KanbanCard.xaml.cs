using ProjectManager.DataObjects;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManager.Controls
{
    /// <summary>
    /// Interaction logic for KanbanCard.xaml
    /// </summary>
    public partial class KanbanCard : UserControl
    {

        public KanbanCard()
        {
            // A temporary dummy initialization to prevent nulls
            DataModel = new KanbanCardDO();
            
            InitializeComponent();
        }

        public KanbanCardDO DataModel
        {
            get { return (KanbanCardDO)GetValue(DataModelProperty); }
            set { SetValue(DataModelProperty, value);}
        }

        public static readonly DependencyProperty DataModelProperty =
        DependencyProperty.Register("DataModel", typeof(KanbanCardDO), typeof(KanbanCard));

    }
}
