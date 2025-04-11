using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Repositories;
using it_tools.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace it_tools.Presentation.Views
{
    public sealed partial class AuthPage : Page
    {

        public static AuthViewModel ViewModel;
        public AuthPage()
        {
            ViewModel = AppServices.Services.GetService<AuthViewModel>();
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

            var result = await ViewModel.RegisterAsync(email, password);
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
            SuccessTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thành công
        }

        private void ShowSuccess(string message)
        {
            SuccessTextBlock.Text = message;
            SuccessTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thất bại
        }
        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            // 🔹 Chuyển từ Đăng ký -> Đăng nhập
            RegisterForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
            ErrorTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thất bại
            SuccessTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thành công
        }
        private void SignUpLink_Click(object sender, RoutedEventArgs e)
        {
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
            ErrorTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thất bại
            SuccessTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo thành công
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị ProgressRing và ẩn văn bản
            LoginButtonText.Visibility = Visibility.Collapsed;
            LoginProgressRing.Visibility = Visibility.Visible;
            LoginProgressRing.IsActive = true;

            string username = EmailLoginBox.Text;
            string password = PasswordLoginBox.Password;

            var (success, message, token) = await ViewModel.LoginAsync(username, password);

            // Khôi phục trạng thái ban đầu
            LoginProgressRing.IsActive = false;
            LoginProgressRing.Visibility = Visibility.Collapsed;
            LoginButtonText.Visibility = Visibility.Visible;

            if (success)
            {
                // Lưu token và chuyển sang trang HomePage
                SettingsService.SaveToken(token);
                ShowSuccess(message);
                App.MainAppWindow.NavigateToHome();
            }
            else
            {
                ShowError(message);
            }
        }

        private async void GuessButton_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị ProgressRing và ẩn văn bản
            GuessButtonText.Visibility = Visibility.Collapsed;
            GuessProgressRing.Visibility = Visibility.Visible;
            GuessProgressRing.IsActive = true;

            // Giả lập xử lý (ví dụ: tải dữ liệu hoặc điều hướng)
            await Task.Delay(2000); // Thay bằng logic thực tế của bạn

            // Khôi phục trạng thái ban đầu
            GuessProgressRing.IsActive = false;
            GuessProgressRing.Visibility = Visibility.Collapsed;
            GuessButtonText.Visibility = Visibility.Visible;

            // Điều hướng đến HomePage
            App.MainAppWindow.NavigateToHome();
        }
        private bool IsValidLogin()
        {
            return true; // Kiểm tra thông tin đăng nhập (giả định)
        }
    }
}
