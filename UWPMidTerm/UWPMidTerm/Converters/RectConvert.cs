using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace UWPMidTerm.Converters
{
    public class RectConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new Rect((int)value * 179.2, 0, 179.2, 400);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)(((Rect)value).X / 179.2);
        }
    }
}
