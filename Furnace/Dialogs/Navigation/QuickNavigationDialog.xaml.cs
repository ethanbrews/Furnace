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
using Windows.UI.Xaml.Media.Animation;

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
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;
        }

        private void CoreWindow_CharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.KeyCode);
            // K = 11
            
        }

        private void ProcessKeyCode(int x)
        {
            switch (x)
            {
                case 105: //I
                    _frame.NavigateFrame(typeof(Pages.General.Installed), transitionInfo: new SuppressNavigationTransitionInfo());
                    break;
                case 115: //S
                    _frame.NavigateFrame(typeof(Pages.General.Servers), transitionInfo: new SuppressNavigationTransitionInfo());
                    break;
                case 100: //D
                    _frame.NavigateFrame(typeof(Pages.General.Discover), transitionInfo: new SuppressNavigationTransitionInfo());
                    break;
                case 111: //O
                    _frame.NavigateFrame(typeof(Pages.Settings.Settings), transitionInfo: new SuppressNavigationTransitionInfo());
                    break;
                case 113: //Q
                    _frame.NavigateFrame(typeof(Pages.Settings.Diagnostics), transitionInfo: new SuppressNavigationTransitionInfo());
                    break;
                case 27: //ESC
                    break;
                default:
                    return;
            }

            this.Hide();
        }

        private void QuickNavigationDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ShortcutsList.ItemsSource = new Tuple<string, string, int>[]
            {
                new Tuple<string, string, int>("I", "Installed", 105),
                new Tuple<string, string, int>("S", "Servers", 115),
                new Tuple<string, string, int>("D", "Discover", 100),
                new Tuple<string, string, int>("O", "Settings", 111),
                new Tuple<string, string, int>("Q", "Debug Page", 113),
                new Tuple<string, string, int>("ESC", "Cancel", 27),
            };
        }

        private void ShortcutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ctx = (sender as ListView).SelectedItem as Tuple<string, string, int>;
            ProcessKeyCode(ctx.Item3);
        }
    }
}
