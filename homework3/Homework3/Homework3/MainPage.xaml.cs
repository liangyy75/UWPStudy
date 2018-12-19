using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Homework3
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // https://www.cnblogs.com/TianFang/p/4732471.html
            // http://www.cnblogs.com/durow/p/4897773.html
            // http://blog.csdn.net/xuzhongxuan/article/details/50814809
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Blue;
            titleBar.ButtonBackgroundColor = Colors.Blue;
        }

        private void cb_Click(object sender, RoutedEventArgs e)
        {
            if (sender == cb1)
                L1.Visibility = L1.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else if (sender == cb2)
                L2.Visibility = L2.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewPage));
        }
    }
}
