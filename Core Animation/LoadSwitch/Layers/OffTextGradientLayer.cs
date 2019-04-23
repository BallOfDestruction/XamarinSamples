using CoreAnimation;
using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal class OffTextGradientLayer : LoadSwitchWithMaskGradientLayer<CALayer>
    {
        public OffTextGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
        }

        protected override void InnerUpdate()
        {
            var image = this.CreateTextImage(this.GetOffStateRestPlace(), LoadSwitch.OffText, LoadSwitch.Font);
            MaskLayer.Contents = image?.CGImage;
        }
    }
}