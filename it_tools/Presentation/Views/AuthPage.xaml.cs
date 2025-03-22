using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Repositories;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace it_tools.Presentation.Views
{
    public sealed partial class AuthPage : Page
    {
        private readonly AuthRepository _authService = new AuthRepository();
        public AuthPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailRegisterBox.Text;
            string password = PasswordRegisterBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            if (password != confirmPassword)
            {
                ShowError("Passwords do not match!");
                return;
            }

            var result = await _authService.RegisterAsync(email, password);
            if (result.success)
            {
                ShowSuccess("Account created! Please login.");
                RegisterForm.Visibility = Visibility.Collapsed;
                LoginForm.Visibility = Visibility.Visible;
            }
            else
            {
                ShowError(result.message);
            }
        }
        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void ShowSuccess(string message)
        {
            SuccessTextBlock.Text = message;
            SuccessTextBlock.Visibility = Visibility.Visible;
        }
        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            // 🔹 Chuyển từ Đăng ký -> Đăng nhập
            RegisterForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = EmailLoginBox.Text;
            string password = PasswordLoginBox.Password;

            var (success, message, token) = await _authService.LoginAsync(username, password);

            if (success)
            {
                //  Lưu token vào local storage để sử dụng sau này
                SettingsService.SaveToken(token);
                ShowSuccess(message);

                //  Chuyển sang trang HomePage
                App.MainAppWindow.NavigateToHome();
            }
            else
            {
                ShowError(message);
            }
        }


        private bool IsValidLogin()
        {
            return true; // Kiểm tra thông tin đăng nhập (giả định)
        }
    }
}
