using System;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace Droid
{
    public class AnimatableFragment : Fragment
    {
        private readonly FragmentNavigation _fragmentNavigation;

        public AnimatableFragment(FragmentNavigation fragmentNavigation)
        {
            _fragmentNavigation = fragmentNavigation;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var random = new Random((int) DateTime.Now.Ticks);

            var linearLayout = new LinearLayout(Activity)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent),
                Clickable = true,
            };

            linearLayout.SetBackgroundColor(
                new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
            
            linearLayout.Orientation = Orientation.Vertical;

            AddButton(linearLayout, AnimationType.None, AnimationType.None);
            AddButton(linearLayout, AnimationType.Fade, AnimationType.Fade);
            AddButton(linearLayout, AnimationType.Explode, AnimationType.Explode);
            AddButton(linearLayout, AnimationType.Top, AnimationType.Top);
            AddButton(linearLayout, AnimationType.Top, AnimationType.Bottom);
            AddButton(linearLayout, AnimationType.Right, AnimationType.Left);
            AddButton(linearLayout, AnimationType.Left, AnimationType.Right);

            return linearLayout;
        }

        private void AddButton(LinearLayout linearLayout, AnimationType enter, AnimationType exit)
        {
            var button = new Button(Activity)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    160){Gravity = GravityFlags.CenterHorizontal}
            };

            button.Text = $"Enter: {enter} Exit: {exit}";
            button.Click += (sender, args) =>
            {
                _fragmentNavigation.GoTo(new AnimatableFragment(_fragmentNavigation), enter, exit);
            };
            linearLayout.AddView(button);
        }
    }
}