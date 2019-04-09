namespace LiveScore.Shared.Configurations
{
    public interface IAppSettings
    {
        string AppPath { get; }

        bool IsUseStaticData { get; }
    }
}