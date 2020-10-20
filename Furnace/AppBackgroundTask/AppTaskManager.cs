using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Furnace.AppBackgroundTask
{
    public class AppTaskManager
    {
        public ObservableCollection<BackgroundTask> RunningTasks { get; private set; } = new ObservableCollection<BackgroundTask>();

        public void BeginTask(BackgroundTask task)
        {
            RunningTasks.Add(task);
            MainPage.Current.DebugMessage = "Beginning Task";
            Task.Run(task.Run);
        }
    }
}