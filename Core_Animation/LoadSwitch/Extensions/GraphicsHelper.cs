using CoreAnimation;
using CoreGraphics;
using CoreText;
using Foundation;
using JetBrains.Annotations;
using UIKit;

namespace LoadSwitch.Extensions
{
    internal static class GraphicsHelper
    {
        [CanBeNull]
        public static UIImage GetRevertImage([NotNull] UIImage image, CGRect rect, CGRect cropFrame)
        {
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 1);
            UIColor.Black?.SetColor();
            UIGraphics.RectFill(rect);
            image.Draw(cropFrame, CGBlendMode.DestinationOut, 1);
            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }

        [CanBeNull]
        public static UIImage CreateTextImage([NotNull] this CALayer layer, CGRect rect, string text, [NotNull] UIFont font)
        {
            UIGraphics.BeginImageContextWithOptions(layer.GetClearFrame().Size, false, 1);
            // TODO рисаовть вертикально по центру
            var attributedString = new NSAttributedString(
                text,
                new CTStringAttributes()
                {
                    ForegroundColor = UIColor.Black?.CGColor,
                    Font = new CTFont(font.Name, font.PointSize),
                });
            attributedString.DrawString(rect);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }

        [CanBeNull]
        public static UIImage CreateInverseTextImage([NotNull] this CALayer layer, CGRect rect, string text, [NotNull] UIFont font)
        {
            var clearFrame = layer.GetClearFrame();
            // TODO рисаовть вертикально по центру
            UIGraphics.BeginImageContextWithOptions(clearFrame.Size, false, 1);
            var attributedString = new NSAttributedString(
                text,
                new CTStringAttributes()
                {
                    ForegroundColor = UIColor.Black?.CGColor,
                    Font = new CTFont(font.Name, font.PointSize),
                });
            attributedString.DrawString(rect);

            UIColor.Black?.SetColor();
            UIGraphics.RectFillUsingBlendMode(clearFrame, CGBlendMode.SourceOut);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}