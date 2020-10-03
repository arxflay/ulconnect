using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UlConnect.Views
{
    public class UlConnectView : UserControl
    {
        public UlConnectView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}