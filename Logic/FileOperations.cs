using System.IO;
using UlConnect.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace UlConnect.Logic
{
    public static class FileOperations
    {
        public static Action UnsuccessImportAction {get; set;}
        public static string AppDirectory {get {return AppDomain.CurrentDomain.BaseDirectory;}}
        public static string ReadDataFromFile(string filename)
        {
            string data = "";
            TextReader tr = null;
            if (File.Exists(AppDirectory + filename + ".json"))
            {
                try
                {
                    tr = new StreamReader(AppDirectory + filename + ".json");
                    data = tr.ReadToEnd();
                }
                catch 
                {
                    Debug.WriteLine("Unexpected error");
                }
                finally
                {
                    tr.Close();
                }
            }
            return data;
        }
        public static List<ConnectionInfoItem> LoadDataToDatabase(string data)
        {
            List<ConnectionInfoItem> database = new List<ConnectionInfoItem>();
            try
            {
                if (!(JsonConvert.DeserializeObject<ObservableCollection<ConnectionInfoItem>>(data) == null))
                {
                    database = JsonConvert.DeserializeObject<List<ConnectionInfoItem>>(data);
                    Debug.WriteLine("successful import, number of connections = " + database.Count);
                }
            }
            catch (JsonReaderException e)
            {
                // TODO UnsuccessImportAction();
                Debug.WriteLine("Unsuccessful import ");
            }
            return database;
        }
        public static void SynchronizeFileWithDatabase(ObservableCollection<ConnectionInfoItem> database, string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            TextWriter tw = new StreamWriter(AppDirectory + filename + ".json");
            serializer.Serialize(tw, database);
            tw.Close();
        }
        public static List<string> GetFileNames(string path)
        {
            List<string> names = new List<string>();
            if (Directory.Exists(AppDirectory + path))
            {
                DirectoryInfo dr = new DirectoryInfo(AppDirectory + path);
                FileInfo[] files = dr.GetFiles();
                foreach (var fileinfo in files)
                {
                    names.Add(fileinfo.Name.Split('.')[0]);
                }
            }
            return names;
        }
        public static Dictionary<string,string> LoadDataToStringDictionary(string data)
        {
            Dictionary <string,string> dictionary = new Dictionary<string, string>();
            try
            {
                if (!(JsonConvert.DeserializeObject<Dictionary<string,string>>(data) == null))
                {
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string,string>>(data);
                    Debug.WriteLine("successful import, elements " + dictionary.Count);
                }
            }
            catch (Exception e)
            {
                // TODO UnsuccessImportAction();
                Debug.WriteLine("Unsuccessful import ");
            }
            return dictionary;
        }
        public static void SaveStringDictionary(Dictionary<string,string> dictionary, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            TextWriter tw = new StreamWriter(String.Format("{0}{1}", AppDirectory, path));
            serializer.Serialize(tw, dictionary);
            tw.Close();
        }
        
    }
}