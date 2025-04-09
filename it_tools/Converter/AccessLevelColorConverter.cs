using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace it_tools.Converter
{
    public class AccessLevelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string accessLevel = value as string;
            return accessLevel switch
            {
                "premium" => new SolidColorBrush(Colors.Gold),
                "membership" => new SolidColorBrush(Colors.CornflowerBlue),
                "anonymous" => new SolidColorBrush(Colors.Gray),
                _ => new SolidColorBrush(Colors.Black)
            };
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;

        
    }
}
