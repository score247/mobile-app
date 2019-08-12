namespace LiveScore.Common.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;

    public static class HubConnectionExtension
    {
        public static async Task<HubConnection> StartWithKeepAlive(this HubConnection hubConnection, TimeSpan interval, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    await hubConnection.StartAsync();
                    await Task.Delay(interval, cancellationToken);
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                await hubConnection.StopAsync();
            }

            return hubConnection;
        }
    }
}
