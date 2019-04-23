using System;
using JetBrains.Annotations;

namespace LoadSwitch.Extensions
{
    internal static class NotNullExtension
    {
        [NotNull]
        public static T NotNull<T>([CanBeNull] this T item)
        {
            if (item == null)
            {
                throw new Exception($"Item is null {typeof(T)}");
            }
            
            return item;
        }
    }
}