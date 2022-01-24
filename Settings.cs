using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AidanKay.ExtraDataPlugin
{
    internal static class Settings
    {
        public static int LeaderboardNumberOfDrivers = 20;


        // https://coachdaveacademy.com/tutorials/the-ultimate-acc-car-setup-guide/

        public static ColourGradient AccGt3DryTyrePressureGradient =
            new ColourGradient() {
                { 24.3, Color.FromName("DeepSkyBlue") },
                { 27.3, Color.FromName("LimeGreen") },
                { 27.8, Color.FromName("LimeGreen") },
                { 30.8, Color.FromName("Red") }
            };

        public static ColourGradient AccGt3WetTyrePressureGradient =
            new ColourGradient() {
                { 26.5, Color.FromName("DeepSkyBlue") },
                { 29.5, Color.FromName("LimeGreen") },
                { 31.0, Color.FromName("LimeGreen") },
                { 34.0, Color.FromName("Red") }
            };

        public static ColourGradient AccGt4DryTyrePressureGradient =
            new ColourGradient() {
                { 23.5, Color.FromName("DeepSkyBlue") },
                { 26.5, Color.FromName("LimeGreen") },
                { 27.5, Color.FromName("LimeGreen") },
                { 30.5, Color.FromName("Red") }
            };

        public static ColourGradient AccGt4WetTyrePressureGradient =
            new ColourGradient() {
                { 26.5, Color.FromName("DeepSkyBlue") },
                { 29.5, Color.FromName("LimeGreen") },
                { 31.0, Color.FromName("LimeGreen") },
                { 34.0, Color.FromName("Red") }
            };

        public static ColourGradient TyreTemperatureGradient =
            new ColourGradient() {
                { 50, Color.FromName("DeepSkyBlue") },
                { 80, Color.FromName("LimeGreen") },
                { 85, Color.FromName("LimeGreen") },
                { 105, Color.FromName("Red") }
            };

        public static ColourGradient AccFrontBrakeTemperatureGradient =
            new ColourGradient() {
                { 200, Color.FromName("DeepSkyBlue") },
                { 300, Color.FromName("LimeGreen") },
                { 650, Color.FromName("LimeGreen") },
                { 750, Color.FromName("Red") }
            };

        public static ColourGradient AccRearBrakeTemperatureGradient =
            new ColourGradient() {
                { 150, Color.FromName("DeepSkyBlue") },
                { 250, Color.FromName("LimeGreen") },
                { 450, Color.FromName("LimeGreen") },
                { 550, Color.FromName("Red") }
            };
    }
}
