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
        public void SetLanguageForConnDatabaseElements(ObservableCollection<ConnectionInfoItem> connDatabase)
        {
            foreach (var connection in connDatabase)
            {
                SetLanguageForConnection(connection.PageVariables);
            }
        }
        public void SetLanguageForConnection(ConnectionPageVariables variables)
        {
            variables.ConnectionName = Database["ConnectionName"];
            variables.BoardAddress = Database["BoardAddress"];
            variables.Password = Database["Password"];
            variables.SaveInfoBtn = Database["SaveInfoBtn"];
            variables.ConnectBtn = Database["ConnectBtn"];
            variables.DisconnectBtn = Database["DisconnectBtn"];
            variables.TurnPConoffBtn = Database["TurnPConoffBtn"];
            if (variables.IsConnected == false)
            {
                variables.ConnectionStatusText = Database["ConnectionDefault"];
            }
            else
            {
                variables.ConnectionStatusText = Database["Connected"];
            }
            if (variables.PCStatusColor == Avalonia.Media.Brushes.Green)
            {
                 variables.PCStatusText = Database["PCOn"];
            }
            else
            {
               variables.PCStatusText = Database["PCOff"];
            }
        }
        public void CreateDefaultLanguageFile (string path)
        {
            Dictionary<string, string> langDictionary = new Dictionary<string, string>();
            if (!Directory.Exists(FileOperations.AppDirectory  + path))
            {
                Directory.CreateDirectory(FileOperations.AppDirectory + path);              
            }
            //For ConnectionPage
            langDictionary[LanguageDatabase.lang_keys[0]] =  "Connection name";
            langDictionary[LanguageDatabase.lang_keys[1]] = "Board address";
            langDictionary[LanguageDatabase.lang_keys[2]] = "Password";
            langDictionary[LanguageDatabase.lang_keys[3]] = "Save info";
            langDictionary[LanguageDatabase.lang_keys[4]] = "Connect";
            langDictionary[LanguageDatabase.lang_keys[5]] = "Disconnect";
            langDictionary[LanguageDatabase.lang_keys[6]] = "Turn PC on/off";
            langDictionary[LanguageDatabase.lang_keys[7]] = "Connection state: not connected";
            langDictionary[LanguageDatabase.lang_keys[8]] = "Connection state: trying to connect";
            langDictionary[LanguageDatabase.lang_keys[9]] = "Connection state: connected";
            langDictionary[LanguageDatabase.lang_keys[10]] = "Connection state: invalid ip address";
            langDictionary[LanguageDatabase.lang_keys[11]] = "Connection state: disconnecting...";
            langDictionary[LanguageDatabase.lang_keys[12]] = "Connection state: disconnected";
            langDictionary[LanguageDatabase.lang_keys[13]] = "Connection state: Send timeout, disconnecting...";
            langDictionary[LanguageDatabase.lang_keys[14]] = "PC state: off";
            langDictionary[LanguageDatabase.lang_keys[15]] = "PC state: on";
            //For UlConnect
            langDictionary[LanguageDatabase.lang_keys[16]] = "Click on + to add new connection";
            //For Settings
            langDictionary[LanguageDatabase.lang_keys[17]] = "Language";
            langDictionary[LanguageDatabase.lang_keys[18]] = "Return to menu"; 
            langDictionary[LanguageDatabase.lang_keys[19]] = "Save"; 
            this.Database = langDictionary;
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            TextWriter tw = new StreamWriter(String.Format("{0}{1}/en.json", FileOperations.AppDirectory, path));
            serializer.Serialize(tw, langDictionary);
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