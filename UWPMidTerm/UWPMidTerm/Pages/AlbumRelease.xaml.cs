using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPMidTerm.Models;
using UWPMidTerm.ViewModels;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class AlbumRelease : Page
    {
        MyParameter data = new MyParameter();
        ObservableCollection<Album> albums = new ObservableCollection<Album>();
        string[] parameters = { "1", "-1", "-1", "-1", "-1", "0", "20"};
        int index = 0;

        public AlbumRelease()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                border1.Width = e.NewSize.Width / 4 - 30;
            };
            LoadResource();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += AlbumLoad_Tick;
            timer.Start();
        }

        private async void AlbumLoad_Tick(object sender, object e)
        {
            if (MainPage.isButton)
            {
                string str1 = parameters[5];
                string str2 = parameters[6];
                parameters[5] = str2;
                parameters[6] = "20";
                ObservableCollection<Album> otherAlbums = new ObservableCollection<Album>();
                await DealWithJson(false, otherAlbums);
                foreach (var item in otherAlbums)
                    albums.Add(item);
                parameters[5] = "0";
                parameters[6] = albums.Count().ToString();
            }
        }

        private async void LoadResource()
        {
            await DealWithJson(true, albums);
            DealWithColor(data.tags, data.tags[0], true);
            foreach (var item in data.tags)
                DealWithColor(item.tags, item.tags[0], false);
            SmallTags.ItemsSource = data.tags[0].tags;
        }

        private async Task DealWithJson(bool flag, ObservableCollection<Album> albums)
        {
            string result = await UrlHelper.GetSomeAlbums(parameters);
            JsonObject jsonData = JsonObject.Parse(result)["albumlib"].GetObject()["data"].GetObject();
            if (flag)
                JsonHelper.GetTagsFromJson(data.tags, jsonData.GetNamedObject("tags"));
            JsonHelper.GetAlbumRelease(albums, jsonData.GetNamedArray("list"), 20);
        }

        private void DealWithColor(ObservableCollection<MyParameter> tags, MyParameter tag, bool isBig)
        {
            foreach (var item in tags)
            {
                item.foreb.Color = Colors.Black;
                item.color.Color = Colors.White;
            }
            tag.foreb.Color = Colors.White;
            tag.color.Color = isBig ? Colors.Red : Colors.LightGreen;
        }

        private void BigTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyParameter tag = (MyParameter)e.ClickedItem;
            DealWithColor(data.tags, tag, true);
            SmallTags.ItemsSource = tag.tags;
            index = tag.id;
        }

        private async void SmallTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyParameter tag = (MyParameter)e.ClickedItem;
            ObservableCollection<MyParameter> smallTags = SmallTags.ItemsSource as ObservableCollection<MyParameter>;
            DealWithColor(smallTags, tag, false);
            parameters[index] = tag.id.ToString();
            albums.Clear();
            await DealWithJson(false, albums);
        }

        private void AlbumList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
