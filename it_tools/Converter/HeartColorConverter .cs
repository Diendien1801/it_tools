using it_tools.Presentation.ViewModels;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace it_tools.Converter
{
    public class HeartColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                // Gradient cho trái tim yêu thích (khi value = true)
                LinearGradientBrush gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Windows.Foundation.Point(0, 0),
                    EndPoint = new Windows.Foundation.Point(1, 1)
                };

                // Màu gradient từ hồng đậm sang đỏ
                gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.DeepPink, Offset = 0.0 });
                gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 1.0 });

                return gradientBrush;
            }
            else
            {
                // Gradient cho trái tim không yêu thích (khi value = false)
                LinearGradientBrush gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Windows.Foundation.Point(0, 0),
                    EndPoint = new Windows.Foundation.Point(1, 1)
                };

                // Màu gradient từ xám nhạt sang xám đậm - phù hợp với nền xanh
                gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightGray, Offset = 0.0 });
                gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.DarkGray, Offset = 1.0 });

                return gradientBrush;
            }
        }
        
        

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
