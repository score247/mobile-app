﻿namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;

    public interface IHubService
    {
        Task Start();

        Task ReConnect(byte retryTimes = 5);
    }
}