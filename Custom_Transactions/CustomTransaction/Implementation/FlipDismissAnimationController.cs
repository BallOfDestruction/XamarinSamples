using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// Dissmiss animation
    /// 1. Postpone 2. Rotate 3. Brings closer
    /// </summary>
    public class FlipDismissAnimationController : UIViewControllerAnimatedTransitioning
    {
        private readonly CGRect _frame;

        public FlipDismissAnimationController(CGRect frame, SwipeInterractionController interractionController)
        {
            _frame = frame;
            InterractionController = interractionController;
        }

        public SwipeInterractionController InterractionController { get; }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var fromController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var lastSnapshot = fromController.View.SnapshotView(false);

            var container = transitionContext.ContainerView;
            var toFrame = transitionContext.GetFinalFrameForViewController(toController);
            var duration = TransitionDuration(transitionContext);

            lastSnapshot.Frame = _frame;
            lastSnapshot.Layer.CornerRadius = 0;
            lastSnapshot.Layer.MasksToBounds = true;
            fromController.View.Layer.MasksToBounds = true;

            container.AddSubview(toController.View);
            container.AddSubview(lastSnapshot);
            fromController.View.Hidden = true;


            var rotate = AnimationHelper.PerspectiveTransform(container, -Math.PI / 2);
            toController.View.Layer.Transform = rotate;
            toController.View.Layer.CornerRadius = 25;

            UIView.AnimateKeyframes(duration, 0, UIViewKeyframeAnimationOptions.CalculationModeCubic | UIViewKeyframeAnimationOptions.LayoutSubviews, () =>
            {
                UIView.AddKeyframeWithRelativeStartTime(0, 1 / 4f, () =>
                {
                    var coef = 0.75f;
                    var antiCoef = 1 - coef;
                    var height = toFrame.Height * coef;
                    var width = toFrame.Width * coef;
                    var x = (toFrame.Width * antiCoef) / 2;
                    var y = (toFrame.Height * antiCoef) / 2;
                    var frame = new CGRect(x, y, width, height);
                    lastSnapshot.Frame = frame;
                    toController.View.Frame = frame;

                    lastSnapshot.Layer.CornerRadius = 25;
                });

                UIView.AddKeyframeWithRelativeStartTime(1 / 4f, 1 / 4f, () =>
                {
                    lastSnapshot.Layer.Transform = AnimationHelper.PerspectiveTransform(lastSnapshot, Math.PI / 2);
                });

                UIView.AddKeyframeWithRelativeStartTime(2 / 4f, 1 / 4f, () =>
                {
                    toController.View.Layer.Transform = AnimationHelper.PerspectiveTransform(toController.View, 0);
                });

                UIView.AddKeyframeWithRelativeStartTime(3 / 4f, 1 / 4f, () =>
                {
                    toController.View.Frame = toFrame;
                    toController.View.Layer.CornerRadius = 0;
                    fromController.View.Layer.CornerRadius = 0;
                });

            }, (finished) =>
            {
                fromController.View.Hidden = false;
                lastSnapshot.RemoveFromSuperview();
                fromController.View.Layer.Transform = CATransform3D.Identity;
                transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
            });
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 5;
        }
    }
}