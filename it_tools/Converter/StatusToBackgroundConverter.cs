using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using it_tools.DataAccess.Models;

namespace it_tools.Converter
{
    public class StatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Get the data context to check for isDelete
            if (parameter is Tool tool && tool.isDelete)
            {
                // Return a red background for deleted tools
                return new SolidColorBrush(ColorHelper.FromArgb(90, 255, 150, 150)); // Light red color
            }

            string status = value as string;
            if (status == null)
                return new SolidColorBrush(Colors.LightGray); // Default for null status

            return status.ToLower() switch
            {
                "active" => new SolidColorBrush(ColorHelper.FromArgb(102, 208, 232, 255)), // #66D0E8FF
                "disable" => new SolidColorBrush(Colors.DarkGray),
                _ => new SolidColorBrush(Colors.LightGray)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}