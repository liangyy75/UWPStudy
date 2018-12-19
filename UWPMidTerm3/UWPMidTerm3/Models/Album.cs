using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPMidTerm3.Models
{
    public class Album : SongList
    {
        // 歌手
        private List<string> _singers = new List<string>();
        public List<string> singers
        {
            get { return _singers; }
            set { _singers = value; OnPropertyChanged("singers"); }
        }
        // 公司
        private string _company;
        public string company
        {
            get { return _company; }
            set { _company = value; OnPropertyChanged("company"); }
        }
        // 流派
        private string _genre;
        public string genre
        {
            get { return _genre; }
            set { _genre = value; OnPropertyChanged("genre"); }
        }
        // 语言
        private string _lan;
        public string lan
        {
            get { return _lan; }
            set { _lan = value; OnPropertyChanged("lan"); }
        }
        // 简介
        private string _desc;
        public string desc
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChanged("desc"); }
        }

        public Album() { }
    }
}
