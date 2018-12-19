using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace MusicPlayer.Services
{
    public class SongSearchService
    {
        public static async Task<string> GetMusic(string str)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(str);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private static async void GetMusicFile(string songmid, string albummid, string keyword)
        {
            Uri source = new Uri("http://ws.stream.qqmusic.qq.com/C100" + songmid + ".m4a?fromtag=0&guid=126548448");
            var file = await KnownFolders.MusicLibrary.CreateFileAsync(keyword + ".m4a", CreationCollisionOption.FailIfExists);
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, file);
            await download.StartAsync();

            Uri imgSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + albummid + ".jpg");
            var imgFile = await KnownFolders.PicturesLibrary.CreateFileAsync(keyword + ".jpg", CreationCollisionOption.FailIfExists);
            DownloadOperation imgDownload = downloader.CreateDownload(imgSource, imgFile);
            await imgDownload.StartAsync();
        }

        private static async void GetMusicLrc(string songmid)
        {
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.110 Safari/537.36");
            http.DefaultRequestHeaders.Add("Accept", "*/*");
            http.DefaultRequestHeaders.Add("Referer", "https://y.qq.com/portal/player.html");
            http.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8");
            http.DefaultRequestHeaders.Add("Cookie", "pgv_pvid=8455821612; ts_uid=1596880404; pgv_pvi=9708980224; yq_index=0; pgv_si=s3191448576; pgv_info=ssid=s8059271672; ts_refer=ADTAGmyqq; yq_playdata=s; ts_last=y.qq.com/portal/player.html; yqq_stat=0; yq_playschange=0; player_exist=1; qqmusic_fromtag=66; yplayer_open=1");
            http.DefaultRequestHeaders.Add("Host", "c.y.qq.com");
            HttpResponseMessage message = await http.GetAsync("https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?callback=MusicJsonCallback_lrc&pcachetime=1494070301711&songmid=" + songmid + "&g_tk=5381&jsonpCallback=MusicJsonCallback_lrc&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8¬ice=0&platform=yqq&needNewCode=0");
            string originResult = await message.Content.ReadAsStringAsync();
            string result = originResult.Substring(65, originResult.Length - 79);
            byte[] bytes = Convert.FromBase64String(result);
            result = Encoding.UTF8.GetString(bytes);
        }
    }
}
