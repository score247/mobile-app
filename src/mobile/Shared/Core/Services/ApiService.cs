namespace LiveScore.Core.Services
{
    using Refit;

    public interface IApiService<T>
    {
        T GetApi();
    }

    public class ApiService<T> : IApiService<T>
    {
        public T GetApi() => RestService.For<T>(SettingsService.ApiEndPoint);
    }
}
