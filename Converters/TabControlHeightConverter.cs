using Avalonia.Data.Converters;
using System;
namespace UlConnect.Converters
{
    public class TabControlHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, System.Globalization.CultureInfo culture)
        {
            return (double)value - 50; //50 = heigth of NavButtonsBar Canvas in UlConnectView.xaml
        }
        public object ConvertBack(object value, Type targetType, object paramater, System.Globalization.CultureInfo culture)
        {
            return (double)value;
        }
    }
}