using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm.Models;
using UWPMidTerm.Pages;

namespace UWPMidTerm.ViewModels
{
    public class MenuItemManager
    {
        public ObservableCollection<MenuItem> menuItems { set; get; } = new ObservableCollection<MenuItem>();

        public void GetRecommendMenu()
        {
            // 发现音乐
            menuItems.Add(new MenuItem("\xE094", "发现音乐", typeof(FindMusic)));
            // 歌单推荐
            menuItems.Add(new MenuItem("\xE142", "歌单推荐", typeof(SongSheetRecommend)));
            // 最新音乐
            menuItems.Add(new MenuItem("\xE189", "最新音乐", typeof(NewMusicRecommend)));
            // 排行榜
            menuItems.Add(new MenuItem("\xE1E5", "排行榜", typeof(TopList)));
            // 专辑发送
            menuItems.Add(new MenuItem("\xE772", "专辑发送", typeof(AlbumRelease)));
        }

        public void GetMyMusicMenu()
        {
            // 本地音乐
            menuItems.Add(new MenuItem("\xEDE3", "本地音乐", typeof(NativeMusic)));
            // 下载管理
            menuItems.Add(new MenuItem("\xE118", "下载管理", typeof(DownloadManager)));
            // 我的收藏
            menuItems.Add(new MenuItem("\xE0B4", "我的收藏", typeof(MyCollection)));
            // 听歌历史
            menuItems.Add(new MenuItem("\xE003", "听歌历史" ,typeof(SongHistory)));
        }
    }
}
