using System;
using CoreAnimation;
using UIKit;

namespace CustomTransaction.Implementation
{
    /// <summary>
    /// Add perspective transform
    /// </summary>
    public static class AnimationHelper
    {
        public static CATransform3D PerspectiveTransform(UIView view, double angle)
        {
            var transform = CATransform3D.Identity;
            transform.m34 = new nfloat(-0.002f);
            view.Layer.SublayerTransform = transform;

            return CATransform3D.MakeRotation(new nfloat(angle), 0, 1, 0);
        }
    }
}