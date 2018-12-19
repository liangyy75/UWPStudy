using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace UWPMidTerm3.Models
{
    public class Tag
    {
        public int id { set; get; }
        public string name { get; set; }
        public string desc { get; set; }
        public SolidColorBrush background { get; set; }
        public SolidColorBrush foreground { get; set; }
        public SolidColorBrush borderColor { get; set; }

        public ObservableCollection<Tag> smallTags { get; set; }

        public Tag(string name, Color b, Color f, Color bc, int id = 0, string desc = "")
        {
            this.id = id;
            this.name = name;
            this.desc = desc;
            background = new SolidColorBrush(b);
            foreground = new SolidColorBrush(f);
            borderColor = new SolidColorBrush(bc);
        }

        public Tag(string name, int id = 0, string desc = "", bool flag = false)
        {
            this.id = id;
            this.name = name;
            this.desc = desc;
            if (flag)
                smallTags = new ObservableCollection<Tag>();
            background = new SolidColorBrush(Colors.White);
            foreground = new SolidColorBrush(Colors.Black);
            borderColor = new SolidColorBrush(Colors.White);
        }

        public void Change(Color b, Color f, Color bc)
        {
            background.Color = b;
            foreground.Color = f;
            borderColor.Color = bc;
        }

        public void BackUp()
        {
            Change(Colors.White, Colors.Black, Colors.White);
        }
    }
}
