using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Extensions;
using UIKit;

namespace LoadSwitch.Layers
{
    internal sealed class CenterGradientWithTransparentFloatingCircleGradientLayer : LoadSwitchWithMaskGradientLayer<CAShapeLayer>
    {
        public CenterGradientWithTransparentFloatingCircleGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
            MaskLayer.FillColor = ClearColor;
            MaskLayer.StrokeColor = ClearColor;
            MaskLayer.FillRule = CAShapeLayer.FillRuleEvenOdd;
        }

        protected override void InnerUpdate()
        {
            MaskLayer.LineWidth = this.GetBorderWidth();
        }

        public void SetState(LoadSwitchViewState state, bool animate)
        {
            switch (state)
            {
                default:
                case LoadSwitchViewState.Off:
                    UpdateWithState(this.GetOffStateCirclePath(), ClearColor, animate);
                    break;
                case LoadSwitchViewState.Load:
                    UpdateWithState(this.GetLoadStateCirclePath(), BlackColor, animate);
                    break;
                case LoadSwitchViewState.On:
                    UpdateWithState(this.GetOnStateCirclePath(), BlackColor, animate);
                    break;
            }
        }

        private void UpdateWithState(UIBezierPath path, CGColor fillColor, bool animate)
        {
            var borderPath = this.GetBordersPath();
            borderPath.AppendPath(path);
            borderPath.UsesEvenOddFillRule = true;
            
            if (animate)
            {
                MaskLayer.ToFillColorAnimate(fillColor, LoadSwitch.AnimateDuration);
                MaskLayer.ToPathAnimate(borderPath.CGPath, LoadSwitch.AnimateDuration);
            }
            else
            {
                MaskLayer.FillColor = fillColor;
                MaskLayer.Path = path.CGPath;
            }
        }
    }
}