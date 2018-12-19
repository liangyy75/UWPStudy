using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm3.Models;
using UWPMidTerm3.Pages;
using UWPMidTerm3.Services;
using UWPMidTerm3.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPMidTerm3
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // 设置播放歌曲列表
        // public static List<string> songmids { get; set; } = new List<string>();
        public static SongList songList { get; set; } = new SongList();
        public static bool continueFlag { get; set; } = true;
        public static int index { get; set; } = 0;
        public Song song { get; set; } = new Song(true);
        // scrollviewer是否到达底部
        public static bool isButton { set; get; } = false;
        // 关于frame页面导航
        public static int frameId { set; get; } = 0;
        private int nowFrameId = 0;
        // 关于菜单
        private MenuItemManager recommandMenu = new MenuItemManager();
        private MenuItemManager myMusicMenu = new MenuItemManager();
        // 关于播放详情页
        private DispatcherTimer displayTimer = new DispatcherTimer();
        private double wb, hb, ob;
        // 关于播放
        MediaPlayer mediaPlayer = new MediaPlayer();
        MediaTimelineController mediaTimelineController = new MediaTimelineController();
        TimeSpan _duration;
        public static int playStype = 0;
        public static bool notSingleFlag = true;
        // 歌词时间管理
        DispatcherTimer lyricTimer, timer;
        double nowTime = 0, time_bit = 1;
        int nowIndex = 0;
        DispatcherTimer songTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();
            // 界面处理
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = MyTheme.themeBrush.Color;
            titleBar.ButtonBackgroundColor = MyTheme.themeBrush.Color;
            LeftControl.Background = MyTheme.menuBrush;
            SongMessagePanel.Background = MyTheme.menuBrush;
            ButtonControl.Background = MyTheme.bottomBrush;
            // 自适应
            this.SizeChanged += (s, e) =>
            {
                double width = e.NewSize.Width;
                double height = e.NewSize.Height;
                if (width < 1000 || e.NewSize.Height < 600)
                {
                    ApplicationView.GetForCurrentView().TryResizeView(new Size(1000, 600));
                    width = 1000;
                    height = 600;
                }
                MainContent.Width = width;
                MainContent.Height = height - 40;
                if (Display.Width != 0)
                {
                    Display.Width = MainContent.Width;
                    Display.Height = MainContent.Height;
                }
                Display.SetValue(Canvas.TopProperty, height - 40 - Display.Height);
                double h = MainContent.Height - 330;
                SongImageEllipse.Width = SongImageEllipse.Height = h > 380 ? 380 : h;
                if (width > 1000)
                {
                    MediaProgress.Width = (width - 260) * 2 / 3 - 80;
                    VolumeRange.Width = (width - 260) / 3 - 50;
                }
            };
            // 数据准备
            UrlHelper.PrePareFolder();
            SqlHelper.PrepareConnection();
            recommandMenu.GetRecommendMenu();
            myMusicMenu.GetMyMusicMenu();
            MyFrame.Navigate(typeof(FindMusic)/*, null, new EntranceNavigationTransitionInfo()*/);

            mediaPlayer.TimelineController = mediaTimelineController;
            MyMediaPlayerElement.SetMediaPlayer(mediaPlayer);

            // 如果上次放的歌还有，那就不用，否则。。。
            // EqualDisPlay.Height = new GridLength(0);
            // 实时检测
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += menuTimer_Tick;
            timer.Start();

            displayTimer.Interval = TimeSpan.FromSeconds(0.01);
            displayTimer.Tick += dispalySize_Tick;

            songTimer.Interval = TimeSpan.FromSeconds(0.01);
            songTimer.Tick += SongDisplay_Tick;
            songTimer.Start();

            // ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            // composite["songmid"] = "12345";
            // ApplicationData.Current.LocalSettings.Values["SongMessage"] = composite;
            // var composite = ApplicationData.Current.LocalSettings.Values["SongMessage"] as ApplicationDataCompositeValue;
            // composite["songmid"] = "67890";
            // Debug.WriteLine((string)composite["songmid"]);
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
            data.Properties.Title = "Share " + song.title;
            if (song.musicFile != null)
                data.SetStorageItems(new List<StorageFile> { song.musicFile });
            else
                data.SetWebLink(new Uri("https://y.qq.com/n/yqq/song/" + song.songmid + ".html"));
            data.Properties.Description = "my test";
            deferral.Complete();
        }

        private void SongDisplay_Tick(object sender, object e)
        {
            if (songList.songs.Count() > index && continueFlag && notSingleFlag)
            {
                StopMedia();
                continueFlag = false;
                PlayMedia();
                return;
            }
        }

        private async void PlayMedia()
        {
            PlayIcon.Text = "\xE103";
            MediaSource mediaSource;
            if (songList.songs[index].musicFile == null)
            {
                try
                {
                    string result = await UrlHelper.GetSongMessage(songList.songs[index].songmid);
                    song.musicFile = null;
                    JsonHelper.DealWithSongMessage(song, result);
                    string result2 = await UrlHelper.GetAlbumMessage(song.album.id);
                    JsonHelper.DealWithAlbumMessage(song.album, result2);
                    try
                    {
                        string result3 = await UrlHelper.GetMusicLrc(song.songmid);
                        JsonHelper.DealWithSongLrc(song.lyrics, result3);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    mediaSource = MediaSource.CreateFromUri(new Uri("http://ws.stream.qqmusic.qq.com/C100" + song.songmid + ".m4a?fromtag=0&guid=126548448"));
                    DownloadSong.Visibility = Visibility.Visible;
                    SqlHelper.DeleteSong("History", "", song.songmid);
                    DownloadText.Text = "下载";
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    MessageDialog message = new MessageDialog("");
                    message.Content = "该音频已经下载或者下载失败";
                    await message.ShowAsync();
                    return;
                }
            }
            else
            {
                song.Copy(songList.songs[index]);
                mediaSource = MediaSource.CreateFromStorageFile(songList.songs[index].musicFile);
                SqlHelper.DeleteSong("History", song.musicFile.Name);
                DownloadText.Text = "已下载";
            }
            if (song.lyrics.Count() == 0)
            {
                LrcPanelWidthControl.Width = new GridLength(0);
                SongLrcPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                LrcPanelWidthControl.Width = new GridLength(1, GridUnitType.Star);
                SongLrcPanel.Visibility = Visibility.Visible;
            }
            SqlHelper.AddSong(song, "History");
            MyMediaPlayerElement.Source = mediaSource;
            MediaTotalTime.Text = songList.songs[index].interval;
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            StartMedia();
        }

        private void StartMedia()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            lyricTimer = new DispatcherTimer();
            lyricTimer.Interval = TimeSpan.FromSeconds(0.01);
            lyricTimer.Tick += LyricManager_Tick;
            lyricTimer.Start();
            mediaTimelineController.Start();
            EllipseStoryboard.Begin();
        }

        private void StopMedia()
        {
            mediaTimelineController.Position = TimeSpan.FromSeconds(0);
            mediaTimelineController.Pause();
            MediaTime.Text = "00:00";
            MediaProgress.Value = 0;
            EllipseStoryboard.Stop();
            if (lyricTimer != null)
            {
                SongLrcPanel.ChangeView(null, 0 - SongLrcPanel.VerticalOffset, null);
                lyricTimer.Stop();
                timer.Stop();
            }
        }

        // 控制播放进度
        private void Timer_Tick(object sender, object e)
        {
            TimeSpan time = ((TimeSpan)mediaTimelineController.Position);
            MediaProgress.Value = time.TotalSeconds;
            MediaTime.Text = readTime(time);
            if (MediaProgress.Value == MediaProgress.Maximum)
            {
                mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                mediaTimelineController.Pause();
                EllipseStoryboard.Stop();
                int count = songList.songs.Count();
                switch (playStype)
                {
                    case 1: notSingleFlag = false; break;
                    case 3: index++; notSingleFlag = true; break;
                    case 4: index = (index + 1) % count; notSingleFlag = true; break;
                    case 5: index = new Random().Next(count); notSingleFlag = true; break;
                }
                continueFlag = true;
            }
        }

        // 控制歌词
        private void LyricManager_Tick(object sender, object e)
        {
            if (nowTime >= song._time * 100)
            {
                nowTime = 0;
                return;
            }
            nowTime += time_bit;
            if (nowIndex < song.lyrics.Count() && nowTime >= song.lyrics[nowIndex].intTime * 100)
            {
                SongLrcPanel.ChangeView(null, nowIndex * 45, null);
                nowIndex++;
            }
        }

        // 控制播放页面大小
        private void dispalySize_Tick(object sender, object e)
        {
            double height = this.Frame.ActualHeight < 600 ? 600 : this.Frame.ActualHeight - 40;
            double width = this.Frame.ActualWidth < 1000 ? 1000 : this.Frame.ActualWidth;
            if (Display.Height == height && ob == 0.1 || Display.Height == 0 && ob == -0.1)
            {
                displayTimer.Stop();
                return;
            }
            double h = Display.Height + hb;
            double w = Display.Width + wb;
            double o = Display.Opacity + ob;
            Display.Height = h < 0 ? 0 : (h > height ? height : h);
            Display.Width = w < 0 ? 0 : (w > width ? width : w);
            Display.Opacity = o < 0 ? 0 : (o > 1 ? 1 : o);
            Display.SetValue(Canvas.TopProperty, height - Display.Height);
        }

        // 检测左边菜单栏
        private void menuTimer_Tick(object sender, object e)
        {
            isButton = MyScrollViewer.ScrollableHeight == MyScrollViewer.VerticalOffset;
            if (nowFrameId == frameId)
                return;
            nowFrameId = frameId;
            if (frameId >= 0 && frameId < 5)
            {
                RecommendMenu.SelectedIndex = frameId;
                MyFrame.Navigate(recommandMenu.menuItems[frameId].purpose/*, null, recommandMenu.menuItems[frameId].transitionInfo*/);
            }
            else if (frameId > 4 && frameId < 9)
            {
                MyMusicMenu.SelectedIndex = frameId - 5;
                MyFrame.Navigate(myMusicMenu.menuItems[frameId - 5].purpose/*, null, recommandMenu.menuItems[frameId - 5].transitionInfo*/);
            }
        }

        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            _duration = sender.Duration.GetValueOrDefault();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MediaProgress.Minimum = 0;
                MediaProgress.Maximum = _duration.TotalSeconds;
                MediaProgress.StepFrequency = 1;
            });
        }

        private string readTime(TimeSpan time)
        {
            int seconds = ((int)time.TotalSeconds) % 60;
            int minutes = ((int)time.TotalMinutes) % 60;
            return ExtendString(minutes.ToString()) + ":" + ExtendString(seconds.ToString());
        }

        private string ExtendString(string str)
        {
            return str.Length == 1 ? "0" + str : str;
        }

        private void MyMusicMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem item = (MenuItem)e.ClickedItem;
            frameId = item.id;
            RecommendMenu.SelectedIndex = -1;
        }

        private void RecommendMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem item = (MenuItem)e.ClickedItem;
            frameId = item.id;
            MyMusicMenu.SelectedIndex = -1;
        }

        private void ShowSong_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ShowSongClick.Visibility = Visibility.Visible;
        }

        private void ShowSong_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ShowSongClick.Visibility = Visibility.Collapsed;
        }

        private void Cursor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Cursor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void ShowSongClick_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            hb = (this.Frame.ActualHeight - 40) / 10;
            wb = this.Frame.ActualWidth / 10;
            ob = 0.1;
            displayTimer.Start();
        }

        private void HideSongClick_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            HideDisplay();
        }

        private void HideDisplay()
        {
            hb = 0 - (this.Frame.ActualHeight - 40) / 10;
            wb = 0 - this.Frame.ActualWidth / 10;
            ob = -0.1;
            displayTimer.Start();
        }

        private void Collect_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Collect.Text == "\xE006")
            {
                Collect.Text = "\xE00B";
                Collect.Foreground = MyTheme.themeBrush;
                // 收藏
                SqlHelper.AddSong(song, "Collection");
            }
            else
            {
                Collect.Text = "\xE006";
                Collect.Foreground = MyTheme.normalBrush;
                // 取消收藏
                if (song.musicFile != null)
                    SqlHelper.DeleteSong("Collection", song.fileName);
                else
                    SqlHelper.DeleteSong("Collection", song.fileName, song.songmid);
            }
        }

        private void Share_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 分享
            if(song != null)
                DataTransferManager.ShowShareUI();
        }

        private void Previous_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 上一首歌
            StopMedia();
            index = (index - 1 + songList.songs.Count()) % songList.songs.Count();
            continueFlag = true;
            notSingleFlag = true;
        }

        private void Play_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 播放或暂停
            notSingleFlag = true;
            if (mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                time_bit = 0;
                EllipseStoryboard.Pause();
                mediaTimelineController.Pause();
                PlayIcon.Text = "\xE102";
            }
            else if (mediaTimelineController.State == MediaTimelineControllerState.Paused)
            {
                EllipseStoryboard.Resume();
                mediaTimelineController.Resume();
                time_bit = 1;
                PlayIcon.Text = "\xE103";
            }
        }

        private void Next_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 下一首歌
            StopMedia();
            index = (index + 1) % songList.songs.Count();
            continueFlag = true;
            notSingleFlag = true;
        }

        private void VolumeRange_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // 声音控制
            mediaPlayer.Volume = (double)VolumeRange.Value;
        }

        private void PStyle_Click(object sender, RoutedEventArgs e)
        {
            // 设置播放样式
            MenuFlyoutItem item = (MenuFlyoutItem)sender;
            if (item.Text == "单曲播放")
                playStype = 1;
            else if (item.Text == "单曲循环")
                playStype = 2;
            else if (item.Text == "顺序播放")
                playStype = 3;
            else if (item.Text == "循环播放")
                playStype = 4;
            else if (item.Text == "随机播放")
                playStype = 5;
        }

        private void AlbumClick_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 跳转到对应的专辑的详情页
            SongListDetail.type = "专辑";
            frameId = -1;
            SongListDetail.id = songList.songs[index].album.id;
            MyFrame.Navigate(typeof(SongListDetail));
            HideDisplay();
        }

        private void LyricGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 跳转到歌曲对应歌词的位置
        }

        private void Lyric_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // ?
        }

        private void LoveSong_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 喜欢
        }

        private async void CollectSong_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 收藏
            if (await SqlHelper.SelectSong(new Song(false), "Collection", song.fileName) == false && song.type != "net" || song.type == "net" && await SqlHelper.SelectSong(new Song(false), "Collection", song.fileName, song.songmid) == false)
                SqlHelper.AddSong(song, "Collection");
            else if (song.musicFile != null)
                SqlHelper.DeleteSong("Collection", song.fileName);
            else
                SqlHelper.DeleteSong("Collection", song.fileName, song.songmid);
        }

        private void DownloadSong_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 下载
            if (song.musicFile == null)
            {
                DownloadText.Text = "已下载";
                MusicManager.DownloadSingMusic(song);
            }
        }

        private void ShareSong_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 分享
            if(song != null)
                DataTransferManager.ShowShareUI();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            // 添加本地歌曲
            FileOpenPicker vidioFilePicker = new FileOpenPicker();
            vidioFilePicker.ViewMode = PickerViewMode.Thumbnail;
            vidioFilePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            vidioFilePicker.FileTypeFilter.Add(".m4a");
            vidioFilePicker.FileTypeFilter.Add(".mp3");
            StorageFile file = await vidioFilePicker.PickSingleFileAsync();
            if (file != null)
            {
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 300, ThumbnailOptions.UseCurrentScale);
                var fileProperties = await file.GetBasicPropertiesAsync();
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                Song dragSong = new Song(file, musicProperties, fileProperties.Size);
                songList.songs.Clear();
                songList.songs.Add(dragSong);
                index = 0;
                continueFlag = true;
                notSingleFlag = true;
                playStype = 2;
            }
        }

        private void ScreenControl_Click(object sender, RoutedEventArgs e)
        {
            // 控制屏幕
        }
    }
}
