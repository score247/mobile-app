using MessagePack;

namespace LiveScore.Core.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CommandResult
    {
        public CommandResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }
    }
}
