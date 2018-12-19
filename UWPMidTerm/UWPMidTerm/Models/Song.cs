using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.Models
{
    public class Song
    {
        // songmid
        public string Id { get; set; }
        // albummid
        public string CoverId { get; set; }
        // 歌手
        public List<string> Singers { get; set; }
        // 歌曲名
        public string Name { get; set; }
        // 专辑名
        public string Albnum { get; set; }
        // 播放时长
        public string Time { get; set; }
        // 大小
        public int Size { get; set; }
        // 专辑图片
        public BitmapImage AlbumCover { get; set; } = new BitmapImage();

        public int num { get; set; }
        public SolidColorBrush brush { get; set; }

        public Song() { }

        // 未进入详情页的最新歌曲
        public Song(string songmid, string albummid, string title, List<string> singers)
        {
            this.Id = songmid;
            this.CoverId = albummid;
            this.Name = title;
            this.Singers = singers;
        }
    }
}
