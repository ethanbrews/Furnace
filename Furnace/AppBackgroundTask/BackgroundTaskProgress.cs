using System.ComponentModel;

namespace Furnace.AppBackgroundTask
{
    public class BackgroundTaskProgress : INotifyPropertyChanged
    {
        public int TotalItems { get; set; } = 20;
        public int CompletedItems { get; set; } = 10;

        public void SetPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}