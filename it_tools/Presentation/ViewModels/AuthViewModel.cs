using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using it_tools.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class AuthViewModel
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly NavigationViewModel _navigationViewModel;
        public  string token { get; private set; }
        public AuthViewModel(IAuthService authService, IAccountService accountService, NavigationViewModel navigationViewModel)
        {
            _authService = authService;
            _accountService = accountService;
            token = string.Empty;
            _navigationViewModel = navigationViewModel;
        }



        // Đăng ký tài khoản
        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            var result = await _authService.RegisterAsync(username, password);
            
            return result;
        }

        // Đăng nhập tài khoản
        public async Task<(bool success, string message, string token)> LoginAsync(string username, string password)
        {
            Debug.WriteLine($"[LoginAsync] Đang đăng nhập với Username: {username}");

            var result = await _authService.LoginAsync(username, password);

            Debug.WriteLine($"[LoginAsync] Kết quả trả về: success={result.success}, message={result.message}, token={result.token}");

            if(result.success)
            {
                token = result.token;
                Debug.WriteLine($"[LoginAsync] Lưu token: {token}");
                var (success, message, user) = await _accountService.GetAccountInfoAsync(token);
                if(user.role == "user")
                {
                    _navigationViewModel.IsUser = true;
                }
                else if( user.role == "admin")
                {
                    _navigationViewModel.IsUser = true;
                    _navigationViewModel.IsAdmin = true;
                }
            }
            else
            {
                Debug.WriteLine($"[LoginAsync] Đăng nhập thất bại: {result.message}");
            }

            return result;
        }
    }
}
