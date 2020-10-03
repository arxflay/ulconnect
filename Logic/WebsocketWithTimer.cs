using System.Threading;
using WebSocketSharp;
using System;
namespace UlConnect.Logic
{
    class WebsocketWithTimer
    {
        public WebsocketWithTimer(string address)
        {
            elapsedTime = new TimeSpan();
            TimeoutCallback = null;
            Timer = null;
            WaitTime = new TimeSpan(0,0,5);
            Websocket = new WebSocket(address);
        }
        public void CreateTimer(TimerCallback timerFunc, int index, int seconds = 1000)
        {
            if (Timer != null) Timer.Dispose();
            TimerCallback modifiedTImerFunc = o => { timerFunc(index);
                if (TimeoutCallback != null)
                {
                    elapsedTime += TimeSpan.FromMilliseconds(seconds);
                    if (elapsedTime >= WaitTime) TimeoutCallback();
                }
            };
            Timer = new Timer(modifiedTImerFunc, this.index as object, 0, seconds);
        }
        
        public Action TimeoutCallback {get; set;}
        private TimeSpan elapsedTime;
        public TimeSpan WaitTime {get; set;}
        public Timer Timer {get; set;}
        private int index;
        public WebSocket Websocket {get; set;}
      
        public void ResetElapsedTime()
        {
            elapsedTime = new TimeSpan();
        }
        public void Close()
        {
            if (Timer != null) Timer.Dispose();
            Websocket.Close();
        }
    }
}