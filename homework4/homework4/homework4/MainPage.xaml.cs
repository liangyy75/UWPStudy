using homework4.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using homework4.ViewModels;
using Windows.UI;
using Windows.UI.Popups;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Windows.ApplicationModel.DataTransfer;
using homework4.Services;
using System.Diagnostics;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace homework4
{
    // https://docs.microsoft.com/zh-cn/windows/uwp/files/how-to-track-recently-used-files-and-folders
    // http://www.genshuixue.com/i-cxy/p/7978410
    // http://www.bubuko.com/infodetail-1823935.html
    // https://www.2cto.com/kf/201610/554143.html
    // https://docs.microsoft.com/zh-cn/windows/uwp/files/file-access-permissions#accessing-additional-locations
    // https://docs.microsoft.com/zh-cn/windows/uwp/packaging/app-capability-declarations
    // https://msdn.microsoft.com/library/windows/apps/xaml/br230259.aspx
    // https://msdn.microsoft.com/zh-cn/library/windows/apps/windows.storage.accesscache.storageapplicationpermissions.aspx

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //public MemoViewModel memoViewModel = MemoViewModel.getInstance();
        public MemoViewModel memoViewModel = ((App)App.Current).memoViewModel;
        double width = 0;
        StorageFile imageFile = null;
        StorageFile defaultImageFile = null;
        public MainPage()
        {
            this.InitializeComponent();
            // 把顶端对应的栏目变蓝
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Blue;
            titleBar.ButtonBackgroundColor = Colors.Blue;
            // 调整滑轮高度
            this.SizeChanged += (s, e) =>
            {
                width = e.NewSize.Width;
                MyScrollViewer.Height = e.NewSize.Height - 100;
                MyOtherScrollViewer.Height = e.NewSize.Height - 100;
            };
            NavigationCacheMode = NavigationCacheMode.Enabled;
            MyNowDetail.Text = ApplicationData.Current.LocalFolder.Path;
        }

        // 启动时得到默认的imagefile。
        private async void getImageFile()
        {
            defaultImageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/13.png"));
            imageFile = defaultImageFile;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            memoViewModel.select = (Memo)e.ClickedItem;
            memoViewModel.selectId = memoViewModel.select.id;
            showSelectItem();
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
            if(file != null)
            {
                imageFile = file;
                readSourceImage();
            }
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
                    MyButton.Content = "Create";
                    delete.Visibility = Visibility.Collapsed;
                }
            }
            await message.ShowAsync();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (memoViewModel.select != null)
            {
                MyNowImage.Source = memoViewModel.select.Img;
                MyNowDetail.Text = memoViewModel.select.Description;
                MyNowTitle.Text = memoViewModel.select.Title;
                MyNowTime.Date = memoViewModel.select.Time;
                imageFile = memoViewModel.select.imgFile;
            }
            else
            {
                MyNowDetail.Text = "";
                MyNowTime.Date = DateTimeOffset.Now;
                MyNowTitle.Text = "";
                imageFile = defaultImageFile;
                readSourceImage();
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (width < 800)
            {
                this.Frame.Navigate(typeof(NewPage));
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            deleteSelectItem();
        }

        // 离开
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                if (imageFile != null)
                {
                    composite["image"] = StorageApplicationPermissions.FutureAccessList.Add(imageFile);
                }
                composite["title"] = MyNowTitle.Text;
                composite["detail"] = MyNowDetail.Text;
                composite["time"] = MyNowTime.Date;
                //composite["viewmodel"] = JsonConvert.SerializeObject(memoViewModel);
                ApplicationData.Current.LocalSettings.Values["newpage"] = composite;
            }
            DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
        }

        // 启动
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
                    //memoViewModel = JsonConvert.DeserializeObject<MemoViewModel>((string)composite["viewmodel"]);
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage");
                    if (memoViewModel.select != null)
                        MyButton.Content = "Update";
                }
                ((App)App.Current).first = false;
                // memoViewModel.UpdatePrimaryTile();
                DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;
            }
            else
            {
                getImageFile();
            }
        }

        private async void readImageFile(string Token)
        {
            imageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(Token);
            // StorageApplicationPermissions.FutureAccessList.Remove(Token); // ???
            // StorageApplicationPermissions.FutureAccessList.Clear();  // ???
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

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            selectItem(sender);
            deleteSelectItem();
        }

        private void deleteSelectItem()
        {
            if (memoViewModel.select != null)
            {
                memoViewModel.removeMemo(memoViewModel.select.id);
                memoViewModel.isUpdate = false;
                memoViewModel.select = null;
                memoViewModel.selectId = "";
                MyButton.Content = "Create";
                delete.Visibility = Visibility.Collapsed;
            }
        }

        private void EditMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            selectItem(sender);
            showSelectItem();
        }

        // 将sender转化为选中的memo
        private void selectItem(object sender)
        {
            var s = sender as FrameworkElement;
            memoViewModel.select = (Memo)s.DataContext;
            memoViewModel.selectId = memoViewModel.select.id;
        }

        private void showSelectItem()
        {
            if (width < 800)
            {
                memoViewModel.isUpdate = true;
                delete.Visibility = Visibility.Collapsed;
                //imageFile = null;
                this.Frame.Navigate(typeof(NewPage));
            }
            else
            {
                MyButton.Content = "Update";
                MyNowImage.Source = memoViewModel.select.Img;
                MyNowDetail.Text = memoViewModel.select.Description;
                MyNowTitle.Text = memoViewModel.select.Title;
                MyNowTime.Date = memoViewModel.select.Time;
                imageFile = memoViewModel.select.imgFile;
                delete.Visibility = Visibility.Visible;
            }
        }

        // 数据共享
        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            selectItem(sender);
            DataTransferManager.ShowShareUI();
        }

        // 数据共享的方法
        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var data = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            data.Properties.Title = memoViewModel.select.Title;
            data.Properties.Description = "my test";
            data.SetStorageItems(new List<StorageFile> { memoViewModel.select.imgFile });
            data.SetWebLink(new Uri("http://seattletimes.com/ABPub/2006/01/10/2002732410.jpg"));
            data.SetText(memoViewModel.select.Description);
            deferral.Complete();
        }

        // 完成memo的点击
        private void MyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            selectItem(sender);
            if(memoViewModel.select != null)
            {
                memoViewModel.finishMemo(memoViewModel.select.id);
            }
        }

        // 模糊查询
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = memoViewModel.findMemo(MyNowSearch.Text);
            var messageDialog = new MessageDialog(builder.ToString());
            await messageDialog.ShowAsync();
        }
    }
}
