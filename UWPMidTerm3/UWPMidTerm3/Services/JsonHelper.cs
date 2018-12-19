using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm3.Models;
using Windows.Data.Json;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm3.Services
{
    public class JsonHelper
    {
        // 处理时间，补0
        public static string DealWithTime(int t)
        {
            return t < 10 ? "0" + t.ToString() : t.ToString();
        }

        // 处理启动页获取的所有json信息
        public static void DealWithAllMessage(string jsonStr, ObservableCollection<Tag> tags, ObservableCollection<ModelCollection> songSheets, ObservableCollection<Tag> tags2, ObservableCollection<ModelCollection> newSongs, ObservableCollection<Tag> tags3, ObservableCollection<ModelCollection> newAlbums, ObservableCollection<Chart> charts)
        {
            JsonObject recommend = JsonObject.Parse(jsonStr);
            GetSongSheetTags(tags, recommend.GetNamedObject("category").GetNamedObject("data").GetNamedArray("category").GetObjectAt(0).GetNamedArray("items"));
            GetSongSheetRecommend(songSheets, jsonStr);
            GetNewSongTags(tags2, recommend.GetNamedObject("new_song").GetNamedObject("data").GetNamedArray("type_info"));
            GetNewSongRecommend(newSongs, jsonStr);
            GetNewAlbumTags(tags3, jsonStr, true);
            GetNewAlbumRecommend(newAlbums, jsonStr);
            GetTopListRecommend(charts, jsonStr, true);
        }

        // 获取推荐歌单的标签
        public static void GetSongSheetTags(ObservableCollection<Tag> tags, JsonArray jsonArray, int total = 5)
        {
            tags.Add(new Tag("为你推荐", Colors.White, Colors.LightGreen, Colors.LightGreen));
            for (int i = 0; i < total && i < jsonArray.Count(); i++)
            {
                JsonObject jb = jsonArray[i].GetObject();
                Tag tag = new Tag(jb.GetNamedString("item_name"), (int)jb.GetNamedNumber("item_id"), jb.GetNamedString("item_desc"));
                tags.Add(tag);
            }
        }

        // 获取最新音乐的标签
        public static void GetNewSongTags(ObservableCollection<Tag> tags, JsonArray jsonArr)
        {
            for (int i = 0; i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Tag tag = new Tag(jb.GetNamedString("title"), (int)jb.GetNamedNumber("id"));
                tags.Add(tag);
            }
            // tags[0].Change(Colors.SkyBlue, Colors.White, Colors.White);
            tags[0].Change(Colors.White, Colors.LightGreen, Colors.LightGreen);
        }

        // 分情况获得专辑推送的标签
        public static void GetNewAlbumTags(ObservableCollection<Tag> tags, string jsonStr, bool flag = false)
        {
            if (flag)
                AddNewAlbumTags(tags, JsonObject.Parse(jsonStr).GetNamedObject("new_album").GetNamedObject("data").GetNamedObject("tags").GetNamedArray("area"));
            else
            {
                JsonObject jb = JsonObject.Parse(jsonStr).GetNamedObject("new_album").GetNamedObject("data").GetNamedObject("tags");
                string[] bigTagNames1 = { "area", "company", "genre", "type", "year" };
                string[] bigTagNames2 = { "地区", "公司", "流派", "类别", "年份" };
                for (int i = 0; i < bigTagNames1.Length; i++)
                {
                    Tag tag = new Tag(bigTagNames2[i], i, "", true);
                    AddNewAlbumTags(tag.smallTags, jb.GetNamedArray(bigTagNames1[i]), false);
                    tags.Add(tag);
                }
                tags[0].Change(Colors.Red, Colors.White, Colors.White);
            }
        }

        // 获得专辑推送的标签
        public static void AddNewAlbumTags(ObservableCollection<Tag> tags, JsonArray jsonArr, bool flag = true)
        {
            for (int i = 0; i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Tag tag = new Tag(jb.GetNamedString("name"), (int)jb.GetNamedNumber("id"));
                tags.Add(tag);
            }
            if (flag)
                tags[0].Change(Colors.White, Colors.LightGreen, Colors.LightGreen);
            else
                tags[0].Change(Colors.LightGreen, Colors.White, Colors.White);
        }

        // 获得分类歌单的标签
        public static void AddSongSheetTags(ObservableCollection<Tag> tags, string jsonStr)
        {
            JsonArray jsonArr = JsonObject.Parse(jsonStr).GetNamedObject("data").GetNamedArray("categories");
            for (int i = 1; i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Tag tag = new Tag(jb.GetNamedString("categoryGroupName"), i, "", true);
                JsonArray ja = jb.GetNamedArray("items");
                for (int j = 0; j < ja.Count(); j++)
                {
                    JsonObject jo = ja[j].GetObject();
                    Tag smallTag = new Tag(jo.GetNamedString("categoryName"), (int)jo.GetNamedNumber("categoryId"));
                    tag.smallTags.Add(smallTag);
                }
                tags.Add(tag);
            }
            tags[0].smallTags[0].Change(Colors.LightGreen, Colors.White, Colors.White);
            tags[0].Change(Colors.Red, Colors.White, Colors.White);
        }

        // 获取推荐歌单/分类歌单
        public static void GetSongSheetRecommend(ObservableCollection<ModelCollection> songSheets, string jsonStr, bool flag = true, int num = 4, bool flag2 = true, ObservableCollection<SongSheet> sheets = null)
        {
            if (songSheets != null)
                for (int i = 1; i < 11; i++)
                    songSheets[i].collection.Clear();
            if (sheets != null)
                sheets.Clear();
            JsonArray jsonArray = flag2 ? (flag ?
                (JsonObject.Parse(jsonStr).GetNamedObject("recomPlaylist").GetNamedObject("data").GetNamedArray("v_hot")) :
                (JsonObject.Parse(jsonStr).GetNamedObject("playlist").GetNamedObject("data").GetNamedArray("v_playlist"))) :
                JsonObject.Parse(jsonStr).GetNamedObject("data").GetNamedArray("list");
            for (int i = 0; i < jsonArray.Count(); i++)
            {
                JsonObject jb = jsonArray[i].GetObject();
                SongSheet songSheet = new SongSheet();
                songSheet.id = flag2 ? jb.GetNamedNumber(flag ? "content_id" : "tid").ToString() : jb.GetNamedString("dissid");
                songSheet.title = jb.GetNamedString(flag2 ? "title" : "dissname");
                songSheet.cover = new BitmapImage(new Uri(jb.GetNamedString(flag2 ? (flag ? "cover" : "cover_url_small") : "imgurl")));
                songSheet.visitnum = jb.GetNamedNumber(flag2 ? (flag ? "listen_num" : "access_num") : "listennum").ToString();
                if (flag2)
                    songSheets[i / num + 1].collection.Add(songSheet);
                else
                {
                    songSheet.date = jb.GetNamedString("createtime");
                    songSheet.createrName = jb.GetNamedObject("creator").GetNamedString("name");
                    sheets.Add(songSheet);
                }
            }
        }

        // 获得最新音乐 : songmid/title/interval/cover/singers
        public static void GetNewSongRecommend(ObservableCollection<ModelCollection> newSongs, string jsonStr)
        {
            for (int i = 1; i < 7; i++)
                newSongs[i].collection2.Clear();
            JsonArray jsonArr = JsonObject.Parse(jsonStr).GetNamedObject("new_song").GetNamedObject("data").GetNamedArray("song_list");
            for (int i = 0; i < 54; i++)
            {
                Song song = new Song(false);
                DealWithSong(song, jsonArr[i].GetObject());
                newSongs[i / 9 + 1].collection2.Add(song);
            }
        }

        // 获得专辑推送
        public static void GetNewAlbumRecommend(ObservableCollection<ModelCollection> newAlbums, string jsonStr, ModelCollection albums = null, bool flag = true)
        {
            if (albums == null)
                for (int i = 0; i < 4; i++)
                    newAlbums[i].collection3.Clear();
            else if (flag)
                albums.collection3.Clear();
            JsonArray jsonArr = JsonObject.Parse(jsonStr).GetNamedObject("new_album").GetNamedObject("data").GetNamedArray("list");
            for (int i = 0; i < 40 && i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Album album = new Album();
                album.id = jb.GetNamedString("album_mid");
                album.title = jb.GetNamedString("album_name");
                album.cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + album.id + ".jpg");
                album.singers = new List<string>();
                JsonArray jarray = jb.GetNamedArray("singers");
                foreach (var item in jarray)
                    album.singers.Add(item.GetObject().GetNamedString("singer_name"));
                if (albums == null)
                    newAlbums[i / 10 + 1].collection3.Add(album);
                else
                {
                    album.date = jb.GetNamedString("public_time");
                    albums.collection3.Add(album);
                }
            }
        }

        // 获得排行榜
        public static void GetTopListRecommend(ObservableCollection<Chart> charts, string jsonStr, bool flag, ObservableCollection<Chart> restCharts = null, ObservableCollection<Chart> globalCharts = null)
        {
            JsonArray jsonArr = flag ? JsonObject.Parse(jsonStr).GetNamedObject("toplist").GetNamedObject("data").GetNamedArray("group_list") : JsonArray.Parse(jsonStr);
            if (flag)
            {
                AddTopLists(charts, jsonArr.GetObjectAt(0).GetNamedArray("list"), "巅峰榜", 0, 2, true);
                AddTopLists(charts, jsonArr.GetObjectAt(1).GetNamedArray("list"), "全球榜", 0, 2, true, true);
            }
            else
            {
                JsonArray ja = jsonArr.GetObjectAt(0).GetNamedArray("List");
                JsonArray jb = jsonArr.GetObjectAt(1).GetNamedArray("List");
                AddTopLists(charts, ja, "巅峰榜", 0, 5, false);
                AddTopLists(restCharts, ja, "巅峰榜", 5, ja.Count(), false);
                AddTopLists(globalCharts, jb, "全球榜", 0, jb.Count(), false);
            }
        }

        // 添加排行榜
        public static void AddTopLists(ObservableCollection<Chart> charts, JsonArray jsonArr, string type, int start, int num, bool flag, bool flag2 = false)
        {
            // Random random = new Random();
            for (int i = start; i < num && i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Chart chart = new Chart();
                chart.id = jb.GetNamedNumber(flag ? "id" : "topID").ToString();
                chart.type = type;
                chart.title = type == "巅峰榜" ? jb.GetNamedString(flag ? "name" : "ListName").Split('·')[1] : jb.GetNamedString(flag ? "name" : "ListName");
                chart.date = jb.GetNamedString("update_key");
                JsonArray jarray = jb.GetNamedArray("songlist");
                chart.songs = new ObservableCollection<Song>();
                for (int j = 0; j < jarray.Count() && flag; j++)
                {
                    JsonObject jo = jarray[j].GetObject();
                    Song song = new Song(false);
                    song.title = (j + 1) + " " + jo.GetNamedString(flag ? "track_name" : "songname");
                    song.singers = new List<string>();
                    string[] singers = jo.GetNamedString(flag ? "singer_name" : "singername").Split('/');
                    foreach (var singer in singers)
                        song.singers.Add(singer);
                    song.type = "net";
                    chart.songs.Add(song);
                }
                if (flag)
                    //chart.picId = random.Next(5);
                    chart.picId = i + (flag2 ? 0 : 2);
                else
                    chart.cover.UriSource = new Uri(jb.GetNamedString(num == 5 ? "headPic_v12" : "MacDetailPicUrl"));
                chart.picUrl = jb.GetNamedString(flag ? "pic" : "headPic_v12");
                // total_song_num
                charts.Add(chart);
            }
        }

        // 专辑详情
        public static void DealWithAlbumMessage(Album album, string jsonStr)
        {
            JsonObject jb = JsonObject.Parse(jsonStr).GetNamedObject("data");
            album.id = jb.GetNamedString("mid");
            album.title = jb.GetNamedString("name");
            album.cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + album.id + ".jpg");
            album.singers = new List<string>(jb.GetNamedString("singername").Split('/'));
            album.date = jb.GetNamedString("aDate");
            album.company = jb.GetNamedString("company");
            album.genre = jb.GetNamedString("genre");
            album.lan = jb.GetNamedString("lan");
            album.desc = jb.GetNamedString("desc");
            album.num = (int)jb.GetNamedNumber("total_song_num");
            DealWithSongInAlbum(album.songs, jb.GetNamedArray("list"), album);
        }

        // 获取排行榜的歌曲
        /*
         * songs用于装歌
         * jsonStr用于拿到歌曲
         * num要拿到的歌曲的数量
         * flag用于指出是否是用于详情页(true)，还是排行榜页(false)
         */
        public static void DealWithSongInChart(ObservableCollection<Song> songs, string jsonStr, int num, bool flag, Chart chart)
        {
            JsonObject jb = JsonObject.Parse(jsonStr);
            songs.Clear();
            chart.num = (int)jb.GetNamedNumber("total_song_num");
            JsonArray jsonArr = jb.GetNamedArray("songlist");
            for (int i = 0; i < num && i < jsonArr.Count(); i++)
            {
                Song song = new Song(false);
                song.index = i + 1;
                DealWithSong(song, jsonArr[i].GetObject().GetNamedObject("data"), false);
                song.background.Color = i % 2 == 0 ? Color.FromArgb(255, 230, 230, 230) : Color.FromArgb(255, 250, 250, 250);
                song.type = "net";
                songs.Add(song);
            }
        }

        // 处理专辑中的歌曲信息
        public static void DealWithSongInAlbum(ObservableCollection<Song> songs, JsonArray jsonArr, Album album)
        {
            for (int i = 0; i < jsonArr.Count(); i++)
            {
                JsonObject jb = jsonArr[i].GetObject();
                Song song = new Song(false);
                song.songmid = jb.GetNamedString("songmid");
                song.title = jb.GetNamedString("songname");
                int t = (int)jb.GetNamedNumber("interval");
                song.interval = DealWithTime(t / 60) + ":" + DealWithTime(t % 60);
                song.cover = album.cover;
                List<string> vs = new List<string>();
                JsonArray singer = jb.GetNamedArray("singer");
                foreach (var item in singer)
                    vs.Add(item.GetObject().GetNamedString("name"));
                song.singers = vs;
                song.album = album;
                song.type = "net";
                songs.Add(song);
            }
        }

        // 歌曲页详情
        public static void DealWithSongMessage(Song song, string jsonStr)
        {
            JsonObject jb = JsonObject.Parse(jsonStr).GetNamedArray("data").GetObjectAt(0);
            DealWithSong(song, jb);
            song.album.id = jb.GetNamedObject("album").GetNamedString("mid");
            song.date = jb.GetNamedString("time_public");
        }

        // 处理歌曲中的歌词
        public static void DealWithSongLrc(ObservableCollection<Lyric> lyrics, string jsonStr)
        {
            string[] vs = jsonStr.Split('\n');
            string numStr = "0123456789";
            int index = 0;
            while (numStr.IndexOf(vs[index][1]) < 0)
                index++;
            for (; index < vs.Length; index++)
            {
                Lyric lyric = new Lyric();
                string[] strs = vs[index].Split(']');
                lyric.text = strs[1];
                lyric.strTime = strs[0];
                string[] t = strs[0].Substring(1).Split(':');
                lyric.intTime = double.Parse(t[0]) * 60 + double.Parse(t[1]);
                lyrics.Add(lyric);
            }
        }

        // 处理单个歌单的信息
        public static void DealWithSongSheet(SongSheet songSheet, string jsonStr)
        {
            JsonObject jobject = JsonObject.Parse(jsonStr).GetNamedArray("cdlist").GetObjectAt(0);
            songSheet.id = jobject.GetNamedString("disstid");
            songSheet.title = jobject.GetNamedString("dissname");
            songSheet.cover.UriSource = new Uri(jobject.GetNamedString("logo"));
            songSheet.visitnum = jobject.GetNamedNumber("visitnum").ToString();
            songSheet.num = (int)jobject.GetNamedNumber("songnum");
            songSheet.createrName = jobject.GetNamedString("nickname");
            songSheet.desc = jobject.GetNamedString("desc");
            JsonArray jarr = jobject.GetNamedArray("songlist");
            foreach (var item in jarr)
            {
                JsonObject jo = item.GetObject();
                Song song = new Song(false);
                DealWithSong(song, jo, false);
                song.album.id = jo.GetNamedString("albummid");
                song.album.title = jo.GetNamedString("albumname");
                song.type = "net";
                songSheet.songs.Add(song);
            }
            JsonArray jarr2 = jobject.GetNamedArray("tags");
            List<string> vs = new List<string>();
            foreach (var item in jarr2)
            {
                JsonObject jo = item.GetObject();
                // songSheet.tags.Add(new Tag(jo.GetNamedString("name"), (int)jo.GetNamedNumber("id")));
                vs.Add(jo.GetNamedString("name"));
            }
            songSheet.tag = vs;
        }

        // 处理单个歌曲的信息
        /*
         * flag用于区分json中格式 - false是排行榜, true是歌单与单独的歌
         */
        public static void DealWithSong(Song song, JsonObject jb, bool flag = true)
        {
            song.songmid = jb.GetNamedString(flag ? "mid" : "songmid");
            song.title = jb.GetNamedString(flag ? "title" : "songname");
            int t = (int)jb.GetNamedNumber("interval");
            song._time = t;
            song.interval = DealWithTime(t / 60) + ":" + DealWithTime(t % 60);
            song.album.id = flag ? jb.GetNamedObject("album").GetNamedString("mid") : jb.GetNamedString("albummid");
            song.cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + song.album.id + ".jpg");
            JsonArray jarray = jb.GetNamedArray("singer");
            List<string> vs = new List<string>();
            foreach (var item in jarray)
                vs.Add(item.GetObject().GetNamedString("name"));
            song.singers = vs;
            song.type = "net";
        }

        public static void GetSearchResult(SongList songList, string json)
        {
            songList.songs.Clear();
            JsonObject song = JsonObject.Parse(json).GetNamedObject("data").GetNamedObject("song");
            songList.Total = (int)song.GetNamedNumber("totalnum");
            songList.CurrentPage = (int)song.GetNamedNumber("curpage");
            JsonArray list = song.GetNamedArray("list");
            songList.Count = list.Count;
            for (int i = 0; i < list.Count; ++i)
            {
                JsonObject item = list[i].GetObject();
                Song s = new Song(false);
                s.songmid = item.GetNamedObject("file").GetNamedString("media_mid");
                s.title = item.GetNamedString("title");
                s.album.id = item.GetNamedObject("album").GetNamedString("mid");
                s.album.title = item.GetNamedObject("album").GetNamedString("title");
                int interval = (int)item.GetNamedNumber("interval");
                s.interval = string.Format("{0:00}:{1:00}", interval / 60, interval % 60);
                JsonArray singers = item.GetNamedArray("singer");
                for (int j = 0; j < singers.Count; ++j)
                {
                    s.singers.Add(singers[j].GetObject().GetNamedString("title"));
                }
                s.type = "net";
                songList.songs.Add(s);
            }
        }
        public static void GetSmarkBox(ModelCollection col, string json)
        {
            JsonArray songs = JsonObject.Parse(json).GetNamedObject("data").GetNamedObject("song").GetNamedArray("itemlist");
            JsonArray albums = JsonObject.Parse(json).GetNamedObject("data").GetNamedObject("album").GetNamedArray("itemlist");
            col.collection2.Clear();
            col.collection3.Clear();
            for (int i = 0; i < songs.Count; ++i)
            {
                JsonObject song = songs[i].GetObject();
                Song s = new Song(false);
                s.album.id = song.GetNamedString("albummid");
                s.songmid = song.GetNamedString("mid");
                s.title = song.GetNamedString("name");
                s.singers = new List<string>(song.GetNamedString("singer").Split('/'));
                s.type = "net";
                col.collection2.Add(s);
            }
            for (int i = 0; i < albums.Count; ++i)
            {
                JsonObject album = albums[i].GetObject();
                Album a = new Album();
                a.id = album.GetNamedString("mid");
                a.title = album.GetNamedString("name");
                a.singers = new List<string>(album.GetNamedString("singer").Split('/'));
                col.collection3.Add(a);
            }
        }
    }
}
