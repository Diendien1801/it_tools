using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Models;
using it_tools.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
        public NavigationViewModel ViewModel { get; set; }
        



        public HomePage()
        { 
            this.InitializeComponent();
            ViewModel = AppServices.Services.GetService<NavigationViewModel>();

            this.DataContext = ViewModel;
            
            ContentFrame.Navigate(typeof(ToolPage), ("0", "All"));
        }
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is ToolCategory category) 
            {
                Debug.WriteLine($"Selected category: {category.name}");
                Debug.WriteLine($"Navigating to ToolPage with id: {category.idToolType}");
                

                ContentFrame.Navigate(typeof(ToolPage), (category.idToolType, category.name));
            }
        }
        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (ContentFrame.Content is ToolPage toolPage && toolPage.DataContext is ToolPageViewModel viewModel)
            {
                viewModel.FilterTools(sender.Text);
            }
        }

        private void SearchBox_Nav(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("SearchBox clicked! Navigating to All...");
            ContentFrame.Navigate(typeof(ToolPage), ("0", "All"));
        }
        private void TaskbarAccount_Tapped(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AccountPage));
        }
        private void TaskbarTaskManagement_Tapped(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ManagerPage));
        }







    }
}
