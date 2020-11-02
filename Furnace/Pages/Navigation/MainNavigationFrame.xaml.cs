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
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Threading.Tasks;
using Furnace.Helpers;
using System.Threading;
using System.ComponentModel;

namespace Furnace.Pages.Navigation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainNavigationFrame : Page
    {

        private double Width;

        public MainNavigationFrame()
        {
            this.InitializeComponent();
            Width = Window.Current.CoreWindow.Bounds.Width;
            this.DataContext = this;
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
                Position = new Point(((Frame)Window.Current.Content).ActualWidth - 205, 100+(84*Math.Max(App.AppTaskManager.RunningTasks.Count, 1)))
            });
            
        }

        private void MainNavigationFrame_OnLoaded(object sender, RoutedEventArgs e)
        {
            BackgroundTaskListItemsControl.ItemsSource = App.AppTaskManager.RunningTasks;

            App.AppTaskManager.RunningTasks.CollectionChanged += AppManagerRunningTasks_CollectionChanged;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            App.AppTaskManager.RunningTasks.CollectionChanged -= AppManagerRunningTasks_CollectionChanged;
        }

        private void AppManagerRunningTasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= ProgressItem_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += ProgressItem_PropertyChanged;
            }
        }

        private void RefreshCompletedTasksPercentage()
        {
            _ = ThreadHelper.RunOnUIThreadAsync(() =>
            {
                var totalCompleted = 0;
                var totalTasks = 0;
                foreach (var task in App.AppTaskManager.RunningTasks)
                {
                    totalTasks += task.TotalItems;
                    totalCompleted += task.CompletedItems;
                }
                if (totalTasks == 0)
                    RadialProgressBarControl.Value = 0;
                else
                {
                    decimal percentage = ((decimal)totalCompleted / (decimal)totalTasks) * 100;
                    RadialProgressBarControl.Value = (int)percentage;
                    System.Diagnostics.Debug.WriteLine((int)percentage);
                }
            });
        }

        private void ProgressItem_PropertyChanged(object sender, PropertyChangedEventArgs e) => RefreshCompletedTasksPercentage();

        private void Profiles_NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Showing at null means Position is relative to (0,0) on the window.
            AccountsFlyout.ShowAt(null, new FlyoutShowOptions
            {
                Position = new Point(Width/2, 380)
            });
        }
        
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => Width = Window.Current.CoreWindow.Bounds.Width;

        private void AccountsFlyout_Opened(object sender, object e)
        {
            Flyout f = sender as Flyout;
            Style s = new Windows.UI.Xaml.Style { TargetType = typeof(FlyoutPresenter) };
            s.Setters.Add(new Setter(MinHeightProperty, "200"));
            s.Setters.Add(new Setter(MinWidthProperty, (Width-20).ToString()));
            f.FlyoutPresenterStyle = s;

            foreach(var a in App.AccountManager.MojangAccounts)
            {
                a.IsSelected = a == Furnace.Settings.AppSettings.SelectedAccount;
            }
            AccountsItemsControl.ItemsSource = App.AccountManager.MojangAccounts;
        }

        private void AddMojangAccountGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = (new Dialogs.Yggdrasil.LoginDialog()).ShowAsync();
        }

        private void AccountSelectButton_Click(object sender, RoutedEventArgs e)
        {
            var account = (sender as FrameworkElement).DataContext as Yggdrasil.Json.Account.MojangAccount;
            Furnace.Settings.AppSettings.SelectedAccount = account;
            foreach (var a in App.AccountManager.MojangAccounts)
            {
                a.IsSelected = a == Furnace.Settings.AppSettings.SelectedAccount;
            }
            AccountsItemsControl.ItemsSource = App.AccountManager.MojangAccounts;
        }

        private async void AccountDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var account = (sender as FrameworkElement).DataContext as Yggdrasil.Json.Account.MojangAccount;

            // Deselect account if it's the one currently selected
            if (Furnace.Settings.AppSettings.SelectedAccount == account)
                Furnace.Settings.AppSettings.SelectedAccount = null;
            App.AccountManager.MojangAccounts.Remove(account);
            await App.AccountManager.SaveMojangAccountsToFileAsync();
            AccountsItemsControl.ItemsSource = App.AccountManager.MojangAccounts;
        }
    }
}
