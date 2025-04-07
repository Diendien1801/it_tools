using it_tools.DataAccess.Models;
using it_tools.Presentation.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace it_tools.Presentation.Views
{
    public sealed partial class ManagerPage : Page
    {
        public ManagementViewModel ViewModel { get; set; }

        public ManagerPage()
        {
            this.InitializeComponent();
            var authViewModel = (AuthPage.ViewModel);
            ViewModel = new ManagementViewModel(authViewModel);
            this.DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            bool requestSuccess = await ViewModel.LoadRequestsAsync();
            bool toolSuccess = await ViewModel.LoadToolsAsync(); 

            if (requestSuccess && toolSuccess)
            {
                Debug.WriteLine("[INFO] LoadDataAsync: Data loaded successfully");
            }
            else
            {
                Debug.WriteLine("[ERROR] LoadDataAsync: Failed to load data");
            }
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idRequest)
            {
                var (success, message) = await ViewModel.AcceptRequestAsync(idRequest);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                if (success)
                {
                    await LoadDataAsync(); // Làm mới lại danh sách
                }
            }
        }

        private async void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idRequest)
            {
                var (success, message) = await ViewModel.RejectRequestAsync(idRequest);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                if (success)
                {
                    await LoadDataAsync(); // Làm mới lại danh sách
                }
            }
        }
        private async void EnableTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                var (success, message) = await ViewModel.EnableToolAsync(idTool);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                
            }
        }

        private async void DisableTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                var (success, message) = await ViewModel.DisableToolAsync(idTool);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

            }
        }



        private async void AccessLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.SelectedValue is string newAccessLevel &&
                comboBox.Tag is string idTool)
            {
                var (success, message) = await ViewModel.UpdateToolAccessLevelAsync(idTool, newAccessLevel);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

              
            }
        }

        private async Task ShowMessageDialog(string content, string title)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot 
            };

            await dialog.ShowAsync();
        }


    }
}