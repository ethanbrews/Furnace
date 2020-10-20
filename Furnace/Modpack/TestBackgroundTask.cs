using System.Threading;
using Furnace.AppBackgroundTask;

namespace Furnace.Modpack
{
    public class TestBackgroundTask : AppBackgroundTask.BackgroundTask
    {
        public override string Title => "Test Background Task";
        public override string Description => "Test background task description";

        public override void Run()
        {
            var total = 30;
            ReportProgress(0, total);
            for (var i = 0; i < total; i++)
            {
                Thread.Sleep(1000);
                ReportProgress(i);
            }
        }
    }
}