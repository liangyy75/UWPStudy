using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm3.Models
{
    public class Song : INotifyPropertyChanged
    {
        // 歌曲编号
        public string songmid { get; set; }
        // 歌曲名
        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("title"); }
        }
        // 播放时长
        public int _time { get; set; }
        private string _interval;
        public string interval
        {
            get { return _interval; }
            set { _interval = value; OnPropertyChanged("interval"); }
        }
        // 歌曲图片
        private BitmapImage _cover = new BitmapImage();
        public BitmapImage cover
        {
            get { return _cover; }
            set { _cover = value; OnPropertyChanged("cover"); }
        }
        // 歌手
        private List<string> _singers = new List<string>();
        public List<string> singers
        {
            get { return _singers; }
            set { _singers = value; OnPropertyChanged("singers"); }
        }

        // 所属专辑
        public Album album { get; set; } = new Album(); // album/mid -- title/genre/aDate/company
        // 发行时间
        private string _date;
        public string date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("date"); }
        }
        // 歌词
        public ObservableCollection<Lyric> lyrics { get; set; }

        // 歌曲在album/chart/toplist里面的位置
        public int index { get; set; }
        // 背景颜色
        public SolidColorBrush background { get; set; }
        // 字体颜色
        public SolidColorBrush foreground { get; set; }

        public StorageFile musicFile { get; set; } = null;
        public StorageFile pictuFile { get; set; } = null;
        public string size { get; set; }
        public string type { get; set; }
        public string fileName { get; set; }

        public Song(bool flag = false)
        {
            if (flag)
            {
                lyrics = new ObservableCollection<Lyric>();
            }
            else
            {
                background = new SolidColorBrush(Colors.Transparent);
                foreground = new SolidColorBrush(Colors.LightGreen);
            }
        }

        public Song(StorageFile file, MusicProperties musicProperties, ulong size)
        {
            this.musicFile = file;
            this.title = musicProperties.Title != "" ? musicProperties.Title : "未知歌名";
            singers.Add(musicProperties.Artist != "" ? musicProperties.Artist : "未知歌手");
            this.fileName = musicFile.Name;
            album.title = musicProperties.Album != "" ? musicProperties.Album : "未知专辑";
            TimeSpan timeSpan = musicProperties.Duration;
            this._time = timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            this.interval = string.Format("{0:00}:{1:00}", (timeSpan.Hours * 60) + timeSpan.Minutes, timeSpan.Seconds);
            this.size = string.Format("{0}Mb", ((double)((int)((double)size / 1024 / 1024 * 100))) / 100);
        }

        public void Copy(Song song)
        {
            this.songmid = song.songmid;
            this.musicFile = song.musicFile;
            this.pictuFile = song.pictuFile;
            this.title = song.title;
            this.singers = song.singers;
            this.album.title = song.album.title;
            this._time = song._time;
            this.interval = song.interval;
            this.cover = song.cover;
            this.size = song.size;
            this.type = song.type;
            this.lyrics.Clear();
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
