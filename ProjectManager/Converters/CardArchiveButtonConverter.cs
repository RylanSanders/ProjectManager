using ProjectManager.DataObjects;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectManager.Converters
{
    public class CardArchiveButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            KanbanCardDO card = (KanbanCardDO)value;
            if (card.ColumnNumber==3)
            {
                return 35;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
