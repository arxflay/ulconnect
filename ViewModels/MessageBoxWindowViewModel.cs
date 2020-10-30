using ReactiveUI;
using System.Reactive;
using System;
using Avalonia;
namespace UlConnect.ViewModels
{
    public class MessageBoxWindowViewModel : ViewModelBase
    {
        private string title;
        private string message;
        public MessageBoxWindowViewModel(string title, string message)
        {
            this.ExitButtonCommand = ReactiveCommand.Create(() => Environment.Exit(0));
            Title = title;
            Message = message;
        }
        public ReactiveCommand<Unit, Unit> ExitButtonCommand {get; set;}
        public ReactiveCommand<Unit, Unit> CloseButtonCommand {get; set;}
        public string Title {get {return title;} set {title = value;}}
        public string Message {get {return message;} set {message = value;}}
    }
}