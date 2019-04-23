using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// Transition delegate.
    /// Provide animations to viewControllers
    /// </summary>
    public class TransitionDelegate : UIViewControllerTransitioningDelegate
    {
        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            return new FlipOpenAnimationController(presented.View.Frame);
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            if (dismissed is CustomTransactionViewController firstController)
            {
                return new FlipDismissAnimationController(dismissed.View.Frame, firstController.SwipeInteraction);
            }

            return null;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForDismissal(IUIViewControllerAnimatedTransitioning animator)
        {
            if (animator is FlipDismissAnimationController flipController && flipController.InterractionController.InteractionInProgress)
                return flipController.InterractionController;
            return null;
        }
    }
}