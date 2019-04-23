using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreAnimation;
using JetBrains.Annotations;
using LoadSwitch.Extensions;
using LoadSwitch.Layers;
using UIKit;

namespace LoadSwitch
{
    public sealed partial class LoadSwitch : UIView
    {
        public float LineWidthCoef { get; } = 0.18f; 

        public event EventHandler LoadStarted;
        public event EventHandler LoadEnded;
        public event EventHandler<bool> Loaded;
        public event EventHandler<bool> ValueChanged;
        
        [NotNull] private NSLayoutConstraint _heightConstraint;
        [NotNull] private NSLayoutConstraint _widthConstraint;

        [NotNull] private readonly CenterGradientWithTransparentFloatingCircleGradientLayer _centerGradientWithTransparentFloatingCircleGradientLayer;
        [NotNull] private readonly MovingCircleGradientLayer _movingCircleGradientLayer;
        [NotNull] private readonly LoadSwitchViewMaskLayer _loadSwitchViewLayerMask;
        [NotNull] private readonly LoadImageGradientLayer _loadImageGradientLayer;
        [NotNull] private readonly OnImageGradientLayer _onImageGradientLayer;
        [NotNull] private readonly OffTextGradientLayer _offTextGradientLayer;
        [NotNull] private readonly List<LoadSwitchGradientLayer> _allCustomGradientLayers;

        public LoadSwitch()
        {
            var borderGradientLayer = new BorderGradientLayer(this);
            Layer.NotNull().AddSublayer(borderGradientLayer);

            _centerGradientWithTransparentFloatingCircleGradientLayer = new CenterGradientWithTransparentFloatingCircleGradientLayer(this);
            Layer.AddSublayer(_centerGradientWithTransparentFloatingCircleGradientLayer);

            _movingCircleGradientLayer = new MovingCircleGradientLayer(this);
            Layer.AddSublayer(_movingCircleGradientLayer);

            _offTextGradientLayer = new OffTextGradientLayer(this);
            Layer.AddSublayer(_offTextGradientLayer);
            
            _loadImageGradientLayer = new LoadImageGradientLayer(this);
            Layer.AddSublayer(_loadImageGradientLayer);

            _onImageGradientLayer = new OnImageGradientLayer(this);
            Layer.AddSublayer(_onImageGradientLayer);
            
            _loadSwitchViewLayerMask = new LoadSwitchViewMaskLayer(this);
            Layer.Mask = _loadSwitchViewLayerMask;
            
            _allCustomGradientLayers = new List<LoadSwitchGradientLayer>()
            {
                borderGradientLayer,
                _centerGradientWithTransparentFloatingCircleGradientLayer,
                _movingCircleGradientLayer,
                _offTextGradientLayer,
                _loadImageGradientLayer,
                _onImageGradientLayer,
                _loadSwitchViewLayerMask
            };
        }
        
        public LoadSwitchViewState State { get; private set; } = LoadSwitchViewState.Off;
        
        public bool IsChecked { get; private set; }
        
        public void SetChecked(bool value, bool animate)
        {
            IsChecked = value;
            UpdateViewState(value ? LoadSwitchViewState.Off : LoadSwitchViewState.On, animate);
        }
        
        public override void LayoutSublayersOfLayer(CALayer layer)
        {
            base.LayoutSublayersOfLayer(layer);

            Layer.NotNull().CornerRadius = Layer.Frame.Height / 2;

            _allCustomGradientLayers.ForEach(w => w.NotNull().Update());
            
            UpdateViewState(State, true);
        }
        
        private void LoadView()
        {
            InitHimself();

            InitGestures();

            UpdateViewState(LoadSwitchViewState.Off, false);
            
            LayoutSublayersOfLayer(Layer);
        }

        private void InitHimself()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            
            _heightConstraint = HeightAnchor.NotNull().ConstraintEqualTo(Height).NotNull();
            _widthConstraint = WidthAnchor.NotNull().ConstraintEqualTo(Width).NotNull();

            _heightConstraint.Active = true;
            _widthConstraint.Active = true;

            BackgroundColor = UIColor.Clear;
            Opaque = false;
            ClipsToBounds = true;
            Layer.NotNull().MasksToBounds = true;

            Layer.Delegate = this;
        }

        private void InitGestures()
        {
            AddGestureRecognizer(new UITapGestureRecognizer(async () => { await HandleTap(); }));
            
            AddGestureRecognizer(new UIPanGestureRecognizer(HandlePanGesture));
        }

        private async Task HandleTap()
        {
            switch (State)
            {
                case LoadSwitchViewState.Off:
                    await ExecuteTurnOn();
                    break;
                default:
                case LoadSwitchViewState.Load:
                    break;
                case LoadSwitchViewState.On:
                    await ExecuteTurnOff();
                    break;
            }
        }
        
        private void HandlePanGesture(UIPanGestureRecognizer obj)
        {
        }

        private async Task ExecuteTurnOn()
        {
            UpdateViewState(LoadSwitchViewState.Load, true);
            
            LoadStarted?.Invoke(this, null);
            
            var isNextOn = await TurnOnAction().NotNull();
            
            LoadEnded?.Invoke(this, null);
            Loaded?.Invoke(this, isNextOn);
            
            UpdateIsCheckedIfNeed(isNextOn);
            UpdateViewState(isNextOn ? LoadSwitchViewState.On : LoadSwitchViewState.Off, true);
        }

        private async Task ExecuteTurnOff()
        {
            UpdateViewState(LoadSwitchViewState.Load, true);
            
            LoadStarted?.Invoke(this, null);
            
            var isNextOff = await TurnOffAction().NotNull();
            
            LoadEnded?.Invoke(this, null);
            Loaded?.Invoke(this, isNextOff);
            
            UpdateIsCheckedIfNeed(!isNextOff);
            UpdateViewState(isNextOff ? LoadSwitchViewState.Off : LoadSwitchViewState.On, true);
        }

        private void UpdateIsCheckedIfNeed(bool value)
        {
            if (IsChecked != value)
            {
                IsChecked = value;
                ValueChanged?.Invoke(this, IsChecked);
            }
        }

        private void UpdateViewState(LoadSwitchViewState state, bool animate)
        {
            _movingCircleGradientLayer.SetState(state, animate);
            _centerGradientWithTransparentFloatingCircleGradientLayer.SetState(state, animate);
            _loadSwitchViewLayerMask.SetMaskState(state, animate);

            switch (state)
            {
                default:
                case LoadSwitchViewState.Off:
                    _loadImageGradientLayer.SetHiddenWithoutAnimation(true);
                    _onImageGradientLayer.SetHiddenWithoutAnimation(true);
                    _offTextGradientLayer.SetHiddenWithoutAnimation(false);
                    _loadImageGradientLayer.StopRotate();
                    break;
                case LoadSwitchViewState.Load:
                    _loadImageGradientLayer.SetHiddenWithoutAnimation(false);
                    _onImageGradientLayer.SetHiddenWithoutAnimation(true);
                    _offTextGradientLayer.SetHiddenWithoutAnimation(true);
                    _loadImageGradientLayer.StartRotate();
                    break;
                case LoadSwitchViewState.On:
                    _loadImageGradientLayer.SetHiddenWithoutAnimation(true);
                    _onImageGradientLayer.SetHiddenWithoutAnimation(false);
                    _offTextGradientLayer.SetHiddenWithoutAnimation(true);
                    _loadImageGradientLayer.StopRotate();
                    break;
            }

            _allCustomGradientLayers.ForEach(w => w.NotNull().ToSetGradientColors(state, animate));

            State = state;
        }
    }
}
