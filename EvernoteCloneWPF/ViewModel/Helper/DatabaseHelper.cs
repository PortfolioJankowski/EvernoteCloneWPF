using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.Helper
{
    public class DatabaseHelper
    {
       private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
    
       public static bool Insert<T>(T item)
        {
            bool result = false;
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Update(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Delete(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        // tu będzie zwrot listy<T> bo czytam z JAKIEJŚ (nie wiem jeszcze) tabeli
        public static List<T> Read<T>(T item) where T : new()
        {
            List<T> items;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                //bez tego where T:new() nie mógłbym użyć poniższej metody, bo nie może być to abstrakt
                items = conn.Table<T>().ToList();
            }
            return items;
        }
    }
}
