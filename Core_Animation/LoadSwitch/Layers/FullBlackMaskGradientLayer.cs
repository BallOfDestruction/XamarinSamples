using JetBrains.Annotations;
using LoadSwitch.Extensions;
using UIKit;

namespace LoadSwitch.Layers
{
    internal sealed class FullBlackMaskGradientLayer : LoadSwitchGradientLayer
    {
        public FullBlackMaskGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
            BackgroundColor = UIColor.Black.NotNull().CGColor;
        }

        protected override void InnerUpdate()
        {
        }
    }
}