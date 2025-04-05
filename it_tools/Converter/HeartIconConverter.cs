using it_tools.Presentation.ViewModels;
using Microsoft.UI.Xaml.Data;
using System;

namespace it_tools.Converter
{
    public class HeartIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? "\uEB52" : "\uEB51"; // Icon Unicode: ♥ (Filled) / ♡ (Empty)
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
