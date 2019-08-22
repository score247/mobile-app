using System;
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

        public Utf8JsonContentSerializer(IJsonFormatterResolver jsonFormatterResolver)
        {
            this.jsonFormatterResolver = new Lazy<IJsonFormatterResolver>
            {

            };
        }

        public Task<T> DeserializeAsync<T>(HttpContent content)
        {            
            throw new NotImplementedException();
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var content = new StringContent(JsonSerializer.ToJsonString(item, jsonFormatterResolver.Value), Encoding.UTF8, "application/json");

            return Task.FromResult((HttpContent)content);
        }
    }
}
