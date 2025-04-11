using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.BusinessLogic.Services
{
    public interface IManagementService
    {
        Task<(bool success, string message, List<UpgradeRequest> data)> GetAllRequestAsync(string token);
        Task<(bool success, string message)> AcceptUpgradeRequest(string token, string idRequest);
        Task<(bool success, string message)> RejectUpgradeRequest(string token, string idRequest);
        Task<(bool success, string message, List<Tool> data)> GetAllToolAsync(string token);
        Task<(bool success, string message)> DisableTool(string token, string idTool);
        Task<(bool success, string message)> EnableTool(string token, string idTool);
        Task<(bool success, string message)> DeleteToolAsync(string token, string idTool);
        Task<(bool success, string message)> AddToolAsync(
                                        string token,
                                        string name,
                                        string descript,
                                        string iconURL,
                                        string access_level,
                                        string dllPath,
                                        string idToolType
                                                );
        Task<(bool success, string message)> UpdateAccessLevel(string token, string idTool, string accessLevel);
    }
}
