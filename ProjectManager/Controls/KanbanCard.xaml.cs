using ProjectManager.Contracts;
using ProjectManager.DataObjects;
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
            InitializeComponent();

            MouseDoubleClick += KanbanCard_MouseDoubleClick;
        }

        private void KanbanCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           EditKanbanCardContract editKanbanCardContract = new EditKanbanCardContract(DataModel);
            editKanbanCardContract.ShowDialog();
            if (editKanbanCardContract.DialogResult == true)
            {
                DataModel.Merge(editKanbanCardContract.EditedKanbanCard);
                InvalidateProperty(DataModelProperty);
                InvalidateVisual();
            }
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
