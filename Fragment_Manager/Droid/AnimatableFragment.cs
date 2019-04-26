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
            
            linearLayout.Orientation = Orientation.Horizontal;

            AddButton(linearLayout, () => Activity.OnBackPressed(), "Go previous");
            AddButton(linearLayout, () => _fragmentNavigation.GoTo(new AnimatableFragment(_fragmentNavigation)), "Go next");

            return linearLayout;
        }

        private void AddButton(LinearLayout linearLayout, Action action, string title)
        {
            var button = new Button(Activity)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    160){Gravity = GravityFlags.CenterHorizontal}
            };

            button.Text = title;
            button.Click += (sender, args) =>
            {
                action.Invoke();
            };
            linearLayout.AddView(button);
        }
    }
}