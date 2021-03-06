﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Furnace.Helpers;
using Furnace.Pages.Console;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Furnace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        public static MainPage Current;

        private List<string> _consoleLines;
        public string ConsoleLines => string.Join('\n', _consoleLines);

        public void Log(string ln)
        {
            _consoleLines.Add(ln);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConsoleLines"));
        }
        
        public MainPage()
        {
            Current = this;
            this.InitializeComponent();
            
            _consoleLines = new List<string>();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var package = Package.Current;
            AppTitleTextBlock.Text =
                $"{package.DisplayName} {package.Id.Version.Major}.{package.Id.Version.Minor}.{package.Id.Version.Build}.{package.Id.Version.Revision}";
            
            Window.Current.SetTitleBar(AppTitleBarGrid);
            
            Window.Current.CoreWindow.SizeChanged += (s, e) => UpdateAppTitle();
            coreTitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle();
        }

        public string DebugMessage
        {
            set => DebugTextBlock.Text = value;
            get => DebugTextBlock.Text;
        }

        private void UpdateAppTitle()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset+8);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            TitleBarButton.Margin = new Thickness(0,0,coreTitleBar.SystemOverlayRightInset,0);
            AppTitleBarGrid.Height = coreTitleBar.Height;
        }

        private void MainPage_OnLoading(FrameworkElement sender, object args)
        {
            MainPageFrame.Navigate(typeof(Pages.Navigation.MainNavigationFrame), null,
                new SuppressNavigationTransitionInfo());

            System.Diagnostics.Debug.WriteLine(Furnace.Settings.AppSettings.SelectedAccount);

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
