using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm3.Models;
using UWPMidTerm3.Services;
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
    public sealed partial class TopList : Page
    {
        ObservableCollection<Chart> concreteCharts = new ObservableCollection<Chart>();
        ObservableCollection<Chart> restCharts = new ObservableCollection<Chart>();
        ObservableCollection<Chart> globalCharts = new ObservableCollection<Chart>();

        private DispatcherTimer gridOpacityTimer = new DispatcherTimer();
        private List<Grid> grids = new List<Grid>();
        private List<double> obs = new List<double>();

        public TopList()
        {
            this.InitializeComponent();
            LoadResource();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double h = e.NewSize.Height;
                if (w < 300)
                    return;
                line1.Width = (w - 120) / 3 - 25;
                line1.Height = line1.Width * 2.1;
                line1.X1 = line1.Width * 4;
                line1.X2 = 0 - line1.Width * 1.5;
                line1.Y1 = line1.Width * 0.375;
                line1.Y2 = line1.Height - line1.Y1;
                line2.X1 = line1.Y2 / 8 - 10;
                // line2.X2 = line1.Width - 80;
                // line2.Y1 = line1.Y1 / 2 - 25;
                line2.Y2 = (w - 120) / 4 - 25;
                line2.X2 = line2.Y2 + 20;
                line2.Y1 = line2.Y2 - 40;
                rectgome1.Rect = new Rect(0 - line1.X2, 0, line1.Width, line1.Y1);
            };
            gridOpacityTimer.Interval = TimeSpan.FromMilliseconds(10);
            gridOpacityTimer.Tick += GridOpacityChange_Tick;
            gridOpacityTimer.Start();
        }

        private async void LoadResource()
        {
            try
            {
                string result = await UrlHelper.GetAllTopList();
                JsonHelper.GetTopListRecommend(concreteCharts, result, false, restCharts, globalCharts);
                for (int i = 0; i < concreteCharts.Count(); i++)
                {
                    Chart chart = concreteCharts[i];
                    string r = await UrlHelper.GetDifferentTopList(new string[5] { chart.date, chart.id, "top", "0", "8" });
                    JsonHelper.DealWithSongInChart(chart.songs, r, 8, false, chart);
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载失败，发生未知错误").ShowAsync();
            }
        }

        private void GridOpacityChange_Tick(object sender, object e)
        {
            for (int i = 0; i < grids.Count(); i++)
            {
                Grid grid = grids[i];
                double ob = obs[i];
                if (grid.Opacity == 1 && ob > 0 || grid.Opacity == 0 && ob < 0)
                {
                    if (grid.Opacity == 0 && ob < 0)
                    {
                        grids.RemoveAt(i);
                        obs.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                double o = grid.Opacity + ob;
                grid.Opacity = o < 0 ? 0 : (o > 1 ? 1 : o);
            }
        }

        private void ConcreteCharts_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 进入排行榜详情页
            SongListDetail.type = "排行榜";
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)e.ClickedItem);
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void Cursor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            /*Canvas canvas = (Canvas)sender;
            Grid grid = (Grid)canvas.FindName("ChartDisPlay");
            grid.Visibility = Visibility.Visible;
            grid.BorderBrush = new SolidColorBrush(Colors.Black);
            ((TextBlock)grid.FindName("DisPlayIcon")).Foreground = new SolidColorBrush(Colors.Black);*/
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Cursor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            /*Canvas canvas = (Canvas)sender;
            Grid grid = (Grid)canvas.FindName("ChartDisPlay");
            grid.Visibility = Visibility.Collapsed;
            grid.BorderBrush = new SolidColorBrush(Colors.LightGray);
            ((TextBlock)grid.FindName("DisPlayIcon")).Foreground = new SolidColorBrush(Colors.LightGray);*/
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void ChartSongs_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 歌曲点击，播放歌曲/歌曲详情页
            MainPage.index = 0;
            MainPage.songList.songs.Clear();
            MainPage.songList.songs.Add((Song)e.ClickedItem);
            MainPage.continueFlag = true;
            MainPage.playStype = 2;
        }

        private void RestCharts_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 排行榜详情页
            SongListDetail.type = "排行榜";
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)e.ClickedItem);
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void ChartCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 排行榜详情页
            SongListDetail.type = "排行榜";
            SongListDetail.playAll = true;
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)((Canvas)sender).DataContext);
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void GlobalCharts_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 排行榜详情页
            SongListDetail.type = "排行榜";
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)e.ClickedItem);
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void RestChartPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            StackPanel panel = (StackPanel)sender;
            grids.Add(((Grid)panel.FindName("ChartDisPlay")));
            obs.Add(0.1);
        }

        private void RestChartPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            obs[obs.Count() - 1] = 0 - obs[obs.Count() - 1];
        }

        private void RestChartPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SongListDetail.type = "排行榜";
            SongListDetail.playAll = true;
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)((StackPanel)sender).DataContext);
            this.Frame.Navigate(typeof(SongListDetail));
        }
    }
}
