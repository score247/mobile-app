namespace LiveScore.Common.Helpers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MessagePack;
    using Refit;

    public class MessagePackContentSerializer : IContentSerializer
    {
        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            var data = MessagePackSerializer.Deserialize<T>(await content.ReadAsStreamAsync());

            return data;
        }

        public Task<HttpContent> SerializeAsync<T>(T item)
        {
            var content = new ByteArrayContent(MessagePackSerializer.Serialize(item));
            return Task.FromResult((HttpContent)content);
        }
    }
}