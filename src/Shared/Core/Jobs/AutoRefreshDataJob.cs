namespace LiveScore.Core.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Core.Services;

    public class AutoRefreshDataJob : IBackgroundJob
    {
        public AutoRefreshDataJob()
        {
        }

        public void Initialize(object data)
        {
            throw new NotImplementedException();
        }

        public Task Start(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
