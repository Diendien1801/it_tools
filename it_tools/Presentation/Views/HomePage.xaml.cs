using it_tools.DataAccess.Models;
using it_tools.Presentation.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace it_tools.Presentation.Views
{
    
    public sealed partial class HomePage : Page
    {
        public NavigationViewModel ViewModel { get; } = new NavigationViewModel();
        public HomePage()
        {
            this.InitializeComponent();
            if (this.Content is FrameworkElement rootElement)
            {
                rootElement.DataContext = ViewModel;
            }
            ContentFrame.Navigate(typeof(ToolPage), ("0", "All"));
        }
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is ToolCategory category) // Ép ki?u v? ToolCategory
            {
                Debug.WriteLine($"Selected category: {category.name}");
                Debug.WriteLine($"Navigating to ToolPage with id: {category.idToolType}");

                ContentFrame.Navigate(typeof(ToolPage), (category.idToolType, category.name));
            }
        }
    }
}
