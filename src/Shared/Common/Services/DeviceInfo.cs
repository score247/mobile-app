using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public interface IDeviceInfo
    {
        double Width { get; }

        double Height { get; }

        double Density { get; }
    }

    public class DeviceInfo : IDeviceInfo
    {
        public double Width => DeviceDisplay.MainDisplayInfo.Width;

        public double Height => DeviceDisplay.MainDisplayInfo.Height;

        public double Density => DeviceDisplay.MainDisplayInfo.Density;
    }
}