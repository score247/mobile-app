using System;
using System.Threading.Tasks;

namespace LiveScore.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static bool IsIgnore(this Exception ex)
            => ex.GetType() == typeof(OperationCanceledException) ||
                ex.GetType() == typeof(TaskCanceledException);
    }
}
