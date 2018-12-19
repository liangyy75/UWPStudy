using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace UWPMidTerm3.Models
{
    public class MyTheme
    {
        public static SolidColorBrush themeBrush { get; } = new SolidColorBrush(Colors.Red);
        public static SolidColorBrush normalBrush { get; } = new SolidColorBrush(Colors.Black);
        public static SolidColorBrush menuBrush { get; } = new SolidColorBrush(Color.FromArgb(255, 245, 245, 247));
        public static SolidColorBrush bottomBrush { get; } = new SolidColorBrush(Color.FromArgb(255, 246, 246, 248));
    }
}
