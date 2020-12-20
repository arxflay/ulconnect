using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using UlConnect.Models;
using System.Windows.Input;
using UlConnect.Logic;
using System;

using UlConnect.ViewModels;
namespace UlConnect.Views
{
    public class MainWindow : Window
    {
        public static bool IsWindowLoaded {get; set;}
        public MainWindow()
        {
            FileOperations.UnsucessImportAction = this.UnsuccessImportAction;
            this.Initialized += (sender, e) => {
                for (int i = 0; i < FileOperations.UnsuccessImportTaskStack.Count; i++)
                {
                    var task = FileOperations.UnsuccessImportTaskStack.Pop();
                    task.Start();               
                }            
            };
            this.Closed += (e,sender) => {Environment.Exit(0);};
            InitializeComponent(); 
        }
      
        private void UnsuccessImportAction(string errorMessage)
        {
            //Shows MessageBoxWindow in UIThread (used for calling methods from threads)
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                MessageBoxWindow msgbox = MessageBoxWindow.CreateMessageBox("Error", errorMessage);
                msgbox.ShowDialog(this); //ShowDialog - used for blocking gui
            });
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        
        }
    }

}