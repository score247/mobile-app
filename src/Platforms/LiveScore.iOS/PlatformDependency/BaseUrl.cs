using Foundation;
using LiveScore.Common.PlatformDependency;
using LiveScore.iOS.PlatformDependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrliOS))]

namespace LiveScore.iOS.PlatformDependency
{
    public class BaseUrliOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}
