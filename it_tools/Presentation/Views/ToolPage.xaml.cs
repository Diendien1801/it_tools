using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using it_tools.Presentation.ViewModels;
using it_tools.DataAccess.Models;

namespace it_tools.Presentation.Views
{
    public sealed partial class ToolPage : Page
    {
        public ToolPageViewModel ViewModel { get; private set; }

        public ToolPage()
        {
            this.InitializeComponent();
            ViewModel = new ToolPageViewModel(
                AuthPage.ViewModel
                );
            DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ValueTuple<string, string> param)
            {
                string idToolType = param.Item1;
                string name = param.Item2;

                Debug.WriteLine($"Received idToolType: {idToolType}, name: {name}");

                TitleTextBlock.Text = name;
                await ViewModel.LoadTools(idToolType);
                
            }
        }

        private async void OnToolSelected(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Tool selectedTool)
            {
                Debug.WriteLine($"[DEBUG] 🔍 Tool được chọn: {selectedTool.name} (access_level = {selectedTool.access_level}, is_disabled = {selectedTool.status == "disable"})");

                // Gọi hàm kiểm tra quyền trong ViewModel
                bool hasAccess = await ViewModel.IsUserLevelSufficient(selectedTool.access_level);

                Debug.WriteLine($"[DEBUG] ✅ Quyền truy cập {(hasAccess ? "được cho phép" : "bị từ chối")} đối với tool: {selectedTool.name}");

                if (!hasAccess)
                {
                    Debug.WriteLine($"[WARNING] ❌ Không đủ quyền để truy cập tool: {selectedTool.name}");

                    var deniedDialog = new ContentDialog
                    {
                        Title = "Truy cập bị từ chối",
                        Content = $"Bạn cần cấp quyền '{selectedTool.access_level}' để sử dụng công cụ này.",
                        CloseButtonText = "Đóng",
                        XamlRoot = this.XamlRoot
                    };

                    await deniedDialog.ShowAsync();
                    return;
                }

                if (selectedTool.status == "disable")
                {
                    Debug.WriteLine($"[WARNING] 🚫 Tool đã bị vô hiệu hóa: {selectedTool.name}");

                    var disabledDialog = new ContentDialog
                    {
                        Title = "Công cụ bị vô hiệu hóa",
                        Content = "Công cụ này hiện đang bị vô hiệu hóa và không thể sử dụng.",
                        CloseButtonText = "Đóng",
                        XamlRoot = this.XamlRoot
                    };

                    await disabledDialog.ShowAsync();
                    return;
                }

                Debug.WriteLine($"[INFO] 🔄 Điều hướng sang ToolDetailPage với plugin: {selectedTool.LoadedPlugin}");
                Frame.Navigate(typeof(ToolDetailPage), selectedTool.LoadedPlugin);
            }
            else
            {
                Debug.WriteLine("[ERROR] ❌ e.ClickedItem không phải là một Tool hợp lệ!");
            }
        }



        private async void OnFavoriteButtonClick(object sender, RoutedEventArgs e)
{
    if (sender is Button button && button.Tag is Tool selectedTool)
    {
        bool isAllowed = await ViewModel.IsUserAuthenticated();

        if (!isAllowed)
        {
            await ShowMessageDialog("Bạn cần đăng nhập hoặc nâng cấp tài khoản để sử dụng chức năng này.", "Truy cập bị hạn chế");
            return;
        }

        await ViewModel.UpdateFavoriteStatus(selectedTool);
    }
}


        private async Task ShowMessageDialog(string content, string title)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
