using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal class OffImageMaskGradientLayer : LoadSwitchGradientLayer
    {
        public OffImageMaskGradientLayer([NotNull] LoadSwitch view) : base(view)
        {
        }

        protected override void InnerUpdate()
        {
            var offFrame = this.GetOffStateCircleRect();
            offFrame = offFrame.CutOffRect();
            var offImage = GraphicsHelper.GetRevertImage(LoadSwitch.OffImage, this.GetClearFrame(), offFrame);
            Contents = offImage?.CGImage;
        }
    }
}