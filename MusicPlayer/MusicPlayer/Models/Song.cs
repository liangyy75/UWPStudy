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
    public class Song
    {
        public string id { set; get; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public StorageFile MusicFile { get; set; }
        public BitmapImage AlbumCover { get; set; }

        public Song(StorageFile file, MusicProperties musicProperties, StorageItemThumbnail thumbnail)
        {
            id = Guid.NewGuid().ToString();
            MusicFile = file;
            Title = musicProperties.Title;
            Artist = musicProperties.Artist;
            Album = musicProperties.Album;
            Duration = musicProperties.Duration;
            AlbumCover = new BitmapImage();
            AlbumCover.SetSource(thumbnail);
        }
    }
}
