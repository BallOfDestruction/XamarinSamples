using System;

namespace Droid
{
    public static class AnimationSpeedExtension
    {
        public static int GetCalculationAnimationTime(this AnimationSpeed animationSpeed)
        {
            var defaultTime = 500;
            switch (animationSpeed)
            {
                case AnimationSpeed.X025:
                    return defaultTime * 4;
                case AnimationSpeed.X033:
                    return defaultTime * 3;
                case AnimationSpeed.X05:
                    return defaultTime * 2;
                case AnimationSpeed.X:
                    return defaultTime;
                case AnimationSpeed.X2:
                    return defaultTime / 2;
                case AnimationSpeed.X3:
                    return defaultTime / 3;
                case AnimationSpeed.X4:
                    return defaultTime / 4;
                case AnimationSpeed.Max:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}