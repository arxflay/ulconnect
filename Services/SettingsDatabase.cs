using System.Collections.Generic;
using ReactiveUI;
using UlConnect.Logic;
using System.IO;
using System.Globalization;
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
        ///<summary>
        ///Loads deserialized data from settings.json in string dictionary Database
        ///</summary>
        public void LoadSettingsFile()
        {
            if (!File.Exists(FileOperations.AppDirectory + "settings.json"))
            {
               List<string> filenames = FileOperations.GetFileNames("lang", FileOperations.IsJsonChecker);
               CultureInfo ci = CultureInfo.CurrentCulture;
               if (filenames.Contains(ci.TwoLetterISOLanguageName))
               {
                   CreateDefaultSettingsFile(ci.TwoLetterISOLanguageName);
               }
               else
               {
                   CreateDefaultSettingsFile("en");
               }
            }
            else
            {
                this.Database = FileOperations.LoadDataToStringDictionary(FileOperations.ReadDataFromFile("settings"));
            }
        }
        ///<summary>
        ///Creates settigns file with default values and keys
        ///</summary>
        public void CreateDefaultSettingsFile(string language)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings["Language"] = language;
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