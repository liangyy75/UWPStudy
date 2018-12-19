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
    public sealed partial class SearchDetail : Page
    {
        public SongList songlist = new SongList();
        public string SearchKey = "";
        public int page = 1;
        public int count = 20;
        public int totalsize = 0;
        public ObservableCollection<Song> searchsong = new ObservableCollection<Song>();

        public SearchDetail()
        {
            page = 1;
            LoadResource();
            this.InitializeComponent();
        }

        private async void LoadResource()
        {
            try
            {
                SearchKey = "两只老虎";
                JsonHelper.GetSearchResult(songlist, await UrlHelper.Search(SearchKey, page, count));
                for (int i = 0; i < count; i++)
                {
                    songlist.songs[i].cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + songlist.songs[i].album.id + ".jpg");
                }
                totalsize = songlist.Total;
                alert.Text = "搜索\"" + SearchKey + "\"总共搜索到" + totalsize + "纪录";
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("搜索失败，出现玄学").ShowAsync();
            }
        }

        private async void ab2_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            try
            {
                page = 1;
                SearchKey = sender.Text;
                JsonHelper.GetSearchResult(songlist, await UrlHelper.Search(SearchKey, page, count));
                for (int i = 0; i < songlist.songs.Count(); i++)
                {
                    songlist.songs[i].cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + songlist.songs[i].album.id + ".jpg");
                }
                totalsize = songlist.Total;
                alert.Text = "搜索\"" + SearchKey + "\"总共搜索到" + totalsize + "纪录";
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                await new MessageDialog("搜索失败，出现玄学").ShowAsync();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (page != 1)
            {
                page--;
                try
                {
                    JsonHelper.GetSearchResult(songlist, await UrlHelper.Search(SearchKey, page, count));
                    for (int i = 0; i < count; i++)
                    {
                        songlist.songs[i].cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + songlist.songs[i].album.id + ".jpg");
                    }
                }
                catch (Exception error)
                {
                    page++;
                    Debug.WriteLine(error.Message);
                    await new MessageDialog("搜索失败，出现玄学").ShowAsync();
                }
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((page + 1) * count < totalsize)
            {
                page++;
                try
                {
                    JsonHelper.GetSearchResult(songlist, await UrlHelper.Search(SearchKey, page, count));
                    for (int i = 0; i < count; i++)
                    {
                        songlist.songs[i].cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + songlist.songs[i].album.id + ".jpg");
                    }
                }
                catch (Exception error)
                {
                    page--;
                    Debug.Write(error.Message);
                    await new MessageDialog("搜索失败，出现玄学").ShowAsync();
                }
            }
        }

        private void mySearchList_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.index = 0;
            MainPage.songList.songs.Clear();
            MainPage.songList.songs.Add((Song)e.ClickedItem);
            MainPage.continueFlag = true;
            MainPage.playStype = 2;
        }

        private async void search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

            //尚未实现search的提示
            if (!string.IsNullOrEmpty(search.Text))
            {
                try
                {
                    ModelCollection col = new ModelCollection(5);
                    JsonHelper.GetSmarkBox(col, await UrlHelper.GetSmarkBox(search.Text));
                    for (int i = 0; i < col.collection2.Count(); i++)
                    {
                        col.collection2[i].cover.UriSource = new Uri("https://y.gtimg.cn/music/photo_new/T002R300x300M000" + col.collection2[i].album.id + ".jpg");
                    }
                    search.ItemsSource = col.collection2;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                    await new MessageDialog("搜索失败，出现玄学").ShowAsync();
                }
            }
            // search.ItemsSource = songlist.songs;
        }
    }
}
