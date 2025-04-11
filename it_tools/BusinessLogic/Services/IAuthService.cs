using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message)> RegisterAsync(string username, string password);
        Task<(bool success, string message, string token)> LoginAsync(string username, string password);
        Task<bool> LogoutAsync(string token);
    }
}
