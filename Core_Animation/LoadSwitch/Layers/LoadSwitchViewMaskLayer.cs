using JetBrains.Annotations;
using LoadSwitch.Extensions;

namespace LoadSwitch.Layers
{
    internal sealed class LoadSwitchViewMaskLayer : LoadSwitchGradientLayer
    {
        [NotNull] private readonly OffImageMaskGradientLayer _offImageMaskGradientLayer;
        [NotNull] private readonly FullBlackMaskGradientLayer _fullBlackMaskGradientLayer;
        [NotNull] private readonly OnTextMaskLayer _onTextMaskLayer;

        public LoadSwitchViewMaskLayer([NotNull] LoadSwitch customView) : base(customView)
        {
            _fullBlackMaskGradientLayer = new FullBlackMaskGradientLayer(customView);
            AddSublayer(_fullBlackMaskGradientLayer);

            _offImageMaskGradientLayer = new OffImageMaskGradientLayer(customView);
            AddSublayer(_offImageMaskGradientLayer);

            _onTextMaskLayer = new OnTextMaskLayer(customView);
            AddSublayer(_onTextMaskLayer);
        }

        protected override void InnerUpdate()
        {
            _fullBlackMaskGradientLayer.Update();

            _offImageMaskGradientLayer.Update();

            _onTextMaskLayer.Update();
        }

        public void SetMaskState(LoadSwitchViewState state, bool animate)
        {
            switch (state)
            {
                default:
                case LoadSwitchViewState.Off:
                    _fullBlackMaskGradientLayer.SetHiddenWithoutAnimation(true);
                    _onTextMaskLayer.SetHiddenWithoutAnimation(true);
                    _offImageMaskGradientLayer.SetHiddenWithoutAnimation(false);
                    break;
                case LoadSwitchViewState.Load:
                    _fullBlackMaskGradientLayer.SetHiddenWithoutAnimation(false);
                    _onTextMaskLayer.SetHiddenWithoutAnimation(true);
                    _offImageMaskGradientLayer.SetHiddenWithoutAnimation(true);
                    break;
                case LoadSwitchViewState.On:
                    _fullBlackMaskGradientLayer.SetHiddenWithoutAnimation(true);
                    _onTextMaskLayer.SetHiddenWithoutAnimation(false);
                    _offImageMaskGradientLayer.SetHiddenWithoutAnimation(true);
                    break;
            }
        }
        
        public override void ToSetGradientColors(LoadSwitchViewState state, bool animate)
        {
        }
    }
}