using homework4.Model;
using homework4.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homework4
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        MemoViewModel memoViewModel = ((App)App.Current).memoViewModel;
        StorageFile imageFile = null;
        StorageFile defaultImageFile = null;
        public NewPage()
        {
            this.InitializeComponent();
            // 把顶端对应的栏目变蓝
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Blue;
            titleBar.ButtonBackgroundColor = Colors.Blue;
            // 调整滑轮高度
            this.SizeChanged += (s, e) =>
            {
                MyScrollViewer.Height = e.NewSize.Height - 50;
            };
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void getImageFile()
        {
            defaultImageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/13.png"));
            imageFile = defaultImageFile;
        }

        private async void selectPicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker imageOpenPicker = new FileOpenPicker();
            imageOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            imageOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            imageOpenPicker.FileTypeFilter.Add(".jpg");
            imageOpenPicker.FileTypeFilter.Add(".jpeg");
            imageOpenPicker.FileTypeFilter.Add(".png");
            imageOpenPicker.FileTypeFilter.Add(".bmp");
            StorageFile file = await imageOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                imageFile = file;
                readSourceImage();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if ((string)MyButton.Content == "Create")
            {
                MyNowDetail.Text = "";
                MyNowTitle.Text = "";
                MyNowTime.Date = DateTimeOffset.Now;
                imageFile = defaultImageFile;
                readSourceImage();
            }
            else
            {
                MyNowImage.Source = memoViewModel.select.Img;
                MyNowDetail.Text = memoViewModel.select.Description;
                MyNowTitle.Text = memoViewModel.select.Title;
                MyNowTime.Date = memoViewModel.select.Time;
                imageFile = memoViewModel.select.imgFile;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = MyNowTitle.Text;
                composite["detail"] = MyNowDetail.Text;
                composite["time"] = MyNowTime.Date;
                if(imageFile != null)
                {
                    composite["image"] = StorageApplicationPermissions.FutureAccessList.Add(imageFile);
                }
                ApplicationData.Current.LocalSettings.Values["newpage"] = composite;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (((App)App.Current).first)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage");
                }
                else if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["newpage"] as ApplicationDataCompositeValue;
                    MyNowTitle.Text = (string)composite["title"];
                    MyNowDetail.Text = (string)composite["detail"];
                    MyNowTime.Date = (DateTimeOffset)composite["time"];
                    if (composite.ContainsKey("image"))
                    {
                        readImageFile((string)composite["image"]);
                        readSourceImage();
                    }
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage");
                    if (memoViewModel.select != null)
                        MyButton.Content = "Update";
                }
                ((App)App.Current).first = false;
            }
            else if (memoViewModel.isUpdate)
            {
                MyButton.Content = "Update";
                MyNowImage.Source = memoViewModel.select.Img;
                MyNowDetail.Text = memoViewModel.select.Description;
                MyNowTitle.Text = memoViewModel.select.Title;
                MyNowTime.Date = memoViewModel.select.Time;
                imageFile = memoViewModel.select.imgFile;
                delete.Visibility = Visibility.Visible;
            }
            else
            {
                MyButton.Content = "Create";
                delete.Visibility = Visibility.Collapsed;
                getImageFile();
            }
        }

        private async void readImageFile(string Token)
        {
            imageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(Token);
            // StorageApplicationPermissions.FutureAccessList.Remove(Token);
            // StorageApplicationPermissions.FutureAccessList.Clear();
        }

        private async void readSourceImage()
        {
            BitmapImage srcImage = new BitmapImage();
            if (imageFile != null)
            {
                using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
                {
                    await srcImage.SetSourceAsync(stream);
                    MyNowImage.Source = srcImage;
                }
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            memoViewModel.isUpdate = false;
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog("");
            bool flag = false;
            if (MyNowTitle.Text == "" || MyNowTitle.Text.Trim() == "")
            {
                flag = true;
                message.Content = "Title is empty!\n";
            }
            if (MyNowDetail.Text == "" || MyNowDetail.Text.Trim() == "")
            {
                flag = true;
                message.Content += "Description is empty!\n";
            }
            if (MyNowTime.Date.CompareTo(DateTime.Today) < 0)
            {
                flag = true;
                message.Content += "Time is Wrong";
            }
            if (!flag)
            {
                if ((string)MyButton.Content == "Create")
                {
                    memoViewModel.addMemo(MyNowTitle.Text, MyNowDetail.Text, (BitmapImage)MyNowImage.Source, MyNowTime.Date, imageFile);
                    message.Content = "Create Successfully!";
                }
                else
                {
                    memoViewModel.UpdateMemo(memoViewModel.select.id, MyNowTitle.Text, MyNowDetail.Text, (BitmapImage)MyNowImage.Source, MyNowTime.Date, imageFile);
                    message.Content = "Update Successfully!";
                    delete.Visibility = Visibility.Collapsed;
                }
            }
            await message.ShowAsync();
            if (!flag)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (memoViewModel.select != null)
            {
                memoViewModel.removeMemo(memoViewModel.select.id);
                memoViewModel.isUpdate = false;
                memoViewModel.select = null;
                memoViewModel.selectId = "";
                delete.Visibility = Visibility.Collapsed;
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
