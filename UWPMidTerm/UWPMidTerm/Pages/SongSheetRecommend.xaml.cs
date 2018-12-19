using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm.Models;
using UWPMidTerm.ViewModels;
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
    public sealed partial class SongSheetRecommend : Page
    {
        public ObservableCollection<CategoryGroup> categoryGroup = new ObservableCollection<CategoryGroup>();
        public ObservableCollection<Category> currentCategories = new ObservableCollection<Category>();
        public ObservableCollection<SongSheet> songSheets = new ObservableCollection<SongSheet>();
        public Category category;
        public int sin;
        public int ein;
        public SongSheetRecommend()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double width = w / 5 - 30;

                border1.Width = width;
                border2.Width = w / 5 - 30;
                border2.Height = 0.3125 * width;
                border1.Height = 380 + border2.Height;
                border3.Width = w / 4 - 30;
            };
            sin = 0;
            ein = 29;
            LoadResource();

        }

        public async void LoadResource()
        {
            ObservableCollection<CategoryGroup> groups = JsonHelper.GetPlayListTags(await UrlHelper.GetPlayListTags());
            category = groups[0].items[0];
            for (int i = 1; i < groups.Count; ++i)
            {
                categoryGroup.Add(groups[i]);
            }
            foreach (Category c in groups[1].items)
            {
                currentCategories.Add(c);
            }
            JsonHelper.GetSongSheets(songSheets, await UrlHelper.GetPlayListByTagId(category.CategoryId, category.SortId, sin.ToString(), ein.ToString()));
            songSheetsTag.SelectedIndex = 0;
        }

        private void ItemSelect(object sender, ItemClickEventArgs e)
        {
            CategoryGroup group = e.ClickedItem as CategoryGroup;
            currentCategories.Clear();
            foreach (Category c in group.items)
            {
                currentCategories.Add(c);
            }
        }

        private async void Select_SongSheet(object sender, ItemClickEventArgs e)
        {
            sin = 0;
            ein = 29;
            songSheets.Clear();
            category = e.ClickedItem as Category;
            JsonHelper.GetSongSheets(songSheets, await UrlHelper.GetPlayListByTagId(category.CategoryId, category.SortId, sin.ToString(), ein.ToString()));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sin != 0)
            {
                sin = sin - 30;
                ein = ein - 30;
                songSheets.Clear();
                JsonHelper.GetSongSheets(songSheets, await UrlHelper.GetPlayListByTagId(category.CategoryId, category.SortId, sin.ToString(), ein.ToString()));
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sin = sin + 30;
            ein = ein + 30;
            songSheets.Clear();
            JsonHelper.GetSongSheets(songSheets, await UrlHelper.GetPlayListByTagId(category.CategoryId, category.SortId, sin.ToString(), ein.ToString()));

        }
    }
}
