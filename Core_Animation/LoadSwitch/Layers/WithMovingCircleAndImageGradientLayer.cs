using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Extensions;
using UIKit;

namespace LoadSwitch.Layers
{
    internal sealed class MovingCircleGradientLayer : LoadSwitchWithMaskGradientLayer<CAShapeLayer>
    {
        public MovingCircleGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
            MaskLayer.FillColor = BlackColor;
            MaskLayer.StrokeColor = ClearColor;
        }
        
        protected override void InnerUpdate()
        {
        }

        public void SetState(LoadSwitchViewState state, bool animate)
        {
            switch (state)
            {
                default:
                case LoadSwitchViewState.Off:
                    UpdateWithState(this.GetOffStateCirclePath(), BlackColor, animate);
                    break;
                case LoadSwitchViewState.Load:
                    UpdateWithState(this.GetLoadStateCirclePath(), ClearColor, animate);
                    break;
                case LoadSwitchViewState.On:
                    UpdateWithState(this.GetOnStateCirclePath(), ClearColor, animate);
                    break;
            }
        }

        private void UpdateWithState([NotNull] UIBezierPath path, [NotNull] CGColor color, bool animate)
        {
            if (animate)
            {
                MaskLayer.ToFillColorAnimate(color, LoadSwitch.AnimateDuration);
                MaskLayer.ToPathAnimate(path.CGPath, LoadSwitch.AnimateDuration);
            }
            else
            {
                MaskLayer.FillColor = color;
                MaskLayer.Path = path.CGPath;
            }
        }
    }
}