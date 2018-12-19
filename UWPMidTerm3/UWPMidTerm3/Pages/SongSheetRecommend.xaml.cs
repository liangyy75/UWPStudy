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
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPMidTerm3.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SongSheetRecommend : Page
    {
        public ObservableCollection<Tag> bigTags = new ObservableCollection<Tag>();
        public ObservableCollection<SongSheet> songSheets = new ObservableCollection<SongSheet>();
        public int[] paras = { 165, 5, 0, 29 };
        public int nowIndex = 0;

        DispatcherTimer timer = new DispatcherTimer();
        private List<Image> covers = new List<Image>();
        private List<SolidColorBrush> bgs = new List<SolidColorBrush>();
        private List<Rectangle> rects = new List<Rectangle>();
        private List<TextBlock> texts = new List<TextBlock>();
        private List<double> finals = new List<double>();
        private List<double> scales = new List<double>();

        public SongSheetRecommend()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double h = e.NewSize.Height;
                if (w < 300)
                    return;
                line1.Width = ((w > 1200 ? 1200 : w) - 120) / 5 - 25;
                line1.Height = line1.Width / 2 - 25;
                rectgome1.Rect = new Rect(0, 0, line1.Width, line1.Width);
            };
            LoadResource();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += ImageChange_Tick;
            timer.Start();
        }

        private async void LoadResource()
        {
            try
            {
                string result = await UrlHelper.GetSongSheetTags();
                JsonHelper.AddSongSheetTags(bigTags, result);
                string result2 = await UrlHelper.GetDetailSongSheets(paras);
                JsonHelper.GetSongSheetRecommend(null, result2, false, 0, false, songSheets);
                SmallTags.ItemsSource = bigTags[0].smallTags;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载失败，出现蜜汁问题").ShowAsync();
            }
        }

        private void ImageChange_Tick(object sender, object e)
        {
            for (int i = 0; i < covers.Count(); i++)
            {
                Image image = covers[i];
                double scale = scales[i];
                double final = finals[i];
                SolidColorBrush bg = bgs[i];
                Rectangle rect = rects[i];
                TextBlock text = texts[i];
                if (image.Width >= final && scale > 0 || image.Width <= line1.Width && scale < 0)
                {
                    if (image.Width <= line1.Width && scale < 0)
                    {
                        covers.RemoveAt(i);
                        scales.RemoveAt(i);
                        finals.RemoveAt(i);
                        bgs.RemoveAt(i);
                        rects.RemoveAt(i);
                        texts.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                double w = image.Width + scale;
                image.Height = image.Width = w < line1.Width ? line1.Width : (w > final ? final : w);
                double h = (double)image.GetValue(Canvas.LeftProperty) - scale / 2;
                image.SetValue(Canvas.LeftProperty, h);
                image.SetValue(Canvas.TopProperty, h);
                double o = bg.Opacity + (scale < 0 ? -0.03 : 0.03);
                bg.Opacity = o < 0 ? 0 : (o > 1 ? 1 : o);
                rect.Height = rect.Width = rect.Width + (scale < 0 ? -5 : 5);
                text.Opacity = text.Opacity + (scale < 0 ? -0.1 : 0.1);
            }
        }

        private void BigTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var item in bigTags)
                item.BackUp();
            Tag tag = (Tag)e.ClickedItem;
            tag.Change(Colors.Red, Colors.White, Colors.White);
            SmallTags.ItemsSource = bigTags[tag.id - 1].smallTags;
            nowIndex = tag.id - 1;
        }

        private async void SmallTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var item in bigTags[nowIndex].smallTags)
                item.BackUp();
            Tag tag = (Tag)e.ClickedItem;
            tag.Change(Colors.LightGreen, Colors.White, Colors.White);
            paras[0] = tag.id;
            try
            {
                string result = await UrlHelper.GetDetailSongSheets(paras);
                JsonHelper.GetSongSheetRecommend(null, result, false, 0, false, songSheets);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载失败，出现问题").ShowAsync();
            }
        }

        private void SongSheetList_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 歌单详情页
            SongListDetail.type = "歌单";
            SongListDetail.id = ((SongSheet)e.ClickedItem).id;
            MainPage.frameId = -1;
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void ImageCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            covers.Add((Image)canvas.FindName("AlbumCover"));
            bgs.Add((SolidColorBrush)canvas.FindName("ImageCover"));
            rects.Add((Rectangle)canvas.FindName("Ellipse"));
            texts.Add((TextBlock)canvas.FindName("AlbumPlay"));
            scales.Add(canvas.Width / 100);
            finals.Add(canvas.Width * 1.1);
        }

        private void ImageCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            scales[scales.Count() - 1] = 0 - scales[scales.Count() - 1];
        }

        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 播放全部歌单歌曲并跳到歌单详情页
            SongListDetail.type = "歌单";
            MainPage.frameId = -1;
            SongListDetail.id = ((SongSheet)((Ellipse)sender).DataContext).id;
            SongListDetail.playAll = true;
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (paras[2] != 0)
            {
                try
                {
                    paras[2] -= 30;
                    paras[3] -= 30;
                    string result = await UrlHelper.GetDetailSongSheets(paras);
                    JsonHelper.GetSongSheetRecommend(null, result, false, 0, false, songSheets);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                    paras[2] += 30;
                    paras[3] += 30;
                    await new MessageDialog("加载失败，出现蜜汁问题").ShowAsync();
                }
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                paras[2] += 30;
                paras[3] += 30;
                string result = await UrlHelper.GetDetailSongSheets(paras);
                JsonHelper.GetSongSheetRecommend(null, result, false, 0, false, songSheets);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                paras[2] -= 30;
                paras[3] -= 30;
                await new MessageDialog("加载失败，出现问题").ShowAsync();
            }
        }
    }
}
