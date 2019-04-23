using CoreAnimation;
using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal class OnImageGradientLayer : LoadSwitchWithMaskGradientLayer<CAShapeLayer>
    {
        public OnImageGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
        }

        protected override void InnerUpdate()
        {
            var rightRect = this.GetOnStateCircleRect();
            rightRect = rightRect.CutOffRect();
            var rightImage = LoadSwitch.OnImage;
            MaskLayer.Frame = rightRect;
            MaskLayer.Contents = rightImage.CGImage;
        }
    }
}