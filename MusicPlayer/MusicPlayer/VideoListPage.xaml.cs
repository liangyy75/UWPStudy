using MusicPlayer.Models;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class VideoListPage : Page
    {
        private SongManager songManager = SongManager.getInstance();
        private VideoManager videoManager = VideoManager.GetInstance();
        bool flag = true;

        public VideoListPage()
        {
            this.InitializeComponent();
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Red;
            titleBar.ButtonBackgroundColor = Colors.Red;
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
            data.Properties.Title = "Share " + videoManager.select.Title;
            data.SetStorageItems(new List<StorageFile> { videoManager.select.VideoFile });
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private async void LocalVideo_Click(object sender, RoutedEventArgs e)
        {
            await videoManager.GetAllVediosFromVedioLibrary();
        }

        private async void AddVideo_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker vidioFilePicker = new FileOpenPicker();
            vidioFilePicker.ViewMode = PickerViewMode.Thumbnail;
            vidioFilePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            vidioFilePicker.FileTypeFilter.Add(".mp4");
            StorageFile file = await vidioFilePicker.PickSingleFileAsync();
            if (file != null)
            {
                await videoManager.AddVideo(file, false);
            }
        }

        private void MediaPlay_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void MyVideoList_ItemClick(object sender, ItemClickEventArgs e)
        {
            videoManager.select = (Video)e.ClickedItem;
            songManager.select = null;
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void MyVideoList_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    StorageFile storageFile = items[0] as StorageFile;
                    string contentType = storageFile.ContentType;
                    if(contentType == "video/mp4")
                    {
                        // StorageFile newFile = await storageFile.CopyAsync(ApplicationData.Current.LocalFolder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        await videoManager.AddVideo(storageFile, false);
                    }
                }
            }
        }

        private void MyVideoList_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drag to add a video";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            videoManager.select = (Video)s.DataContext;
            songManager.select = null;
            RightMenuFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            videoManager.DeleteVideo(videoManager.select.Id, false);
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void SearchVideo_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(flag == false)
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
            videoManager.SaveAllVideos();
            SearchVideo.ItemsSource = videoManager.SearchVideos(sender.Text);
        }

        private void SearchVideo_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (videoManager.videos.Count() > 0)
            {
                videoManager.SelectVideo(args.ChosenSuggestion);
                songManager.select = null;
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
            videoManager.ReGetAllVideos();
            BackButton.Visibility = Visibility.Collapsed;
            SearchVideo.ItemsSource = null;
            if(SearchVideo.Text.Length > 0)
                SearchVideo.Text = "";
        }
    }
}
