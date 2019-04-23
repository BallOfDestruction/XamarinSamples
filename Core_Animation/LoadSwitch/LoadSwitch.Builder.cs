using System;
using System.Threading.Tasks;
using CoreGraphics;
using JetBrains.Annotations;
using LoadSwitch.Extensions;
using UIKit;

namespace LoadSwitch
{
    public sealed partial class LoadSwitch
    {
        [NotNull] private string _offText;
        [NotNull] private UIImage _onImage;
        [NotNull] private UIImage _loadImage;
        [NotNull] private string _onText;
        [NotNull] private UIImage _offImage;
        [NotNull] private UIFont _font = UIFont.SystemFontOfSize(50).NotNull();
        private float _width;
        private float _height;

        public class Builder
        {
            [NotNull] private readonly LoadSwitch _loadSwitch;

            public Builder()
            {
                _loadSwitch = new LoadSwitch();
            }

            [NotNull]
            public Builder SetAllDefault()
            {
                SetOffGradientColors(new[] { UIColor.Blue?.CGColor, UIColor.Green?.CGColor });
                SetLoadGradientColors(new[] { UIColor.Green?.CGColor, UIColor.Blue?.CGColor });
                SetOnGradientColors(new[] { UIColor.Green?.CGColor, UIColor.Red?.CGColor });
                SetHeight(100);
                SetWidth(180);
                SetOnText("On");
                SetOffText("Off");
                SetAnimateDuration(0.4f);
                SetFont(UIFont.FromName("Helvetica-bold", 45).NotNull());
                SetTurnOnAction(DelayAction);
                SetTurnOffAction(DelayAction);

                return this;
            }

            [NotNull]
            private Task<bool> DelayAction()
            {
                return Task.Run(
                    async () =>
                    {
                        await Task.Delay(3000);
                        return await Task.FromResult(true);
                    });
            }

            [NotNull]
            public Builder SetOffGradientColors(CGColor[] gradientColors)
            {
                _loadSwitch.OffGradientColors = gradientColors ?? new CGColor[0];

                return this;
            }

            [NotNull]
            public Builder SetLoadGradientColors(CGColor[] gradientColors)
            {
                _loadSwitch.LoadGradientColors = gradientColors ?? new CGColor[0];

                return this;
            }

            [NotNull]
            public Builder SetOnGradientColors(CGColor[] gradientColors)
            {
                _loadSwitch.OnGradientColors = gradientColors ?? new CGColor[0];

                return this;
            }

            [NotNull]
            public Builder SetHeight(float height)
            {
                _loadSwitch._height = height;

                return this;
            }

            [NotNull]
            public Builder SetWidth(float width)
            {
                _loadSwitch._width = width;

                return this;
            }

            [NotNull]
            public Builder SetOnText(string text)
            {
                _loadSwitch._onText = text ?? "";

                return this;
            }

            [NotNull]
            public Builder SetOnImage(UIImage image)
            {
                _loadSwitch._onImage = image ?? new UIImage();

                return this;
            }

            [NotNull]
            public Builder SetLoadImage(UIImage loadImage)
            {
                _loadSwitch._loadImage = loadImage ?? new UIImage();

                return this;
            }

            [NotNull]
            public Builder SetOffText(string text)
            {
                _loadSwitch._offText = text ?? "";

                return this;
            }

            [NotNull]
            public Builder SetFont([NotNull] UIFont font)
            {
                _loadSwitch._font = font;

                return this;
            }

            [NotNull]
            public Builder SetOffImage(UIImage image)
            {
                _loadSwitch._offImage = image ?? new UIImage();

                return this;
            }

            [NotNull]
            public Builder SetAnimateDuration(float animateDuration)
            {
                _loadSwitch.AnimateDuration = animateDuration;

                return this;
            }

            [NotNull]
            public Builder SetTurnOnAction(Func<Task<bool>> action)
            {
                _loadSwitch.TurnOnAction = action ?? (() => Task.FromResult(false));

                return this;
            }
            
            [NotNull]
            public Builder SetTurnOffAction(Func<Task<bool>> action)
            {
                _loadSwitch.TurnOffAction = action ?? (() => Task.FromResult(false));

                return this;
            }

            [NotNull]
            public LoadSwitch Build()
            {
                _loadSwitch.LoadView();

                return _loadSwitch;
            }
        }

        [NotNull]
        public CGColor[] OffGradientColors { get; private set; }

        [NotNull]
        public CGColor[] LoadGradientColors { get; private set; }

        [NotNull]
        public CGColor[] OnGradientColors { get; private set; }

        [NotNull]
        public UIImage OnImage
        {
            get => _onImage;
            set
            {
                _onImage = value;
                _onImageGradientLayer.Update();
            }
        }

        [NotNull]
        public UIImage LoadImage
        {
            get => _loadImage;
            set
            {
                _loadImage = value;
                _loadImageGradientLayer.Update();
            }
        }

        [NotNull]
        public string OnText
        {
            get => _onText;
            set
            {
                _onText = value;
                _loadSwitchViewLayerMask.Update();
            }
        }

        [NotNull]
        public UIImage OffImage
        {
            get => _offImage;
            set
            {
                _offImage = value;
                _loadSwitchViewLayerMask.Update();
            }
        }

        [NotNull]
        public string OffText
        {
            get => _offText;
            set
            {
                _offText = value;
                _offTextGradientLayer.Update();
            }
        }

        [NotNull]
        public UIFont Font
        {
            get => _font;
            set
            {
                _font = value;
                _offTextGradientLayer.Update();
                _loadSwitchViewLayerMask.Update();
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                _widthConstraint.Constant = value;
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                _heightConstraint.Constant = value;
            }
        }

        public float AnimateDuration { get; private set; }

        [NotNull]
        public Func<Task<bool>> TurnOnAction { get; set; }
        
        [NotNull]
        public Func<Task<bool>> TurnOffAction { get; set; }
    }
}
