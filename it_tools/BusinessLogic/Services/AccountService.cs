using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.BusinessLogic.Services
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<(bool success, string message, User data)> GetAccountInfoAsync(string token)
        {
            return await _accountRepository.GetAccountInfoAsync(token);
        }
        public async Task<(bool success, string message, List<Tool> tools)> GetFavoriteToolsAsync(string token)
        {
            return await _accountRepository.GetFavoriteToolsAsync(token);
        }
        public async Task<(bool success, string message)> AddFavoriteToolAsync(string token, string idTool)
        {
            return await _accountRepository.AddFavoriteToolAsync(token, idTool);
        }
        public async Task<(bool success, string message)> RemoveFavoriteToolAsync(string token, string idTool)
        {
            return await _accountRepository.RemoveFavoriteToolAsync(token, idTool);
        }
        public async Task<(bool success, string message)> SendUpgradeRequestAsync(string token)
        {
            return await _accountRepository.SendUpgradeRequestAsync(token);
        }
    }
}
