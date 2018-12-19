using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPMidTerm3.Models;
using Windows.Storage;

namespace UWPMidTerm3.Services
{
    public class SqlHelper
    {
        private static SQLiteConnection connection;

        // 连接数据库
        public static void PrepareConnection()
        {
            connection = new SQLiteConnection("music.db");
            string historyMusicSql = @"CREATE TABLE IF NOT EXISTS
                History(songmid     VARCHAR(10) PRIMARY KEY NOT NULL,
                        title       VARCHAR(10),
                        _time       VARCHAR(5),
                        interval    VARCHAR(5),
                        singers     VARCHAR(20),
                        albummid    VARCHAR(10),
                        albumname   VARCHAR(10),
                        type        VARCHAR(10),
                        fileName    VARCHAR(10)
            );";
            string collectMusicSql = @"CREATE TABLE IF NOT EXISTS
                Collection(songmid     VARCHAR(10) PRIMARY KEY NOT NULL,
                        title       VARCHAR(10),
                        _time       VARCHAR(5),
                        interval    VARCHAR(5),
                        singers     VARCHAR(20),
                        albummid    VARCHAR(10),
                        albumname   VARCHAR(10),
                        type        VARCHAR(10),
                        fileName    VARCHAR(10)
            );";
            using (var statement = connection.Prepare(historyMusicSql))
                statement.Step();
            using (var statement = connection.Prepare(collectMusicSql))
                statement.Step();
        }

        // 增
        public static void AddSong(Song song, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(songmid, title, _time, interval, singers, albummid, albumname, type, fileName) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)"))
                {
                    bool localFlag = song.musicFile != null;
                    statement.Bind(1, localFlag ? song.musicFile.Name : song.songmid);
                    statement.Bind(2, song.title);
                    statement.Bind(3, song._time.ToString());
                    statement.Bind(4, song.interval);
                    statement.Bind(5, string.Join("/", song.singers.ToArray()));
                    statement.Bind(6, (localFlag && song.pictuFile != null) ? song.pictuFile.Name : song.album.id);
                    statement.Bind(7, song.album.title);
                    statement.Bind(8, song.type);
                    statement.Bind(9, localFlag ? song.musicFile.Name : "");
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // 删 : 本地的用title/网络的用id
        public static void DeleteSong(string tableName, string fileName, string id = "")
        {
            try
            {
                bool flag = id == "";
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + (flag ? "fileName = ?" : "songmid = ?")))
                {
                    statement.Bind(1, flag ? fileName : id);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // 检查是否已存在
        public static async Task<bool> SelectSong(Song song, string tableName, string fileName, string id = "")
        {
            try
            {
                bool flag = id == "";
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + (flag ? "fileName = " + fileName : "songmid = " + id)))
                {
                    if (statement.Step() == SQLiteResult.ROW)
                    {
                        song.type = (string)statement[7];
                        if (song.type == "native")
                            song.musicFile = await KnownFolders.MusicLibrary.GetFileAsync((string)statement[0]);
                        else if (song.type == "net")
                        {
                            song.songmid = (string)statement[0];
                            song.album.id = (string)statement[5];
                        }
                        else if (song.type == "download")
                        {
                            song.musicFile = await UrlHelper.music.GetFileAsync((string)statement[0]);
                            song.pictuFile = await UrlHelper.picture.GetFileAsync((string)statement[5]);
                        }
                        song.title = (string)statement[1];
                        song._time = int.Parse((string)statement[2]);
                        song.interval = (string)statement[3];
                        song.singers = new List<string>(((string)statement[4]).Split('/'));
                        song.album.title = (string)statement[6];
                        song.fileName = (string)statement[8];
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        // 将所有数据转化为song/模糊查询
        public async static void GetAllSongs(ObservableCollection<Song> songs, string tableName, string searchStr = "")
        {
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
                sql += " WHERE title LIKE '%" + searchStr + "%' OR singers LIKE '%" + searchStr + "%' OR albumname LIKE '%" + searchStr + "%'";
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Song song = new Song(false);
                    song.type = (string)statement[7];
                    if (song.type == "native")
                        song.musicFile = await KnownFolders.MusicLibrary.GetFileAsync((string)statement[0]);
                    else if (song.type == "net")
                    {
                        song.songmid = (string)statement[0];
                        song.album.id = (string)statement[5];
                    }
                    else if (song.type == "download")
                    {
                        song.musicFile = await UrlHelper.music.GetFileAsync((string)statement[0]);
                        song.pictuFile = await UrlHelper.picture.GetFileAsync((string)statement[5]);
                    }
                    song.title = (string)statement[1];
                    song._time = int.Parse((string)statement[2]);
                    song.interval = (string)statement[3];
                    song.singers = new List<string>(((string)statement[4]).Split('/'));
                    song.album.title = (string)statement[6];
                    song.fileName = (string)statement[8];
                    songs.Add(song);
                }
            }
        }
    }
}
