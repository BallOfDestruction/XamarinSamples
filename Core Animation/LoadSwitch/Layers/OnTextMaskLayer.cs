using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal class OnTextMaskLayer : LoadSwitchGradientLayer
    {
        public OnTextMaskLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
        }
        
        protected override void InnerUpdate()
        {
            var image = this.CreateInverseTextImage(this.GetOnStateRestPlace(), LoadSwitch.OnText, LoadSwitch.Font);
            Contents = image?.CGImage;
        }
    }
}