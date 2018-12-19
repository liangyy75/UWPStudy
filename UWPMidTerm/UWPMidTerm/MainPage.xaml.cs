using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMidTerm.Models;
using UWPMidTerm.Pages;
using UWPMidTerm.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPMidTerm
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool isButton = false;

        private MenuItemManager recommandMenu = new MenuItemManager();
        private MenuItemManager myMusicMenu = new MenuItemManager();
        // 所有界面
        private Type[] types = { typeof(FindMusic), typeof(SongSheetRecommend), typeof(NewMusicRecommend), typeof(TopList), typeof(AlbumRelease),
            typeof(NativeMusic), typeof(DownloadManager), typeof(MyCollection), typeof(SongHistory)};

        public MainPage()
        {
            this.InitializeComponent();
            // 界面处理
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Red;
            titleBar.ButtonBackgroundColor = Colors.Red;
            LeftControl.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 247));
            ButtonControl.Background = new SolidColorBrush(Color.FromArgb(255, 246, 246, 248));
            // 自适应
            this.SizeChanged += (s, e) => 
            {
                double width = e.NewSize.Width;
                if (width > 800)
                {
                    MediaProgress.Width = (width - 260) * 2 / 3 - 80;
                    VolumeRange.Width = (width - 260) / 3 - 50;
                }
                if (width < 800 || e.NewSize.Height < 600)
                    ApplicationView.GetForCurrentView().TryResizeView(new Size(1000, 600));
            };
            // 数据准备
            UrlHelper.PrePareFolder();
            recommandMenu.GetRecommendMenu();
            myMusicMenu.GetMyMusicMenu();
            MyFrame.Navigate(typeof(FindMusic));
            // 实时检测
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += menuTimer_Tick;
            timer.Start();
        }

        private void menuTimer_Tick(object sender, object e)
        {
            for(int i = 0; i < types.Length; i++)
            {
                if (MyFrame.SourcePageType == types[i] && i < 5)
                    RecommendMenu.SelectedIndex = i;
                else if (MyFrame.SourcePageType == types[i] && i > 4)
                    MyMusicMenu.SelectedIndex = i - 5;
            }
            isButton = MyScrollViewer.ScrollableHeight == MyScrollViewer.VerticalOffset;
        }

        private void RecommendMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyMusicMenu.SelectedIndex = -1;
            MyFrame.Navigate(((MenuItem)e.ClickedItem).purpose);
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VolumeRange_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoopPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RandomPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScreenControl_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyMusicMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            RecommendMenu.SelectedIndex = -1;
            MyFrame.Navigate(((MenuItem)e.ClickedItem).purpose);
        }
    }
}
