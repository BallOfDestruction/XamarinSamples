using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal abstract class LoadSwitchGradientLayer : CAGradientLayer
    {
        [NotNull]
        public LoadSwitch LoadSwitch { get; }

        protected LoadSwitchGradientLayer([NotNull] LoadSwitch loadSwitch)
        {
            LoadSwitch = loadSwitch;
            MasksToBounds = true;
            StartPoint = new CGPoint(0, 0.5f);
            EndPoint = new CGPoint(1, 0.5f);
        }
        
        public void Update()
        {
            if (SuperLayer != null)
            {
                var frame = SuperLayer.GetClearFrame();

                var cornerRadius = frame.Height / 2f;

                Frame = frame;
                CornerRadius = cornerRadius;

                if (Mask != null)
                    Mask.Frame = frame;

                InnerUpdate();
            }
        }

        public virtual void ToSetGradientColors(LoadSwitchViewState state, bool animate)
        {
            switch (state)
            {
                default:
                case LoadSwitchViewState.Off:
                    SetGradientColors(LoadSwitch.OffGradientColors, animate);
                    break;
                case LoadSwitchViewState.Load:
                    SetGradientColors(LoadSwitch.LoadGradientColors, animate);
                    break;
                case LoadSwitchViewState.On:
                    SetGradientColors(LoadSwitch.OnGradientColors, animate);
                    break;
            }
        }

        private void SetGradientColors(CGColor[] colors, bool animate)
        {
            if (animate)
            {
                // Implicit animation
                Colors = colors;
            }
            else
            {
                // without implicit animation
                this.SetColorsWithoutAnimation(colors);
            }
        }
        
        protected abstract void InnerUpdate();
    }
}