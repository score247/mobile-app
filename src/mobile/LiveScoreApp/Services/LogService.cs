namespace LiveScoreApp.Services
{
    using System.Diagnostics;
    using Prism.Logging;

    public class LogService : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            Debug.WriteLine($"{category} - {priority}: {message}");
        }
    }
}