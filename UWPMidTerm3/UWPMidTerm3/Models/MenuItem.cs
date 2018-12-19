using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace UWPMidTerm3.Models
{
    public class MenuItem
    {
        public int id { get; set; }
        public string icon { get; set; }
        public string content { get; set; }
        public Type purpose { get; set; }
        // public NavigationTransitionInfo transitionInfo { get; set; }

        public SolidColorBrush background { get; set; } = new SolidColorBrush(Color.FromArgb(255, 245, 245, 247));
        public SolidColorBrush borderground { get; set; } = new SolidColorBrush(Color.FromArgb(255, 245, 245, 247));

        public MenuItem(int id, string icon, string content, Type purpose/*, NavigationTransitionInfo transitionInfo*/)
        {
            this.id = id;
            this.icon = icon;
            this.content = content;
            this.purpose = purpose;
            // this.transitionInfo = transitionInfo;
        }
    }
}
