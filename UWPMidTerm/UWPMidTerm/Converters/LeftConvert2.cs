using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPMidTerm.Converters
{
    public class LeftConvert2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (0 - (double)value * 1.5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (0 - (double)value / 1.5);
        }
    }
}
