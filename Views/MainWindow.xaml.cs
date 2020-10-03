using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using UlConnect.Models;
using System.Windows.Input;
namespace UlConnect.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        
        }
    }

}