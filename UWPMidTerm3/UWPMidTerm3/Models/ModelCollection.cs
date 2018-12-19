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
    public class ModelCollection
    {
        public ObservableCollection<SongSheet> collection { get; set; }

        public ObservableCollection<Song> collection2 { get; set; }

        public ObservableCollection<Album> collection3 { get; set; }

        public SolidColorBrush brush { get; set; } = new SolidColorBrush(Colors.White);
        public int index { get; set; } = 0;

        public ModelCollection(int type, int index = 0)
        {
            switch (type)
            {
                case 1:
                    collection = new ObservableCollection<SongSheet>();
                    break;
                case 2:
                    brush = new SolidColorBrush(Colors.White);
                    this.index = index;
                    break;
                case 3:
                    collection2 = new ObservableCollection<Song>();
                    break;
                case 4:
                    collection3 = new ObservableCollection<Album>();
                    break;
                case 5:
                    collection2 = new ObservableCollection<Song>();
                    collection3 = new ObservableCollection<Album>();
                    break;
            }
        }
    }
}
