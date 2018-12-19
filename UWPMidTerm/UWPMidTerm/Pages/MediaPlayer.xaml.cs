using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class MediaPlayer : Page
    {
        public static string songmid = "";
        ObservableCollection<Song> songs = new ObservableCollection<Song>();

        public MediaPlayer()
        {
            this.InitializeComponent();
            LoadResource();
        }

        private async void LoadResource()
        {
            string result = await UrlHelper.GetSongMessageBySongmid(songmid);
            Song song = new Song();
            JsonHelper.GetSongFromJson(song, JsonObject.Parse(result).GetNamedArray("data").GetObjectAt(0));
            songs.Add(song);
        }
    }
}
