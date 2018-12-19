using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.Models
{
    public class SongSheet
    {
        // desstid
        public string disstid { get; set; }
        // 歌单图片
        public BitmapImage cover { set; get; }
        // 图片文件
        public StorageFile file { set; get; }
        // 歌单名
        public string dissname { get; set; }
        // 歌单简介
        public string desc { get; set; }
        // 所有歌曲
        public Song[] songlist { get; set; }
        // 播放量
        public string visitnum { get; set; }
        // 歌曲数
        public string songnum { get; set; }
        // 作者昵称
        public string nick { set; get; }

        public SongSheet() { }

        // 未进入详情页的歌单
        public SongSheet(string id, string content)
        {
            this.disstid = id;
            this.dissname = content;
        }
    }
}
