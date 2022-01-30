using System.Drawing;

namespace AidanKay.ExtraDataPlugin
{
    internal static class Settings
    {
        public static int LeaderboardNumberOfDrivers = 64;

        public static string NullValueColour = "DimGray";
        public static string GeneralValueColour = "White";

        public static string SectorTimeNotImprovedColour = "Yellow";
        public static string SectorTimeNotImprovedMutedColour = "#FF4F4F00";

        public static string PersonalBestTimeColour = "LimeGreen";
        public static string PersonalBestTimeMutedColour = "#FF004600";

        public static string OverallBestTimeColour = "Magenta";
        public static string OverallBestTimeMutedColour = "#FF5A005A";

        public static string PositiveTimeDeltaColour = "Red";
        public static string NegativeTimeDeltaColour = "LimeGreen";

        public static string InvalidLapColour = "Red";

        public static string ColdTemperatureColour = "DeepSkyBlue";
        public static string IdealTemperatureColour = "LimeGreen";
        public static string HotTemperatureColour = "Red";

        // Kinda nice also: Royal Blue, Lime Green, Firebrick

        // A lot of the values from: https://coachdaveacademy.com/tutorials/the-ultimate-acc-car-setup-guide/

        public static ColourGradient AccGt3DryTyrePressureGradient =
            new ColourGradient() {
                { 24.3, Color.FromName(ColdTemperatureColour) },
                { 27.3, Color.FromName(IdealTemperatureColour) },
                { 27.8, Color.FromName(IdealTemperatureColour) },
                { 30.8, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient AccGt3WetTyrePressureGradient =
            new ColourGradient() {
                { 26.5, Color.FromName(ColdTemperatureColour) },
                { 29.5, Color.FromName(IdealTemperatureColour) },
                { 31.0, Color.FromName(IdealTemperatureColour) },
                { 34.0, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient AccGt4DryTyrePressureGradient =
            new ColourGradient() {
                { 23.5, Color.FromName(ColdTemperatureColour) },
                { 26.5, Color.FromName(IdealTemperatureColour) },
                { 27.5, Color.FromName(IdealTemperatureColour) },
                { 30.5, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient AccGt4WetTyrePressureGradient =
            new ColourGradient() {
                { 26.5, Color.FromName(ColdTemperatureColour) },
                { 29.5, Color.FromName(IdealTemperatureColour) },
                { 31.0, Color.FromName(IdealTemperatureColour) },
                { 34.0, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient TyreTemperatureGradient =
            new ColourGradient() {
                { 50, Color.FromName(ColdTemperatureColour) },
                { 80, Color.FromName(IdealTemperatureColour) },
                { 85, Color.FromName(IdealTemperatureColour) },
                { 105, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient AccFrontBrakeTemperatureGradient =
            new ColourGradient() {
                { 200, Color.FromName(ColdTemperatureColour) },
                { 300, Color.FromName(IdealTemperatureColour) },
                { 650, Color.FromName(IdealTemperatureColour) },
                { 750, Color.FromName(HotTemperatureColour) }
            };

        public static ColourGradient AccRearBrakeTemperatureGradient =
            new ColourGradient() {
                { 150, Color.FromName(ColdTemperatureColour) },
                { 250, Color.FromName(IdealTemperatureColour) },
                { 450, Color.FromName(IdealTemperatureColour) },
                { 550, Color.FromName(HotTemperatureColour) }
            };
    }
}
