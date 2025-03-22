using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ToolLib;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace it_tools.Presentation.Views
{
   
        public sealed partial class ToolDetailPage : Page
        {
            private ITool _tool;

            public ToolDetailPage()
            {
                this.InitializeComponent();
            }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Debug.WriteLine("🔹 OnNavigatedTo được gọi");

            if (e.Parameter is ITool tool)
            {
                _tool = tool;
                Debug.WriteLine($"✅ Nhận được tool: {_tool.name}");

                TitleTextBlock.Text = _tool.name;

                try
                {
                    // Thử lấy UI từ DLL
                    Debug.WriteLine("🔹 Đang gọi _tool.GetUI()");
                    UserControl toolUI = _tool.GetUI();

                    if (toolUI != null)
                    {
                        Debug.WriteLine("✅ UI của tool đã được tạo thành công");
                        ToolContent.Content = toolUI;
                    }
                    else
                    {
                        Debug.WriteLine("⚠️ _tool.GetUI() trả về null!");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Lỗi khi load UI từ DLL: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("⚠️ e.Parameter không phải là ITool!");
            }
        }

    }

}
