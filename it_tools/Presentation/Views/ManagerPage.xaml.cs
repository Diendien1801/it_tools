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
using it_tools.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace it_tools.Presentation.Views
{
    public sealed partial class ManagerPage : Page
    {
        public ManagementViewModel ViewModel { get; set; }
        private bool isDialogOpen_AddTool = false;
        private ContentDialog _activeDialog;
        private bool isDialogOpen_AddToolType = false;
        public ManagerPage()
        {
            this.InitializeComponent();
            
            ViewModel = AppServices.Services.GetService<ManagementViewModel>();
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

            if (requestSuccess 
                &&
                toolSuccess
                )
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

                // Cập nhật UI sau khi thay đổi status
                if (success)
                {
                    // Làm mới FilteredTools với trạng thái hiện tại
                    string statusFilter = ((ComboBoxItem)StatusFilterComboBox.SelectedItem)?.Tag?.ToString() ?? "all";
                    string searchText = ToolSearchBox?.Text ?? string.Empty;
                    ViewModel.FilterTools(searchText, statusFilter);
                }
            }
        }

        private async void DisableTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                var (success, message) = await ViewModel.DisableToolAsync(idTool);
                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                // Cập nhật UI sau khi thay đổi status
                if (success)
                {
                    // Làm mới FilteredTools với trạng thái hiện tại
                    string statusFilter = ((ComboBoxItem)StatusFilterComboBox.SelectedItem)?.Tag?.ToString() ?? "all";
                    string searchText = ToolSearchBox?.Text ?? string.Empty;
                    ViewModel.FilterTools(searchText, statusFilter);
                }
            }
        }



        private async void AccessLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.SelectedValue is string newAccessLevel &&
                comboBox.Tag is string idTool)
            {
                // Find the tool in the ViewModel
                var tool = ViewModel.Tools.FirstOrDefault(t => t.idTool == idTool);

                // Only proceed if we found the tool and the access level actually changed
                if (tool != null && tool.OriginalAccessLevel != newAccessLevel)
                {
                    // First update the OriginalAccessLevel to prevent repeat notifications
                    tool.OriginalAccessLevel = newAccessLevel;

                    // Then call the API
                    var (success, message) = await ViewModel.UpdateToolAccessLevelAsync(idTool, newAccessLevel);
                    await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");
                }
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

                // Check if file already exists and is in use
                if (File.Exists(destinationPath))
                {
                    try
                    {
                        // Try to open the file to see if it's locked
                        using (FileStream fs = File.Open(destinationPath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            // File is not locked, we can close and continue with copy
                        }
                    }
                    catch (IOException)
                    {
                        // File is locked, just use the existing file
                        Debug.WriteLine($"[INFO] File {fileName} is already in use, skipping copy");
                        // Continue with the existing file
                        var (addToolSuccess, addToolMessage) = await ViewModel.AddToolAsync(
     name,
     description,
     iconUrl,
     accessLevel,
     ParseDllPath(destinationPath),
     selectedCategory.idToolType
 );

                        // Show results and return
                        isDialogOpen_AddTool = true;
                        await ShowMessageDialog(addToolSuccess ? "Đã thêm công cụ thành công!" : $"Không thể thêm công cụ: {addToolMessage}",
                                                addToolSuccess ? "Thành công" : "Lỗi");
                        isDialogOpen_AddTool = false;

                        if (addToolSuccess)
                        {
                            // Reset các trường sau khi thêm
                            ResetToolInputFields();
                        }

                        return;
                    }
                }

                // If we get here, we can safely copy the file
                try
                {
                    File.Copy(originalDllPath, destinationPath, overwrite: true);
                }
                catch (IOException ex)
                {
                    Debug.WriteLine($"[ERROR] Cannot copy DLL: {ex.Message}");
                    isDialogOpen_AddTool = true;
                    await ShowMessageDialog($"Không thể sao chép file DLL: {ex.Message}\nVui lòng đảm bảo file không đang được sử dụng.", "Lỗi");
                    isDialogOpen_AddTool = false;
                    return;
                }

                // ✅ Lấy đường dẫn tương đối
                string parsedDllPath = ParseDllPath(destinationPath);
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

                    // Show success message
                    isDialogOpen_AddTool = true;
                    await ShowMessageDialog("Đã thêm công cụ thành công!", "Thành công");
                    isDialogOpen_AddTool = false;

                    // Reset các trường sau khi thêm
                    ResetToolInputFields();
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

        private void ResetToolInputFields()
        {
            ToolNameTextBox.Text = "";
            ToolDescriptionTextBox.Text = "";
            IconUrlTextBox.Text = "";
            DllPathTextBox.Text = "";
            ToolTypeComboBox.SelectedItem = null;
            AccessLevelComboBox.SelectedItem = null;
        }

        public static string ParseDllPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return string.Empty;

            string fileName = System.IO.Path.GetFileName(fullPath); // Lấy 'test.dll'
            return $"Plugins/{fileName}";
        }






        // SEARCH
        private void ToolSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string searchText = sender.Text;
                string statusFilter = ((ComboBoxItem)StatusFilterComboBox.SelectedItem)?.Tag?.ToString() ?? "all";

                ViewModel.FilterTools(searchText, statusFilter);
            }
        }
        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null)
            {
                Debug.WriteLine("[ERROR] ViewModel is null in StatusFilterComboBox_SelectionChanged.");
                return;
            }

            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selectedItem)
            {
                string statusFilter = selectedItem.Tag?.ToString() ?? "all";
                string searchText = ToolSearchBox?.Text ?? string.Empty;

                ViewModel.FilterTools(searchText, statusFilter);
            }
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
        private async void RestoreTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string idTool)
            {
                Debug.WriteLine($"[DEBUG] Bắt đầu khôi phục tool với idTool: {idTool}");

                var (success, message) = await ViewModel.ReCoverToolAsync(idTool);

                Debug.WriteLine($"[DEBUG] Kết quả khôi phục tool: Success = {success}, Message = {message}");

                await ShowMessageDialog(message, success ? "Thành công" : "Thất bại");

                if (success)
                {
                    // Refresh the list after successful restoration
                    await ViewModel.LoadToolsAsync();
                }
            }
            else
            {
                Debug.WriteLine("[ERROR] Không thể lấy idTool từ Tag của Button");
            }
        }


        private async void AddToolType_Click(object sender, RoutedEventArgs e)
        {
            if (isDialogOpen_AddToolType || _activeDialog != null)
            {
                Debug.WriteLine("[WARN] Một Dialog đang hiển thị, bỏ qua thao tác thêm loại công cụ.");
                return;
            }

            try
            {
                isDialogOpen_AddToolType = true;

                // Create dialog content
                Grid dialogContent = new Grid();
                dialogContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                dialogContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                dialogContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                dialogContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                dialogContent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                dialogContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Name field
                TextBlock nameLabel = new TextBlock { Text = "Tool Type Name:", Margin = new Thickness(0, 0, 10, 10), VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(nameLabel, 0);
                Grid.SetColumn(nameLabel, 0);
                dialogContent.Children.Add(nameLabel);

                TextBox nameTextBox = new TextBox { PlaceholderText = "Enter New ToolType's Name", Margin = new Thickness(0, 0, 0, 10), Width = 300 };
                Grid.SetRow(nameTextBox, 0);
                Grid.SetColumn(nameTextBox, 1);
                dialogContent.Children.Add(nameTextBox);

                // Icon URL field
                TextBlock iconUrlLabel = new TextBlock { Text = "Icon URL:", Margin = new Thickness(0, 0, 10, 10), VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(iconUrlLabel, 1);
                Grid.SetColumn(iconUrlLabel, 0);
                dialogContent.Children.Add(iconUrlLabel);

                TextBox iconUrlTextBox = new TextBox { PlaceholderText = "https://example.com/icon.png", Margin = new Thickness(0, 0, 0, 10), Width = 300 };
                Grid.SetRow(iconUrlTextBox, 1);
                Grid.SetColumn(iconUrlTextBox, 1);
                dialogContent.Children.Add(iconUrlTextBox);

                // Create and configure the dialog
                ContentDialog addDialog = new ContentDialog
                {
                    Title = "Add New ToolType",
                    Content = dialogContent,
                    PrimaryButtonText = "Add",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                // Show the dialog and wait for user input
                var result = await addDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string name = nameTextBox.Text?.Trim();
                    string iconUrl = iconUrlTextBox.Text?.Trim();

                    if (string.IsNullOrEmpty(name))
                    {
                        // Reset the flag before showing the error dialog
                        isDialogOpen_AddToolType = false;

                        // Show error message using a new dialog
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "Empty Tool Type name",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                        return;
                    }

                    // Create a new ToolCategory object
                    ToolCategory newToolType = new ToolCategory
                    {
                        name = name,
                        iconURL = iconUrl
                    };

                    // Call API to add the new tool type
                    Debug.WriteLine($"[DEBUG] Thêm loại công cụ mới: {name}, Icon URL: {iconUrl}");
                    var (success, message) = await ViewModel.AddNewToolTypeAsync(newToolType);

                    // Reset the flag before showing the result dialog
                    isDialogOpen_AddToolType = false;

                    // Show result message
                    ContentDialog resultDialog = new ContentDialog
                    {
                        Title = success ? "Thành công" : "Lỗi",
                        Content = success ? "Add New ToolType Successfully" : $" Can't Add This New ToolType: {message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await resultDialog.ShowAsync();

                    if (success)
                    {
                        // Refresh the combobox
                        ToolTypeComboBox.SelectedItem = ViewModel.ToolCategories.FirstOrDefault(c => c.name == name);
                    }
                }
                else
                {
                    // Reset the flag if user cancelled
                    isDialogOpen_AddToolType = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EXCEPTION] Lỗi khi thêm loại công cụ: {ex}");

                // Reset the flag before showing the exception dialog
                isDialogOpen_AddToolType = false;

                // Show exception message
                ContentDialog exceptionDialog = new ContentDialog
                {
                    Title = "Server Error",
                    Content = $"Error While Add New ToolType: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await exceptionDialog.ShowAsync();
            }
        }
    }
}