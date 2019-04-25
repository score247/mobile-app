namespace LiveScore.Core.Services
{
    using LiveScore.Common.Configuration;
    using Refit;

    public interface IApiService
    {
        T GetApi<T>();
    }

    public class ApiService : IApiService
    {
        public T GetApi<T>() => RestService.For<T>(Configuration.LocalEndPoint);        
    }
}