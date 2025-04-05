using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class ToolPageViewModel
    {
        private readonly ToolRepository _toolRepository;
        private readonly AccountRepository _accountRepository;
        private readonly AuthViewModel _authViewModel;
        public ObservableCollection<Tool> Tools { get; private set; }
        private List<Tool> AllTools { get; set; } = new List<Tool>(); // Lưu trữ danh sách đầy đủ
        private List<Tool> FavouriteTools { get; set; } = new List<Tool>();

        public ToolPageViewModel(AuthViewModel authViewModel)
        {
            _toolRepository = new ToolRepository();
            _accountRepository = new AccountRepository();
            _authViewModel = authViewModel;
            Tools = new ObservableCollection<Tool>();
        }

        public async Task UpdateFavoriteStatus(Tool tool)
        {
            if (_authViewModel.token == null)
            {
                await ShowMessageDialog("Bạn cần đăng nhập để sử dụng tính năng này.", "Thông báo");
                return;
            }

            bool isFavorite = IsToolFavorite(tool.idTool);

            if (isFavorite)
            {
                var result = await _accountRepository.RemoveFavoriteToolAsync(_authViewModel.token, tool.idTool);
                if (result.success)
                {
                    FavouriteTools.RemoveAll(t => t.idTool == tool.idTool);
                    tool.isFavourite = false; // Cập nhật trực tiếp
                }
            }
            else
            {
                var result = await _accountRepository.AddFavoriteToolAsync(_authViewModel.token, tool.idTool);
                if (result.success)
                {
                    FavouriteTools.Add(tool);
                    tool.isFavourite = true; // Cập nhật trực tiếp
                }
            }
        }

        private async Task ShowMessageDialog(string content, string title)
        {
            var dialog = new Microsoft.UI.Xaml.Controls.ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = Microsoft.UI.Xaml.Window.Current.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        public bool IsToolFavorite(string toolId)
        {
            return FavouriteTools.Any(fav => fav.idTool == toolId);
        }

        public async Task LoadFavouriteTools()
        {
            if (_authViewModel.token == null)
            {
                return; // Không có user hoặc token, không load danh sách yêu thích
            }

            var (success, message, favoriteTools) = await _accountRepository.GetFavoriteToolsAsync(_authViewModel.token);

            if (success)
            {
                FavouriteTools = favoriteTools;
            }
        }

        // Load danh sách tool theo category
        public async Task LoadTools(string idToolType)
        {
            //  Load danh sách tool từ repository
            var tools = await _toolRepository.GetToolsByCategoryAsync(idToolType);

            //  Cập nhật danh sách gốc ban đầu
            AllTools = tools.ToList();

            //  Load danh sách yêu thích từ API
            await LoadFavouriteTools();

            //  Gắn cờ isFavourite cho từng tool
            foreach (var tool in AllTools)
            {
                tool.isFavourite = FavouriteTools.Any(fav => fav.idTool == tool.idTool);
            }

            //  Cập nhật UI
            Tools.Clear();
            foreach (var tool in AllTools)
            {
                Tools.Add(tool);
            }
        }

        // Hàm lọc tool theo keyword
        public void FilterTools(string keyword)
        {
            var filteredTools = string.IsNullOrWhiteSpace(keyword)
                ? AllTools
                : AllTools.Where(t => t.name.ToLower().Contains(keyword.ToLower())).ToList();

            Tools.Clear();
            foreach (var tool in filteredTools)
            {
                Tools.Add(tool);
            }
        }
    }
}