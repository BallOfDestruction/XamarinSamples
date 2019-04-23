using System;
using System.Threading.Tasks;
using PassKit;
using UIKit;

namespace LoadSwitch.Example
{
    public class SampleViewController : UIViewController
    {
        private LoadSwitch _switch;
        private UIButton _backButton;
        private UIButton _sizeButton;
        private UITextView _logTextView;
        private UIButton _badGayButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var builder = new LoadSwitch.Builder().SetAllDefault();
            builder.SetLoadImage(UIImage.FromBundle("Load"))
                .SetOffImage(UIImage.FromBundle("Turn"))
                .SetOnImage(UIImage.FromBundle("Turn"))
                .SetTurnOnAction(async () =>
                {
                    await Task.Delay(300);

                    return false;
                });

            _switch = builder.Build();
            
            View.AddSubview(_switch);

            _switch.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _switch.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;

            InitBackButton();

            InitSizeButton();
            
            InitLogLabel();

            InitCustomActionButton();
        }

        private void InitCustomActionButton()
        {
            _badGayButton = UIButton.FromType(UIButtonType.System);
            _badGayButton.SetTitleColor(UIColor.Red, UIControlState.Normal);
            _badGayButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _badGayButton.SetTitle("Do bad thing", UIControlState.Normal);
            _badGayButton.AddGestureRecognizer(
                new UITapGestureRecognizer(
                    () =>
                    {            
                        // DO something bad
                        _switch.SetChecked(true, false);
                    }));

            View.AddSubview(_badGayButton);

            _badGayButton.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 20).Active = true;
            _badGayButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -100).Active = true;
            _badGayButton.HeightAnchor.ConstraintEqualTo(50).Active = true;
            _badGayButton.WidthAnchor.ConstraintEqualTo(120).Active = true;
        }

        public void InitBackButton()
        {
            var random = new Random();

            _backButton = UIButton.FromType(UIButtonType.System);
            _backButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _backButton.SetTitle("Change back", UIControlState.Normal);
            _backButton.AddGestureRecognizer(
                new UITapGestureRecognizer(
                    () =>
                    {
                        View.BackgroundColor = UIColor.FromRGB(random.Next(255), random.Next(255), random.Next(255));
                    }));

            View.AddSubview(_backButton);

            _backButton.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 20).Active = true;
            _backButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -40).Active = true;
            _backButton.HeightAnchor.ConstraintEqualTo(50).Active = true;
            _backButton.WidthAnchor.ConstraintEqualTo(120).Active = true;
        }
        
        public void InitSizeButton()
        {
            var random = new Random();

            _sizeButton = UIButton.FromType(UIButtonType.System);
            _sizeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _sizeButton.SetTitle("Change size", UIControlState.Normal);
            _sizeButton.AddGestureRecognizer(
                new UITapGestureRecognizer(
                    () =>
                    {
                        var value = random.Next(100, 400);
                        _switch.Height = value;
                        _switch.Width =value * 1.8f;
                    }));

            View.AddSubview(_sizeButton);

            _sizeButton.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 140).Active = true;
            _sizeButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -40).Active = true;
            _sizeButton.HeightAnchor.ConstraintEqualTo(50).Active = true;
            _sizeButton.WidthAnchor.ConstraintEqualTo(120).Active = true;
        }

        public void InitLogLabel()
        {
            _logTextView = new UITextView
            {
                TranslatesAutoresizingMaskIntoConstraints = false, Text = "logs\n", TextColor = UIColor.White,
                BackgroundColor = UIColor.Black,
                Editable = false
            };
            
            _switch.LoadStarted += (sender, args) => _logTextView.Text += "LoadStarted\n";
            _switch.LoadEnded += (sender, args) => _logTextView.Text += $"LoadEnded\n";
            _switch.Loaded += (sender, args) => _logTextView.Text += $"Loaded: {args}\n";
            _switch.ValueChanged += (sender, args) => _logTextView.Text += $"ValueChanged: {args}\n";

            View.AddSubview(_logTextView);

            _logTextView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 260).Active = true;
            _logTextView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -40).Active = true;
            _logTextView.HeightAnchor.ConstraintEqualTo(100).Active = true;
            _logTextView.WidthAnchor.ConstraintEqualTo(150).Active = true;
        }
    }
}