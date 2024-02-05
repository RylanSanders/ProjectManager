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
    class NumTasksPerDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            var count = DataUtil.GetInstance().Dates.Where(d => d.Interval.StartTime.Date == date.Date).Count();
            if (count==0) return "";
            return count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
