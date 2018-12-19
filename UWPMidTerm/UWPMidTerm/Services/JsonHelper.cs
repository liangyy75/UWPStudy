using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm.Models;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.ViewModels
{
    public class JsonHelper
    {
        // 获取推荐歌单
        public static void GetSongSheetRecommend(ObservableCollection<SongSheet> songSheets, JsonArray array)
        {
            for(int i = 0; i < array.Count() && i < 6; i++)
            {
                JsonObject jobject = array[i].GetObject();
                string id = jobject.GetNamedNumber("content_id").ToString();
                SongSheet songSheet = new SongSheet(id, jobject.GetNamedString("title"));
                songSheet.cover = new BitmapImage(new Uri(jobject.GetNamedString("cover")));
                // songSheet.file = await UrlHelper.picture.CreateFileAsync(id + ".jpg", CreationCollisionOption.OpenIfExists);
                // UrlHelper.GetFile(new Uri(jobject.GetNamedString("cover")), songSheet.file, songSheet.cover);
                songSheets.Add(songSheet);
            }
        }

        // 获取最新音乐
        public static void GetNewMusicRecommend(ObservableCollection<Song> songs, JsonArray array, int num = 12)
        {
            for(int i = 0; i < array.Count() && i < num; i++)
            {
                JsonObject jobject = array[i].GetObject();
                Song song = new Song();
                GetSongFromJson(song, jobject);
                songs.Add(song);
            }
        }

        // 获取专辑推送
        public static void GetAlbumRelease(ObservableCollection<Album> albums, JsonArray array, int num = 10)
        {
            for(int i = 0; i < array.Count() && i < num; i++)
            {
                JsonObject jobject = array[i].GetObject();
                string albummid = jobject.GetNamedString("album_mid");
                string name = jobject.GetNamedString("album_name");
                List<string> singers = new List<string>();
                JsonArray singerArray = jobject.GetNamedArray("singers");
                foreach (var singer in singerArray)
                    singers.Add(singer.GetObject().GetNamedString("singer_name"));
                Album album = new Album(albummid, name, singers);
                album.cover = new BitmapImage(new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + albummid + ".jpg"));
                // album.AlbumFile = await UrlHelper.picture.CreateFileAsync(albummid + ".jpg", CreationCollisionOption.OpenIfExists);
                // UrlHelper.GetFile(new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + albummid + ".jpg"), album.AlbumFile, album.cover);
                albums.Add(album);
            }
        }

        // 获取排行榜
        public static void GetChart(ObservableCollection<Chart> allCharts, ObservableCollection<Chart> charts, JsonArray array)
        {
            Random random = new Random();
            for(int i = 0; i < array.Count() && i < 7; i++)
            {
                JsonObject jobject = array[i].GetObject();
                string[] strs = jobject.GetNamedString("name").Split('·');
                JsonArray songlist = jobject.GetNamedArray("songlist");
                List<string> tracks = new List<string>();
                List<string> singers = new List<string>();
                for(int j = 0; j < 4 && j < songlist.Count(); j++)
                {
                    JsonObject jo = songlist[j].GetObject();
                    singers.Add(jo.GetNamedString("singer_name"));
                    tracks.Add((j + 1) + " " + jo.GetNamedString("track_name"));
                }
                allCharts.Add(new Chart(random.Next(5), strs[0], strs[1], tracks, singers));
                if (i < 4)
                    charts.Add(allCharts[i]);
            }
        }

        // 获取具体排行榜
        public static async void GetConcreteChart(ObservableCollection<Chart> concreteCharts, ObservableCollection<Chart> restCharts, ObservableCollection<Chart> globalCharts, JsonArray array1, JsonArray array2)
        {
            for(int i = 0; i < array1.Count(); i++)
            {
                JsonObject jobject = array1[i].GetObject();
                string[] strs = jobject.GetNamedString("ListName").Split('·');
                int topid = (int)jobject.GetNamedNumber("topID");
                string update_key = jobject.GetNamedString("update_key");
                ObservableCollection<Song> songs = new ObservableCollection<Song>();
                if(i < 3)
                {
                    string result = await UrlHelper.GetSpecificChart(topid, update_key, 8);
                    GetSongsFromSpecificChart(songs, JsonObject.Parse(result)["songlist"].GetArray(), false);
                }
                Chart chart = new Chart(strs[0], strs[1], topid, update_key, songs);
                if (i < 3)
                {
                    chart.cover = new BitmapImage(new Uri(jobject.GetNamedString("headPic_v12")));
                    concreteCharts.Add(chart);
                }
                else
                {
                    chart.cover = new BitmapImage(new Uri(jobject.GetNamedString("MacDetailPicUrl")));
                    restCharts.Add(chart);
                }
            }
            for(int i = 0; i < array2.Count(); i++)
            {
                JsonObject jobject = array2[i].GetObject();
                string name = jobject.GetNamedString("ListName");
                int topid = (int)jobject.GetNamedNumber("topID");
                string update_key = jobject.GetNamedString("update_key");
                ObservableCollection<Song> songs = new ObservableCollection<Song>();
                Chart chart = new Chart("全球榜", name, topid, update_key, songs);
                chart.cover = new BitmapImage(new Uri(jobject.GetNamedString("MacDetailPicUrl")));
                globalCharts.Add(chart);
            }
        }

        // 处理具体排行榜得到歌曲
        public static void GetSongsFromSpecificChart(ObservableCollection<Song> songs, JsonArray array, bool getPic)
        {
            for(int i = 0; i < array.Count(); i++)
            {
                JsonObject jobject = array[i].GetObject()["data"].GetObject();
                string albummid = jobject.GetNamedString("albummid");
                string title = jobject.GetNamedString("songname");
                string songmid = jobject.GetNamedString("songmid");
                List<string> singers = new List<string>();
                JsonArray singerArr = jobject.GetNamedArray("singer");
                foreach (var singer in singerArr)
                    singers.Add(singer.GetObject().GetNamedString("name"));
                Song song = new Song(songmid, albummid, title, singers);
                song.num = i + 1;
                song.brush = new Windows.UI.Xaml.Media.SolidColorBrush(i % 2 == 0 ? Color.FromArgb(255, 230, 230, 230) : Color.FromArgb(255, 250, 250, 250));
                if (getPic)
                    song.AlbumCover = new BitmapImage(new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + albummid + ".jpg"));
                songs.Add(song);
            }
        }

        // 处理专辑标签
        public static void GetTagsFromJson(ObservableCollection<MyParameter> tags, JsonObject jobject)
        {
            string[] names = { "area", "company", "genre", "type", "year" };
            string[] bigTagNames = { "地区", "公司", "流派", "类型", "年代" };
            for(int i = 0; i < names.Length; i++)
            {
                JsonArray array = jobject.GetNamedArray(names[i]);
                MyParameter bitTag = new MyParameter(i, bigTagNames[i]);
                bitTag.tags = new ObservableCollection<MyParameter>();
                tags.Add(bitTag);
                foreach (var item in array)
                {
                    JsonObject jo = item.GetObject();
                    bitTag.tags.Add(new MyParameter((int)jo.GetNamedNumber("id"), jo.GetNamedString("name")));
                }
            }
        }

        //获取分类标签
        public static ObservableCollection<CategoryGroup> GetPlayListTags(string jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            string data = jsonObject.GetNamedObject("data").GetNamedArray("categories").ToString();
            return JsonConvert.DeserializeObject<ObservableCollection<CategoryGroup>>(data);
        }

        public static void GetSongSheets(ObservableCollection<SongSheet> songSheets, string jsonString)
        {
            JsonArray arr = JsonObject.Parse(jsonString).GetNamedObject("data").GetNamedArray("list");


            foreach (var value in arr)
            {
                JsonObject song = value.GetObject();
                SongSheet songSheet = new SongSheet();
                songSheet.disstid = song.GetNamedString("dissid");
                songSheet.dissname = song.GetNamedString("dissname");
                songSheet.desc = song.GetNamedObject("creator").GetNamedString("name");
                songSheet.cover = new BitmapImage(new Uri(song.GetNamedString("imgurl")));
                songSheets.Add(songSheet);
            }
        }

        public static SongSheet GetSongSheet(string jsonString)
        {
            SongSheet songSheet = new SongSheet();
            JsonObject obj = JsonObject.Parse(jsonString).GetNamedArray("cdlist")[0].GetObject();
            songSheet.disstid = obj.GetNamedString("disstid");
            songSheet.dissname = obj.GetNamedString("dissname");
            songSheet.cover = new BitmapImage(new Uri(obj.GetNamedString("logo")));
            songSheet.nick = obj.GetNamedString("nick");
            songSheet.songnum = obj.GetNamedNumber("songnum").ToString();
            songSheet.visitnum = obj.GetNamedNumber("visitnum").ToString();
            songSheet.desc = obj.GetNamedString("desc");
            List<Song> songs = new List<Song>();
            JsonArray arr = obj.GetNamedArray("songlist");
            foreach (var val in arr)
            {
                JsonObject song = val.GetObject();
                Song s = new Song();
                s.Albnum = song.GetNamedString("albumname");
                s.Name = song.GetNamedString("songname");
                s.Id = song.GetNamedString("songmid");
                s.CoverId = song.GetNamedString("albummid");

                int time = int.Parse(song.GetNamedNumber("interval").ToString());
                s.Time = string.Format("{0:00}:{1:00}",
                    time / 60, time % 60);
                List<String> singers = new List<string>();
                foreach (var value in song.GetNamedArray("singer"))
                {
                    singers.Add(value.GetObject().GetNamedString("name"));
                }
                s.Singers = singers;
                songs.Add(s);
            }
            songSheet.songlist = songs.ToArray();
            return songSheet;
        }

        // 处理Song信息
        public static void GetSongFromJson(Song song, JsonObject jobject)
        {
            song.CoverId = jobject.GetNamedObject("album").GetNamedString("mid");
            song.AlbumCover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + song.CoverId + ".jpg");
            song.Id = jobject.GetNamedObject("ksong").GetNamedString("mid");
            song.Name = jobject.GetNamedString("title");
            song.Singers = new List<string>();
            JsonArray singerArray = jobject.GetNamedArray("singer");
            foreach (var singer in singerArray)
                song.Singers.Add(singer.GetObject().GetNamedString("name"));
        }
    }
}
