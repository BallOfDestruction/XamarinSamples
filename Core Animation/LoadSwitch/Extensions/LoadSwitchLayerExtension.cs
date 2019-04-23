using System;
using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Layers;
using UIKit;

namespace LoadSwitch.Extensions
{
    internal static class SuperCustomViewLayerExtension
    {
        [NotNull]
        public static UIBezierPath GetOffStateCirclePath([NotNull] this CALayer layer)
        {
            var rect = GetOffStateCircleRect(layer);

            return UIBezierPath.FromRoundedRect(rect, rect.Height / 2).NotNull();
        }

        [NotNull]
        public static UIBezierPath GetLoadStateCirclePath([NotNull] this CALayer layer)
        {
            var rect = GetLoadStateCircleRect(layer);

            return UIBezierPath.FromRoundedRect(rect, rect.Height / 2).NotNull();
        }

        [NotNull]
        public static UIBezierPath GetOnStateCirclePath([NotNull] this CALayer layer)
        {
            var rect = GetOnStateCircleRect(layer);

            return UIBezierPath.FromRoundedRect(rect, rect.Height / 2).NotNull();
        }

        public static CGRect GetOffStateCircleRect([NotNull] this CALayer layer)
        {
            var size = (float)GetCircleSize(layer);
            var offset = (float)GetCircleOffset(layer);

            return new CGRect(offset, offset, size, size);
        }

        public static CGRect GetOffStateRestPlace([NotNull] this CALayer layer)
        {
            var size = (float)GetCircleSize(layer);
            var offset = (float)GetCircleOffset(layer);

            return new CGRect(1.5 * offset + size, offset, layer.Frame.Width - 1.5 * offset - size, size);
        }

        public static CGRect GetLoadStateCircleRect([NotNull] this CALayer layer)
        {
            var frame = layer.SuperLayer.NotNull().GetClearFrame();

            var size = (float)GetCircleSize(layer);
            var offset = (float)GetCircleOffset(layer);

            return new CGRect((frame.Width - size) / 2, offset, size, size);
        }

        public static CGRect GetOnStateCircleRect([NotNull] this CALayer layer)
        {
            var frame = layer.SuperLayer.NotNull().GetClearFrame();

            var size = (float)GetCircleSize(layer);
            var offset = (float)GetCircleOffset(layer);

            return new CGRect(frame.Width - size - offset, offset, size, size);
        }

        public static CGRect GetOnStateRestPlace([NotNull] this CALayer layer)
        {
            var size = (float)GetCircleSize(layer);
            var offset = (float)GetCircleOffset(layer);

            return new CGRect(offset, offset, layer.Frame.Width - 1.5 * offset - size, size);
        }

        public static nfloat GetCircleOffset([NotNull] this CALayer layer)
        {
            return layer.Frame.Height * 0.4f / 2f;
        }

        public static nfloat GetCircleSize([NotNull] this CALayer layer)
        {
            return layer.Frame.Height * 0.6f;
        }

        public static UIBezierPath GetBordersPath([NotNull] this LoadSwitchGradientLayer layer)
        {
            return UIBezierPath.FromRoundedRect(layer.GetClearFrame(), layer.CornerRadius);
        }

        public static nfloat GetBorderWidth([NotNull] this LoadSwitchGradientLayer layer)
        {
            return layer.GetClearFrame().Height * layer.LoadSwitch.LineWidthCoef;
        }

        public static void ToPathAnimate(
            [NotNull] this CAShapeLayer shapeLayer,
            CGPath newPath,
            float animateDuration)
        {
            var currentPath = (shapeLayer.PresentationLayer as CAShapeLayer)?.Path ?? shapeLayer.Path ?? newPath;
            shapeLayer.Path = currentPath;
            
            var anim = CABasicAnimation.FromKeyPath("path").NotNull();
            anim.Duration = animateDuration;
            anim.SetFrom(currentPath);
            anim.SetTo(newPath);

            shapeLayer.Path = newPath;

            shapeLayer.AddAnimation(anim, "path");
        }

        public static void ToFillColorAnimate(
            [NotNull] this CAShapeLayer shapeLayer,
            CGColor color,
            float animateDuration)
        {
            var currentColor = (shapeLayer.PresentationLayer as CAShapeLayer)?.FillColor ?? shapeLayer.FillColor ?? color;
            shapeLayer.FillColor = currentColor;
            
            var anim = CABasicAnimation.FromKeyPath("fillColor").NotNull();
            anim.Duration = animateDuration;
            anim.SetTo(color);
            anim.SetFrom(currentColor);

            shapeLayer.FillColor = color;

            shapeLayer.AddAnimation(anim, "fillColor");
        }

        public static void SetHiddenWithoutAnimation([NotNull] this CALayer layer, bool value)
        {
            var action = new Action(() => { layer.Hidden = value; });
            action.DoWithoutImplicitAnimation();
        }

        public static void SetColorsWithoutAnimation([NotNull] this CAGradientLayer layer, CGColor[] colors)
        {
            var action = new Action(() => { layer.Colors = colors; });
            action.DoWithoutImplicitAnimation();
        }
        
        public static void DoWithoutImplicitAnimation([NotNull] this Action action)
        {
            CATransaction.Begin();
            CATransaction.DisableActions = true;

            action?.Invoke();

            CATransaction.Commit();
        }
    }
}