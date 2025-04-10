using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace it_tools.Converter
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;
            string targetStatus = parameter as string;

            if (status == null || targetStatus == null)
                return Visibility.Collapsed;

            // Show the button if the status matches the parameter
            return status.ToLower() == targetStatus.ToLower() ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
