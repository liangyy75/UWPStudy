using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPMidTerm3.Converters
{
    public class SingersConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<string> singers = (List<string>)value;
            if (singers.Count() == 0)
                return "";
            StringBuilder result = new StringBuilder(singers[0]);
            for (int i = 1; i < singers.Count(); i++)
                result.Append("/").Append(singers[i]);
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string singerStr = (string)value;
            string[] singerArr = singerStr.Split('/');
            List<string> singers = new List<string>(singerArr);
            return singers;
        }
    }
}
