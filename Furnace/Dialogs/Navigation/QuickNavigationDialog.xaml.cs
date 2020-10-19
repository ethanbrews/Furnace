using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Furnace.Pages.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Furnace.Dialogs.Navigation
{
    public sealed partial class QuickNavigationDialog : ContentDialog
    {

        private MainNavigationFrame _frame;

        public QuickNavigationDialog(MainNavigationFrame frame)
        {
            this.InitializeComponent();
            _frame = frame;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void GridOrButton_KeyDown(object sender, KeyRoutedEventArgs e) 
        {
            System.Diagnostics.Debug.WriteLine(e.Key);
            switch (e.Key)
            {
                case VirtualKey.I:
                    _frame.NavigateFrame(typeof(Pages.General.Installed));
                    break;
                case VirtualKey.S:
                    _frame.NavigateFrame(typeof(Pages.General.Servers));
                    break;
                case VirtualKey.D:
                    _frame.NavigateFrame(typeof(Pages.General.Discover));
                    break;
                case VirtualKey.O:
                    _frame.NavigateFrame(typeof(Pages.Settings.Settings));
                    break;
                case VirtualKey.Q:
                    _frame.NavigateFrame(typeof(Pages.Settings.Diagnostics));
                    break;
                case VirtualKey.Escape:
                    break;
                default:
                    return;
            }

            this.Hide();
        }

        private void QuickNavigationDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ShortcutsList.ItemsSource = new Dictionary<string, string>
            {
                {"I", "Installed"},
                {"S", "Servers"},
                {"D", "Discover"},
                {"O", "Settings"},
                {"Q", "Debug Page"},
            };
        }

        private void ShortcutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
