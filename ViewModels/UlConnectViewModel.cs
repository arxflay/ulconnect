using System.Text.RegularExpressions;
using System.Threading;
using UlConnect.Logic;
using System;
using System.Text;
using System.Linq;
using System.IO;
using Avalonia.Media;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebSocketSharp;
using UlConnect.Models;
using ReactiveUI;
using System.Reactive;
using UlConnect.Services;
namespace UlConnect.ViewModels
{
    public class UlConnectViewModel : ViewModelBase
    {
        private object key = new object();
        private Regex rg;
        private int selectedTabIndex;
        
        private string validWebsocketAddress;
        public int SelectedIndex {get {return selectedTabIndex;} set{this.RaiseAndSetIfChanged(ref selectedTabIndex, value);} }
        private LanguageDatabase languageDatabase;
        public ConnectionInfoDatabase ConnectionInfoDatabase {get; set;}
       
        private SortedList<int, WebsocketWithTimer> webSockets;
        public UlConnectViewModel(LanguageDatabase languageDatabase)
        {
            this.languageDatabase = languageDatabase;
            ConnectionInfoDatabase = new ConnectionInfoDatabase();
            //Websocket variables
            validWebsocketAddress = String.Format(@"ws://{0}.{0}.{0}.{0}:8266", @"(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])"); //regex for valid IP
            rg = new Regex(validWebsocketAddress);
            webSockets = new SortedList<int, WebsocketWithTimer>();
            //Database configuration
            SelectedIndex = 0;
            ConnectionInfoDatabase.ImportDatabaseFromFile("data");
            if (ConnectionInfoDatabase.Database.Count != 0)
            {
                ConnectionInfoDatabase.IsDatabaseEmpty = false;
                languageDatabase.SetLanguageForAllElementsInDatabase(ConnectionInfoDatabase.Database); 
            }
            //Intialize ReactiveCommands
            DisconnectButtonCommand = ReactiveCommand.Create(() => {ThreadPool.QueueUserWorkItem(Disconnect);});
            ConnectButtonCommand = ReactiveCommand.Create(() => {ThreadPool.QueueUserWorkItem(Connect);});
            AddItemButtonCommand = ReactiveCommand.Create(() => 
            {
                var connItem = new ConnectionInfoItem();
                connItem.PageVariables.SetLanguage(languageDatabase.Database);
                ConnectionInfoDatabase.AddItem(connItem, "data");
            });
            RemoveItemButtonCommand = ReactiveCommand.Create(() => { 
                if (ConnectionInfoDatabase.Database.Count != 0) 
                {
                    ConnectionInfoDatabase.RemoveItem(SelectedIndex, "data");
                    if (webSockets.Keys.Contains(SelectedIndex)) //Checking if tab has runned WebSocket
                    {
                       CloseWebSocket(SelectedIndex,true);
                    }              
                }});
            SaveInfoButtonCommand = ReactiveCommand.Create(() => { ConnectionInfoDatabase.SaveInfo(SelectedIndex, "data");});
            SendSignalButtonCommand = ReactiveCommand.Create(() => SendSignal());
        }
        ///<summary>
        ///Connects to board
        ///</summary>
        ///<param name="data">
        ///Data isn't used, don't enter something here
        ///</param>
        public void Connect(object data = null)
        {
            var index = SelectedIndex; //used to store index of current tab
            if (rg.Match(ConnectionInfoDatabase.Database[index].Address).Value == "") //Check if address is valid
            {
                ConnectionInfoDatabase.Database[index].Address = "ws://192.168.0.0:8266";
                ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = "Connection: invalid ip address";
                return;
            }
           
            ChangeConnPageVisual(index, ConnectionState.Connecting);
            lock(key)
            {
                webSockets.Add(index, new WebsocketWithTimer(ConnectionInfoDatabase.Database[index].Address));
            }
            webSockets[index].Websocket.WaitTime = new TimeSpan(0,0,5);
            webSockets[index].Websocket.Connect(); //Tries to connect
            webSockets[index].Websocket.Send(ConnectionInfoDatabase.Database[index].Password + "\r"); //Sends password which is shoud be set on esp32
                if (webSockets[index].Websocket.ReadyState == WebSocketState.Open)
                {              
                    ChangeConnPageVisual(index, ConnectionState.Connected);
                    webSockets[index].Websocket.Send("import ulcommands\r");
                    webSockets[index].Websocket.OnMessage += (sender, e) => OnWebsocketMessage(e,index);
                    webSockets[index].TimeoutCallback = () => WebsocketSendTimeout(index);
                    webSockets[index].CreateTimer(PowerLedCheck, index);                  
                }
                else
                {       
                      ChangeConnPageVisual(index, ConnectionState.Error);
                      CloseWebSocket(index);
                }           
        }
        ///<summary>
        ///Gets message from socket and checks if it's 0 or 1
        ///Method shoud be add to OnMessage event in websocket
        ///</summary>
        ///<param name="messageArgs">
        ///Used for getting messages from websocket.
        ///</param>
        ///<param name="index">
        ///Index of item in websocket List
        ///</param>
        public void OnWebsocketMessage(MessageEventArgs messageArgs, int index)
        {
            webSockets[index].ResetElapsedTime();
            if (messageArgs.Data == "0")
            {
                Debug.WriteLine("PC with " + index + " is off");
                if (ConnectionInfoDatabase.Database[index].PageVariables.PCStatusColor != Brushes.Red)
                {
                    ChangeConnPageVisual(index, ConnectionState.PCoff);
                }
            }
            else if (messageArgs.Data == "1")
            {
                Debug.WriteLine("PC with " + index + " is on");
                if (ConnectionInfoDatabase.Database[index].PageVariables.PCStatusColor != Brushes.Green)
                {
                    ChangeConnPageVisual(index, ConnectionState.PCon);
                }
            }
        }
        ///<summary>
        ///Method which is used for timeoutCallback for WebsocketWithTimer
        ///</summary>
        public void WebsocketSendTimeout(int index)
        {
            ChangeConnPageVisual(index, ConnectionState.SendTimeout);
            ChangeConnPageVisual(index, ConnectionState.PCoff);
            webSockets[index].Close();
            CloseWebSocket(index);
            ChangeConnPageVisual(index, ConnectionState.Disconnected);
        }
        ///<summary>
        ///Updates connection page visual depends on state 
        ///</summary>
        ///<param name="index">
        /// Index of item in websocket List
        ///</param>
        public void ChangeConnPageVisual(int index, ConnectionState state)
        {
            switch (state)
            {            
                case ConnectionState.Connecting:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["Connecting"];
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Orange;
                    ConnectionInfoDatabase.Database[index].PageVariables.IsConnected = true;
                    break;
                case ConnectionState.Connected:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["Connected"];
                    ConnectionInfoDatabase.Database[index].PageVariables.EnableDisconnectButton = true;
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Green;
                    break;
                case ConnectionState.Error:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["Error"];
                    ConnectionInfoDatabase.Database[index].PageVariables.IsConnected = false;
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Red;
                    break;
                case ConnectionState.Disconnecting:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["Disconnecting"];
                    ConnectionInfoDatabase.Database[index].PageVariables.EnableDisconnectButton = false; 
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Orange; 
                    break;
                case ConnectionState.SendTimeout:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["SendTimeout"];
                    ConnectionInfoDatabase.Database[index].PageVariables.EnableDisconnectButton = false; 
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Orange; 
                    break;
                case ConnectionState.Disconnected:
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusColor = Brushes.Red;
                    ConnectionInfoDatabase.Database[index].PageVariables.ConnectionStatusText = languageDatabase.Database["Disconnected"];  
                    ConnectionInfoDatabase.Database[index].PageVariables.IsConnected = false; 
                    break;
                case ConnectionState.PCoff:
                    ConnectionInfoDatabase.Database[index].PageVariables.PCStatusColor = Brushes.Red;
                    ConnectionInfoDatabase.Database[index].PageVariables.PCStatusText = languageDatabase.Database["PCOff"];
                    break;
                case ConnectionState.PCon:
                    ConnectionInfoDatabase.Database[index].PageVariables.PCStatusColor = Brushes.Green;
                    ConnectionInfoDatabase.Database[index].PageVariables.PCStatusText = languageDatabase.Database["PCOn"];
                    break;
                    
            }
        }
        ///<summary>
        ///Enables/disables PC
        ///</summary>
        public void SendSignal()
        {
            webSockets[SelectedIndex].Websocket.Send("ulcommands.run()\r\n");
        }
        ///<summary>
        ///Gets value from powerled pin
        ///</summary>
        ///<param name="indexObject">
        ///Index of item in websocket List
        ///</param>
        public void PowerLedCheck(object indexObject)
        {
            var index = Convert.ToInt32(indexObject);
            webSockets[index].Websocket.Send("ulcommands.checkpowerled()\r");
        }
        public void Disconnect(object data)
        {
            CloseWebSocket(SelectedIndex); 
        }
        ///<summary>
        ///Closes websocket and removes websocket from list
        ///</summary>
        ///<param name="indexInList">
        ///Index of item in websocket List
        ///</param>
        public void CloseWebSocket(int indexInList, bool checkWebsocketState = false)
        {
            int index = indexInList; //used to store index of current tab
            ChangeConnPageVisual(index, ConnectionState.PCoff);
            ChangeConnPageVisual(index, ConnectionState.Disconnecting);
            if (checkWebsocketState == true && webSockets[index].Websocket.ReadyState == WebSocketState.Open || checkWebsocketState == false) 
            {
                lock(key)
                {
                    webSockets[index].Close();
                    webSockets.Remove(index);
                }
            }
            ChangeConnPageVisual(index, ConnectionState.Disconnected);
        }
        //Commands binded to buttons
        public ReactiveCommand<Unit, Unit> ConnectButtonCommand {get;}
        public ReactiveCommand<Unit, Unit> DisconnectButtonCommand {get;}
        public ReactiveCommand<Unit, Unit> AddItemButtonCommand {get;}
        public ReactiveCommand<Unit, Unit> RemoveItemButtonCommand {get;}
        public ReactiveCommand<Unit, Unit> SaveInfoButtonCommand {get;}
        public ReactiveCommand<Unit, Unit> SendSignalButtonCommand {get;}
    }
}