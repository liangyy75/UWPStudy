using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace UWPMidTerm.Models
{
    public class MyParameter
    {
        public int id { get; set; }
        public string name { get; set; }
        public SolidColorBrush color { get; set; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush foreb { get; set; } = new SolidColorBrush(Colors.Black);

        public ObservableCollection<MyParameter> tags { set; get; }

        public MyParameter(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public MyParameter()
        {
            tags = new ObservableCollection<MyParameter>();
        }
    }
}
