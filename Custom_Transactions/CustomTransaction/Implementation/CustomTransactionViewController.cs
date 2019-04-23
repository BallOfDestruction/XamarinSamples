using System;
using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// Just sample controller
    /// </summary>
    public class CustomTransactionViewController : UIViewController
    {
        public SwipeInterractionController SwipeInteraction { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.UserInteractionEnabled = true;

            SwipeInteraction = new SwipeInterractionController(this);

            var random = new Random(DateTime.Now.Millisecond);
            var color = UIColor.FromRGB(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            View.BackgroundColor = color;

            var dismissButton = new UIButton() { TranslatesAutoresizingMaskIntoConstraints = false };
            dismissButton.SetTitle("dismiss", UIControlState.Normal);
            View.AddSubview(dismissButton);

            dismissButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            dismissButton.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;

            dismissButton.TouchUpInside += (sender, e) =>
            {
                DismissViewController(true, null);
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                var secondController = new CustomTransactionViewController
                {
                    TransitioningDelegate = new TransitionDelegate()
                };
                PresentViewController(secondController, true, null);
            }));
        }
    }
}