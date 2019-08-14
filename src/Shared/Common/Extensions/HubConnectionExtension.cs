namespace LiveScore.Common.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using Microsoft.AspNetCore.SignalR.Client;

    public static class HubConnectionExtension
    {
        public static async Task<HubConnection> StartWithKeepAlive(this HubConnection hubConnection, TimeSpan interval, ILoggingService logger, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    await hubConnection.StartAsync();
                    await Task.Delay(interval, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                await hubConnection.StopAsync();
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync(ex);
            }

            return hubConnection;
        }
    }
}