﻿using homework4.Model;
using homework4.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace homework4
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        public MemoViewModel memoViewModel = null;
        public bool first = true;
        public bool issuspend = false;
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
            memoViewModel = new MemoViewModel();
        }

        private void OnResuming(object sender, object e)
        {
            //...
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //ApplicationExecutionState previousState = e.PreviousExecutionState;
            //ActivationKind activationKind = e.Kind;

            Frame rootFrame = Window.Current.Content as Frame;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState"))
                    {
                        /*if (ApplicationData.Current.LocalSettings.Values.ContainsKey("viewmodel"))
                        {
                            var composite = ApplicationData.Current.LocalSettings.Values["viewmodel"] as ApplicationDataCompositeValue;
                            string jsonString = (string)composite["viewmodel"];
                            if (jsonString != null)
                                memoViewModel = MemoViewModel.FromJson(jsonString);
                            memoViewModel.readStorageFile();
                            for(int i = 0; i < memoViewModel.AllItems.Count(); i++)
                                if(memoViewModel.selectId == memoViewModel.AllItems.ElementAt(i).id)
                                {
                                    memoViewModel.select = memoViewModel.AllItems.ElementAt(i);
                                    break;
                                }
                            ApplicationData.Current.LocalSettings.Values.Remove("viewmodel");
                        }*/
                        memoViewModel.addDatabaseItem();
                        rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
                    }
                }
                else
                {
                    memoViewModel.addDatabaseItem();
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = rootFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            rootFrame.Navigated += onNavigated;
        }

        private void onNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            issuspend = true;
            var deferral = e.SuspendingOperation.GetDeferral();
            
            /*ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            for (int i = 0; i < memoViewModel.AllItems.Count(); i++)
            {
                memoViewModel.AllItems.ElementAt(i).token = StorageApplicationPermissions.FutureAccessList.Add(memoViewModel.AllItems.ElementAt(i).imgFile);
            }
            composite["viewmodel"] = memoViewModel.ToJson();
            ApplicationData.Current.LocalSettings.Values["viewmodel"] = composite;*/

            //TODO: 保存应用程序状态并停止任何后台活动

            //Get the frame navigation state serialized as a string and save in settings
            Frame frame = Window.Current.Content as Frame;
            ApplicationData.Current.LocalSettings.Values["NavigationState"] = frame.GetNavigationState();

            deferral.Complete();
        }
    }
}
