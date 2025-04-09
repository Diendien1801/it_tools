using it_tools.DataAccess.Models;
using it_tools.Presentation.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;

namespace it_tools.Presentation.Views
{
    public sealed partial class ManagerPage : Page
    {
        public ManagementViewModel ViewModel { get; set; }
        private bool isDialogOpen_AddTool = false;
        private ContentDialog _activeDialog;
        public ManagerPage()
        {
            this.InitializeComponent();
            var authViewModel = (AuthPage.ViewModel);
            var navigationViewModel = (HomePage.ViewModel);
            ViewModel = new ManagementViewModel(authViewModel, navigationViewModel);
            this.DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            bool requestSuccess = await ViewModel.LoadRequestsAsync();
            bool toolSuccess = await ViewModel.LoadToolsAsync(); 

            if (requestSuccess && toolSuccess)
            {
                Debug.WriteLine("[INFO] LoadDataAsync: Data loaded successfully");
            }
            else
            {
                Debug.WriteLine("[ERROR] LoadDataAsync: Failed to load data");
            }
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idRequest)
            {
                var (success, message) = await ViewModel.AcceptRequestAsync(idRequest);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                if (success)
                {
                    await LoadDataAsync(); // Làm mới lại danh sách
                }
            }
        }

        private async void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idRequest)
            {
                var (success, message) = await ViewModel.RejectRequestAsync(idRequest);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                if (success)
                {
                    await LoadDataAsync(); // Làm mới lại danh sách
                }
            }
        }
        private async void EnableTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                var (success, message) = await ViewModel.EnableToolAsync(idTool);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                
            }
        }

        private async void DisableTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                var (success, message) = await ViewModel.DisableToolAsync(idTool);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

            }
        }



        private async void AccessLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.SelectedValue is string newAccessLevel &&
                comboBox.Tag is string idTool)
            {
                var (success, message) = await ViewModel.UpdateToolAccessLevelAsync(idTool, newAccessLevel);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

              
            }
        }
        private async void DeleteTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                Debug.WriteLine($"[DEBUG] Bắt đầu xóa tool với idTool: {idTool}");

                var (success, message) = await ViewModel.DeleteToolAsync(idTool);

                Debug.WriteLine($"[DEBUG] Kết quả xóa tool: Success = {success}, Message = {message}");

                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");
            }
            else
            {
                Debug.WriteLine("[ERROR] Không thể lấy idTool từ Tag của Button");
            }
        }

        private async void BrowseDll_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".dll");
            picker.SuggestedStartLocation = PickerLocationId.Desktop;

            // Lấy HWND từ cửa sổ chứa trang hiện tại
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainAppWindow); // App.WindowInstance là Window hiện tại
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                DllPathTextBox.Text = file.Path;
            }
        }


        private async void AddTool_Click(object sender, RoutedEventArgs e)
        {
            if (isDialogOpen_AddTool)
            {
                Debug.WriteLine("[WARN] Một ContentDialog đang hiển thị, bỏ qua thao tác thêm tool.");
                return;
            }

            string name = ToolNameTextBox.Text?.Trim();
            string description = ToolDescriptionTextBox.Text?.Trim();
            string iconUrl = IconUrlTextBox.Text?.Trim();
            var selectedCategory = ViewModel.SelectedToolCategory;
            var accessLevelItem = AccessLevelComboBox.SelectedItem as ComboBoxItem;
            string accessLevel = accessLevelItem?.Content?.ToString();
            string originalDllPath = DllPathTextBox.Text;

            Debug.WriteLine($"[DEBUG] AddTool_Click: Bắt đầu xử lý");
            Debug.WriteLine($"[DEBUG] Tên: {name}, Mô tả: {description}, Icon: {iconUrl}, DLL: {originalDllPath}, AccessLevel: {accessLevel}, Loại: {selectedCategory?.name}");

            // ✅ Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) ||
                selectedCategory == null || string.IsNullOrEmpty(accessLevel) ||
                string.IsNullOrEmpty(originalDllPath))
            {
                Debug.WriteLine("[WARN] Thiếu thông tin đầu vào");
                isDialogOpen_AddTool = true;
                await ShowMessageDialog("Vui lòng điền đầy đủ thông tin.", "Thiếu thông tin");
                isDialogOpen_AddTool = false;
                return;
            }

            try
            {
                string pluginsFolder = "C:\\Users\\dient\\source\\repos\\it_tools\\it_tools\\Plugins";
                if (!Directory.Exists(pluginsFolder))
                {
                    Directory.CreateDirectory(pluginsFolder);
                    Debug.WriteLine($"[INFO] Tạo thư mục Plugins tại {pluginsFolder}");
                }

                // ✅ Copy file DLL vào thư mục Plugins
                string fileName = Path.GetFileName(originalDllPath);
                string destinationPath = Path.Combine(pluginsFolder, fileName);

                Debug.WriteLine($"[INFO] Copy DLL từ {originalDllPath} vào {destinationPath}");
                File.Copy(originalDllPath, destinationPath, overwrite: true);

                // ✅ Lấy đường dẫn tương đối
                string parsedDllPath = ParseDllPath(originalDllPath);
                Debug.WriteLine($"[INFO] DLL parsed path: {parsedDllPath}");

                // ✅ Gọi ViewModel để thêm tool
                var (success, message) = await ViewModel.AddToolAsync(
                    name,
                    description,
                    iconUrl,
                    accessLevel,
                    parsedDllPath,
                    selectedCategory.idToolType
                );

                if (success)
                {
                    Debug.WriteLine("[SUCCESS] Thêm công cụ thành công");

                    // Reset các trường sau khi thêm
                    ToolNameTextBox.Text = "";
                    ToolDescriptionTextBox.Text = "";
                    IconUrlTextBox.Text = "";
                    DllPathTextBox.Text = "";
                    ToolTypeComboBox.SelectedItem = null;
                    AccessLevelComboBox.SelectedItem = null;
                }
                else
                {
                    Debug.WriteLine($"[ERROR] Thêm công cụ thất bại: {message}");
                    isDialogOpen_AddTool = true;
                    await ShowMessageDialog($"Không thể thêm công cụ: {message}", "Lỗi");
                    isDialogOpen_AddTool = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EXCEPTION] Lỗi khi thêm tool: {ex}");
                isDialogOpen_AddTool = true;
                await ShowMessageDialog($"Lỗi khi thêm công cụ: {ex.Message}", "Lỗi hệ thống");
                isDialogOpen_AddTool = false;
            }
        }



        public static string ParseDllPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return string.Empty;

            string fileName = System.IO.Path.GetFileName(fullPath); // Lấy 'test.dll'
            return $"Plugins/{fileName}";
        }
        private async Task ShowMessageDialog(string content, string title)
        {
            if (_activeDialog != null)
                return;

            _activeDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await _activeDialog.ShowAsync();
            _activeDialog = null;
        }


    }
}