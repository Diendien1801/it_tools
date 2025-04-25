using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using it_tools.BusinessLogic.Services;

namespace it_tools.Presentation.ViewModels
{
    internal class AccountViewModel : INotifyPropertyChanged
    {
        private readonly IAccountService _accountService;
        private readonly AuthViewModel _authViewModel;


        private User? _userInfo;

        private Tool _selectedTool;
        public Tool SelectedTool
        {
            get => _selectedTool;
            set
            {
                _selectedTool = value;
                OnPropertyChanged(nameof(SelectedTool));
            }
        }
        public User? UserInfo
        {
            get => _userInfo;
            private set
            {
                _userInfo = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<UpgradeRequest> _historyRequest = new();
        public ObservableCollection<UpgradeRequest> HistoryRequest
        {
            get => _historyRequest;
            set
            {
                _historyRequest = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Tool> _favoriteTools = new();
        public ObservableCollection<Tool> FavoriteTools
        {
            get => _favoriteTools;
            private set
            {
                _favoriteTools = value;
                OnPropertyChanged();
            }
        }

        public AccountViewModel(AuthViewModel authViewModel,IAccountService accountService )
        {
            _accountService = accountService;
            _authViewModel = authViewModel;
           
        }
        public async Task<(bool success, string message)> SendUpgradeRequestAsync()
        {
            if (_authViewModel.token == null)
            {
                return (false, "Bạn cần đăng nhập để sử dụng tính năng này.");
            }

            var (success, message) = await _accountService.SendUpgradeRequestAsync(_authViewModel.token);
            return (success, message);
        }

        public async Task<bool> LoadAccountDataAsync()
        {
            if (_authViewModel.token == null)
            {
                return false;
            }

            var accountTask = _accountService.GetAccountInfoAsync(_authViewModel.token);
            var favoriteTask = _accountService.GetFavoriteToolsAsync(_authViewModel.token);
            var historyRequestTask = _accountService.GetHistoryRequest(_authViewModel.token);

            await Task.WhenAll(accountTask, favoriteTask, historyRequestTask); // 🔹 Chạy song song hai API

            var accountResult = await accountTask;
            var favoriteResult = await favoriteTask;

            if (accountResult.success)
            {
                UserInfo = accountResult.data;
            }

            if (favoriteResult.success)
            {
                FavoriteTools = new ObservableCollection<Tool>(favoriteResult.tools);
            }

            if (historyRequestTask.Result.success)
            {
               HistoryRequest = new ObservableCollection<UpgradeRequest>(historyRequestTask.Result.Item3);
            }
            return accountResult.success && favoriteResult.success;
        }


        public async Task<string> AddFavoriteToolAsynce(string idTool)
        {
            if (_authViewModel.token == null)
            {
                return "Bạn cần đăng nhập để thêm tool vào danh sách yêu thích";
            }
            var (success, message) = await _accountService.AddFavoriteToolAsync(_authViewModel.token, idTool);
            return success ? $"✅ {message}" : $"❌ {message}";
        }

        public async Task<string> RemoveFavoriteToolAsynce(string idTool)
        {
            if (_authViewModel.token == null)
            {
                return "Bạn cần đăng nhập để xóa tool khỏi danh sách yêu thích";
            }
            var (success, message) = await _accountService.RemoveFavoriteToolAsync(_authViewModel.token, idTool);
            return success ? $"✅ {message}" : $"❌ {message}";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IAccountService đá
        {
            get => default;
            set
            {
            }
        }
    }
}
