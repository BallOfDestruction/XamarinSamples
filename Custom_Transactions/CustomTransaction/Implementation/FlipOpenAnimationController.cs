using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// View controller animation
    /// 1. Postpone 2. Rotate 3. Brings closer
    /// </summary>
    public class FlipOpenAnimationController : UIViewControllerAnimatedTransitioning
    {
        private readonly CGRect _frame;

        public FlipOpenAnimationController(CGRect frame)
        {
            _frame = frame;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var fromController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var lastSnapshot = toController.View.SnapshotView(true);

            var container = transitionContext.ContainerView;
            var toFrame = transitionContext.GetFinalFrameForViewController(toController);
            var duration = TransitionDuration(transitionContext);

            lastSnapshot.Frame = _frame;
            lastSnapshot.Layer.CornerRadius = 25;
            lastSnapshot.Layer.MasksToBounds = true;


            container.AddSubview(toController.View);
            container.AddSubview(lastSnapshot);
            toController.View.Hidden = true;
            fromController.View.Layer.MasksToBounds = true;


            var rotate = AnimationHelper.PerspectiveTransform(container, Math.PI / 2);
            lastSnapshot.Layer.Transform = rotate;

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
                    fromController.View.Frame = frame;
                    fromController.View.Layer.CornerRadius = 25;
                });

                UIView.AddKeyframeWithRelativeStartTime(1 / 4f, 1 / 4f, () =>
                    {
                        fromController.View.Layer.Transform = AnimationHelper.PerspectiveTransform(fromController.View, -Math.PI / 2);
                    });

                UIView.AddKeyframeWithRelativeStartTime(2 / 4f, 1 / 4f, () =>
                  {
                      lastSnapshot.Layer.Transform = AnimationHelper.PerspectiveTransform(lastSnapshot, 0);
                  });

                UIView.AddKeyframeWithRelativeStartTime(3 / 4f, 1 / 4f, () =>
                  {
                      lastSnapshot.Frame = toFrame;
                      lastSnapshot.Layer.CornerRadius = 0;
                      fromController.View.Layer.CornerRadius = 0;
                  });

            }, (finished) =>
            {
                toController.View.Hidden = false;
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