using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public interface IDeviceInfo
    {
        string Id { get; }

        double Width { get; }

        double Height { get; }

        double Density { get; }
    }

    public class UserDeviceInfo : IDeviceInfo
    {
        public double Width => DeviceDisplay.MainDisplayInfo.Width;

        public double Height => DeviceDisplay.MainDisplayInfo.Height;

        public double Density => DeviceDisplay.MainDisplayInfo.Density;

        public string Id => $"{DeviceInfo.Name}-{DeviceInfo.Model}-{DeviceInfo.DeviceType}";
    }
}