using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm.Models;
using UWPMidTerm.ViewModels;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPMidTerm.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FindMusic : Page
    {
        // 推荐歌单
        public ObservableCollection<SongSheet> songSheets = new ObservableCollection<SongSheet>();
        // 最新音乐
        public ObservableCollection<Song> songs = new ObservableCollection<Song>();
        // 排行榜
        public ObservableCollection<Chart> charts = new ObservableCollection<Chart>();
        public ObservableCollection<Chart> allCharts = new ObservableCollection<Chart>();
        // 专辑发行
        public ObservableCollection<Album> albums = new ObservableCollection<Album>();

        public FindMusic()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                width1.Width = w / 6 - 30;
                width1.Height = width1.Width + 40;
                width2.Width = w / 3 - 30;
                width3.Width = w / 5 - 30;
                width4.Width = w / 4 - 30;
                width4.Height = 400;

                int num = (int)w / 205;
                int index = charts.Count();
                if (index < num && index < 7 && index < allCharts.Count())
                    charts.Add(allCharts[index]);
                else if (index > num && index > 0)
                    charts.RemoveAt(index - 1);
            };
            LoadResource();
        }

        private async void LoadResource()
        {
            string result = await UrlHelper.GetAllMessage();
            JsonObject recommend = JsonObject.Parse(result);
            // 推荐歌单
            JsonHelper.GetSongSheetRecommend(songSheets, recommend["recomPlaylist"].GetObject()["data"].GetObject()["v_hot"].GetArray());
            // 最新歌曲
            JsonHelper.GetNewMusicRecommend(songs, recommend["new_song"].GetObject()["data"].GetObject()["song_list"].GetArray());
            // 排行榜(QQ音乐巅峰榜)
            JsonHelper.GetChart(allCharts, charts, recommend["toplist"].GetObject()["data"].GetObject()["group_list"].GetArray()[0].GetObject()["list"].GetArray());
            // 专辑推送
            JsonHelper.GetAlbumRelease(albums, recommend["new_album"].GetObject()["data"].GetObject()["list"].GetArray());
        }

        private void SongSheetRecommend_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SongSheetRecommend));
        }

        private void RecommendSongSheet_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void NewMusicRecommend_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMusicRecommend));
        }

        private void RecommendNewMusic_ItemClick(object sender, ItemClickEventArgs e)
        {
            MediaPlayer.songmid = ((Song)e.ClickedItem).Id;
            this.Frame.Navigate(typeof(MediaPlayer));
        }

        private void AlbumRelease_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumRelease));
        }

        private void ReleaseAlbum_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ListTopMusic_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TopList));
        }

        private void TopListMusic_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void RightControl_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine(e.GetCurrentPoint(MyCanvas).Position.X.ToString());
            Debug.WriteLine(this.Frame.ActualWidth.ToString());
        }
    }
}
