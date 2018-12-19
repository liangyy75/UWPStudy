using MusicPlayer.Models;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MusicPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MusicListPage : Page
    {
        private SongManager songManager = SongManager.getInstance();
        private VideoManager videoManager = VideoManager.GetInstance();
        bool flag = true;

        public MusicListPage()
        {
            this.InitializeComponent();
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Red;
            titleBar.ButtonBackgroundColor = Colors.Red;
            SearchMusic.MaxSuggestionListHeight = 30;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GoBack();
            DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage data = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            data.Properties.Title = "Share " + songManager.select.Title;
            data.SetStorageItems(new List<StorageFile> { songManager.select.MusicFile });
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private void LocalMusic_Click(object sender, RoutedEventArgs e)
        {
            songManager.GetAllSongsFromMusicLibrary();
        }

        private async void AddMusic_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker musicFilePicker = new FileOpenPicker();
            musicFilePicker.ViewMode = PickerViewMode.Thumbnail;
            musicFilePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            musicFilePicker.FileTypeFilter.Add(".mp3");
            musicFilePicker.FileTypeFilter.Add(".m4a");
            StorageFile file = await musicFilePicker.PickSingleFileAsync();
            if (file != null)
            {
                await songManager.AddSong(file, false);
            }
        }

        private void MediaPlay_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void MyMusicList_ItemClick(object sender, ItemClickEventArgs e)
        {
            songManager.select = (Song)e.ClickedItem;
            videoManager.select = null;
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SelectItem(object sender)
        {
            var s = sender as FrameworkElement;
            songManager.select = (Song)s.DataContext;
            videoManager.select = null;
        }

        private void PlayMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            this.Frame.Navigate(typeof(MainPage));
        }

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            songManager.DeleteSong(songManager.select.id, false);
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            DataTransferManager.ShowShareUI();
        }

        private async void MyMusicList_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    StorageFile storageFile = items[0] as StorageFile;
                    string contentType = storageFile.ContentType;
                    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                    if (contentType == "audio/mpeg" || contentType == "audio/mp4")
                    {
                        // StorageFile newFile = await storageFile.CopyAsync(storageFolder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        await songManager.AddSong(storageFile, false);
                    }
                }
            }
        }

        private void MyMusicList_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drag to add a audio";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private void SearchMusic_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (flag == false)
            {
                flag = true;
                return;
            }
            if (string.IsNullOrEmpty(sender.Text))
            {
                GoBack();
                return;
            }
            BackButton.Visibility = Visibility.Visible;
            songManager.SaveAllSongs();
            SearchMusic.ItemsSource = songManager.SearchSongs(sender.Text);
        }

        private void SearchMusic_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if(songManager.songs.Count() > 0)
            {
                songManager.SelectMusic(args.ChosenSuggestion);
                videoManager.select = null;
            }
            GoBack();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            GoBack();
        }

        private void GoBack()
        {
            songManager.ReGetAllSongs();
            BackButton.Visibility = Visibility.Collapsed;
            SearchMusic.ItemsSource = null;
            if(SearchMusic.Text.Length > 0)
                SearchMusic.Text = "";
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            MyScrollViewer.ChangeView(null, MyScrollViewer.ScrollableHeight, null);
            Test.Content = MyScrollViewer.ScrollableHeight.ToString() + " " + MyScrollViewer.VerticalOffset.ToString();
        }
    }
}

/**
 * 最新音乐 https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_cp.fcg?g_tk=5381&uin=0&format=json&inCharset=utf-8&outCharset=utf-8&notice=0&platform=h5&needNewCode=1&tpl=3&page=detail&type=top&topid=27&_=1519963122923
 * url:url, type:"get", dataType:'jsonp', jsonp: "jsonpCallback", scriptCharset: 'GBK',//解决中文乱码
 * 
 * 推荐音乐 https://c.y.qq.com/v8/fcg-bin/fcg_v8_toplist_cp.fcg?g_tk=5381&uin=0&format=json&inCharset=utf-8&outCharset=utf-8&notice=0&platform=h5&needNewCode=1&tpl=3&page=detail&type=top&topid=36&_=1520777874472
 * url:url, type:"get", dataType:'jsonp', jsonp: "jsonpCallback", scriptCharset: 'GBK',//解决中文乱码
 * 
 * 歌曲 http://ws.stream.qqmusic.qq.com/C100+songmid+.m4a?fromtag=0&guid=126548448
 * url:url, type:"get", dataType:'jsonp', jsonp: "callback", jsonpCallback:'callback', scriptCharset: 'GBK',//解决中文乱码
 * 
 * 歌词 https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?callback=MusicJsonCallback_lrc&pcachetime=1494070301711&songmid=+songId+&g_tk=5381&jsonpCallback=MusicJsonCallback_lrc&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8¬ice=0&platform=yqq&needNewCode=0
 * url: url,
    headers: {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.110 Safari/537.36",
        "Accept": "*//*",(只有一个/，另一个用来转义)
        "Referer": "https://y.qq.com/portal/player.html",
        "Accept-Language": "zh-CN,zh;q=0.8",
        "Cookie": "pgv_pvid=8455821612; ts_uid=1596880404; pgv_pvi=9708980224; yq_index=0; pgv_si=s3191448576; pgv_info=ssid=s8059271672; ts_refer=ADTAGmyqq; yq_playdata=s; ts_last=y.qq.com/portal/player.html; yqq_stat=0; yq_playschange=0; player_exist=1; qqmusic_fromtag=66; yplayer_open=1",
        "Host": "c.y.qq.com",
    }
 */
