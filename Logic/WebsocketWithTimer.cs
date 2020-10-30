using System.Threading;
using WebSocketSharp;
using System;
namespace UlConnect.Logic
{
    class WebsocketWithTimer
    {
        public Action TimeoutCallback {get; set;}
        private TimeSpan elapsedTime;
        public TimeSpan WaitTime {get; set;}
        public Timer Timer {get; set;}
        public WebSocket Websocket {get; set;}
        public WebsocketWithTimer(string address)
        {
            elapsedTime = new TimeSpan();
            TimeoutCallback = null;
            Timer = null;
            WaitTime = new TimeSpan(0,0,5); //default WaitTime is 5 seconds
            Websocket = new WebSocket(address);
        }
        ///<summary>
        /// Creates timer
        ///</summary>
        ///<param name="timeoutCallback">
        /// function which will be called if time is out
        ///</param>
        ///<param name="index">
        /// Index of WebsocketWithTimer
        ///</param>
        ///<param name="seconds">
        /// Defines how fast timer will be (for example 1000 - 1 call per second)
        ///</param>
        public void CreateTimer(TimerCallback timeoutCallback, int index, int seconds = 1000)
        {
            if (Timer != null) Timer.Dispose();
            TimerCallback modifiedTImerFunc = o => { timeoutCallback(index);
                if (TimeoutCallback != null)
                {
                    elapsedTime += TimeSpan.FromMilliseconds(seconds);
                    if (elapsedTime >= WaitTime) TimeoutCallback();
                }
            };
            Timer = new Timer(modifiedTImerFunc, index as object, 0, seconds);
        }
             
        public void ResetElapsedTime()
        {
            elapsedTime = new TimeSpan();
        }
         ///<summary>
        /// Clears timer and closes websocket
        ///</summary>
        public void Close()
        {
            if (Timer != null) Timer.Dispose();
            Websocket.Close();
        }
    }
}