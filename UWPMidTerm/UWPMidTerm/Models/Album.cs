using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.Models
{
    public class Album
    {
        public string albummid { get; set; }
        public BitmapImage cover { get; set; } = new BitmapImage();
        public StorageFile AlbumFile { set; get; }
        public string name { get; set; }
        public List<string> singers { get; set; }

        // 未进入详情页的专辑
        public Album(string albummid, string name, List<string> singers)
        {
            this.albummid = albummid;
            this.name = name;
            this.singers = singers;
        }
    }
}
