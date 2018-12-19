using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace MusicPlayer.ViewModels
{
    public class VideoManager
    {
        private ObservableCollection<Video> allVideos = new ObservableCollection<Video>();
        public ObservableCollection<Video> videos { set; get; }
        public Video select { set; get; } = null;
        public bool status { get; set; } = true;

        private static VideoManager _instance;

        public static VideoManager GetInstance()
        {
            return _instance ?? (_instance = new VideoManager());
        }

        private VideoManager()
        {
            videos = new ObservableCollection<Video>();
        }
        public async Task GetAllVediosFromVedioLibrary()
        {
            StorageFolder folder = KnownFolders.VideosLibrary;
            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".mp4");
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            IReadOnlyList<StorageFile> videoFiles = await folder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            foreach (var file in videoFiles)
            {
                await AddVideo(file, false);
            }
        }
        public async Task AddVideo(StorageFile file, bool flag)
        {
            foreach (var video in videos)
            {
                if (video.Title == file.Name)
                {
                    if (flag)
                    {
                        select = video;
                    }
                    return;
                }
            }
            VideoProperties videoProperties = await file.Properties.GetVideoPropertiesAsync();
            StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 100, ThumbnailOptions.UseCurrentScale);
            Video video2 = new Video(file, videoProperties, thumbnail);
            videos.Add(video2);
            if (flag)
            {
                select = video2;
                status = true;
            }
        }
        public async void DeleteVideo(string Id, bool deleteFile)
        {
            for (int i = 0; i < videos.Count(); i++)
            {
                if (videos[i].Id == Id)
                {
                    if (select.Id == Id)
                    {
                        select = null;
                    }
                    StorageFile file = videos[i].VideoFile;
                    videos.RemoveAt(i);
                    if (deleteFile)
                    {
                        await file.DeleteAsync();
                    }
                    break;
                }
            }
        }
        public void SaveAllVideos()
        {
            if(allVideos.Count() == 0 && videos.Count() > 0)
            {
                foreach (var video in videos)
                    allVideos.Add(video);
                videos.Clear();
            }
        }
        public void ReGetAllVideos()
        {
            if(allVideos.Count() != 0)
            {
                videos.Clear();
                foreach (var video in allVideos)
                    videos.Add(video);
                allVideos.Clear();
            }
        }
        public List<string> SearchVideos(string text)
        {
            videos.Clear();
            List<string> strings = new List<string>();
            foreach (var video in allVideos)
            {
                if (video.Title.StartsWith(text))
                {
                    strings.Add(video.Title);
                    videos.Add(video);
                }
            }
            return strings;
        }
        public bool SelectVideo(object chosenSuggestion)
        {
            if (chosenSuggestion == null)
            {
                select = videos[0];
                return true;
            }
            string text = (string)chosenSuggestion;
            foreach (var video in videos)
            {
                if (video.Title.StartsWith(text))
                {
                    select = video;
                    return true;
                }
            }
            return false;
        }
    }
}
