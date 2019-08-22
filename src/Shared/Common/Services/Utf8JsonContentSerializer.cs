using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Utf8Json;

namespace LiveScore.Common.Services
{
    public class Utf8JsonContentSerializer : IContentSerializer
    {
        private readonly Lazy<IJsonFormatterResolver> jsonFormatterResolver;

        public Utf8JsonContentSerializer() : this(null)
        {
        }

        public Utf8JsonContentSerializer(IJsonFormatterResolver jsonFormatterResolver)
        {
            this.jsonFormatterResolver = new Lazy<IJsonFormatterResolver>(() =>
            {
                return jsonFormatterResolver ?? JsonSerializer.DefaultResolver;
            });
        }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            using (var stream = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
                return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var content = new StringContent(JsonSerializer.ToJsonString(item, jsonFormatterResolver.Value), Encoding.UTF8, "application/json");

            return Task.FromResult((HttpContent)content);
        }
    }
}