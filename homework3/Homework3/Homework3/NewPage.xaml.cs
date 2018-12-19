using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Homework3
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog("");
            message.Title = "Error";
            bool flag = false;
            if (TitleBox.Text == "" || TitleBox.Text.Trim() == "")
            {
                message.Content = "Title is empty!";
                flag = true;
            }
            if (DetailBox.Text == "" || DetailBox.Text.Trim() == "")
            {
                message.Content += "Description is empty!";
                flag = true;
            }
            if (DatePicker.Date.CompareTo(DateTime.Today) < 0) // !DatePicker.Date.Equals(DateTime.Now) &&
            {
                message.Content += "Time is wrong!";
                flag = true;
            }
            if (flag)
                message.ShowAsync();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TitleBox.Text = "";
            DetailBox.Text = "";
            DatePicker.Date = DateTime.Now;
        }
    }
}
