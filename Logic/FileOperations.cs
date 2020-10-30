using System.IO;
using UlConnect.Models;
using Newtonsoft.Json;
using System;
using UlConnect.Views;
using Avalonia.Controls.ApplicationLifetimes;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace UlConnect.Logic
{
    public static class FileOperations
    {
        public static Action<string> UnsuccessImportAction {get; set;} //function which is called when occurs error while import something        
        public static string AppDirectory {get {return AppDomain.CurrentDomain.BaseDirectory;}} //Directory where is stored app (compiled)
        ///<summary>
        ///Check if MainWindow's loaded until MainWindow's loaded and then calls UnsuccessImportAction.
        ///</summary>
        ///<param name="error">
        ///Error text
        ///</param>
        public static void CallUnsucessfullImportAction(string error)
        {  
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (!MainWindow.IsWindowLoaded) {
                    Thread.Sleep(100);
                };
                UnsuccessImportAction(error);
            });
        }
        ///<summary>
        ///Reads data from file and return string data
        ///</summary>
        ///<returns>
        ///Data in string from file
        ///</returns>
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
                catch (Exception e)
                {
                    CallUnsucessfullImportAction("Error when read file");
                }
                finally
                {
                    tr.Close();
                }
            }
            return data;
        }
        ///<summary>
        /// Deserealizes JSON data into List where are stored ConnectionInfoItems
        ///</summary>
        ///<param name="data">
        /// JSON data
        ///</param>
        public static List<ConnectionInfoItem> LoadDataToDatabase(string data)
        {
            List<ConnectionInfoItem> database = new List<ConnectionInfoItem>();
            try
            {
                if (!(JsonConvert.DeserializeObject<List<ConnectionInfoItem>>(data) == null))
                {
                    database = JsonConvert.DeserializeObject<List<ConnectionInfoItem>>(data);
                    Debug.WriteLine("successful import, number of connections = " + database.Count);
                }
            }
            catch (Exception e)
            {
                CallUnsucessfullImportAction("data.json file is corrupted");
            }
            return database;
        }
        ///<summary>
        /// Saves ConnectionInfoItems collection into JSON file
        ///</summary>
        ///<param name="database">
        /// ObservableCollection where are stored ConnectionInfoItems
        ///</param>
        public static void SynchronizeFileWithDatabase(ObservableCollection<ConnectionInfoItem> database, string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            TextWriter tw = new StreamWriter(AppDirectory + filename + ".json");
            serializer.Serialize(tw, database);
            tw.Close();
        }
        ///<summary>
        /// Gets all file names in folder and returns string List with file names without extentension
        ///</summary>
        ///<param name="path">
        /// Path inside application folder
        ///</param>
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
        ///<summary>
        /// Loads JSON data into string dictionary
        ///</summary>
        ///<param name="data">
        /// JSON data
        ///</param>
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
                CallUnsucessfullImportAction("Error when tried to load text from file \n to string Dictionary");
                Debug.WriteLine("Unsuccessful import ");
            }
            return dictionary;
        }
        ///<summary>
        /// Saves string dictionary into JSON file
        ///</summary>
        ///<param name="path">
        /// Path inside application folder
        ///</param>
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