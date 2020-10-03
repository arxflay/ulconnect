using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UlConnect.Pages
{
    public class ConnectionPage : UserControl
    {
        public ConnectionPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        }
    }
}