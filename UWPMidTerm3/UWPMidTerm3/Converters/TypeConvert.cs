using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPMidTerm3.Converters
{
    public class TypeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string str = (string)value;
            return str == "net" ? "下载" : (str == "native" ? "删除本地" : "删除已下载");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string str = (string)value;
            return str == "下载" ? "net" : (str == "删除本地" ? "native" : "download");
        }
    }
}
