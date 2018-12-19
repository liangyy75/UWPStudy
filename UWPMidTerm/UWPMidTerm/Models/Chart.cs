using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.Models
{
    public class Chart
    {
        public int num { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<string> tracks { get; set; }
        public List<string> singers { get; set; }

        public string update_key { get; set; }
        public ObservableCollection<Song> songs { get; set; }
        public BitmapImage cover { get; set; }

        // 未进入详情页的排行榜
        public Chart(int num, string type, string name, List<string> tracks, List<string> singers)
        {
            this.num = num;
            this.type = type;
            this.name = name;
            this.tracks = tracks;
            this.singers = singers;
        }

        // 进入到TopList页面的排行榜
        public Chart(string type, string name, int topid, string update_key, ObservableCollection<Song> songs)
        {
            this.type = type;
            this.name = name;
            this.num = topid;
            this.update_key = update_key;
            this.songs = songs;
        }
    }
}
