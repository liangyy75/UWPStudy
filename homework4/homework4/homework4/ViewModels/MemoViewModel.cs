using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using System.IO;
using homework4.Model;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using homework4.Services;
using Windows.Data.Xml.Dom;
using System.Diagnostics;

namespace homework4.ViewModels
{
    [DataContract]
    public class MemoViewModel
    {
        public MemoViewModel()
        {
            select = null;
            isUpdate = false;
        }

        // 用于数据库操作
        private SQLiteService sQLiteService = new SQLiteService();
        [DataMember(Order = 0)]
        private ObservableCollection<Memo> allItems = new ObservableCollection<Model.Memo>();
        public ObservableCollection<Memo> AllItems { get { return this.allItems; } }
        [DataMember(Order = 1)]

        // 被选中的memo的id
        public string selectId { set; get; } = "";
        // 被选中的memo
        public Memo select { set; get; }
        [DataMember(Order = 2)]
        // 被选中的是否处于更新
        public bool isUpdate = false;

        // 添加memo
        public void addMemo(string title, string description, BitmapImage img, DateTimeOffset time, StorageFile imgFile)
        {
            Memo memo = new Memo(title, description, img, time, imgFile);
            this.allItems.Add(memo);
            sQLiteService.add(memo);
            UpdatePrimaryTile();
        }

        // 删除memo
        public void removeMemo(string id)
        {
            int count = AllItems.Count();
            for (int i = 0; i < count; i++)
            {
                if(AllItems.ElementAt(i).id == id)
                {
                    AllItems.RemoveAt(i);
                    break;
                }
            }
            select = null;
            sQLiteService.delete(id);
            UpdatePrimaryTile();
        }

        // 完成memo
        public void finishMemo(string id)
        {
            int count = AllItems.Count();
            for (int i = 0; i < count; i++)
            {
                if (AllItems.ElementAt(i).id == id)
                {
                    Memo memo = AllItems.ElementAt(i);
                    memo.Completed = !memo.Completed;
                    sQLiteService.update(memo);
                }
            }
        }

        // 更新memo
        public void UpdateMemo(string id, string title, string description, BitmapImage img, DateTimeOffset time, StorageFile imgFile)
        {
            int count = AllItems.Count();
            int i;
            for (i = 0; i < count; i++)
            {
                if(AllItems.ElementAt(i).id == id)
                {
                    Memo memo = AllItems.ElementAt(i);
                    memo.Title = title;
                    memo.Description = description;
                    memo.Img = img;
                    memo.Time = time;
                    memo.imgFile = imgFile;
                    sQLiteService.update(memo);
                    //AllItems[i] = new Model.Memo(title, description, img, time);
                }
            }
            select = null;
            UpdatePrimaryTile();
        }

        // 对实体类对象进行序列化
        public string ToJson()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MemoViewModel));
            string result = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, this);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        // 将Json字符串反序列化为实体类对象
        public static MemoViewModel FromJson(string jsonString)
        {
            var ds = new DataContractJsonSerializer(typeof(MemoViewModel));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            MemoViewModel obj = (MemoViewModel)ds.ReadObject(ms);
            ms.Dispose();
            return obj;
        }

        // 从FutureAccessList中读取存储的StorageFile
        public async void readStorageFile()
        {
            for (int i = 0; i < AllItems.Count(); i++)
            {
                BitmapImage srcImage = new BitmapImage();
                Memo obj = AllItems.ElementAt(i);
                obj.imgFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(obj.token);
                if (obj.imgFile != null)
                {
                    using (IRandomAccessStream stream = await obj.imgFile.OpenAsync(FileAccessMode.Read))
                    {
                        await srcImage.SetSourceAsync(stream);
                        obj.Img = srcImage;
                    }
                }
            }
            UpdatePrimaryTile();
        }

        // 更新磁贴
        public void UpdatePrimaryTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueueForWide310x150(true);
            updater.EnableNotificationQueueForSquare150x150(true);
            updater.EnableNotificationQueueForSquare310x310(true);
            updater.EnableNotificationQueue(true);
            updater.Clear();
            for(int i = 0; i < AllItems.Count() && i < 5; i++)
            {
                XmlDocument xmlDoc = TileService.CreateTiles(AllItems[i]);
                updater.Update(new TileNotification(xmlDoc));
            }
        }

        // 加载数据库的memo
        public async void addDatabaseItem()
        {
            var smg = await sQLiteService.readTables();
            for(int i = 0; i < smg.Count; i++)
            {
                allItems.Add(smg[i]);
            }
            UpdatePrimaryTile();
        }
        
        // 模糊查询
        public StringBuilder findMemo(string message)
        {
            return sQLiteService.query(message);
        }
    }
}
