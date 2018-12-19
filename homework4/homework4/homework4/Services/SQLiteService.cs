using homework4.Model;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace homework4.Services
{
    class SQLiteService
    {
        private SQLiteConnection connection;
        public StorageFolder folder = ApplicationData.Current.LocalFolder;
        public StorageFolder imageFolder = null;

        // 构造函数，连接数据库和表
        public SQLiteService()
        {
            LoadDatabase();
        }

        // 加载表
        private async void LoadDatabase()
        {
            connection = new SQLiteConnection("mysqlite.db");
            string sql = @"CREATE TABLE IF NOT EXISTS
                Memo(Id         VARCHAR(20) PRIMARY KEY NOT NULL,
                     Title      VARCHAR(20),
                     Detail     VARCHAR(200),
                     Time       DATETIME,
                     Completed  VARCHAR(5),
                     ImgPath    VARCHAR(20)
            );";
            using(var statement = connection.Prepare(sql))
            {
                statement.Step();
            }
            if(await folder.TryGetItemAsync("images") == null)
            {
                await folder.CreateFolderAsync("images");
            }
            imageFolder = await folder.GetFolderAsync("images");
        }

        // 增
        public async void add(Memo memo)
        {
            using (var statement = connection.Prepare("INSERT INTO Memo(Id, Title, Detail, Time, Completed, ImgPath) VALUES (?, ?, ?, ?, ?, ?)"))
            {
                statement.Bind(1, memo.id);
                statement.Bind(2, memo.Title);
                statement.Bind(3, memo.Description);
                statement.Bind(4, memo.Time.ToString());
                statement.Bind(5, memo.Completed.ToString());
                StorageFolder nowImageFolder = await imageFolder.CreateFolderAsync(memo.id);
                memo.imgFile = await memo.imgFile.CopyAsync(nowImageFolder);
                statement.Bind(6, memo.imgFile.Name);
                statement.Step();
            }
        }

        // 删
        public async void delete(string id)
        {
            using (var statement = connection.Prepare("DELETE FROM Memo WHERE Id = ?"))
            {
                statement.Bind(1, id);
                statement.Step();
                StorageFolder nowImageFolder = await imageFolder.GetFolderAsync(id);
                await nowImageFolder.DeleteAsync();
            }
        }

        // 改
        public async void update(Memo memo)
        {
            Memo oldmemo = await select(memo.id);
            if (oldmemo != null)
            {
                using (var statement = connection.Prepare("UPDATE Memo SET Title = ?, Detail = ?, Time = ?, Completed = ?, ImgPath = ? WHERE Id = ?"))
                {
                    if (oldmemo.imgFile.Name != memo.imgFile.Name)
                    {
                        StorageFolder nowImageFolder = await imageFolder.GetFolderAsync(memo.id);
                        await oldmemo.imgFile.DeleteAsync();
                        memo.imgFile = await memo.imgFile.CopyAsync(nowImageFolder);
                    }
                    statement.Bind(1, memo.Title);
                    statement.Bind(2, memo.Description);
                    statement.Bind(3, memo.Time.ToString());
                    statement.Bind(4, memo.Completed.ToString());
                    statement.Bind(5, memo.imgFile.Name);
                    statement.Bind(6, memo.id);
                    statement.Step();
                }
            }
        }

        // 查
        public async Task<Memo> select(string id)
        {
            Memo memo = null;
            using(var statement = connection.Prepare("SELECT Id, Title, Detail, Time, Completed, ImgPath FROM Memo WHERE Id = ?"))
            {
                statement.Bind(1, id);
                if(SQLiteResult.ROW == statement.Step())
                {
                    memo = await getMemo(statement, id);
                }
            }
            return memo;
        }

        // 将statement得到的数据转化为memo
        private async Task<Memo> getMemo(ISQLiteStatement statement, string id)
        {
            StorageFolder nowImageFolder = await imageFolder.GetFolderAsync(id);
            StorageFile file = await nowImageFolder.GetFileAsync((string)statement[5]);
            BitmapImage srcImage = new BitmapImage();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    await srcImage.SetSourceAsync(stream);
                }
            }
            Memo memo = new Memo((string)statement[1], (string)statement[2], srcImage, DateTimeOffset.Parse((string)statement[3]), file);
            memo.Completed = bool.Parse((string)statement[4]);
            memo.id = id;
            return memo;
        }

        // 将数据库所有数据转化为memo，应用开启时使用
        public async Task<ObservableCollection<Memo>> readTables()
        {
            ObservableCollection<Memo> memos = new ObservableCollection<Memo>();
            using (var statement = connection.Prepare("SELECT Id, Title, Detail, Time, Completed, ImgPath FROM Memo"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    memos.Add(await getMemo(statement, (string)statement[0]));
                }
            }
            return memos;
        }

        // 模糊查询
        public StringBuilder query(string message)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            using (var statement = connection.Prepare("SELECT Title, Detail, Time FROM Memo WHERE Title LIKE ? OR DetaIL LIKE ? OR Time LIKE ?"))
            {
                string bind = "%" + message + "%";
                statement.Bind(1, bind);
                statement.Bind(2, bind);
                statement.Bind(3, bind);
                while (SQLiteResult.ROW == statement.Step())
                {
                    if(flag == false)
                    {
                        flag = true;
                    }
                    else
                    {
                        builder.Append("\n");
                    }
                    builder.Append("Title: ").Append((string)statement[0]).Append(" Description: ").Append((string)statement[1]).Append(" Time: ").Append((string)statement[2]);
                }
            }
            return builder;
        }
    }
}
