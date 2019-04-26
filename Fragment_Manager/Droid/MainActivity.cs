using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
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

            _fragmentNavigation.GoTo(new AnimatableFragment(_fragmentNavigation));

            SupportFragmentManager.BackStackChanged += (sender, args) =>
            {
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(SupportFragmentManager.BackStackEntryCount > 1);

                Title = $" [{SupportFragmentManager.BackStackEntryCount}] ";
            };

            InitRadio();

            InitEnterSpinner();

            InitExitSpinner();

            InitSpeedSpinner();
        }
    
        private void InitEnterSpinner()
        {
            var enterSpinner = FindViewById<AppCompatSpinner>(Resource.Id.enterSpinner);

            var types = AnimationTypeExtension.GetAnimationTypes();
            
            enterSpinner.SetAdapter(new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, types));

            enterSpinner.ItemSelected += (sender, args) => { _fragmentNavigation.Enter = types[args.Position]; };
            
            enterSpinner.SetSelection(types.IndexOf(_fragmentNavigation.Enter));
        }
        
        private void InitSpeedSpinner()
        {
            var speedSpinner = FindViewById<AppCompatSpinner>(Resource.Id.speedSpinner);

            var speeds = AnimationSpeedExtension.GetAnimationSpeeds();
            
            speedSpinner.SetAdapter(new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, speeds));

            speedSpinner.ItemSelected += (sender, args) => { _fragmentNavigation.Speed = speeds[args.Position]; };
            
            speedSpinner.SetSelection(speeds.IndexOf(_fragmentNavigation.Speed));
        }
        
        private void InitExitSpinner()
        {
            var exitSpinner = FindViewById<AppCompatSpinner>(Resource.Id.exitSpinner);
            
            var types = AnimationTypeExtension.GetAnimationTypes();
            
            exitSpinner.SetAdapter(new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, types));

            exitSpinner.ItemSelected += (sender, args) => { _fragmentNavigation.Exit = types[args.Position]; };
            
            exitSpinner.SetSelection(types.IndexOf(_fragmentNavigation.Exit));
        }

        private void InitRadio()
        {
            var group = FindViewById<RadioGroup>(Resource.Id.radioGroup);

            group.CheckedChange += (sender, args) =>
            {
                if (args.CheckedId == Resource.Id.radioButtonDelay)
                {
                    _fragmentNavigation.IsDelay = true;
                }
                else
                {
                    _fragmentNavigation.IsDelay = false;
                }
            };
            
            group.Check(_fragmentNavigation.IsDelay ? Resource.Id.radioButtonDelay : Resource.Id.radioButtonWithoutDelay);
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}