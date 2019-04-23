using CustomTransaction.Implementation;
using Foundation;
using UIKit;

namespace Blank
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, IUITabBarControllerDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = new CustomTransactionViewController()
            };

            Window.MakeKeyAndVisible();

            return true;
        }
    }
}