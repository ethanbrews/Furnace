using System;
using System.ComponentModel;
using System.Threading;
using Windows.Foundation;
using Furnace.Helpers;

namespace Furnace.AppBackgroundTask
{
    public abstract class BackgroundTask : INotifyPropertyChanged
    {
        public abstract string Title { get; }
        public abstract string Description { get; }
        public BackgroundTaskProgress Progress { get; protected set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void ReportProgress(int completed, int? total = null)
        {
            if (Progress == null)
                Progress = new BackgroundTaskProgress {CompletedItems = completed, TotalItems = total ?? completed};
            else
            {
                Progress.CompletedItems = completed;
                Progress.TotalItems = total ?? Progress.TotalItems;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Progress"));

            ThreadHelper.RunOnUIThreadAsync(() =>
            {
                #error
                //The RunOnUIThreadAsync doesn't work. DONT use Window.Current.Dispatcher
                MainPage.Current.DebugMessage = $"Progress Changed {completed}/{total ?? Progress.TotalItems}";
            });
        }

        public abstract void Run();
    }
}