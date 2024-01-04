using EvernoteCloneWPF.Model;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.Helper
{
    public class DatabaseHelper
    {
       private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
       private static string dbPath = "https://nevernote-511c2-default-rtdb.firebaseio.com/";

        public static async Task<bool> InsertAsync<T>(T item)
        {
            
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //jeżeli item jest z klasy T i zadeklaruje, że T implementuje HasId
        //to w kodzie będę mógł się odwołać do item.prop z interfejsu 
        public async static Task<bool> Update<T>(T item) where T : HasId
        {
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.PatchAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async static Task<bool> Delete<T>(T item) where T : HasId
        {
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // tu będzie zwrot listy<T> bo czytam z JAKIEJŚ (nie wiem jeszcze) tabeli
        // już wiem z jakiej -> obiekty z tej tabeli implementują interfejs HasId
        public static async Task<List<T>> Read<T>() where T  : HasId
        {
            //List<T> items;

            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    items = conn.Table<T>().ToList();
            //}

            //return items;
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                    var jsonResult = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);
                        if (objects != null)
                        {
                            List<T> list = new List<T>();
                            foreach (var o in objects)
                            {
                                o.Value.Id = o.Key;
                                list.Add(o.Value);
                            }

                            return list;
                        }
                        else
                        {
                            List<T> elist = new List<T>();
                            return elist;
                        }
                        
                    }
                    else
                    {
                       
                        Console.WriteLine($"HTTP request failed with status code: {result.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }

    }
}
