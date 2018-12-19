using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm3.Models
{
    public class SongList : INotifyPropertyChanged
    {
        // id
        public string id { get; set; }
        // 名字
        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("title"); }
        }
        // 封面
        public BitmapImage cover { get; set; } = new BitmapImage();
        // 时间 : 歌单的制作时间 / 专辑的发行时间 / 排行榜的更新时间
        private string _date;
        public string date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("date"); }
        }
        // 所有歌曲
        public ObservableCollection<Song> songs { get; set; } = new ObservableCollection<Song>();
        // 歌曲数量
        private int _num;
        public int num
        {
            get { return _num; }
            set { _num = value; OnPropertyChanged("num"); }
        }

        //总共的个数
        public int Total { get; set; }
        //当前页
        public int CurrentPage { get; set; }
        //总数
        public int Count { get; set; }

        public SongList() { }

        public void Copy(SongList songList)
        {
            this.id = songList.id;
            this.title = songList.title;
            this.cover.UriSource = songList.cover.UriSource;
            this.date = songList.date;
            this.songs.Clear();
            foreach (var song in songList.songs)
                songs.Add(song);
            this.num = songList.num;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
