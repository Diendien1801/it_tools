using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class ManagementViewModel : INotifyPropertyChanged
    {
        private readonly IManagementService _managementService;
        private readonly AuthViewModel _authViewModel;
        private readonly NavigationViewModel _navigationViewModel;

        public ObservableCollection<UpgradeRequest> Requests { get; set; }
        public ObservableCollection<Tool> Tools { get; set; } // Thêm danh sách tools

        public ObservableCollection<ToolCategory> ToolCategories { get; set; }
        private ToolCategory _selectedToolCategory;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public ToolCategory SelectedToolCategory
        {
            get => _selectedToolCategory;
            set
            {
                if (_selectedToolCategory != value)
                {
                    _selectedToolCategory = value;
                    OnPropertyChanged(nameof(SelectedToolCategory));
                }
            }
        }

        private ObservableCollection<Tool> _filteredTools;
        public ObservableCollection<Tool> FilteredTools
        {
            get => _filteredTools;
            set
            {
                if (_filteredTools != value)
                {
                    _filteredTools = value;
                    OnPropertyChanged(nameof(FilteredTools));
                }
            }
        }

        private string _searchText = "";
        private string _statusFilter = "all";

        public ManagementViewModel(AuthViewModel authViewModel, NavigationViewModel navigationViewModel, IManagementService managementService)
        {
            _managementService = managementService;
            _authViewModel = authViewModel;
            _navigationViewModel = navigationViewModel;
            Requests = new ObservableCollection<UpgradeRequest>();
            Tools = new ObservableCollection<Tool>();
            FilteredTools = new ObservableCollection<Tool>();
            ToolCategories = new ObservableCollection<ToolCategory>();
            foreach (var category in navigationViewModel.ToolCategories)
            {
                if (category.idToolType != "0" && category.idToolType != "-1")
                {
                    ToolCategories.Add(category);
                }
            }

        }
        public async Task<(bool success, string message)> ReCoverToolAsync(string idTool)
        {
            try
            {
                string token = _authViewModel.token;
                // Call your backend method to restore the tool
                var result = await _managementService.ReCoverToolAsync(token, idTool);

                if (result.success)
                {
                    // Update tool's isDelete status in the list
                    var tool = Tools.FirstOrDefault(t => t.idTool == idTool);
                    if (tool != null)
                    {
                        tool.isDelete = false;
                        // Refresh filtered tools
                        FilterTools(_searchText, _statusFilter);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi khôi phục tool: {ex.Message}");
            }
        }
        // Add this method to handle filtering
        public void FilterTools(string searchText, string statusFilter)
        {
            _searchText = searchText?.ToLower() ?? "";
            _statusFilter = statusFilter?.ToLower() ?? "all";

            var filteredList = Tools.Where(tool =>
                (string.IsNullOrEmpty(_searchText) ||
                 tool.name.ToLower().Contains(_searchText) ||
                 tool.descript.ToLower().Contains(_searchText)) &&
                (_statusFilter == "all" ||
                 (_statusFilter == "deleted" ? tool.isDelete :
                  !tool.isDelete && tool.status.ToLower() == _statusFilter))
            );

            FilteredTools.Clear();
            foreach (var tool in filteredList)
            {
                FilteredTools.Add(tool);
            }

            Debug.WriteLine($"[INFO] FilterTools: Filtered {FilteredTools.Count} tools from {Tools.Count} total tools");
        }
        // Modify LoadToolsAsync to update FilteredTools


        public async Task<(bool success, string message)> DeleteToolAsync(string idTool)
        {
            try
            {
                string token = _authViewModel.token;
                var result = await _managementService.DeleteToolAsync(token, idTool);

                if (result.success)
                {
                    // Update the tool's delete status instead of removing it from the list
                    var tool = Tools.FirstOrDefault(t => t.idTool == idTool);
                    if (tool != null)
                    {
                        tool.isDelete = true;

                        // Refresh filtered tools to reflect the change immediately
                        FilterTools(_searchText, _statusFilter);

                        // Notify UI of property change
                        OnPropertyChanged(nameof(FilteredTools));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa tool: {ex.Message}");
            }
        }

        public async Task<bool> LoadRequestsAsync()
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                Debug.WriteLine("[ERROR] LoadRequestsAsync: No token available");
                return false;
            }

            var (success, message, list) = await _managementService.GetAllRequestAsync(_authViewModel.token);
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

            var (success, message, list) = await _managementService.GetAllToolAsync(_authViewModel.token);
            if (success && list != null)
            {
                Tools.Clear();
                foreach (var tool in list)
                {
                    tool.OriginalAccessLevel = tool.access_level;
                    Tools.Add(tool);
                }

                // Apply current filters to update FilteredTools
                FilterTools(_searchText, _statusFilter);

                Debug.WriteLine("[INFO] LoadToolsAsync: Tools loaded successfully");
            }
            else
            {
                Debug.WriteLine("[ERROR] LoadToolsAsync: " + message);
            }

            return success;
        }
        //public async Task<bool> LoadToolsAsync()
        //{
        //    if (string.IsNullOrEmpty(_authViewModel.token))
        //    {
        //        Debug.WriteLine("[ERROR] LoadToolsAsync: No token available");
        //        return false;
        //    }

        //    var (success, message, list) = await _repository.GetAllToolAsync(_authViewModel.token);
        //    if (success && list != null)
        //    {
        //        Tools.Clear();
        //        foreach (var tool in list)
        //        {
        //            Tools.Add(tool);
        //        }
        //        Debug.WriteLine("[INFO] LoadToolsAsync: Tools loaded successfully");
        //    }
        //    else
        //    {
        //        Debug.WriteLine("[ERROR] LoadToolsAsync: " + message);
        //    }

        //    return success;
        //}

        // accept
        public async Task<(bool success, string message)> AcceptRequestAsync(string idRequest)
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            var (success, message) = await _managementService.AcceptUpgradeRequest(_authViewModel.token, idRequest);
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

            var (success, message) = await _managementService.RejectUpgradeRequest(_authViewModel.token, idRequest);
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

            var (success, message) = await _managementService.DisableTool(_authViewModel.token, idTool);
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

            var (success, message) = await _managementService.EnableTool(_authViewModel.token, idTool);
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

            var (success, message) = await _managementService.UpdateAccessLevel(_authViewModel.token, idTool, accessLevel);
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
                tool.OriginalAccessLevel = newAccessLevel; // Update the original value too
                Debug.WriteLine($"[INFO] Tool {idTool} accessLevel updated to {newAccessLevel}");
            }
        }

        public async Task<(bool success, string message)> AddToolAsync(
                                                                    string name,
                                                                    string descript,
                                                                    string iconURL,
                                                                    string access_level,
                                                                    string dllPath,
                                                                    string idToolType
                                                                       )
        {
            if (string.IsNullOrEmpty(_authViewModel.token))
            {
                return (false, "Bạn cần đăng nhập để thực hiện chức năng này");
            }

            try
            {
                var (success, message) = await _managementService.AddToolAsync(
                    _authViewModel.token,
                    name,
                    descript,
                    iconURL,
                    access_level,
                    dllPath,
                    idToolType
                );

                if (success)
                {
                    // load lại tool
                    await LoadToolsAsync();
                    Debug.WriteLine($"[INFO] AddToolAsync: Tool added thành công");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] AddToolAsync: {message}");
                }

                return (success, message);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi thêm tool: {ex.Message}");
            }
        }
        



    }
}
