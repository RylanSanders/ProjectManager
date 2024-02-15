using MaterialDesignThemes.Wpf;
using ProjectManager.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectManager.Converters
{
    public class NoteIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var noteType = (DataObjects.NoteDO.NoteType)value;
            if (noteType == DataObjects.NoteDO.NoteType.Note)
            {
                return PackIconKind.Note;
            }
            else if (noteType == DataObjects.NoteDO.NoteType.Folder)
            {
                return PackIconKind.Folder;
            }
            return PackIconKind.Abacus;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
