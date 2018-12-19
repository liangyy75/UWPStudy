using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm3.Models;
using UWPMidTerm3.Pages;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;

namespace UWPMidTerm3.ViewModels
{
    public class MenuItemManager
    {
        public ObservableCollection<MenuItem> menuItems { set; get; } = new ObservableCollection<MenuItem>();

        public void GetRecommendMenu()
        {
            // 发现音乐
            menuItems.Add(new MenuItem(0, "\xE094", "发现音乐", typeof(FindMusic)/*, new EntranceNavigationTransitionInfo()*/));
            // 歌单推荐
            menuItems.Add(new MenuItem(1, "\xE142", "歌单推荐", typeof(SongSheetRecommend)/*, new DrillInNavigationTransitionInfo()*/));
            // 排行榜
            menuItems.Add(new MenuItem(2, "\xE1E5", "排行榜", typeof(TopList)/*, new SuppressNavigationTransitionInfo()*/));
            // 专辑发送
            menuItems.Add(new MenuItem(3, "\xE772", "专辑推送", typeof(AlbumRelease)/*, new SlideNavigationTransitionInfo()*/));
            // 搜索音乐
            menuItems.Add(new MenuItem(4, "\xE189", "搜索音乐", typeof(SearchDetail)/*, new ContinuumNavigationTransitionInfo()*/));
        }

        public void GetMyMusicMenu()
        {
            // 本地音乐
            menuItems.Add(new MenuItem(5, "\xEDE3", "本地音乐", typeof(NativeMusic)/*, new CommonNavigationTransitionInfo()*/));
            // 下载管理
            menuItems.Add(new MenuItem(6, "\xE118", "下载管理", typeof(DownloadManager)/*, new EntranceNavigationTransitionInfo()*/));
            // 我的收藏
            menuItems.Add(new MenuItem(7, "\xE0B4", "我的收藏", typeof(MyCollection)/*, new EntranceNavigationTransitionInfo()*/));
            // 听歌历史
            menuItems.Add(new MenuItem(8, "\xE003", "听歌历史", typeof(SongHistory)/*, new EntranceNavigationTransitionInfo()*/));
        }

        public void BackUp()
        {
            for (int i = 0; i < menuItems.Count(); i++)
            {
                menuItems[i].background.Color = Color.FromArgb(255, 245, 245, 247);
                menuItems[i].borderground.Color = Color.FromArgb(255, 245, 245, 247);
            }
        }
    }
}
