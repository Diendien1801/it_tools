using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using it_tools.DataAccess.Models;

namespace it_tools.Converter
{
    public class DeletedStatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Check if the value is a Tool object
            if (value is Tool tool)
            {
                // Check if the tool is deleted
                if (tool.isDelete)
                {
                    // Red background for deleted tools
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 255, 200, 200)); // Light red
                }
                else
                {
                    // Use the existing status for non-deleted tools
                    string status = tool.status?.ToLower();
                    return status switch
                    {
                        "active" => new SolidColorBrush(ColorHelper.FromArgb(102, 208, 232, 255)), // #66D0E8FF
                        "disable" => new SolidColorBrush(Colors.LightGray),
                        _ => new SolidColorBrush(Colors.White)
                    };
                }
            }

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}