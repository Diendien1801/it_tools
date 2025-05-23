﻿using it_tools.BusinessLogic.Services;
using it_tools.Presentation.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace it_tools
{
    public partial class App : Application
    {
        public static MainWindow MainAppWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
            AppServices.ConfigureServices();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainAppWindow = new MainWindow(); // Fix: Instantiate MainWindow instead of Window

            var rootFrame = new Frame();
            MainAppWindow.Content = rootFrame;

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(AuthPage)); // Trang khởi đầu
            }

            MainAppWindow.Activate();
        }
    }
}
