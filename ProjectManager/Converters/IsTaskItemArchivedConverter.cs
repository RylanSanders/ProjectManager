using ProjectManager.Entities;
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
    internal class IsTaskItemArchivedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guid guid = (Guid)value;
            if (guid == Guid.Empty) return false;
            return DataUtil.GetInstance().ArchivedData.Tasks.Any(t => t.ID == guid);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
