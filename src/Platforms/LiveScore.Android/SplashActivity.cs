using Android.App;
using Android.OS;

namespace LiveScore.Droid
{
    [Activity(Theme = "@style/Theme.SplashScreen",
                MainLauncher = true,
                NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}