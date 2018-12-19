using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPMidTerm3.Models
{
    public class SongSheet : SongList
    {
        // 歌单播放量
        private string _visitnum;
        public string visitnum
        {
            get { return _visitnum; }
            set { _visitnum = value; OnPropertyChanged("visitnum"); }
        }
        // 简介
        private string _desc;
        public string desc
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChanged("desc"); }
        }
        // 制作者
        private string _createrName;
        public string createrName
        {
            get { return _createrName; }
            set { _createrName = value; OnPropertyChanged("createName"); }
        }
        // 标签
        private List<string> _tag;
        public List<string> tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged("tag"); }
        }
        // public ObservableCollection<Tag> tags = new ObservableCollection<Tag>();

        public SongSheet() { }
    }
}
