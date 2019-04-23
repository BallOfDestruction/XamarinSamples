using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Extensions;
using UIKit;

namespace LoadSwitch.Layers
{
    internal abstract class LoadSwitchWithMaskGradientLayer<TMaskLayer> : LoadSwitchGradientLayer
        where TMaskLayer : CALayer, new()
    {
        protected LoadSwitchWithMaskGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
            MaskLayer = new TMaskLayer();
            Mask = MaskLayer;
        }

        [NotNull]
        public TMaskLayer MaskLayer { get; }
        
        [NotNull]
        public CGColor ClearColor => UIColor.Clear.NotNull().CGColor.NotNull();

        [NotNull]
        public CGColor BlackColor => UIColor.Black.NotNull().CGColor.NotNull();
    }
}

