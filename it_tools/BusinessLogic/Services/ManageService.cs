using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.BusinessLogic.Services
{
    public class ManageService : IManagementService
    {
        private readonly IManagementRepository managementRepository;

        public ManageService(IManagementRepository managementRepository)
        {
            this.managementRepository = managementRepository;
        }

        public IManagementRepository IManagementRepository
        {
            get => default;
            set
            {
            }
        }

        public async Task<(bool success, string message, List<UpgradeRequest> data)> GetAllRequestAsync(string token)
        {
            return await managementRepository.GetAllRequestAsync(token);
        }
        public async Task<(bool success, string message)> AcceptUpgradeRequest(string token, string idRequest)
        {
            return await managementRepository.AcceptUpgradeRequest(token, idRequest);
        }
        public async Task<(bool success, string message)> RejectUpgradeRequest(string token, string idRequest)
        {
            return await managementRepository.RejectUpgradeRequest(token, idRequest);
        }
        public async Task<(bool success, string message, List<Tool> data)> GetAllToolAsync(string token)
        {
            return await managementRepository.GetAllToolAsync(token);
        }
        public async Task<(bool success, string message)> DisableTool(string token, string idTool)
        {
            return await managementRepository.DisableTool(token, idTool);
        }
        public async Task<(bool success, string message)> EnableTool(string token, string idTool)
        {
            return await managementRepository.EnableTool(token, idTool);
        }
        public async Task<(bool success, string message)> DeleteToolAsync(string token, string idTool)
        {
            return await managementRepository.DeleteToolAsync(token, idTool);
        }
        public async Task<(bool success, string message)> AddToolAsync(
            string token,
            string name,
            string descript,
            string iconURL,
            string access_level,
            string dllPath,
            string idToolType
        )
        {
            return await managementRepository.AddToolAsync(token, name, descript, iconURL, access_level, dllPath, idToolType);
        }
        public async Task<(bool success, string message)> UpdateAccessLevel(string token, string idTool, string accessLevel)
        {
            return await managementRepository.UpdateAccessLevel(token, idTool, accessLevel);
        }
        

        public async Task<(bool success, string message)> ReCoverToolAsync(string token, string idTool)
        {
            return await managementRepository.ReCoverToolAsync(token, idTool);
        }
      

        public async Task<(bool success, string message)> AddNewToolType(string token, ToolCategory toolType)
        {
            return await managementRepository.AddNewToolType(token, toolType);
        }
    }
}
