using HtmlAgilityPack;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

/**
 * 可能做到的扩展：
 * 创建歌单，用数据库存储歌单。
 * 播放网上乐曲。
 * 搜索功能：用c#爬虫爬取网上mp3、mp4资源，利用网易云音乐API等等，同时获取歌词的json数据。
 * 不仅限于mp3和mp4资源。
 * 增加uwp动画。
 * 
 * 可以的改进：
 * 用工厂模式管理song和video。
 * 用Frame来管理MusicListPage、VideoListPage等等。
 * 用UserControl来管理MusicListPage、VideoListPage，使得只要一个Page就能显示两种List。
 * 视频、音乐停止时并非戛然而止，而是慢慢的变小，播放时同理。
 */

namespace MusicPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SongManager songManager = SongManager.getInstance();
        private VideoManager videoManager = VideoManager.GetInstance();

        MediaPlayer mediaPlayer = new MediaPlayer();
        MediaTimelineController mediaTimelineController = new MediaTimelineController();
        TimeSpan _duration;
        DispatcherTimer screenTimer = new DispatcherTimer();
        bool flag = true;

        public MainPage()
        {
            this.InitializeComponent();
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Red;
            titleBar.ButtonBackgroundColor = Colors.Red;
            this.SizeChanged += (s, e) =>
            {
                double width = e.NewSize.Width;
                if (width > 800)
                {
                    MusicProgress.Width = (width - 360) * 2 / 3 - 80;
                    VolumeRange.Width = (width - 360) / 3 - 40;
                }
            };

            mediaPlayer.TimelineController = mediaTimelineController;
            MyMediaPlayerElement.SetMediaPlayer(mediaPlayer);

            screenTimer.Interval = TimeSpan.FromMilliseconds(1);
            screenTimer.Tick += screenTimer_Tick;
            screenTimer.Start();
        }

        private void screenTimer_Tick(object sender, object e)
        {
            if (ApplicationView.GetForCurrentView().IsFullScreenMode == false)
            {
                if (MyMediaPlayerElement.IsFullWindow)
                    MyMediaPlayerElement.IsFullWindow = false;
                ChangeScreen.Symbol = Symbol.FullScreen;
                ScreenControl.Label = "FullScreen";
            }
        }

        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            _duration = sender.Duration.GetValueOrDefault();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MusicProgress.Minimum = 0;
                MusicProgress.Maximum = _duration.TotalSeconds;
                MusicProgress.StepFrequency = 1;
            });
        }

        private void MusicButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MusicListPage));
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoListPage));
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker vidioFilePicker = new FileOpenPicker();
            vidioFilePicker.ViewMode = PickerViewMode.Thumbnail;
            vidioFilePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            vidioFilePicker.FileTypeFilter.Add(".mp4");
            vidioFilePicker.FileTypeFilter.Add(".mp3");
            StorageFile file = await vidioFilePicker.PickSingleFileAsync();
            if (file != null)
            {
                if (file.ContentType == "video/mp4")
                {
                    await videoManager.AddVideo(file, true);
                    songManager.select = null;
                }
                else if(file.ContentType == "audio/mpeg")
                {
                    await songManager.AddSong(file, true);
                    videoManager.select = null;
                }
                if(songManager.select != null || videoManager.select != null)
                {
                    StopMedia();
                    PlayMedia();
                    StartMedia();
                }
            }
        }

        private void StartMedia()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            mediaTimelineController.Start();
            if(songManager.select != null)
                EllipseStoryboard.Begin();
        }

        private void Timer_Tick(object sender, object e)
        {
            if(songManager.select == null && videoManager.select == null)
                return;
            TimeSpan time = ((TimeSpan)mediaTimelineController.Position);
            MusicProgress.Value = time.TotalSeconds;
            MusicTime.Text = readTime(time);
            if (MusicProgress.Value == MusicProgress.Maximum)
            {
                mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                mediaTimelineController.Pause();
                if(songManager.select != null)
                    EllipseStoryboard.Stop();
                ReStartMedia();
            }
        }

        private void PlayMedia()
        {
            StorageFile file = null;
            if (songManager.select != null)
            {
                AlbumCover.Visibility = Visibility.Visible;
                file = songManager.select.MusicFile;
                MusicTotalTime.Text = readTime(songManager.select.Duration);
                MyImage.ImageSource = songManager.select.AlbumCover;
                EllipseStoryboard.Begin();
            }
            else if(videoManager.select != null)
            {
                AlbumCover.Visibility = Visibility.Collapsed;
                file = videoManager.select.VideoFile;
                MusicTotalTime.Text = readTime(videoManager.select.Duration);
            }
            var mediaSource = MediaSource.CreateFromStorageFile(file);
            MyMediaPlayerElement.Source = mediaSource;
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
        }

        private string readTime(TimeSpan time)
        {
            int seconds = ((int)time.TotalSeconds) % 60;
            int minutes = ((int)time.TotalMinutes) % 60;
            return ExtendString(minutes.ToString()) + ":" + ExtendString(seconds.ToString());
        }

        private string ExtendString(string str)
        {
            if (str.Length == 1)
            {
                return "0" + str;
            }
            return str;
        }

        private void StopMedia()
        {

            mediaTimelineController.Position = TimeSpan.FromSeconds(0);
            mediaTimelineController.Pause();
            MusicTime.Text = "00:00";
            MusicProgress.Value = 0;
            if (songManager.select != null)
                EllipseStoryboard.Stop();
        }

        private void DeleteMedia()
        {
            MyMediaPlayerElement.Source = null;
            MyImage.ImageSource = null;
            MusicTotalTime.Text = "00:00";
        }

        private void ReStartMedia()
        {
            if (songManager.select != null)
            {
                if (songManager.songStyle == 2)
                    songManager.select = songManager.songs[new Random().Next(songManager.songs.Count())];
                else if (songManager.songStyle == 1)
                {
                    for (int i = 0; i < songManager.songs.Count(); i++)
                        if (songManager.songs[i] == songManager.select)
                        {
                            songManager.select = songManager.songs[(i + 1) % songManager.songs.Count()];
                            break;
                        }
                }
                if (songManager.songStyle != 0)
                {
                    StopMedia();
                    PlayMedia();
                    StartMedia();
                }
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (songManager.select != null || videoManager.select != null)
            {
                PlayMedia();
                StartMedia();
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                if (songManager.select != null)
                    EllipseStoryboard.Pause();
                mediaTimelineController.Pause();
            }
            else
            {
                if (songManager.select != null)
                    EllipseStoryboard.Begin();
                mediaTimelineController.Resume();
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopMedia();
        }

        private void ScreenControl_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            /*if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
                ChangeScreen.Symbol = Symbol.FullScreen;
                ScreenControl.Label = "FullScreen";
            }
            else*/ if(!view.IsFullScreenMode)
            {
                view.TryEnterFullScreenMode();
                ChangeScreen.Symbol = Symbol.BackToWindow;
                ScreenControl.Label = "BackToWindow";
                if (videoManager.select != null)
                    MyMediaPlayerElement.IsFullWindow = true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            StopMedia();
            if(songManager.select != null)
            {
                songManager.DeleteSong(songManager.select.id, false);
            }
            if(videoManager.select != null)
            {
                videoManager.DeleteVideo(videoManager.select.Id, false);
            }
            // mediaPlayer.Dispose();
            DeleteMedia();
        }

        private void VolumeRange_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaPlayer.Volume = (double)VolumeRange.Value;
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
            List<string> suggestions = new List<string>();
            foreach (var song in songManager.songs)
            {
                if (song.Title.StartsWith(sender.Text))
                {
                    suggestions.Add(song.Title);
                }
            }
            for (int i = 0; i < videoManager.videos.Count(); i++)
            {
                if (videoManager.videos[i].Title.StartsWith(sender.Text))
                {
                    suggestions.Add(videoManager.videos[i].Title);
                }
            }
            BackButton.Visibility = Visibility.Visible;
            SearchAutoSuggestBox.ItemsSource = suggestions;
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text))
            {
                GoBack();
                return;
            }
            string text = args.ChosenSuggestion == null ? sender.Text : (string)args.ChosenSuggestion;
            if(songManager.SelectMusic(text) == false)
            {
                videoManager.SelectVideo(text);
                songManager.select = null;
            }
            else
            {
                videoManager.select = null;
            }
            if (songManager.select != null || videoManager.select != null)
            {
                PlayMedia();
                StartMedia();
            }
            GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            GoBack();
        }

        private void GoBack()
        {
            BackButton.Visibility = Visibility.Collapsed;
            SearchAutoSuggestBox.ItemsSource = null;
            if(SearchAutoSuggestBox.Text.Length > 0)
                SearchAutoSuggestBox.Text = "";
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StopMedia();
            DeleteMedia();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(songManager.select != null || videoManager.select != null)
            {
                PlayMedia();
                StartMedia();
            }
        }

        private void Speed_Click(object sender, RoutedEventArgs e)
        {
            SpeedMenuFlyout.ShowAt(sender as UIElement, new Point(-20, -200));
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            Speed.Content = item.Text;
            mediaTimelineController.ClockRate = double.Parse(item.Text);
        }

        private async void MyMeidaPlace_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    StorageFile storageFile = items[0] as StorageFile;
                    string contentType = storageFile.ContentType;
                    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                    if (contentType == "video/mp4")
                    {
                        // StorageFile newFile = await storageFile.CopyAsync(storageFolder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        await videoManager.AddVideo(storageFile, true);
                        songManager.select = null;
                    }
                    else if(contentType == "audio/mpeg")
                    {
                        // StorageFile newFile = await storageFile.CopyAsync(storageFolder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        await songManager.AddSong(storageFile, true);
                        videoManager.select = null;
                    }
                    if (songManager.select != null || videoManager.select != null)
                    {
                        StopMedia();
                        DeleteMedia();
                        PlayMedia();
                        StartMedia();
                    }
                }
            }
        }

        private void MyMeidaPlace_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drag to add a meida";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            songManager.songStyle = 0;
        }

        private void LoopPlay_Click(object sender, RoutedEventArgs e)
        {
            songManager.songStyle = 1;
        }

        private void RandomPlay_Click(object sender, RoutedEventArgs e)
        {
            songManager.songStyle = 2;
        }
    }
}
