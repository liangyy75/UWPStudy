using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace UWPMidTerm3.Services
{
    public class UrlHelper
    {
        public static StorageFolder folder = ApplicationData.Current.LocalFolder;
        public static StorageFolder music;
        public static StorageFolder picture;

        public static async void PrePareFolder()
        {
            if (await folder.TryGetItemAsync("Music") == null)
                music = await folder.CreateFolderAsync("Music");
            music = await folder.GetFolderAsync("Music");
            if (await folder.TryGetItemAsync("Picture") == null)
                picture = await folder.CreateFolderAsync("Picture");
            picture = await folder.GetFolderAsync("Picture");
        }

        public static async Task<String> GetJson(string url, int num, int len = 1, Dictionary<string, string> headers = null)
        {

            HttpClient http = new HttpClient();
            if (headers != null)
                foreach (KeyValuePair<string, string> k in headers)
                    http.DefaultRequestHeaders.Add(k.Key, k.Value);
            HttpResponseMessage message = await http.GetAsync(url);
            string originResult = await message.Content.ReadAsStringAsync();
            originResult = originResult.Substring(num, originResult.Length - num - len);
            return originResult;
        }

        // 获取所有信息
        public static async Task<string> GetAllMessage()
        {
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=recom6383134644624395&g_tk=297807046&jsonpCallback=recom6383134644624395&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22category%22%3A%7B%22method%22%3A%22get_hot_category%22%2C%22param%22%3A%7B%22qq%22%3A%22%22%7D%2C%22module%22%3A%22music.web_category_svr%22%7D%2C%22recomPlaylist%22%3A%7B%22method%22%3A%22get_hot_recommend%22%2C%22param%22%3A%7B%22async%22%3A1%2C%22cmd%22%3A2%7D%2C%22module%22%3A%22playlist.HotRecommendServer%22%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A8%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A8%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A0%7D%7D%2C%22new_album%22%3A%7B%22module%22%3A%22music.web_album_library%22%2C%22method%22%3A%22get_album_by_tags%22%2C%22param%22%3A%7B%22area%22%3A7%2C%22company%22%3A-1%2C%22genre%22%3A-1%2C%22type%22%3A-1%2C%22year%22%3A-1%2C%22sort%22%3A2%2C%22get_tags%22%3A1%2C%22sin%22%3A0%2C%22num%22%3A40%2C%22click_albumid%22%3A0%7D%7D%2C%22toplist%22%3A%7B%22module%22%3A%22music.web_toplist_svr%22%2C%22method%22%3A%22get_toplist_index%22%2C%22param%22%3A%7B%7D%7D%2C%22focus%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetFocus%22%2C%22param%22%3A%7B%7D%7D%7D", 22);
        }

        // 获取不同类型的推荐歌单
        public static async Task<string> GetDifferentSongSheet(string type, int id)
        {
            // 为你推荐
            if (type == "" && id == 0)
                return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=recom8053969532389473&g_tk=297807046&jsonpCallback=recom8053969532389473&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22recomPlaylist%22%3A%7B%22method%22%3A%22get_hot_recommend%22%2C%22param%22%3A%7B%22async%22%3A1%2C%22cmd%22%3A2%7D%2C%22module%22%3A%22playlist.HotRecommendServer%22%7D%7D", 22);
            // 其他歌曲
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=recom5284318933561554&g_tk=297807046&jsonpCallback=recom5284318933561554&loginUin=3249475320&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22playlist%22%3A%7B%22method%22%3A%22get_playlist_by_category%22%2C%22param%22%3A%7B%22id%22%3A" + id + "%2C%22curPage%22%3A1%2C%22size%22%3A40%2C%22order%22%3A5%2C%22titleid%22%3A" + id + "%7D%2C%22module%22%3A%22playlist.PlayListPlazaServer%22%7D%7D", 22);
        }

        // 获得歌单分类
        public static async Task<string> GetSongSheetTags()
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Referer", "https://y.qq.com/portal/playlist.html");
            return await GetJson("https://c.y.qq.com/splcloud/fcgi-bin/fcg_get_diss_tag_conf.fcg?g_tk=5381&jsonpCallback=getPlaylistTags&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 16, 1, pairs);
        }

        // 获得分类歌单
        public static async Task<string> GetDetailSongSheets(int[] paras)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Referer", "https://y.qq.com/portal/playlist.html");
            return await GetJson("https://c.y.qq.com/splcloud/fcgi-bin/fcg_get_diss_by_tag.fcg?picmid=1&rnd=0.5099222235589547&g_tk=5381&jsonpCallback=getPlaylist&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&categoryId=" + paras[0] + "&sortId=" + paras[1] + "&sin=" + paras[2] + "&ein=" + paras[3], 12, 1, pairs);
        }

        // 获得不同类型的新歌
        public static async Task<string> GetDifferentNewSong(int id)
        {
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=recom4629703711402189&g_tk=5381&jsonpCallback=recom4629703711402189&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_song%22%3A%7B%22module%22%3A%22QQMusic.MusichallServer%22%2C%22method%22%3A%22GetNewSong%22%2C%22param%22%3A%7B%22type%22%3A" + id + "%7D%7D%7D", 22);
        }

        // 获得不同类型的专辑推送
        public static async Task<string> GetDifferentNewAlbum(int[] paras)
        {
            return await GetJson("https://u.y.qq.com/cgi-bin/musicu.fcg?callback=recom8359995610134126&g_tk=5381&jsonpCallback=recom8359995610134126&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&data=%7B%22comm%22%3A%7B%22ct%22%3A24%7D%2C%22new_album%22%3A%7B%22module%22%3A%22music.web_album_library%22%2C%22method%22%3A%22get_album_by_tags%22%2C%22param%22%3A%7B%22area%22%3A" + paras[0] + "%2C%22company%22%3A" + paras[1] + "%2C%22genre%22%3A" + paras[2] + "%2C%22type%22%3A" + paras[3] + "%2C%22year%22%3A" + paras[4] + "%2C%22sort%22%3A2%" + paras[5] + "C%22get_tags%22%3A" + paras[6] + "%2C%22sin%22%3A" + paras[7] + "%2C%22num%22%3A" + paras[8] + "%2C%22click_albumid%22%3A0%7D%7D%7D", 22);
        }

        // 获得不同的排行榜
        public static async Task<string> GetDifferentTopList(string[] paras)
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_cp.fcg?tpl=3&page=detail&date=" + paras[0] + "&topid=" + paras[1] + "&type=" + paras[2] + "&song_begin=" + paras[3] + "&song_num=" + paras[4] + "&g_tk=5381&jsonpCallback=MusicJsonCallbacktoplist&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 26);
        }

        // 获得所有的排行榜的部分信息
        public static async Task<string> GetAllTopList()
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_opt.fcg?page=index&format=html&tpl=macv4&v8debug=1&jsonCallback=jsonCallback", 14);
        }


        // 获得单个歌单信息
        public static async Task<string> GetSongSheetMessage(string disstid)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Referer", "https://y.qq.com/n/yqq/playsquare/" + disstid + ".html");
            return await GetJson("https://c.y.qq.com/qzone/fcg-bin/fcg_ucc_getcdinfo_byids_cp.fcg?type=1&json=1&utf8=1&onlysong=0&disstid=" + disstid + "&format=jsonp&g_tk=5381&jsonpCallback=playlistinfoCallback&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 21, 1, pairs);
        }

        // 获得单个歌曲信息
        public static async Task<string> GetSongMessage(string songmid)
        {
            // Dictionary<string, string> pairs = new Dictionary<string, string>();
            // pairs.Add("Referer", "https://y.qq.com/n/yqq/song/" + songmid + ".html");
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg?songmid=" + songmid + "&tpl=yqq_song_detail&format=jsonp&callback=getOneSongInfoCallback&g_tk=5381&jsonpCallback=getOneSongInfoCallback&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 23);
        }

        // 获得单个专辑信息
        public static async Task<string> GetAlbumMessage(string albummid)
        {
            return await GetJson("https://c.y.qq.com/v8/fcg-bin/fcg_v8_album_info_cp.fcg?albummid=" + albummid + "&g_tk=5381&jsonpCallback=getAlbumInfoCallback&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 22);
        }

        // 获得歌词
        public static async Task<string> GetMusicLrc(string songmid)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Referer", "https://y.qq.com/portal/player.html");
            string result = await GetJson("https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?callback=MusicJsonCallback_lrc&pcachetime=1494070301711&songmid=" + songmid + "&g_tk=5381&jsonpCallback=MusicJsonCallback_lrc&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0", 65, 14, pairs);
            byte[] bytes = Convert.FromBase64String(result);
            result = Encoding.UTF8.GetString(bytes);
            return result;
        }


        // 下载单曲
        public static async Task<bool> DownloadMusic(string songmid, string albummid, string name)
        {
            try
            {
                BackgroundDownloader downloader = new BackgroundDownloader();
                Uri source = new Uri("http://ws.stream.qqmusic.qq.com/C100" + songmid + ".m4a?fromtag=0&guid=126548448");
                var file = await music.CreateFileAsync(name + ".m4a", CreationCollisionOption.FailIfExists);
                DownloadOperation download = downloader.CreateDownload(source, file);
                await download.StartAsync();

                Uri imgSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + albummid + ".jpg");
                var imgFile = await picture.CreateFileAsync(name + ".jpg", CreationCollisionOption.FailIfExists);
                DownloadOperation imgDownload = downloader.CreateDownload(imgSource, imgFile);
                await imgDownload.StartAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        /*
         * @w:搜索关键字
         * @p:第几页,从1开始
         * @count:每页的显示多少个
         * 
         */
        public static async Task<String> Search(string w, int p, int count)
        {
            string url = "https://c.y.qq.com/soso/fcgi-bin/client_search_cp?ct=24&qqmusic_ver=1298&new_json=1&remoteplace=txt.yqq.song&searchid=69534579974505535&t=0&aggr=1&cr=1&catZhida=1&lossless=0&flag_qc=0&p=" + p + "&n=" + count + "&w=" + w + "&g_tk=5381&jsonpCallback=MusicJsonCallback8175481381018259&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0";
            string json = await GetJson(url, 34);
            return json;
        }

        public static async Task<String> GetSmarkBox(string key)
        {
            string url = "https://c.y.qq.com/splcloud/fcgi-bin/smartbox_new.fcg?is_xml=0&format=jsonp&key=" + key + "&g_tk=5381&jsonpCallback=SmartboxKeysCallbackmod_search4330&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0";
            string json = await GetJson(url, 35);
            return json;
        }
    }
}
