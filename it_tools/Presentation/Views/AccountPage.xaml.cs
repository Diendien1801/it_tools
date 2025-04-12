using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Repositories;
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
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace it_tools.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountPage : Page
    {
        private AccountViewModel _viewModel;
        private AuthViewModel _authViewModel;
        public AccountPage()
        {
            _viewModel = AppServices.Services.GetService<AccountViewModel>();
            _authViewModel = AppServices.Services.GetService<AuthViewModel>();
            this.InitializeComponent();
            DataContext = _viewModel;
            // load data
            _ = LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            await _viewModel.LoadAccountDataAsync();
        }
        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedTool == null)
            {
                await ShowMessageDialog("Vui lòng chọn một công cụ để xóa.", "Lỗi");
                return;
            }

            // Tạo ContentDialog mới để tránh lỗi XamlRoot
            var deleteDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = "Bạn có chắc chắn muốn xóa tool này khỏi danh sách yêu thích không?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot // Gán XamlRoot của trang hiện tại
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                
                string message = await _viewModel.RemoveFavoriteToolAsynce(_viewModel.SelectedTool.idTool);

                await ShowMessageDialog(message, "Thông báo");

                // Cập nhật danh sách tool sau khi xóa
                _viewModel.FavoriteTools.Remove(_viewModel.SelectedTool);
            }
        }




        private async Task ShowMessageDialog(string content, string title)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot 
            };

            await dialog.ShowAsync();
        }

        private async void SendRequestUpgrade_Click(object sender, RoutedEventArgs e)
        {
            var (success, message) = await _viewModel.SendUpgradeRequestAsync();

            string title = success ? "Thành công" : "Thông báo";
            await ShowMessageDialog(message, title);
        }


        // logout
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Debug token trước logout
            Debug.WriteLine($"[LogoutButton_Click] Token: {_authViewModel.token}");

            // Xóa token
            _authViewModel.Logout();

            // Điều hướng về AuthPage
            if (App.MainAppWindow.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(AuthPage));
            }
            else
            {
                // Nếu Frame chưa tồn tại, khởi tạo Frame mới
                var frame = new Frame();
                frame.Navigate(typeof(AuthPage));
                App.MainAppWindow.Content = frame;
            }

            // Kích hoạt cửa sổ để đảm bảo hiển thị
            App.MainAppWindow.Activate();

            // Debug token sau khi logout
            Debug.WriteLine($"[LogoutButton_Click] Token sau khi logout: {_authViewModel.token}");
        }


    }
}

