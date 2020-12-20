using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UlConnect.ViewModels;
using ReactiveUI;
using System;
namespace UlConnect
{
    public class MessageBoxWindow : Window
    {
        public bool CloseApp {get; set;}
        ///<summary>
        ///Creates MessageBox
        ///</summary>
        ///<param name="title">
        ///Title displayed on top of the window
        ///</param>
        ///<param name="message">
        ///Message displayed in window
        ///</param>
        ///<param name="closeApp">
        ///If true, app will closed if you press on cross button
        ///</param>
        public static MessageBoxWindow CreateMessageBox(string title, string message, bool closeApp = false)
        {
            var msgbox = new MessageBoxWindow(closeApp);
            var msgboxView = new MessageBoxWindowViewModel(title, message);
            msgboxView.CloseButtonCommand = ReactiveCommand.Create(() => {
                if (msgbox.CloseApp) msgbox.CloseApp = false; 
                msgbox.Close();});
            msgbox.DataContext = msgboxView;
            return msgbox;
        }
        public MessageBoxWindow()
        {
            
        }
        protected MessageBoxWindow(bool closeApp = false)
        {
            InitializeComponent();
            CloseApp = closeApp;
            if (CloseApp) 
            {
                this.Closed += (e,sender) => {if (CloseApp) Environment.Exit(0);}; //if window is closed, closes app
            }          
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}