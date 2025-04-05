using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
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
        private readonly AuthRepository _authRepository;
        public  string token { get; private set; }
        public AuthViewModel()
        {
            _authRepository = new AuthRepository();
            token = string.Empty;
        }

        

        // Đăng ký tài khoản
        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            var result = await _authRepository.RegisterAsync(username, password);
            return result;
        }

        // Đăng nhập tài khoản
        public async Task<(bool success, string message, string token)> LoginAsync(string username, string password)
        {
            Debug.WriteLine($"[LoginAsync] Đang đăng nhập với Username: {username}");

            var result = await _authRepository.LoginAsync(username, password);

            Debug.WriteLine($"[LoginAsync] Kết quả trả về: success={result.success}, message={result.message}, token={result.token}");

            if(result.success)
            {
                token = result.token;
                Debug.WriteLine($"[LoginAsync] Lưu token: {token}");
            }
            else
            {
                Debug.WriteLine($"[LoginAsync] Đăng nhập thất bại: {result.message}");
            }

            return result;
        }
    }
}
