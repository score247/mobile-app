﻿namespace LiveScore.Core.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IBackgroundJob
    {
        void Initialize(object data);

        Task Start(CancellationToken cancellationToken);
    }
}
