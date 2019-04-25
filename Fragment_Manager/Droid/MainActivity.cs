using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private LinearLayout _container;
        private FragmentNavigation _fragmentNavigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _container = FindViewById<LinearLayout>(Resource.Id.container);
            _fragmentNavigation = new FragmentNavigation(this, _container);

            _fragmentNavigation.GoTo(new AnimatableFragment(_fragmentNavigation), AnimationType.None, AnimationType.None);

            SupportFragmentManager.BackStackChanged += (sender, args) =>
            {
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(SupportFragmentManager.BackStackEntryCount > 1);

                Title = $" [{SupportFragmentManager.BackStackEntryCount}] ";
            };
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}