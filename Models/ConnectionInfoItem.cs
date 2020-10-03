
using Newtonsoft.Json;
using ReactiveUI;
using Avalonia.ReactiveUI;
using System.Collections.Generic;
namespace UlConnect.Models
{
    [JsonObject]
    public class ConnectionInfoItem : ReactiveObject
    {
        public ConnectionInfoItem()
        {
            PageVariables = new ConnectionPageVariables();
            Name = "New connection";
            Address = "ws://192.168.0.0:8266";
            Password = "";
        }
        private string address;
        [JsonIgnore]
        public ConnectionPageVariables PageVariables {get; set;}  //Used for storing variables which can be binded to elements in ConnectionPage

        public string Name {get; set ;}
        public string Address {
            get 
            {
                return address;
            } 
            set
            {              
                this.RaiseAndSetIfChanged(ref address, value);
            }
        }
        public string Password {get; set;}
    }
}