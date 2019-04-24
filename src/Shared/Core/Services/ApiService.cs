namespace LiveScore.Core.Services
{
    using LiveScore.Common.Configuration;
    using Refit;

    public interface IApiService<out T>
    {
        T GetApi();
    }

    public class ApiService<T> : IApiService<T>
    {
        public T GetApi() => RestService.For<T>(Configuration.LocalEndPoint);
    }
}