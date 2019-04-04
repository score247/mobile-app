using Xamarin.Essentials;

namespace Common.Services
{
    public interface IDeviceInfoService 
    {
        string Model { get; }

        string Name { get; }

        string OperatingSystemName { get; }

        string OperatingSystemVersion { get; }
    }

    public class DeviceInfoService : IDeviceInfoService
    {
        public string Model => DeviceInfo.Model;

        public string Name => DeviceInfo.Name;

        public string OperatingSystemName => DeviceInfo.Platform.ToString();

        public string OperatingSystemVersion => DeviceInfo.VersionString;
    }
}
