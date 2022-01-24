using System;
using System.Collections.Generic;

namespace AidanKay.ExtraDataPlugin
{
    internal static class CommonHelper
    {
        public static bool IsDifferent<T>(T a, T b) =>
            !a.Equals(b);

        public static T? NullIf<T>(T left, T right) where T : struct =>
            EqualityComparer<T>.Default.Equals(left, right) ? (T?)null : left;

        public static TimeSpan? ToNullableTimeSpan(double? value)
        {
            if (value != null)
                return TimeSpan.FromMilliseconds((double)value);

            return null;
        }

        public static double TimeSpanToSeconds(TimeSpan value) =>
            value.TotalSeconds;

        public static double? TimeSpanToSeconds(TimeSpan? value)
        {
            if (!value.HasValue)
                return null;

            return value.Value.TotalSeconds;
        }

        public static TimeSpan? SecondsToTimeSpan(double? value)
        {
            if (!value.HasValue)
                return null;

            return TimeSpan.FromSeconds((double)value);
        }

        public static TimeSpan? MillisecondsToTimeSpan(double? value)
        {
            if (!value.HasValue)
                return null;

            return TimeSpan.FromMilliseconds((double)value);
        }

        public static double? RoundDelta(double? delta)
        {
            if (!delta.HasValue) return null;
            return Math.Round((double)delta, 3);
        }
    }
}
