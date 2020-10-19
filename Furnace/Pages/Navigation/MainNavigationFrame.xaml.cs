using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Inventory;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Furnace.Dialogs.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Furnace.Pages.Navigation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainNavigationFrame : Page
    {
        public MainNavigationFrame()
        {
            this.InitializeComponent();
        }

        private void Settings_Tapped(object sender, TappedRoutedEventArgs e) =>
            NavigateFrame(typeof(Settings.Settings));

        public void NavigateFrame(Type pageType)
        {
            if (ContentFrame.CurrentSourcePageType == pageType)
                return;
            ContentFrame.Navigate(pageType, null, new ContinuumNavigationTransitionInfo());
        }

        private void MainNavigationBar_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((FrameworkElement)args?.SelectedItem)?.Tag)
            {
                case "Installed":
                    break;
                case "Discover":
                    break;
                case "Servers":
                    break;
            }
        }

        private void ContentFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            MainNavigationBar.IsBackEnabled = ContentFrame.CanGoBack;
            var p = ContentFrame.CurrentSourcePageType;
            if (p == typeof(General.Installed))
                MainNavigationBar.SelectedItem = InstalledNavItem;
            else if (p == typeof(General.Discover))
                MainNavigationBar.SelectedItem = DiscoverNavItem;
            else if (p == typeof(General.Servers))
                MainNavigationBar.SelectedItem = ServersNavItem;
            else
                MainNavigationBar.SelectedItem = null;
        }

        private async void QuickNavigateButton_Click(object sender, RoutedEventArgs e)
        {
            await Helpers.DialogHelper.ScheduleDialogAsync(new QuickNavigationDialog(this));
        }
    }
}
