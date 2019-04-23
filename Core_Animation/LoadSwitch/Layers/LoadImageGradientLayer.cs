using System;
using CoreAnimation;
using Foundation;
using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal class LoadImageGradientLayer : LoadSwitchWithMaskGradientLayer<CALayer>
    {
        private const string AnimationRotateKey = "AnimationRotateKey"; 
        
        public LoadImageGradientLayer([NotNull] LoadSwitch loadSwitch) : base(loadSwitch)
        {
        }

        protected override void InnerUpdate()
        {
            var centerRect = this.GetLoadStateCircleRect();
            centerRect = centerRect.CutOffRect();
            var centerImage = LoadSwitch.LoadImage;
            
            MaskLayer.Frame = centerRect;
            MaskLayer.Contents = centerImage?.CGImage;
        }
        
        public void StartRotate()
        {
            var animation = CABasicAnimation.FromKeyPath("transform.rotation.z").NotNull();
            animation.To = NSNumber.FromFloat(0);
            animation.From = NSNumber.FromFloat((float)Math.PI * 2);
            animation.Duration = LoadSwitch.AnimateDuration;
            animation.Cumulative = true;
            animation.RepeatCount = float.PositiveInfinity;
            
            MaskLayer.AddAnimation(animation, AnimationRotateKey);
        }

        public void StopRotate()
        {
            MaskLayer.RemoveAnimation(AnimationRotateKey);
        }
    }
}