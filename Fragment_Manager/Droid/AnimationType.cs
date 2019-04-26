using System;
using System.Collections.Generic;
using Android.Support.Transitions;
using Android.Views;

namespace Droid
{
    public enum AnimationType
    {
        None,
        Fade,
        Explode,
        Bottom,
        Top,
        Left,
        Right,
    }

    public static class AnimationTypeExtension
    {
        public static List<AnimationType> GetAnimationTypes()
        {
            return new List<AnimationType>()
            {
                AnimationType.None, 
                AnimationType.Bottom, 
                AnimationType.Explode, 
                AnimationType.Fade, 
                AnimationType.Left,
                AnimationType.Right, 
                AnimationType.Top
            };
        }
        
        public static Visibility ToVisibility(this AnimationType type)
        {
            switch (type)
            {
                case AnimationType.None:
                    return null;
                case AnimationType.Fade:
                    return new Fade();
                    break;
                case AnimationType.Bottom:           
                    return new Slide() {SlideEdge = (int) GravityFlags.Bottom };
                case AnimationType.Top:
                    return new Slide() {SlideEdge = (int) GravityFlags.Top };
                    break;
                case AnimationType.Left:
                    return new Slide() {SlideEdge = (int) GravityFlags.Left };
                case AnimationType.Right:
                    return new Slide() {SlideEdge = (int) GravityFlags.Right };
                case AnimationType.Explode:
                    return new Explode();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}