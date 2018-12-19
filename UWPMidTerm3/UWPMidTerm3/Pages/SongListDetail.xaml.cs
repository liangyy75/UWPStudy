using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm3.Models;
using UWPMidTerm3.Services;
using UWPMidTerm3.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPMidTerm3.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SongListDetail : Page
    {
        // 用于设定是否全部播放
        public static bool playAll = false;
        public static string id { get; set; }
        public static string type { get; set; }
        public static Chart chart { get; set; }
        public SongList songList = new SongList();

        public SongSheet songSheet = new SongSheet();
        public Album album = new Album();
        public int start = 0, end = 30;
        Song select = null;

        bool isDownload = false;

        public SongListDetail()
        {
            this.InitializeComponent();
            MyLoadResource();
        }

        private async void MyLoadResource()
        {
            try
            {
                LoadResource();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载失败，出现未知错误").ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage data = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            if (select != null)
            {
                data.Properties.Title = "Share " + select.title;
                data.SetWebLink(new Uri("https://y.qq.com/n/yqq/song/" + select.songmid + ".html"));
            }
            else
            {
                data.Properties.Title = "Share" + songList.title;
                if (type == "专辑")
                    data.SetWebLink(new Uri("https://y.qq.com/n/yqq/album/" + songList.id + ".html"));
                else if (type == "歌单")
                    data.SetWebLink(new Uri("https://y.qq.com/n/yqq/playsquare/" + songList.id + ".html"));
                else if (type == "排行榜")
                    data.SetWebLink(new Uri("https://y.qq.com/n/yqq/toplist/" + songList.id + ".html"));
            }
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private async void LoadResource()
        {
            if (type == "歌单")
            {
                string result = await UrlHelper.GetSongSheetMessage(id);
                JsonHelper.DealWithSongSheet(songSheet, result);
                songList.Copy((SongList)songSheet);
                ListenNum.Text = songSheet.visitnum.ToLower();
                CreaterName.Text = songSheet.createrName;
                Desc.Text = songSheet.desc;
                Tags.Text = string.Join("/", songSheet.tag.ToArray());
            }
            else if (type == "专辑")
            {
                string result = await UrlHelper.GetAlbumMessage(id);
                JsonHelper.DealWithAlbumMessage(album, result);
                songList.Copy((SongList)album);
                TypeName.Text = "[专辑]";
                SplitLine.Visibility = Visibility.Collapsed;
                ListenNumPanel.Visibility = Visibility.Collapsed;
                TagPanel.Visibility = Visibility.Collapsed;
                CreaterName.Text = string.Join("/", album.singers.ToArray());
                AlbumMessage.Visibility = Visibility.Visible;
                Desc.Text = album.desc;
            }
            else if (type == "排行榜")
            {
                chart.cover.UriSource = new Uri(chart.picUrl);
                string result = await UrlHelper.GetDifferentTopList(new string[5] { chart.date, chart.id, chart.type == "巅峰榜" ? "top" : "global", start.ToString(), end.ToString() });
                JsonHelper.DealWithSongInChart(chart.songs, result, 30, false, chart);
                songList.Copy((SongList)chart);
                TypeName.Text = "[排行榜]";
                SplitLine.Visibility = Visibility.Collapsed;
                ListenNumPanel.Visibility = Visibility.Collapsed;
                CreaterPanel.Visibility = Visibility.Collapsed;
                TagPanel.Visibility = Visibility.Collapsed;
                DescPanel.Visibility = Visibility.Collapsed;
                TopListPageChange.Visibility = Visibility.Visible;
            }
            if (playAll)
            {
                PlayAllSongs();
                playAll = false;
            }
        }

        private void PlayAllSongs()
        {
            MainPage.songList.Copy(this.songList);
            MainPage.index = 0;
            MainPage.continueFlag = true;
            MainPage.notSingleFlag = true;
            MainPage.playStype = 4;
        }

        private void myMusicList_ItemClick(object sender, ItemClickEventArgs e)
        {
            select = (Song)e.ClickedItem;
            PlaySelectMusic();
        }
        private void PlaySelectMusic()
        {
            MainPage.songList.Copy(this.songList);
            for (int i = 0; i < MainPage.songList.songs.Count(); i++)
                if (MainPage.songList.songs[i].songmid == select.songmid)
                {
                    MainPage.index = i;
                    MainPage.continueFlag = true;
                    MainPage.notSingleFlag = true;
                    MainPage.playStype = 4;
                    return;
                }
        }

        private void Cursor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Cursor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void DisplayAll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PlayAllSongs();
        }

        private void Collect_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 收藏songlist
        }

        private async void Download_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 下载songlist
            if (isDownload == false)
            {
                isDownload = true;
                foreach (var song in songList.songs)
                    MusicManager.DownloadSingMusic(song);
            }
            else
            {
                MessageDialog message = new MessageDialog("");
                message.Content = "该系列音乐已经下载或者下载失败";
                await message.ShowAsync();
            }
        }

        private void Share_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 分享songlist
            select = null;
            DataTransferManager.ShowShareUI();
        }

        private void SelectItem(object sender)
        {
            var s = sender as FrameworkElement;
            select = (Song)s.DataContext;
        }

        private void PlayMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // 播放单曲
            SelectItem(sender);
            PlaySelectMusic();
        }

        private void DownloadMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // 下载
            SelectItem(sender);
            MusicManager.DownloadSingMusic(select);
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // 分享单曲
            SelectItem(sender);
            DataTransferManager.ShowShareUI();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            int num = end;
            try
            {
                if (start == 0)
                    return;
                end = start;
                start -= 30;
                string result = await UrlHelper.GetDifferentTopList(new string[5] { chart.date, chart.id, chart.type == "巅峰榜" ? "top" : "global", start.ToString(), end.ToString() });
                JsonHelper.DealWithSongInChart(chart.songs, result, 30, false, chart);
                songList.Copy((SongList)chart);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                start += 30;
                end = num;
                await new MessageDialog("加载失败，出现未知错误").ShowAsync();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int num1 = start;
            int num2 = end;
            try
            {
                start = end;
                end = end < 90 ? end + 30 : 100;
                string result = await UrlHelper.GetDifferentTopList(new string[5] { chart.date, chart.id, chart.type == "巅峰榜" ? "top" : "global", start.ToString(), end.ToString() });
                JsonHelper.DealWithSongInChart(chart.songs, result, 30, false, chart);
                songList.Copy((SongList)chart);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                start = num1;
                end = num2;
                await new MessageDialog("加载失败，出现未知错误").ShowAsync();
            }
        }
    }
}
