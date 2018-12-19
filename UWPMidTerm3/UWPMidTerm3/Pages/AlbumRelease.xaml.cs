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
    public sealed partial class AlbumRelease : Page
    {
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        private ModelCollection albums = new ModelCollection(4);
        private int[] paras = { 1, -1, -1, -1, -1, 2, 1, 0, 27 };
        private int nowIndex = 0;
        private DispatcherTimer updateTimer = new DispatcherTimer();
        private double totalTime = 0;
        private bool isUpdating = false;

        DispatcherTimer timer = new DispatcherTimer();
        private List<Image> covers = new List<Image>();
        private List<SolidColorBrush> bgs = new List<SolidColorBrush>();
        private List<Rectangle> rects = new List<Rectangle>();
        private List<TextBlock> texts = new List<TextBlock>();
        private List<double> finals = new List<double>();
        private List<double> scales = new List<double>();

        bool continueFlag = true;

        public AlbumRelease()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double h = e.NewSize.Height;
                if (w < 300)
                    return;
                line1.Width = ((w > 1200 ? 1200 : w) - 120) / 4 - 25;
                line1.Height = line1.Width / 2 - 25;
                rectgome1.Rect = new Rect(0, 0, line1.Width, line1.Width);
            };
            LoadSource();

            updateTimer.Interval = TimeSpan.FromSeconds(0.01);
            updateTimer.Tick += Update_Tick;
            updateTimer.Start();

            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += ImageChange_Tick;
            timer.Start();
        }

        private async void LoadSource()
        {
            try
            {
                string result = await UrlHelper.GetDifferentNewAlbum(paras);
                JsonHelper.GetNewAlbumTags(tags, result);
                JsonHelper.GetNewAlbumRecommend(null, result, albums);
                paras[6] = 0;
                SmallTags.ItemsSource = tags[0].smallTags;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await new MessageDialog("加载专辑失败，出现错误").ShowAsync();
            }
        }

        private async void Update_Tick(object sender, object e)
        {
            if (MainPage.isButton && !isUpdating && continueFlag)
            {
                totalTime++;
                if (totalTime >= 100)
                {
                    try
                    {
                        UpdateTies.Visibility = Visibility.Collapsed;
                        UpdateRing.IsActive = true;
                        isUpdating = true;
                        int num1 = paras[8];
                        paras[7] = num1;
                        paras[8] = 27;
                        string result = await UrlHelper.GetDifferentNewAlbum(paras);
                        JsonHelper.GetNewAlbumRecommend(null, result, albums, false);
                        paras[8] = num1 + 28;
                        paras[7] = 0;
                        UpdateTies.Visibility = Visibility.Visible;
                        UpdateRing.IsActive = false;
                        isUpdating = false;
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine(error.Message);
                        await new MessageDialog("加载专辑失败，出现错误").ShowAsync();
                        continueFlag = false;
                    }
                }
            }
            else
                totalTime = 0;
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
                if (image.Width > final - 1 && scale > 0 || image.Width < line1.Width + 1 && scale < 0)
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
            foreach (var item in tags)
                item.BackUp();
            Tag tag = (Tag)e.ClickedItem;
            tag.Change(Colors.Red, Colors.White, Colors.White);
            SmallTags.ItemsSource = tags[tag.id].smallTags;
            nowIndex = tag.id;
        }

        private async void SmallTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var item in tags[nowIndex].smallTags)
                item.BackUp();
            Tag tag = (Tag)e.ClickedItem;
            tag.Change(Colors.LightGreen, Colors.White, Colors.White);
            paras[nowIndex] = tag.id;
            paras[8] = 20;
            paras[7] = 0;
            try
            {
                string result = await UrlHelper.GetDifferentNewAlbum(paras);
                JsonHelper.GetNewAlbumRecommend(null, result, albums);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载专辑失败，出现未知错误").ShowAsync();
            }
        }

        private void AlbumList_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 进入专辑详情页
            SongListDetail.type = "专辑";
            SongListDetail.id = ((Album)e.ClickedItem).id;
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
            SongListDetail.playAll = true;
        }
    }
}
