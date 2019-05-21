namespace LiveScore.Common.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using System.Threading;

    public static class HubConnectionExtension
    {
        public static async Task<HubConnection> StartWithKeepAlive(this HubConnection hubConnection, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                await hubConnection.StartAsync();
                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}
