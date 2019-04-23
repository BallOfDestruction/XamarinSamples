using Foundation;
using UIKit;

namespace LoadSwitch.Example
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.RootViewController = new SampleViewController();
            Window.MakeKeyAndVisible();

            return true;
        }
    }
}