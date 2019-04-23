using CoreAnimation;
using CoreGraphics;
using JetBrains.Annotations;

namespace LoadSwitch.Extensions
{
    internal static class FrameExtension
    {
        public static CGRect GetClearFrame([NotNull] this CALayer layer)
        {
            var frame = layer.Frame;
            frame.X = 0;
            frame.Y = 0;

            return frame;
        }
        
        public static CGRect CutOffRect(this CGRect frame)
        {
            var valueCut = frame.Height * 0.15f;
            frame.X += valueCut;
            frame.Y += valueCut;
            frame.Height -= valueCut * 2;
            frame.Width -= valueCut * 2;

            return frame;
        }
    }
}