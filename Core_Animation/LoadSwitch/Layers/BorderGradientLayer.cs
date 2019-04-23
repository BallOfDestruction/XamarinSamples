using CoreAnimation;
using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal sealed class BorderGradientLayer : LoadSwitchWithMaskGradientLayer<CAShapeLayer>
    {
        public BorderGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
            MaskLayer.FillColor = ClearColor;
            MaskLayer.StrokeColor = BlackColor;
        }

        protected override void InnerUpdate()
        {
            MaskLayer.Path = this.GetBordersPath()?.CGPath;
            MaskLayer.LineWidth = this.GetBorderWidth();
        }
    }
}