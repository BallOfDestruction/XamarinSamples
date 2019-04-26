using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Android.Icu.Text;
using Android.Support.Transitions;
using Android.Support.V4.App;
using Android.Views;

namespace Droid
{
    public class FragmentNavigation
    {
        public AnimationType Enter { get; set; } = AnimationType.Fade;
        public AnimationType Exit { get; set; } = AnimationType.Fade;
        public AnimationSpeed Speed { get; set; } = AnimationSpeed.X05;
        public bool IsDelay { get; set; } = true;
        
        private readonly FragmentActivity _fragmentActivity;
        private readonly ViewGroup _container;

        public FragmentNavigation(FragmentActivity fragmentActivity, ViewGroup container)
        {
            _fragmentActivity = fragmentActivity;
            _container = container;
        }

        public void GoTo(Fragment fragment)
        {
            var previousFragment = _fragmentActivity.SupportFragmentManager.Fragments?.LastOrDefault();

            SetAnimation(fragment, previousFragment);

            _fragmentActivity.SupportFragmentManager.BeginTransaction()
                .Replace(_container.Id, fragment)
                .AddToBackStack(null)
                .Commit();
        }

        private void SetAnimation(Fragment fragment,
            Fragment previousFragment
        )
        {
            CommonAnimate(fragment,
                previousFragment,
                Speed.GetCalculationAnimationTime(),
                Enter.ToVisibility(),
                Exit.ToVisibility(),
                Enter.ToVisibility(),
                Exit.ToVisibility());
        }

        private void CommonAnimate(
            Fragment fragment,
            Fragment previousFragment,
            int duration,
            Visibility enterEnterFragment,
            Visibility exitEnterFragment,
            Visibility enterPopFragment,
            Visibility exitPopFragment)
        {
            if (previousFragment != null)
            {
                SetRightFragmentAnimation(previousFragment, enterPopFragment, exitPopFragment, duration);
            }

            SetRightFragmentAnimation(fragment, enterEnterFragment, exitEnterFragment, duration);
            
            if (enterEnterFragment != null)
            {
                var listener = new TransitionListener(() =>
                {
                    SetReverseFragmentAnimation(fragment, enterPopFragment, exitPopFragment, duration);
                });

                enterEnterFragment.AddListener(listener);
                exitEnterFragment.AddListener(listener);
            }
        }

        private void SetRightFragmentAnimation(Fragment fragment,
            Visibility enterVisibility,
            Visibility exitVisibility,
            long duration)
        {
            if (enterVisibility != null)
            {
                enterVisibility.SetDuration(duration);
                if(IsDelay)
                    enterVisibility.SetStartDelay(duration);
                fragment.EnterTransition = enterVisibility;
            }
            else
            {
                fragment.EnterTransition = null;
            }

            if (exitVisibility != null)
            {
                exitVisibility.SetDuration(duration);
                if(IsDelay)
                    exitVisibility.SetStartDelay(0);
                fragment.ExitTransition = exitVisibility;
            }
            else
            {
                fragment.ExitTransition = null;
            }
        }
        
        private void SetReverseFragmentAnimation(Fragment fragment,
            Visibility enterVisibility,
            Visibility exitVisibility,
            long duration)
        {
            if (enterVisibility != null)
            {
                enterVisibility.SetDuration(duration);
                if(IsDelay)
                    enterVisibility.SetStartDelay(0);
                fragment.EnterTransition = enterVisibility;
            }
            else
            {
                fragment.EnterTransition = null;
            }

            if (exitVisibility != null)
            {
                exitVisibility.SetDuration(duration);
                if(IsDelay)
                    exitVisibility.SetStartDelay(enterVisibility != null ? duration : 0);
                fragment.ExitTransition = exitVisibility;
            }
            else
            {
                fragment.ExitTransition = null;
            }
        }
    }

    public class TransitionListener : TransitionListenerAdapter
    {
        private readonly Action _action;

        public TransitionListener(Action action)
        {
            _action = action;
        }

        public override void OnTransitionEnd(Transition transition)
        {
            base.OnTransitionEnd(transition);

            _action?.Invoke();
            
            transition.RemoveListener(this);
        }
    }
}