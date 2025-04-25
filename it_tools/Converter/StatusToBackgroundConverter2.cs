using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace it_tools.Converter
{
    public class StatusToBackgroundConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;

            switch (status?.ToLower())
            {
                case "approved":
                case "accepted":
                    return new SolidColorBrush(Color.FromArgb(255, 40, 167, 69)); // Green

                case "pending":
                case "processing":
                    return new SolidColorBrush(Color.FromArgb(255, 255, 193, 7)); // Amber

                case "rejected":
                case "denied":
                    return new SolidColorBrush(Color.FromArgb(255, 220, 53, 69)); // Red

                default:
                    return new SolidColorBrush(Color.FromArgb(255, 108, 117, 125)); // Gray
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
