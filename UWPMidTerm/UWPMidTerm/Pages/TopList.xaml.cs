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
    public sealed partial class TopList : Page
    {
        ObservableCollection<Chart> concreteCharts = new ObservableCollection<Chart>();
        ObservableCollection<Chart> restCharts = new ObservableCollection<Chart>();
        ObservableCollection<Chart> globalCharts = new ObservableCollection<Chart>();

        public TopList()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                double w = e.NewSize.Width;
                double width = w / 3 - 30;
                border1.Width = width;
                border2.Width = width * 4;
                border2.Height = 0.3125 * width;
                border1.Height = 380 + border2.Height;
                rect1.Rect = new Rect(1.5 * width, 0, width, border2.Height);
                border3.Width = w / 4 - 30;
            };
            LoadResource();
        }

        private async void LoadResource()
        {
            string result = await UrlHelper.GetAllCharts();
            JsonArray jarray = JsonArray.Parse(result);
            JsonArray array1 = jarray[0].GetObject()["List"].GetArray();
            JsonArray array2 = jarray[1].GetObject()["List"].GetArray();
            JsonHelper.GetConcreteChart(concreteCharts, restCharts, globalCharts, array1, array2);
        }

        private void ConcreteCharts_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ChartSongs_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void RestCharts_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void GlobalCharts_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
