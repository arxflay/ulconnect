using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UlConnect.Logic;
using UlConnect.Models;
using System.IO;
using Newtonsoft.Json;
using System;
namespace UlConnect.Services
{
    public class LanguageDatabase : ReactiveObject
    {
        public Dictionary<string,string> Database;
        private static readonly string[] lang_keys = new string[]{ //for checking if lang file contains all lines
            //For ConnectionPage
            "ConnectionName",
            "BoardAddress",
            "Password",
            "SaveInfoBtn",
            "ConnectBtn",
            "DisconnectBtn",
            "TurnPConoffBtn",
            "ConnectionDefault",
            "Connecting",
            "Connected",
            "Error",
            "Disconnecting",
            "Disconnected",
            "ConnectionTimeout",
            "PCOff",
            "PCOn",
            //For UlConnect
            "CreateConnectionText",
            //For Settings
            "Language", 
            "ReturnToMenu",
            "Save"
        };
        public void ImportLanguage(string filename)
        {
            this.Database = new Dictionary<string, string>(FileOperations.LoadDataToStringDictionary(FileOperations.ReadDataFromFile("/lang/" + filename)));
            if (CompareKeysWithLangDictionary(Database) == false)
            {          
                CreateDefaultLanguageFile("lang"); //lang - folder name
            }
        }
        public void SetLanguageForAllElementsInDatabase(ObservableCollection<ConnectionInfoItem> database)
        {
            foreach (var el in database)
            {
                el.PageVariables.SetLanguage(this.Database);
            }
        }
        public void CreateDefaultLanguageFile (string path)
        {
            Dictionary<string, string> lang = new Dictionary<string, string>();
            if (!Directory.Exists(FileOperations.AppDirectory  + path))
            {
                Directory.CreateDirectory(FileOperations.AppDirectory + path);              
            }
            //For ConnectionPage
            lang["ConnectionName"] =  "Connection name";
            lang["BoardAddress"] = "Board address";
            lang["Password"] = "Password";
            lang["SaveInfoBtn"] = "Save info";
            lang["ConnectBtn"] = "Connect";
            lang["DisconnectBtn"] = "Disconnect";
            lang["TurnPConoffBtn"] = "Turn PC on/off";
            lang["ConnectionDefault"] = "Connection: not connected";
            lang["Connecting"] = "Connection: trying to connect";
            lang["Connected"] = "Connection: connected";
            lang["Error"] = "Connection: invalid ip address";
            lang["Disconnecting"] = "Connection: disconnecting...";
            lang["Disconnected"] = "Connection: disconnected";
            lang["ConnectionTimeout"] = "Connection: Send timeout, disconnecting...";
            lang["PCOff"] = "PC: off";
            lang["PCOn"] = "PC: on";
            //For UlConnect
            lang["CreateConnectionText"] = "Click on + to add new connection";
            //For Settings
            lang["Language"] = "Language";
            lang["ReturnToMenu"] = "Return to menu"; 
            lang["Save"] = "Save"; 
            this.Database = lang;
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            TextWriter tw = new StreamWriter(String.Format("{0}{1}/en_EN.json", FileOperations.AppDirectory, path));
            serializer.Serialize(tw, lang);
            tw.Close();
        }
        public bool CompareKeysWithLangDictionary(Dictionary<string, string> dictionary)
        {
            foreach (string key in LanguageDatabase.lang_keys)
            {
                if (!dictionary.ContainsKey(key)) return false;
            }
            return true;
        }
    }
}