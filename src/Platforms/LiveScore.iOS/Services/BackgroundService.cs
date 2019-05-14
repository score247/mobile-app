namespace LiveScore.iOS.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Core.Services;
    using Prism.Events;
    using UIKit;

    public class BackgroundService
    {
        private readonly IEventAggregator eventAggregator;
        private CancellationTokenSource cancellationTokenSource;

        public BackgroundService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public async Task Start(IBackgroundJob job)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var taskId = UIApplication.SharedApplication.BeginBackgroundTask("BackgroundService", OnExpiration);

            try
            {
                await job.Start(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // Write log
            }
            finally
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(taskId);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        void OnExpiration()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
