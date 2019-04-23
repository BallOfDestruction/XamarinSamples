using System;
using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// Swipe interraction controller
    /// Make screen edge gesture interraction
    /// </summary>
    public class SwipeInterractionController : UIPercentDrivenInteractiveTransition
    {
        public bool InteractionInProgress { get; private set; }
        private bool shouldCompleteTransition;
        private readonly UIViewController _controller;

        public SwipeInterractionController(UIViewController controller)
        {
            this._controller = controller;
            PrepareGestureRecognizer();
        }

        private void PrepareGestureRecognizer()
        {
            var gesture = new UIScreenEdgePanGestureRecognizer((obj) =>
            {
                HandleGestureRec(obj);
            })
            {
                Edges = UIRectEdge.Left
            };
            _controller.View.AddGestureRecognizer(gesture);
        }

        private void HandleGestureRec(UIScreenEdgePanGestureRecognizer edge)
        {
            var translation = edge.TranslationInView(edge.View.Superview);
            var progress = (translation.X / 300f);
            progress = new nfloat(Math.Min(1, Math.Max(progress, 0)));

            switch (edge.State)
            {
                case UIGestureRecognizerState.Possible:
                    break;
                case UIGestureRecognizerState.Began:
                    InteractionInProgress = true;
                    _controller.DismissViewController(true, null);
                    break;
                case UIGestureRecognizerState.Changed:
                    shouldCompleteTransition = progress > 0.5;
                    UpdateInteractiveTransition(progress);
                    break;
                case UIGestureRecognizerState.Ended:
                    InteractionInProgress = false;
                    if (shouldCompleteTransition)
                    {
                        FinishInteractiveTransition();
                    }
                    else
                    {
                        CancelInteractiveTransition();
                    }
                    break;
                case UIGestureRecognizerState.Cancelled:
                    InteractionInProgress = false;
                    CancelInteractiveTransition();
                    break;
                case UIGestureRecognizerState.Failed:
                    break;
            }
        }
    }
}