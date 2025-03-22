using it_tools.Presentation.ViewModels;
using it_tools.Presentation.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using it_tools.DataAccess.Models;
using System.Diagnostics;


namespace it_tools
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(AuthPage)); // Khởi động vào AuthPage
        }

        public void NavigateToHome()
        {
            ContentFrame.Navigate(typeof(HomePage));
        }
    }
}

