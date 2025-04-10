using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace it_tools.Converter
{
    public class StatusToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;
            if (status == null)
                return new SolidColorBrush(Colors.Gray); // Default for null status

            return status.ToLower() switch
            {
                "active" => new SolidColorBrush(Colors.CornflowerBlue),
                "disable" => new SolidColorBrush(Colors.Gray),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
