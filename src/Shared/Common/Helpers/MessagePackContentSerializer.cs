using System.Net.Http;
using System.Threading.Tasks;
using MessagePack;
using Refit;

namespace LiveScore.Common.Helpers
{
    public class MessagePackContentSerializer : IContentSerializer
    {
        public async Task<T> DeserializeAsync<T>(HttpContent content)
            => MessagePackSerializer.Deserialize<T>(await content.ReadAsStreamAsync().ConfigureAwait(false));

        public Task<HttpContent> SerializeAsync<T>(T item)
            => Task.FromResult((HttpContent)new ByteArrayContent(MessagePackSerializer.Serialize(item)));
    }
}