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
using it_tools.Presentation.ViewModels;
using System.Threading.Tasks;
using System.Diagnostics;
using ToolLib;
using it_tools.DataAccess.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace it_tools.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ToolPage : Page
{
    public ToolPageViewModel ViewModel { get; private set; }

    public ToolPage()
    {
        this.InitializeComponent();
        ViewModel = new ToolPageViewModel();
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
        private void OnToolSelected(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Tool selectedTool)
            {
                Debug.WriteLine($"✅ Chuyển sang ToolDetailPage với tool: {selectedTool.name}");
                Frame.Navigate(typeof(ToolDetailPage), selectedTool.LoadedPlugin);
            }
            
            else
            {
                Debug.WriteLine("❌ e.ClickedItem không phải là ITool!");
            }
        }




    }
}
