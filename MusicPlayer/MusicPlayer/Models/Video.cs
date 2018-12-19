using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicPlayer.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { set; get; }
        public StorageFile VideoFile { get; set; }
        public BitmapImage VideoImage { get; set; }

        public Video(StorageFile file, VideoProperties videoProperties, StorageItemThumbnail thumbnail)
        {
            Id = Guid.NewGuid().ToString();
            Title = file.Name;
            Duration = videoProperties.Duration;
            VideoFile = file;
            VideoImage = new BitmapImage();
            VideoImage.SetSource(thumbnail);
        }
    }
}
