using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace UWPMidTerm3.Models
{
    public class Chart : SongList
    {
        // 类型
        private string _type; // 巅峰榜 - top / 全球榜 - global
        public string type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("type"); }
        }
        // 背景图片切割时id
        public double picId { get; set; }
        // 背景图片切割时left
        private double _picLeft = 0;
        public double picLeft
        {
            get { return _picLeft; }
            set { _picLeft = value; OnPropertyChanged("picLeft"); }
        }
        // 背景图片切割形状
        private Rect _rect = new Rect();
        public Rect rect
        {
            get { return _rect; }
            set { _rect = value; OnPropertyChanged("rect"); }
        }
        // 总的歌曲数量
        public int totalSongNum { get; set; }
        // 获得排行榜图片的url
        public string picUrl { set; get; }

        public Chart() { }
    }
}
