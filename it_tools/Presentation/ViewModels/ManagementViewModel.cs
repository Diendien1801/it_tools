using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class ManagementViewModel
    {
        private readonly ManagementRepository _repository;
        private readonly AuthViewModel _authViewModel;

        public ObservableCollection<UpgradeRequest> Requests { get; set; }
        public ObservableCollection<Tool> Tools { get; set; } // Thêm danh sách tools

        public ManagementViewModel(AuthViewModel authViewModel)
        {
            _repository = new ManagementRepository();
            _authViewModel = authViewModel;
            Requests = new ObservableCollection<UpgradeRequest>();
            Tools = new ObservableCollection<Tool>();
        }

        public async Task<bool> LoadRequestsAsync()
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                Debug.WriteLine("[ERROR] LoadRequestsAsync: No token available");
                return false;
            }

            var (success, message, list) = await _repository.GetAllRequestAsync(_authViewModel.token);
            if (success && list != null)
            {
                Requests.Clear();
                foreach (var item in list)
                {
                    Requests.Add(item);
                }
                Debug.WriteLine("[INFO] LoadRequestsAsync: Requests loaded successfully");
            }
            else
            {
                Debug.WriteLine("[ERROR] LoadRequestsAsync: " + message);
            }
            return success;
        }

        public async Task<bool> LoadToolsAsync()
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                Debug.WriteLine("[ERROR] LoadToolsAsync: No token available");
                return false;
            }

            var (success, message, list) = await _repository.GetAllToolAsync(_authViewModel.token);
            if (success && list != null)
            {
                Tools.Clear();
                foreach (var tool in list)
                {
                    Tools.Add(tool);
                }
                Debug.WriteLine("[INFO] LoadToolsAsync: Tools loaded successfully");
            }
            else
            {
                Debug.WriteLine("[ERROR] LoadToolsAsync: " + message);
            }

            return success;
        }

        // accept
        public async Task<(bool success, string message)> AcceptRequestAsync(string idRequest)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _repository.AcceptUpgradeRequest(_authViewModel.token, idRequest);
            if (success)
            {
                Debug.WriteLine($"[INFO] AcceptRequestAsync: {message}");
                await LoadRequestsAsync(); // Reload danh sách sau khi xử lý
            }
            else
            {
                Debug.WriteLine($"[ERROR] AcceptRequestAsync: {message}");
            }

            return (success, message);
        }

        // reject
        public async Task<(bool success, string message)> RejectRequestAsync(string idRequest)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _repository.RejectUpgradeRequest(_authViewModel.token, idRequest);
            if (success)
            {
                Debug.WriteLine($"[INFO] RejectRequestAsync: {message}");
                await LoadRequestsAsync();
            }
            else
            {
                Debug.WriteLine($"[ERROR] RejectRequestAsync: {message}");
            }

            return (success, message);
        }
        public async Task<(bool success, string message)> DisableToolAsync(string idTool)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _repository.DisableTool(_authViewModel.token, idTool);
            if (success)
            {
                Debug.WriteLine($"[INFO] DisableToolAsync: {message}");
                UpdateToolStatus(idTool, "disable");
                //await LoadToolsAsync(); // Cập nhật lại danh sách
            }
            else
            {
                Debug.WriteLine($"[ERROR] DisableToolAsync: {message}");
            }

            return (success, message);
        }

        public async Task<(bool success, string message)> EnableToolAsync(string idTool)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _repository.EnableTool(_authViewModel.token, idTool);
            if (success)
            {
                Debug.WriteLine($"[INFO] EnableToolAsync: {message}");
                UpdateToolStatus(idTool, "active");
            }
            else
            {
                Debug.WriteLine($"[ERROR] EnableToolAsync: {message}");
            }

            return (success, message);
        }
        public void UpdateToolStatus(string idTool, string newStatus)
        {
            var tool = Tools.FirstOrDefault(t => t.idTool == idTool);
            if (tool != null)
            {
                tool.status = newStatus;
                Debug.WriteLine($"[INFO] Tool {idTool} updated to {newStatus}");
            }
        }
        public async Task<(bool success, string message)> UpdateToolAccessLevelAsync(string idTool, string accessLevel)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _repository.UpdateAccessLevel(_authViewModel.token, idTool, accessLevel);
            if (success)
            {
                Debug.WriteLine($"[INFO] UpdateToolAccessLevelAsync: {message}");
                UpdateToolAccessLevel(idTool, accessLevel);
            }
            else
            {
                Debug.WriteLine($"[ERROR] UpdateToolAccessLevelAsync: {message}");
            }

            return (success, message);
        }
        public void UpdateToolAccessLevel(string idTool, string newAccessLevel)
        {
            var tool = Tools.FirstOrDefault(t => t.idTool == idTool);
            if (tool != null)
            {
                tool.access_level = newAccessLevel;
                Debug.WriteLine($"[INFO] Tool {idTool} accessLevel updated to {newAccessLevel}");
            }
        }



    }
}
