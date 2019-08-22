using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public interface IEssential
    {
        string Model { get; }

        string Name { get; }

        string OperatingSystemName { get; }

        string OperatingSystemVersion { get; }

        string AppVersion { get; }

        string AppName { get; }
    }

    public class Essential : IEssential
    {
        public string Model => DeviceInfo.Model;

        public string Name => DeviceInfo.Name;

        public string OperatingSystemName => DeviceInfo.Platform.ToString();

        public string OperatingSystemVersion => DeviceInfo.VersionString;

        public string AppVersion => AppInfo.VersionString;

        public string AppName => AppInfo.Name;
    }
}
