using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPMidTerm.ViewModels
{
    public class UrlHelper
    {
        public static StorageFolder folder = ApplicationData.Current.LocalFolder;
        public static StorageFolder music;
        public static StorageFolder picture;

        public static async void PrePareFolder()
        {

            if (await folder.TryGetItemAsync("Music") == null)
            {
                music = await folder.CreateFolderAsync("Music");
            }
            music = await folder.GetFolderAsync("Music");
            if (await folder.TryGetItemAsync("Picture") == null)
            {
                picture = await folder.CreateFolderAsync("Picture");
            }
            picture = await folder.GetFolderAsync("Picture");
        }

        public static async Task<string> GetJson(string url, int num)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage message = await http.GetAsync(url);
            string originResult = await message.Content.ReadAsStringAsync();
            originResult = originResult.Substring(num, originResult.Length - num - 1);
            return originResult;
        }

        public static async Task<String> GetJson(string url, int num, Dictionary<string, string> headers)
        {

            HttpClient http = new HttpClient();
            foreach (KeyValuePair<string, string> k in headers)
            {
                http.DefaultRequestHeaders.Add(k.Key, k.Value);
            }
            HttpResponseMessage message = await http.GetAsync(url);
            string originResult = await message.Content.ReadAsStringAsync();
            originResult = originResult.Substring(num, originResult.Length - num - 1);
            return originResult;
        }

        private static async Task<string> GetSongSheet(string parameters)
        {
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?" + parameters, 22);
        }

        // 获取不同类型的推荐歌单
        public static async Task<string> GetDifferentSongSheet(int type)
        {
            switch (type)
            {
                // 为你推荐
                case 1: return await GetSongSheet("callback=recom8053969532389473&g_tk=297807046&jsonpCallback=recom8053969532389473&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22recomPlaylist%22%3A%7B%22method%22%3A%22get_hot_recommend%22%2C%22param%22%3A%7B%22async%22%3A1%2C%22cmd%22%3A2%7D%2C%22module%22%3A%22playlist.HotRecommendServer%22%7D%7D");
                // 热门游戏
                case 2: return await GetSongSheet("callback=recom5284318933561554&g_tk=297807046&jsonpCallback=recom5284318933561554&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A3259%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A3259%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D");
                // 情歌
                case 3: return await GetSongSheet("callback=recom41343104270005204&g_tk=297807046&jsonpCallback=recom41343104270005204&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A71%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A71%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D");
                // 经典
                case 4: return await GetSongSheet("callback=recom8073895163201923&g_tk=297807046&jsonpCallback=recom8073895163201923&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A59%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A59%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D");
                // KTV热歌
                case 5: return await GetSongSheet("callback=recom5651916363845559&g_tk=297807046&jsonpCallback=recom5651916363845559&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A64%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A64%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D");
                // 背景音乐
                case 6: return await GetSongSheet("callback=recom6304430267557626&g_tk=297807046&jsonpCallback=recom6304430267557626&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A107%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A107%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D");
            }
            return null;
        }

        // 获取不同类型的新歌
        public static async Task<string> GetDifferentNewSongs(int type)
        {
            switch (type)
            {
                // 内地
                case 1: return await GetSongSheet("callback=recom645782151483242&g_tk=297807046&jsonpCallback=recom645782151483242&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A1%7D%7D%7D");
                // 港台
                case 2: return await GetSongSheet("callback=recom4223895757624381&g_tk=297807046&jsonpCallback=recom4223895757624381&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A2%7D%7D%7D");
                // 欧美
                case 3: return await GetSongSheet("callback=recom9649167787777511&g_tk=297807046&jsonpCallback=recom9649167787777511&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A3%7D%7D%7D");
                // 日本
                case 4: return await GetSongSheet("callback=recom8174307841680741&g_tk=297807046&jsonpCallback=recom8174307841680741&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A4%7D%7D%7D");
                // 韩国
                case 5: return await GetSongSheet("callback=recom03985070795147805&g_tk=297807046&jsonpCallback=recom03985070795147805&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A5%7D%7D%7D");
            }
            return null;
        }

        // 获取所有mv
        public static async Task<string> GetAllMV()
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage message = await http.GetAsync("https://c.y.qq.com/mv/fcgi-bin/getmv_by_tag?g_tk=297807046&jsonpCallback=MusicJsonCallback5379769150836693&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=GB2312&notice=0&platform=yqq&needNewCode=0&cmd=shoubo&lan=all");
            string originResult = await message.Content.ReadAsStringAsync();
            originResult = originResult.Substring(33, originResult.Length - 34);
            return originResult;
        }

        // 获取所有信息
        public static async Task<string> GetAllMessage()
        {
            return await GetSongSheet("callback=recom6383134644624395&g_tk=297807046&jsonpCallback=recom6383134644624395&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22category%22%3A%7B%22method%22%3A%22get_hot_category%22%2C%22param%22%3A%7B%22qq%22%3A%22%22%7D%2C%22module%22%3A%22music.web_category_svr%22%7D%2C%22recomPlaylist%22%3A%7B%22method%22%3A%22get_hot_recommend%22%2C%22param%22%3A%7B%22async%22%3A1%2C%22cmd%22%3A2%7D%2C%22module%22%3A%22playlist.HotRecommendServer%22%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A8%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A8%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A0%7D%7D%2C%22new_album%22%3A%7B%22module%22%3A%22music.web_album_library%22%2C%22method%22%3A%22get_album_by_tags%22%2C%22param%22%3A%7B%22area%22%3A7%2C%22company%22%3A-1%2C%22genre%22%3A-1%2C%22type%22%3A-1%2C%22year%22%3A-1%2C%22sort%22%3A2%2C%22get_tags%22%3A1%2C%22sin%22%3A0%2C%22num%22%3A40%2C%22click_albumid%22%3A0%7D%7D%2C%22toplist%22%3A%7B%22module%22%3A%22music.web_toplist_svr%22%2C%22method%22%3A%22get_toplist_index%22%2C%22param%22%3A%7B%7D%7D%2C%22focus%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetFocus%22%2C%22param%22%3A%7B%7D%7D%7D");
            // JsonObject pairs = JsonObject.Parse(result);
            // 新歌
            // JsonObject newSong = pairs["new_song"].GetObject();
            // 推荐歌单
            // JsonObject recomPlayList = pairs["recomPlaylist"].GetObject();
            // 排行榜
            // JsonObject topList = pairs["toplist"].GetObject();
            // 专辑
            // JsonObject newAlbum = pairs["new_album"].GetObject();
        }

        // 获取对应歌单
        public static async Task<string> GetSongSheetByDisstid(string disstid)
        {
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Referer", "https://y.qq.com/n/yqq/playsquare" + disstid + ".html");
            HttpResponseMessage message = await http.GetAsync("https://c.y.qq.com/qzone/fcg-bin/fcg_ucc_getcdinfo_byids_cp.fcg?type=1&json=1&utf8=1&onlysong=0&disstid=" + disstid + "&format=jsonp&g_tk=5381&jsonpCallback=playlistinfoCallback&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0");
            string originResult = await message.Content.ReadAsStringAsync();
            originResult = originResult.Substring(21, originResult.Length - 22);
            return originResult;
        }

        // 得到图片
        public static async void GetFile(Uri uri, StorageFile file, BitmapImage cover)
        {
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, file);
            await download.StartAsync();
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                await cover.SetSourceAsync(stream);
            }
        }

        // 获取所有排行榜
        public static async Task<string> GetAllCharts()
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_opt.fcg?page=index&format=html&tpl=macv4&v8debug=1&jsonCallback=jsonCallback", 14);
        }

        // 获取具体排行榜
        public static async Task<string> GetSpecificChart(int topid, string update_key, int num)
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_cp.fcg?tpl=3&page=detail&date=" + update_key + "&topid=" + topid + "&type=top&song_begin=0&song_num=" + num + "&g_tk=5381&jsonpCallback=MusicJsonCallbacktoplist&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 26);
        }

        // 获取某些专辑
        public static async Task<string> GetSomeAlbums(string[] parameters)
        {
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=getUCGI21086710380316087&g_tk=5381&jsonpCallback=getUCGI21086710380316087&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22albumlib%22%3A%7B%22method%22%3A%22get_album_by_tags%22%2C%22param%22%3A%7B%22area%22%3A" + parameters[0] + "%2C%22company%22%3A" + parameters[1] + "%2C%22genre%22%3A" + parameters[2] + "%2C%22type%22%3A" + parameters[3] + "%2C%22year%22%3A" + parameters[4] + "%2C%22sort%22%3A2%2C%22get_tags%22%3A1%2C%22sin%22%3A" + parameters[5] + "%2C%22num%22%3A" + parameters[6] +"%2C%22click_albumid%22%3A0%7D%2C%22module%22%3A%22music.web_album_library%22%7D%7D", 25);
        }

        //获取分类歌单标签
        public static async Task<string> GetPlayListTags()
        {
            string url = "https://c.y.qq.com/splcloud/fcgi-bin/fcg_get_diss_tag_conf.fcg?g_tk=5381&jsonpCallback=getPlaylistTags&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0";
            return await GetJson(url, 16);
        }
        //通过分类歌单的标签id获取歌曲
        public static async Task<string> GetPlayListByTagId(string catetoryId, string sortId, string sin, string ein)
        {
            string url = "https://c.y.qq.com/splcloud/fcgi-bin/fcg_get_diss_by_tag.fcg?picmid=1&rnd=0.47072528817989&g_tk=5381&jsonpCallback=getPlaylist&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&categoryId=" + catetoryId + "&sortId=" + sortId + "&sin=" + sin + "&ein=" + ein;
            Dictionary<string, string> heades = new Dictionary<string, string>();
            heades.Add("Referer", "https://y.qq.com/portal/playlist.html");
            return await GetJson(url, 12, heades);
        }

        // 通过songmid获取歌曲信息
        public static async Task<string> GetSongMessageBySongmid(string songmid)
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg?songmid=" + songmid + "&tpl=yqq_song_detail&format=jsonp&callback=getOneSongInfoCallback&g_tk=5381&jsonpCallback=getOneSongInfoCallback&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 23);
        }
    }
}
