using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Furnace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static MainPage Current;
        public MainPage()
        {
            Current = this;
            this.InitializeComponent();

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
#if DEBUG
            DebugTextBlock.Visibility = Visibility.Visible;
#endif
        }
    }
}
