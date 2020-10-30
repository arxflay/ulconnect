using ReactiveUI;
using Avalonia.Media;
using System.Collections.Generic;
namespace UlConnect.Models
{
    public static class LedColor
    {
        public static Color Connected {get {return Colors.Green;}}
        public static Color NotConnected {get {return Colors.Red;}}
        public static Color Connecting {get {return Colors.Orange;}}
    }
    public enum ConnectionState
    {
        Connecting, Connected, Disconnecting, Disconnected, Error, SendTimeout, PCon, PCoff, Default
    }
    public class ConnectionPageVariables : ReactiveObject
    {
        public ConnectionPageVariables()
        {
            IsConnected = false;
            ConnState = ConnectionState.Default;
            EnableDisconnectButton = false;
            connectionStatusColor = Brushes.Red;
            pcStatusColor = Brushes.Red;
        }

        private bool enableDisconnectButton;
        private IBrush connectionStatusColor;
        private IBrush pcStatusColor;
        private bool isConnected;
        private string connectionStatusText, pcStatusText, connectionName, boardAddress, password, saveInfoBtn, connectionBtn, disconnectBtn, turnPConoffBtn;
        public ConnectionState ConnState {get; set;}
        public string ConnectionName {get {return connectionName;} set {this.RaiseAndSetIfChanged(ref connectionName, value);}}
        public string BoardAddress{get {return boardAddress;} set {this.RaiseAndSetIfChanged(ref boardAddress, value);}}
        public string Password{get {return password;} set {this.RaiseAndSetIfChanged(ref password, value);}}
        public string SaveInfoBtn{get {return saveInfoBtn;} set {this.RaiseAndSetIfChanged(ref saveInfoBtn, value);}}
        public string ConnectBtn{get {return connectionBtn;} set {this.RaiseAndSetIfChanged(ref connectionBtn, value);}}
        public string DisconnectBtn{get {return disconnectBtn;} set {this.RaiseAndSetIfChanged(ref disconnectBtn, value);}}
        public string TurnPConoffBtn{get {return turnPConoffBtn;} set {this.RaiseAndSetIfChanged(ref turnPConoffBtn, value);}}
        public bool IsConnected {get {return isConnected;} set {this.RaiseAndSetIfChanged(ref isConnected, value);}}
        public bool EnableDisconnectButton {get {return enableDisconnectButton;} set {this.RaiseAndSetIfChanged(ref enableDisconnectButton, value);}}
        public string ConnectionStatusText {get {return connectionStatusText;} set{this.RaiseAndSetIfChanged(ref connectionStatusText, value);} }
        public IBrush ConnectionStatusColor {get {return connectionStatusColor;} set{this.RaiseAndSetIfChanged(ref connectionStatusColor, value);} }
        public IBrush PCStatusColor {get {return pcStatusColor;} set{this.RaiseAndSetIfChanged(ref pcStatusColor, value);} }
        public string PCStatusText {get {return pcStatusText;} set{this.RaiseAndSetIfChanged(ref pcStatusText, value);} }
        ///<summary>
        ///Sets language for string page variables
        ///</summary>
        ///<param ="langLines">
        ///Dictionary which contains language lines (sentences, words)
        ///</param>
        public void SetLanguage(Dictionary<string,string> langLines)
        {
            ConnectionName = langLines["ConnectionName"];
            BoardAddress = langLines["BoardAddress"];
            Password = langLines["Password"];
            SaveInfoBtn = langLines["SaveInfoBtn"];
            ConnectBtn = langLines["ConnectBtn"];
            DisconnectBtn = langLines["DisconnectBtn"];
            TurnPConoffBtn = langLines["TurnPConoffBtn"];
            if (IsConnected == false)
            {
                ConnectionStatusText = langLines["ConnectionDefault"];
            }
            else
            {
                ConnectionStatusText = langLines["Connected"];
            }
            if (PCStatusColor == Brushes.Green)
            {
                 PCStatusText = langLines["PCOn"];
            }
            else
            {
                PCStatusText = langLines["PCOff"];
            }
        }

    }
}