using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Inventory;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Furnace.Dialogs.Navigation; 

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

        public void NavigateFrame(Type pageType, object parameter = null, NavigationTransitionInfo transitionInfo = null)
        {
            if (ContentFrame.CurrentSourcePageType == pageType)
                return;
            ContentFrame.Navigate(pageType, parameter, transitionInfo ?? new EntranceNavigationTransitionInfo());
        }

        private void MainNavigationBar_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((FrameworkElement)args?.SelectedItem)?.Tag)
            {
                case "Installed":
                    NavigateFrame(typeof(Pages.General.Installed), transitionInfo: args.RecommendedNavigationTransitionInfo);
                    break;
                case "Discover":
                    NavigateFrame(typeof(Pages.General.Discover), transitionInfo: args.RecommendedNavigationTransitionInfo);
                    break;
                case "Servers":
                    NavigateFrame(typeof(Pages.General.Servers), transitionInfo: args.RecommendedNavigationTransitionInfo);
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

        private void BackgroundTaskIndicator_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            // Showing at null means Position is relative to (0,0) on the window.
            BackgroundTaskListFlyout.ShowAt(null, new FlyoutShowOptions
            {
                Position = new Point(((Frame)Window.Current.Content).ActualWidth - 205, 190)
            });
            
        }

        private void MainNavigationFrame_OnLoaded(object sender, RoutedEventArgs e)
        {
            BackgroundTaskListItemsControl.ItemsSource = App.AppTaskManager.RunningTasks;
            App.AppTaskManager.BeginTask(new Modpack.TestBackgroundTask());
        }
    }
}
