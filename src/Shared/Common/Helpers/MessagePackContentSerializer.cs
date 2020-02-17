using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using MessagePack;
using Newtonsoft.Json;
using Refit;

namespace LiveScore.Common.Helpers
{
    public class MessagePackContentSerializer : IContentSerializer
    {
        private readonly Lazy<JsonSerializerSettings> jsonSerializerSettings;
        private readonly ILoggingService loggingService;

        public MessagePackContentSerializer(ILoggingService loggingService)
        {
            jsonSerializerSettings = new Lazy<JsonSerializerSettings>(() =>
            {
                if (JsonConvert.DefaultSettings == null)
                {
                    return new JsonSerializerSettings();
                }

                return JsonConvert.DefaultSettings();
            });

            this.loggingService = loggingService;
        }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            //var stringContent = await content.ReadAsStringAsync();
            var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            try
            {
                return MessagePackSerializer.Deserialize<T>(stream);                
            }
            catch (Exception ex)
            {
                var stringContent = await content.ReadAsStringAsync();

                await LogException(stream, ex);
            }

            return default;
        }

        private async Task LogException(Stream stream, Exception ex)
        {
            Debug.WriteLine("MessagePackContentSerializer LogException");

            var requestBody = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                requestBody = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            var properties = new Dictionary<string, string>
            {
                { "Body", requestBody},
                { "Message", ex.Message}
            };

            await loggingService.LogExceptionAsync(ex, properties);
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item, jsonSerializerSettings.Value), Encoding.UTF8, "application/json");
            return Task.FromResult((HttpContent)content);
        }
    }
}