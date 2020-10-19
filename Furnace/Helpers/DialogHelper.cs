using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Furnace.Helpers
{
    public class DialogHelper
    {
        private static Queue<ContentDialog> _dialogQueue = new Queue<ContentDialog>();
        private static bool _dialogLock = false;

        public static async Task ScheduleDialogAsync(ContentDialog dialog)
        {
            System.Diagnostics.Debug.WriteLine(dialog);
            dialog.Closed += DialogOnClosedAsync;
            if (IsAnyContentDialogOpen() || _dialogLock)
                _dialogQueue.Enqueue(dialog);
            else
            {
                _ = dialog.ShowAsync();
                _dialogLock = true;
            }

        }

        private static bool IsAnyContentDialogOpen() => VisualTreeHelper.GetOpenPopups(Window.Current).Count > 0;

        private static async void DialogOnClosedAsync(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (_dialogQueue.Count == 0)
            {
                _dialogLock = false;
                return;
            }

            var next = _dialogQueue.Dequeue();
            await next.ShowAsync();
        }
    }
}
