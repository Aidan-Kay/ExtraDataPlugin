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
            if (gradient.Count == 0)
                return new Color();

            if (value < gradient.Keys.Min())
                return gradient[gradient.Keys.Min()];
            if (value > gradient.Keys.Max())
                return gradient[gradient.Keys.Max()];

            var keys = gradient.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                if (value == keys[i])
                    // if value matches one of the defined colour stops
                    return gradient[keys[i]];
                
                else if (value > keys[i] && value < keys[i + 1])
                    // Found two colour stops that our value sits between - calculate color based on distance between the two colour stops
                    return ColorInterpolate(gradient[keys[i]], gradient[keys[i + 1]], Math.Abs((value - keys[i]) / (keys[i + 1] - keys[i])));
            }

            // Should never reach here given previous conditions - figure out a way where this isn't needed...
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
