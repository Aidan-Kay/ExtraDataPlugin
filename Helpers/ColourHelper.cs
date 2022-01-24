using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AidanKay.ExtraDataPlugin
{
    public class ColourGradient : Dictionary<double, Color> { }

    internal static class ColourHelper
    {
        public static Color GradientPick(double value, ColourGradient gradient)
        {
            if (value < gradient.Keys.Min())
                return gradient[gradient.Keys.Min()];
            if (value > gradient.Keys.Max())
                return gradient[gradient.Keys.Max()];

            var keys = gradient.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                // if value matches one of the defined gradient values
                if (value == keys[i])
                    return gradient[keys[i]];
                // Found two gradient values that our value sits between - calculate color (value - startValue) / (endValue - startValue)
                else if (value > keys[i] && value < keys[i + 1])
                    return ColorInterpolate(gradient[keys[i]], gradient[keys[i + 1]], Math.Abs((value - keys[i]) / (keys[i + 1] - keys[i])));
            }

            // Should never reach here given previous conditions
            return new Color();
        }

        public static int LinearInterpolate(int start, int end, double percentage) =>
            start + (int)Math.Round(percentage * (end - start));

        public static Color ColorInterpolate(Color start, Color end, double percentage) =>
            Color.FromArgb(LinearInterpolate(start.A, end.A, percentage),
                           LinearInterpolate(start.R, end.R, percentage),
                           LinearInterpolate(start.G, end.G, percentage),
                           LinearInterpolate(start.B, end.B, percentage));

        public static string ToHex(this Color color) =>
            "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
    }
}
