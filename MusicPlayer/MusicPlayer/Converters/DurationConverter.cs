using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MusicPlayer.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan time = (TimeSpan)value;
            int seconds = ((int)time.TotalSeconds) % 60;
            int minutes = ((int)time.TotalMinutes) % 60;
            return ExtendString(minutes.ToString()) + ":" + ExtendString(seconds.ToString());
        }

        private string ExtendString(string str)
        {
            if (str.Length == 1)
            {
                return "0" + str;
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string str = (string)value;
            string[] strs = str.Split(':');
            int minutes = int.Parse(strs[0]);
            int seconds = int.Parse(strs[1]);
            return TimeSpan.FromSeconds((double)(seconds + minutes * 60));
        }
    }
}
