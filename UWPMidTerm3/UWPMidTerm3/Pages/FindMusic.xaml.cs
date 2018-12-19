using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UWPMidTerm3.Models;
using UWPMidTerm3.Services;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPMidTerm3.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FindMusic : Page
    {
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        private ObservableCollection<ModelCollection> songSheets = new ObservableCollection<ModelCollection>();
        private ObservableCollection<ModelCollection> brushes = new ObservableCollection<ModelCollection>();
        private int bound = 3;

        private DispatcherTimer coverTimer = new DispatcherTimer();
        private List<Grid> nowCovers = new List<Grid>();
        private List<double> coverChangeBits = new List<double>();
        private List<double> coverFianlHeights = new List<double>();

        private ObservableCollection<Tag> tags2 = new ObservableCollection<Tag>();
        private ObservableCollection<ModelCollection> newSongs = new ObservableCollection<ModelCollection>();
        private ObservableCollection<ModelCollection> brushes2 = new ObservableCollection<ModelCollection>();

        private ObservableCollection<Tag> tags3 = new ObservableCollection<Tag>();
        private ObservableCollection<ModelCollection> newAlbums = new ObservableCollection<ModelCollection>();
        private ObservableCollection<ModelCollection> brushes3 = new ObservableCollection<ModelCollection>();
        private int[] paras = { 1, -1, -1, -1, -1, 2, 1, 0, 40 };

        private ObservableCollection<Chart> charts = new ObservableCollection<Chart>();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private List<TextBlock> textblocks = new List<TextBlock>();
        private List<bool> isDisappears = new List<bool>();

        public FindMusic()
        {
            this.InitializeComponent();
            // 准备
            LoadResource();
            // 自适应布局
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double h = e.NewSize.Height;
                SongSheetsPanel.Width = w - 120;
                NewSongsPanel.Width = w - 120;
                NewAlbumsPanel.Width = w - 120;
                border1.Width = (w - 120) / 4 - 25;
                border2.Width = (w - 120) / 3 - 25;
                border3.Width = (w - 120) / 5 - 25;
                border2.Height = w / 10;
                border1.Height = border2.Width - border2.Height - 10;
                border3.Height = border3.Width + 50;
                border4.Height = border1.Width * 2.24;
                border4.Width = border4.Height * 2 / 13 - 12;
                SongSheetsPanel.Height = border1.Width + 50;
                NewSongsPanel.Height = 3 * (border2.Height + 30);
                NewAlbumsPanel.Height = 2 * (border3.Height + 30);
                for (int i = 0; i < charts.Count(); i++)
                {
                    charts[i].picLeft = 0 - border1.Width * charts[i].picId;
                    charts[i].rect = new Rect(0 - charts[i].picLeft, 0, border1.Width, border1.Width * 2.24);
                }
            };
            // 定时器
            coverTimer.Interval = TimeSpan.FromMilliseconds(10);
            coverTimer.Tick += CoverHeight_Tick;
            coverTimer.Start();
        }

        private async void LoadResource()
        {
            try
            {
                for (int i = 0; i < 12; i++)
                    songSheets.Add(new ModelCollection(1));
                for (int i = 0; i < 8; i++)
                    newSongs.Add(new ModelCollection(3));
                for (int i = 0; i < 6; i++)
                    newAlbums.Add(new ModelCollection(4));
                DealWithIndivator();
                string result = await UrlHelper.GetAllMessage();
                JsonHelper.DealWithAllMessage(result, tags, songSheets, tags2, newSongs, tags3, newAlbums, charts);
                SongSheetsPanel.SelectedIndex = 1;
                NewSongsPanel.SelectedIndex = 1;
                NewAlbumsPanel.SelectedIndex = 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await new MessageDialog("加载失败，出现错误").ShowAsync();
            }
        }

        private void DealWithIndivator(bool flag = false)
        {
            if (brushes.Count() != bound)
            {
                brushes.Clear();
                for (int i = 0; i < bound; i++)
                    brushes.Add(new ModelCollection(2, i + 1));
                brushes[0].brush.Color = Colors.Gray;
            }
            if (flag)
                return;
            for (int i = 0; i < 6; i++)
                brushes2.Add(new ModelCollection(2, i + 1));
            brushes2[0].brush.Color = Colors.Gray;
            for (int i = 0; i < 4; i++)
                brushes3.Add(new ModelCollection(2, i + 1));
            brushes3[0].brush.Color = Colors.Gray;
        }

        private void Flip_SelectionChanged(FlipView flip, ObservableCollection<ModelCollection> models, ObservableCollection<ModelCollection> _brushes, int bound)
        {
            if (flip.SelectedIndex == 0 && models.Count() > 1)
                flip.SelectedIndex = bound;
            else if (flip.SelectedIndex == bound + 1)
                flip.SelectedIndex = 1;
            int index = flip.SelectedIndex;
            if (index > 0 && index < bound + 1)
            {
                for (int i = 0; i < bound; i++)
                    _brushes[i].brush.Color = Colors.White;
                _brushes[index - 1].brush.Color = Colors.Gray;
            }
        }

        private Tag Tag_Click(ItemClickEventArgs e, ObservableCollection<Tag> _tags)
        {
            foreach (var item in _tags)
                item.BackUp();
            Tag tag = (Tag)e.ClickedItem;
            tag.Change(Colors.White, Colors.LightGreen, Colors.LightGreen);
            return tag;
        }

        private void AddCover(object sender, double height, string name)
        {
            nowCovers.Add((Grid)(((Canvas)sender).FindName(name)));
            coverChangeBits.Add(height / 10);
            coverFianlHeights.Add(height);
        }

        private async void SongSheetTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Tag tag = Tag_Click(e, tags);
                string result = await UrlHelper.GetDifferentSongSheet(tag.desc, tag.id);
                bool flag = tag.id == 0 && tag.desc == "";
                JsonHelper.GetSongSheetRecommend(songSheets, result, flag);
                bound = flag ? 3 : 10;
                DealWithIndivator(true);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载歌单失败，出现错误").ShowAsync();
            }
        }

        private void SongSheetsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SongSheetsPanel.SelectionChanged -= SongSheetsPanel_SelectionChanged;
            Flip_SelectionChanged(SongSheetsPanel, songSheets, brushes, bound);
            SongSheetsPanel.SelectionChanged += SongSheetsPanel_SelectionChanged;
        }

        private void CoverHeight_Tick(object sender, object e)
        {
            for (int i = 0; i < nowCovers.Count(); i++)
            {
                Grid nowCover = nowCovers.ElementAt(i);
                double coverChangeBit = coverChangeBits.ElementAt(i);
                double coverFianlHeight = coverFianlHeights.ElementAt(i);
                if (coverChangeBit > 0 && nowCover.Height == coverFianlHeight || coverChangeBit < 0 && nowCover.Height == 0)
                {
                    if (coverChangeBit < 0 && nowCover.Height == 0)
                    {
                        nowCovers.RemoveAt(i);
                        coverChangeBits.RemoveAt(i);
                        coverFianlHeights.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                double h = nowCover.Height + coverChangeBit;
                nowCover.Height = h < 0 ? 0 : (h > coverFianlHeight ? coverFianlHeight : h);
            }
            for (int i = 0; i < rectangles.Count(); i++)
            {
                Rectangle rectangle = rectangles[i];
                TextBlock textBlock = textblocks[i];
                bool isDisappear = isDisappears[i];
                if (isDisappear && rectangle.Height < 2 || !isDisappear && rectangle.Height > 50)
                {
                    if (isDisappear && rectangle.Height < 2)
                    {
                        rectangle.Height = 2;
                        rectangle.RadiusX = rectangle.RadiusY = 0;
                        rectangles.RemoveAt(i);
                        textblocks.RemoveAt(i);
                        isDisappears.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                double h = rectangle.Height + (isDisappear ? -5 : 5);
                double o = (double)textBlock.GetValue(TextBlock.OpacityProperty) + (isDisappear ? -0.1 : 0.1);
                rectangle.Height = h < 0 ? 0 : (h > 50 ? 50 : h);
                rectangle.RadiusX = rectangle.RadiusY = rectangle.Height / 2;
                textBlock.SetValue(TextBlock.OpacityProperty, o < 0 ? 0 : (o > 1 ? 1 : o));
            }
        }

        private void SongSheetCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddCover(sender, border1.Width, "SongSheetCover");
        }

        private void SongSheetCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            coverChangeBits[coverChangeBits.Count() - 1] = 0 - border1.Width / 10;
        }

        private void SongSheetsIndicator_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongSheetsPanel.SelectedIndex = ((ModelCollection)e.ClickedItem).index;
        }

        private void SongSheetGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongListDetail.type = "歌单";
            MainPage.frameId = -1;
            SongListDetail.id = ((SongSheet)e.ClickedItem).id;
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private async void NewMusicTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Tag tag = Tag_Click(e, tags2);
                string result = await UrlHelper.GetDifferentNewSong(tag.id);
                JsonHelper.GetNewSongRecommend(newSongs, result);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载音乐失败，出现错误").ShowAsync();
            }
        }

        private void NewSongsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewSongsPanel.SelectionChanged -= NewSongsPanel_SelectionChanged;
            Flip_SelectionChanged(NewSongsPanel, newSongs, brushes2, 6);
            NewSongsPanel.SelectionChanged += NewSongsPanel_SelectionChanged;
        }

        private void NewSongGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 歌曲详情页
            MainPage.index = 0;
            MainPage.songList.songs.Clear();
            MainPage.songList.songs.Add((Song)e.ClickedItem);
            MainPage.continueFlag = true;
            MainPage.playStype = 2;
        }

        private void SongImageCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddCover(sender, border2.Height, "NewSongCover");
        }

        private void SongImageCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            coverChangeBits[coverChangeBits.Count() - 1] = 0 - border2.Height / 10;
        }

        private void NewSongsIndicator_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewSongsPanel.SelectedIndex = ((ModelCollection)e.ClickedItem).index;
        }

        private async void NewAlbumTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Tag tag = Tag_Click(e, tags3);
                paras[0] = tag.id;
                string result = await UrlHelper.GetDifferentNewAlbum(paras);
                JsonHelper.GetNewAlbumRecommend(newAlbums, result);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("加载专辑失败，出现错误").ShowAsync();
            }
        }

        private void NewAlbumsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewAlbumsPanel.SelectionChanged -= NewAlbumsPanel_SelectionChanged;
            Flip_SelectionChanged(NewAlbumsPanel, newAlbums, brushes3, 4);
            NewAlbumsPanel.SelectionChanged += NewAlbumsPanel_SelectionChanged;
        }

        private void NewAlbumGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongListDetail.type = "专辑";
            MainPage.frameId = -1;
            SongListDetail.id = ((Album)e.ClickedItem).id;
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void AlbumImageCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddCover(sender, border3.Width, "NewAlbumCover");
        }

        private void AlbumImageCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            coverChangeBits[coverChangeBits.Count() - 1] = 0 - border3.Width / 10;
        }

        private void NewAlbumsIndicator_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewAlbumsPanel.SelectedIndex = ((ModelCollection)e.ClickedItem).index;
        }

        private void TopListGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongListDetail.type = "排行榜";
            MainPage.frameId = -1;
            SongListDetail.chart = ((Chart)e.ClickedItem);
            this.Frame.Navigate(typeof(SongListDetail));
        }

        private void TopListCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            rectangles.Add((Rectangle)canvas.FindName("TopListEllipse"));
            textblocks.Add((TextBlock)canvas.FindName("TopListPlay"));
            isDisappears.Add(false);
        }

        private void TopListCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isDisappears[isDisappears.Count() - 1] = true;
        }

        private void PlayAll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SongListDetail.playAll = true;
        }
    }
}
