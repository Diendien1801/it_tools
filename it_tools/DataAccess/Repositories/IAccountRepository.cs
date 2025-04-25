using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    public interface IAccountRepository
    {
        Task<(bool success, string message, User data)> GetAccountInfoAsync(string token);

        Task<(bool success, string message, List<Tool> tools)> GetFavoriteToolsAsync(string token);
        Task<(bool success, string message)> AddFavoriteToolAsync(string token, string idTool);
        Task<(bool success, string message)> RemoveFavoriteToolAsync(string token, string idTool);
        Task<(bool success, string message)> SendUpgradeRequestAsync(string token);

        Task<(bool success, string message, List<UpgradeRequest> upgradeRequests)> GetHistoryRequest(string token);

    }
}
