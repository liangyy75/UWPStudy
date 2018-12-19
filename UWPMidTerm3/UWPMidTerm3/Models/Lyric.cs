using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace UWPMidTerm3.Models
{
    public class Lyric
    {
        // 文本内容
        public string text { get; set; }
        // 时间字符串
        public string strTime { get; set; }
        // 时间数
        public double intTime { get; set; }
        // 歌词文本行数
        public int rowNum { get; set; }
        // 文本颜色
        public SolidColorBrush foreground { get; set; } = new SolidColorBrush(Colors.Black);

        public Lyric() { }
    }
}
