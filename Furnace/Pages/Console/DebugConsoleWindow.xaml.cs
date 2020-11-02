using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Furnace.Pages.Console
{
    public partial class DebugConsoleWindow : Page
    {
        public DebugConsoleWindow()
        {
            InitializeComponent();
        }

        private void DebugConsoleWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            OutputBlock.SetBinding(TextBlock.TextProperty, new Binding
            {
                Source = MainPage.Current.ConsoleLines,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }
    }
}