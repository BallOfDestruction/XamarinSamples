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
        private readonly FragmentActivity _fragmentActivity;
        private readonly ViewGroup _container;

        public FragmentNavigation(FragmentActivity fragmentActivity, ViewGroup container)
        {
            _fragmentActivity = fragmentActivity;
            _container = container;
        }

        public void GoTo(Fragment fragment, AnimationType enter, AnimationType exit)
        {
            var previousFragment = _fragmentActivity.SupportFragmentManager.Fragments?.LastOrDefault();

            SetAnimation(previousFragment, fragment, enter, exit, AnimationSpeed.X05);

            _fragmentActivity.SupportFragmentManager.BeginTransaction()
                .Replace(_container.Id, fragment)
                .AddToBackStack(null)
                .Commit();
        }

        private void SetAnimation(Fragment previousFragment, 
            Fragment fragment, 
            AnimationType enterType,
            AnimationType exitType,
            AnimationSpeed speed)
        {
            CommonAnimate(fragment, previousFragment, 
                speed.GetCalculationAnimationTime(),
                enterType.ToVisibility(),
                exitType.ToVisibility(),
                enterType.ToVisibility(), 
                exitType.ToVisibility());
        }
        
        private void CommonAnimate(
            Fragment previousFragment,
            Fragment fragment,
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

            if (enterEnterFragment != null)
            {
                var listener = new TransitionListener(() =>
                {
                    SetReverseFragmentAnimation(fragment, enterPopFragment, exitPopFragment, duration);
                });

                enterEnterFragment.AddListener(listener);
            }
            
            SetRightFragmentAnimation(fragment, enterEnterFragment, exitEnterFragment, duration);
        }

        private void SetRightFragmentAnimation(Fragment fragment,
            Visibility enterPopVisibility,
            Visibility exitPopVisibility,
            long duration)
        {
            if (enterPopVisibility != null)
            {
                enterPopVisibility.SetDuration(duration);
                enterPopVisibility.SetStartDelay(duration);
                fragment.EnterTransition = enterPopVisibility;
            }

            if (exitPopVisibility != null)
            {
                exitPopVisibility.SetDuration(duration);
                exitPopVisibility.SetStartDelay(0);
                fragment.ExitTransition = exitPopVisibility;
            }
        }
        
        private void SetReverseFragmentAnimation(Fragment fragment,
            Visibility enterPopVisibility,
            Visibility exitPopVisibility,
            long duration)
        {
            if (enterPopVisibility != null)
            {
                enterPopVisibility.SetDuration(duration);
                enterPopVisibility.SetStartDelay(0);
                fragment.EnterTransition = enterPopVisibility;
            }

            if (exitPopVisibility != null)
            {
                exitPopVisibility.SetDuration(duration);
                exitPopVisibility.SetStartDelay(duration);
//                exitPopVisibility.SetStartDelay(enterPopVisibility != null ? duration : 0);
                fragment.ExitTransition = exitPopVisibility;
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