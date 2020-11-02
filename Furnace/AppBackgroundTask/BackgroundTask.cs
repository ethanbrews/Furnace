using System;
using System.ComponentModel;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Xaml;
using Furnace.Helpers;

namespace Furnace.AppBackgroundTask
{
    public abstract class BackgroundTask : INotifyPropertyChanged
    {
        public abstract string Title { get; }
        public abstract string Description { get; }
        public int TotalItems { get; private set; } = 0;
        public int CompletedItems { get; private set; } = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void ReportProgress(int completed, int? total = null)
        {
            TotalItems = total ?? TotalItems;
            CompletedItems = completed;
            _ = ThreadHelper.RunOnUIThreadAsync(() => {
                NotifyPropertyChanged("TotalItems");
                NotifyPropertyChanged("CompletedItems");
                MainPage.Current.Log($"Progress Changed {completed}/{total ?? TotalItems}");
            });

        }

        public abstract void Run();
    }
}