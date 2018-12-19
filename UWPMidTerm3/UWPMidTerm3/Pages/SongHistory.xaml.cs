using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm3.Models;
using UWPMidTerm3.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class SongHistory : Page
    {
        public MusicManager musicManager = MusicManager.getInstance();
        Song select = null;

        public SongHistory()
        {
            this.InitializeComponent();
            musicManager.GetAllSongsFromDataBase();
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
            data.Properties.Title = "Share " + select.title;
            if (select.musicFile != null)
                data.SetStorageItems(new List<StorageFile> { select.musicFile });
            else
                data.SetWebLink(new Uri("https://y.qq.com/n/yqq/song/" + select.songmid + ".html"));
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private void Cursor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Cursor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void PlaySelectMusic()
        {
            MainPage.songList.Copy(musicManager.historySongList);
            for (int i = 0; i < MainPage.songList.songs.Count(); i++)
                if (MainPage.songList.songs[i].songmid == select.songmid && select.type == "net" 
                    || MainPage.songList.songs[i].fileName == select.fileName)
                {
                    MainPage.index = i;
                    MainPage.continueFlag = true;
                    MainPage.notSingleFlag = true;
                    MainPage.playStype = 4;
                    return;
                }
        }

        private void DisplayAll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainPage.songList.Copy(musicManager.historySongList);
            MainPage.index = 0;
            MainPage.continueFlag = true;
            MainPage.notSingleFlag = true;
            MainPage.playStype = 4;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text))
            {
                SearchMusic.ItemsSource = null;
                musicManager.ReGetAllSongs(2);
                return;
            }
            musicManager.SaveAllSongs(2);
            SearchMusic.ItemsSource = musicManager.SearchSongs(sender.Text, 2);
        }

        private void SearchMusic_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null && musicManager.historySongList.songs.Count() > 0)
                select = musicManager.historySongList.songs[0];
            else
            {
                foreach (var song in musicManager.historySongList.songs)
                    if (song.title.StartsWith((string)args.ChosenSuggestion))
                    {
                        select = musicManager.historySongList.songs[0];
                        break;
                    }
            }
            PlaySelectMusic();
        }

        private void myMusicList_ItemClick(object sender, ItemClickEventArgs e)
        {
            select = ((Song)e.ClickedItem);
            PlaySelectMusic();
        }

        private void SelectItem(object sender)
        {
            var s = sender as FrameworkElement;
            select = (Song)s.DataContext;
        }

        private void PlayMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            PlaySelectMusic();
        }

        private void SmgMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            MenuFlyoutItem item = (MenuFlyoutItem)sender;
            if (item.Text == "下载")
                MusicManager.DownloadSingMusic(select);
            else if (item.Text == "删除本地")
                musicManager.DeleteSong(select, false);
            else if (item.Text == "删除已下载")
                musicManager.DeleteSong(select, true, 2);
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            DataTransferManager.ShowShareUI();
        }
    }
}
