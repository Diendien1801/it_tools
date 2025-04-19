using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public IAuthRepository IAuthRepository
        {
            get => default;
            set
            {
            }
        }

        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Tên đăng nhập và mật khẩu không được để trống");
            }

            if (username.Length < 4)
            {
                return (false, "Tên đăng nhập phải có ít nhất 4 ký tự");
            }

            // Kiểm tra mật khẩu
            if (!IsStrongPassword(password))
            {
                return (false, "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt");
            }

            return await _authRepository.RegisterAsync(username, password);
        }

        private bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }


        public async Task<(bool success, string message, string token)> LoginAsync(string username, string password)
        {
            // Validate trước khi gọi API
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Tên đăng nhập và mật khẩu không được để trống", string.Empty);
            }

            //
            return await _authRepository.LoginAsync(username, password);
        }

        public async Task<bool> LogoutAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false; 
            }

            return await _authRepository.LogoutAsync(token);
        }
    }
}
