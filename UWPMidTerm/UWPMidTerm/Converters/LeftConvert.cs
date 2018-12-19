﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPMidTerm.Converters
{
    public class LeftConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value * -179.2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)(0 - ((double)value) / 179.2);
        }
    }
}
