using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using Newtonsoft.Json;
using Refit;

namespace LiveScore.Common.Helpers
{
    public class MessagePackContentSerializer : IContentSerializer
    {
        private readonly Lazy<JsonSerializerSettings> jsonSerializerSettings;

        public MessagePackContentSerializer()
        {
            jsonSerializerSettings = new Lazy<JsonSerializerSettings>(() =>
            {
                if (JsonConvert.DefaultSettings == null)
                {
                    return new JsonSerializerSettings();
                }

                return JsonConvert.DefaultSettings();
            });
        }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        => MessagePackSerializer.Deserialize<T>(await content.ReadAsStreamAsync().ConfigureAwait(false));

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item, jsonSerializerSettings.Value), Encoding.UTF8, "application/json");
            return Task.FromResult((HttpContent)content);
        }
    }
}