using System;
using Microsoft.UI.Xaml.Data;

namespace it_tools.Converter
{
    public class DateTimeConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                // Format the DateTime as "dd/MM/yyyy HH:mm:ss"
                return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
            }

            return string.Empty; // Return an empty string if the value is not a DateTime
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // ConvertBack is not implemented as it's not needed in this case
            throw new NotImplementedException();
        }
    }
}
