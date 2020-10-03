using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using UlConnect.Logic;
using System.IO;
using Newtonsoft.Json;
namespace UlConnect.Services
{
    public class SettingsDatabase : ReactiveObject
    {
        public SettingsDatabase()
        {
            LoadSettingsFile();
        }  
        private Dictionary<string,string> database;
        
        public Dictionary<string, string> Database {get{return database;} set {this.RaiseAndSetIfChanged(ref database, database = value);}}
        public void LoadSettingsFile()
        {
            if (!File.Exists(FileOperations.AppDirectory + "settings.json"))
            {
               CreateDefaultSettingsFile();
            }
            else
            {
                this.Database = FileOperations.LoadDataToStringDictionary(FileOperations.ReadDataFromFile("settings"));
            }
        }
        public void CreateDefaultSettingsFile()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings["Language"] = "en_EN";
            this.Database = settings;
            FileOperations.SaveStringDictionary(settings, "settings.json");
        }
        public void ChangeParameter(string paramater, string value)
        {
            this.Database[paramater] = value;
            FileOperations.SaveStringDictionary(this.Database, "settings.json");
        }
    }
}