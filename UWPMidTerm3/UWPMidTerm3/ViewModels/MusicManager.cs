using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm3.Models;
using UWPMidTerm3.Services;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace UWPMidTerm3.ViewModels
{
    public class MusicManager
    {
        // 下载单曲
        public static async void DownloadSingMusic(Song song)
        {
            bool flag = await UrlHelper.DownloadMusic(song.songmid, song.album.id, song.title + " - " + string.Join("-", song.singers.ToArray()));
            if (!flag)
            {
                MessageDialog message = new MessageDialog("");
                message.Content = "该音频已经下载或者下载失败";
                Debug.WriteLine(song.songmid + " " + song.album.id);
                await message.ShowAsync();
            }
        }

        private SongList allCollectedSongs;
        public SongList collectSongList { get; set; }

        private SongList allHistorySongs;
        public SongList historySongList { get; set; }


        // 用来暂时保存所有音乐
        private SongList downloadedSongListForAllSongs;
        // 显示的音乐（搜索的音乐）
        public SongList downloadedLongList { get; set; }

        // 用来暂时保存所有音乐
        private SongList songListForAllSongs;
        // 显示的音乐（搜索的音乐）
        public SongList songList { get; set; }

        private static MusicManager _instance;

        private MusicManager()
        {
            songList = new SongList();
            downloadedLongList = new SongList();
            songListForAllSongs = new SongList();
            downloadedSongListForAllSongs = new SongList();
            historySongList = new SongList();
            allHistorySongs = new SongList();
            collectSongList = new SongList();
            allCollectedSongs = new SongList();
        }

        public static MusicManager getInstance()
        {
            _instance = new MusicManager();
            return _instance;
        }

        // flag: false是本地, true是下载
        public async void GetAllSongs(bool flag = false)
        {
            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".mp3");
            fileTypeFilter.Add(".m4a");
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            IReadOnlyList<StorageFile> musicFiles = null;
            if (!flag)
                musicFiles = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            else
                musicFiles = await UrlHelper.music.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            foreach (var file in musicFiles)
                await AddSong(file, flag);
        }

        // flag : false是听歌历史，true是收藏
        public void GetAllSongsFromDataBase(bool flag = false)
        {
            historySongList.songs.Clear();
            collectSongList.songs.Clear();
            if (!flag)
                SqlHelper.GetAllSongs(historySongList.songs, "History");
            else
                SqlHelper.GetAllSongs(collectSongList.songs, "Collection");
        }

        public async Task AddSong(StorageFile file, bool flag = false)
        {
            MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
            for (int i = 0; i < songList.songs.Count() && flag == false; i++)
                if (songList.songs[i].title == musicProperties.Title && songList.songs[i].musicFile.Path == file.Path)
                    return;
            for (int i = 0; i < downloadedLongList.songs.Count() && flag; i++)
                if (downloadedLongList.songs[i].title == musicProperties.Title && downloadedLongList.songs[i].musicFile.Path == file.Path)
                    return;
            var fileProperties = await file.GetBasicPropertiesAsync();
            Song song = new Song(file, musicProperties, fileProperties.Size);
            song.type = flag ? "download" : "native";
            String picStr = file.Name.Substring(0, file.Name.LastIndexOf('.')) + ".jpg";
            if (flag == false || await UrlHelper.picture.TryGetItemAsync(picStr) == null)
            {
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 300, ThumbnailOptions.UseCurrentScale);
                song.cover.SetSource(thumbnail);
            }
            else
            {
                song.pictuFile = await UrlHelper.picture.GetFileAsync(picStr);
                if (song.pictuFile != null)
                    using (IRandomAccessStream stream = await song.pictuFile.OpenAsync(FileAccessMode.Read))
                    {
                        await song.cover.SetSourceAsync(stream);
                    }
            }
            if (flag == false)
                songList.songs.Add(song);
            else
                downloadedLongList.songs.Add(song);
        }

        public SongList SelectType(int type)
        {
            SongList list;
            switch (type)
            {
                case 0: list = songList; break;
                case 1: list = downloadedLongList; break;
                case 2: list = historySongList; break;
                case 3: list = collectSongList; break;
                default: list = new SongList(); break;
            }
            return list;
        }

        public SongList SelectType2(int type)
        {
            SongList list;
            switch (type)
            {
                case 0: list = songListForAllSongs; break;
                case 1: list = downloadedSongListForAllSongs; break;
                case 2: list = allHistorySongs; break;
                case 3: list = allCollectedSongs; break;
                default: list = new SongList(); break;
            }
            return list;
        }

        public async void DeleteSong(Song song, bool deleteFile, int type = 0)
        {
            SongList list = SelectType(type);
            for (int i = 0; i < list.songs.Count(); i++)
            {
                if (list.songs[i].title == song.title && list.songs[i].musicFile.Path == song.musicFile.Path)
                {
                    StorageFile file = list.songs[i].musicFile;
                    StorageFile picFile = list.songs[i].pictuFile;
                    list.songs.RemoveAt(i);
                    if (deleteFile)
                    {
                        if (type == 1)
                        {
                            SqlHelper.DeleteSong("History", song.fileName);
                            SqlHelper.DeleteSong("Collection", song.fileName);
                        }
                        await file.DeleteAsync();
                        if (picFile != null)
                            await picFile.DeleteAsync();
                        // 怎么样才能完全删掉呢？？？
                    }
                    break;
                }
            }
        }

        public void SaveAllSongs(int type = 0)
        {
            SongList list = SelectType(type), list2 = SelectType2(type);
            if (list.songs.Count() > 0 && list2.songs.Count() == 0)
            {
                list2.Copy(list);
                list.songs.Clear();
            }
        }

        public void ReGetAllSongs(int type = 0)
        {
            SongList list = SelectType(type), list2 = SelectType2(type);
            if (list2.songs.Count() > 0)
            {
                list.Copy(list2);
                list2.songs.Clear();
            }
        }

        // 返回值可作为搜索提示
        public HashSet<string> SearchSongs(string text, int type = 0)
        {
            SongList list = SelectType(type), list2 = SelectType2(type);
            list.songs.Clear();
            HashSet<string> strings = new HashSet<string>();
            foreach (var song in list2.songs)
            {
                int i = 0; //判断歌曲是否已加入搜索列表
                if (song.title.ToLower().IndexOf(text.ToLower()) != -1)
                {
                    strings.Add(song.title);
                    list.songs.Add(song);
                    i++;
                }
                if (song.album.title.ToLower().IndexOf(text.ToLower()) != -1)
                {
                    strings.Add(song.album.title);
                    if (i == 0)
                    {
                        list.songs.Add(song);
                        i++;
                    }
                }
                foreach (string singer in song.singers)
                {
                    if (singer.ToLower().IndexOf(text.ToLower()) != -1)
                    {
                        strings.Add(singer);
                        if (i == 0)
                        {
                            list.songs.Add(song);
                            i++;
                        }
                    }
                }
            }
            return strings;
        }
    }
}
