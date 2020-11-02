using Furnace.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Furnace.AppBackgroundTask
{
    public class AppTaskManager : INotifyPropertyChanged
    {
        public ObservableCollection<BackgroundTask> RunningTasks { get; private set; }


        private int _completedPercentage = 0;
        public int CompletedPercentage
        {
            get => _completedPercentage;
            set
            {
                _completedPercentage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CompletedPercentage"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AppTaskManager()
        {
            RunningTasks = new ObservableCollection<BackgroundTask>();
        }

        public void BeginTask(BackgroundTask task)
        {
            RunningTasks.Add(task);
            MainPage.Current.DebugMessage = "Beginning Task";
            Task.Run(task.Run);
        }
    }
}