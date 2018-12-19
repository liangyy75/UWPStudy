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
    public class SongManager
    {
        private ObservableCollection<Song> allSongs = new ObservableCollection<Song>();

        public ObservableCollection<Song> songs { set; get; }
        public Song select { get; set; } = null;
        public int songStyle { get; set; } = 0;

        private static SongManager _instance;

        private SongManager()
        {
            songs = new ObservableCollection<Song>();
        }

        public static SongManager getInstance()
        {
            return _instance ?? (_instance = new SongManager());
        }

        public async void GetAllSongsFromMusicLibrary()
        {
            StorageFolder folder = KnownFolders.MusicLibrary;
            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".mp3");
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            IReadOnlyList<StorageFile> musicFiles = await folder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            foreach (var file in musicFiles)
            {
                await AddSong(file, false);
            }
        }

        public async Task AddSong(StorageFile file, bool flag)
        {
            MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
            for (int i = 0; i < songs.Count(); i++)
            {
                if (songs[i].Title == musicProperties.Title)
                {
                    if (flag)
                        select = songs[i];
                    return;
                }
            }
            StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 300, ThumbnailOptions.UseCurrentScale);
            Song song2 = new Song(file, musicProperties, thumbnail);
            songs.Add(song2);
            if (flag)
                select = song2;
        }

        public async void DeleteSong(string id, bool deleteFile)
        {
            for (int i = 0; i < songs.Count(); i++)
            {
                if (songs[i].id == id)
                {
                    if (id == select.id)
                    {
                        select = null;
                    }
                    StorageFile file = songs[i].MusicFile;
                    songs.RemoveAt(i);
                    if (deleteFile)
                    {
                        await file.DeleteAsync();
                    }
                    break;
                }
            }
        }

        public void SaveAllSongs()
        {
            if(allSongs.Count() == 0 && songs.Count() > 0)
            {
                foreach (var song in songs)
                {
                    allSongs.Add(song);
                }
                songs.Clear();
            }
        }

        public void ReGetAllSongs()
        {
            if(allSongs.Count() != 0)
            {
                songs.Clear();
                foreach (var song in allSongs)
                {
                    songs.Add(song);
                }
                allSongs.Clear();
            }
        }

        public List<string> SearchSongs(string text)
        {
            songs.Clear();
            List<string> strings = new List<string>();
            foreach (var song in allSongs)
            {
                if (song.Title.StartsWith(text))
                {
                    strings.Add(song.Title);
                    songs.Add(song);
                }
            }
            return strings;
        }

        public bool SelectMusic(object chosenSuggestion)
        {
            if(chosenSuggestion == null)
            {
                select = songs[0];
                return true;
            }
            string text = (string)chosenSuggestion;
            foreach (var song in songs)
            {
                if (song.Title.StartsWith(text))
                {
                    select = song;
                    return true;
                }
            }
            return false;
        }
    }
}
