using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm3.Models;
using UWPMidTerm3.Services;
using UWPMidTerm3.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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
    public sealed partial class DownloadManager : Page
    {
        public MusicManager musicManager = MusicManager.getInstance();
        Song select = null;

        public DownloadManager()
        {
            this.InitializeComponent();
            musicManager.GetAllSongs(true);
            // Where.Text = UrlHelper.folder.Path;
        }

        private void PlaySelectMusic()
        {
            MainPage.songList.Copy(musicManager.downloadedLongList);
            for (int i = 0; i < MainPage.songList.songs.Count(); i++)
                if (MainPage.songList.songs[i].title == select.title)
                {
                    MainPage.index = i;
                    MainPage.continueFlag = true;
                    MainPage.notSingleFlag = true;
                    MainPage.playStype = 4;
                    return;
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
            data.Properties.Title = "Share " + select.title;
            data.SetStorageItems(new List<StorageFile> { select.musicFile });
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private void DisplayAll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainPage.songList.Copy(musicManager.downloadedLongList);
            MainPage.index = 0;
            MainPage.continueFlag = true;
            MainPage.notSingleFlag = true;
            MainPage.playStype = 4;
        }

        private void Cursor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Cursor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text))
            {
                musicManager.ReGetAllSongs(1);
                SearchMusic.ItemsSource = null;
                return;
            }
            musicManager.SaveAllSongs(1);
            SearchMusic.ItemsSource = musicManager.SearchSongs(sender.Text, 1);
        }

        private void SearchMusic_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null && musicManager.downloadedLongList.songs.Count() > 0)
                select = musicManager.downloadedLongList.songs[0];
            else
            {
                foreach (var song in musicManager.downloadedLongList.songs)
                    if (song.title.StartsWith((string)args.ChosenSuggestion))
                    {
                        select = musicManager.downloadedLongList.songs[0];
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

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            musicManager.DeleteSong(select, true, 1);
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SelectItem(sender);
            DataTransferManager.ShowShareUI();
        }

        private async void Where_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(UrlHelper.folder, new FolderLauncherOptions());
        }
    }
}
