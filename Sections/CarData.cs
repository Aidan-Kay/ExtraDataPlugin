using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Drawing;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class CarData : SectionBase
    {
        public AttachedProperty<double?> BrakeTemperatureFrontLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> BrakeTemperatureFrontRight = new AttachedProperty<double?>();
        public AttachedProperty<double?> BrakeTemperatureRearLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> BrakeTemperatureRearRight = new AttachedProperty<double?>();

        public AttachedProperty<string> BrakeTemperatureFrontLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> BrakeTemperatureFrontRightColour = new AttachedProperty<string>();
        public AttachedProperty<string> BrakeTemperatureRearLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> BrakeTemperatureRearRightColour = new AttachedProperty<string>();

        public CarData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void Update()
        {
            if (Plugin.UpdateAt10Fps)
            {
                BrakeTemperatureFrontLeft.Value = NewData.BrakeTemperatureFrontLeft;
                BrakeTemperatureFrontRight.Value = NewData.BrakeTemperatureFrontRight;
                BrakeTemperatureRearLeft.Value = NewData.BrakeTemperatureRearLeft;
                BrakeTemperatureRearRight.Value = NewData.BrakeTemperatureRearRight;

                SetBrakeTemperatureColours();
            }
        }

        protected override void AttachProperties(PluginManager pluginManager)
        {
            Plugin.AttachProperty("CarData.BrakeTemperatureFrontLeft", BrakeTemperatureFrontLeft);
            Plugin.AttachProperty("CarData.BrakeTemperatureFrontRight", BrakeTemperatureFrontRight);
            Plugin.AttachProperty("CarData.BrakeTemperatureRearLeft", BrakeTemperatureRearLeft);
            Plugin.AttachProperty("CarData.BrakeTemperatureRearRight", BrakeTemperatureRearRight);

            Plugin.AttachProperty("CarData.BrakeTemperatureFrontLeftColour", BrakeTemperatureFrontLeftColour);
            Plugin.AttachProperty("CarData.BrakeTemperatureFrontRightColour", BrakeTemperatureFrontRightColour);
            Plugin.AttachProperty("CarData.BrakeTemperatureRearLeftColour", BrakeTemperatureRearLeftColour);
            Plugin.AttachProperty("CarData.BrakeTemperatureRearRightColour", BrakeTemperatureRearRightColour);
        }
        private void SetBrakeTemperatureColours()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
            {
                BrakeTemperatureFrontLeftColour.Value = GetBrakeTemperatureColour(BrakeTemperatureFrontLeft.Value, Settings.AccFrontBrakeTemperatureGradient).ToHex();
                BrakeTemperatureFrontRightColour.Value = GetBrakeTemperatureColour(BrakeTemperatureFrontRight.Value, Settings.AccFrontBrakeTemperatureGradient).ToHex();
                BrakeTemperatureRearLeftColour.Value = GetBrakeTemperatureColour(BrakeTemperatureRearLeft.Value, Settings.AccRearBrakeTemperatureGradient).ToHex();
                BrakeTemperatureRearRightColour.Value = GetBrakeTemperatureColour(BrakeTemperatureRearRight.Value, Settings.AccRearBrakeTemperatureGradient).ToHex();
            }
            else
                SetBrakeTemperatureColoursToWhite();
        }

        private Color GetBrakeTemperatureColour(double? value, ColourGradient gradient)
        {
            if (value == null)
                return Color.FromName("DimGray");

            return ColourHelper.GradientPick((double)value, gradient);
        }

        private void SetBrakeTemperatureColoursToWhite()
        {
            BrakeTemperatureFrontLeftColour.Value = "White";
            BrakeTemperatureFrontRightColour.Value = "White";
            BrakeTemperatureRearLeftColour.Value = "White";
            BrakeTemperatureRearRightColour.Value = "White";
        }
    }
}
