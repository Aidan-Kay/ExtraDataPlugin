using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Drawing;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class TyreData : SectionBase
    {
        public AttachedProperty<string> TyreCompound = new AttachedProperty<string>();

        public AttachedProperty<double?> TyrePressureFrontLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyrePressureFrontRight = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyrePressureRearLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyrePressureRearRight = new AttachedProperty<double?>();

        public AttachedProperty<string> TyrePressureFrontLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyrePressureFrontRightColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyrePressureRearLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyrePressureRearRightColour = new AttachedProperty<string>();

        public AttachedProperty<double?> TyreTemperatureFrontLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureFrontRight = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearLeft = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearRight = new AttachedProperty<double?>();

        public AttachedProperty<double?> TyreTemperatureFrontLeftInner = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureFrontRightInner = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearLeftInner = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearRightInner = new AttachedProperty<double?>();

        public AttachedProperty<double?> TyreTemperatureFrontLeftMiddle = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureFrontRightMiddle = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearLeftMiddle = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearRightMiddle = new AttachedProperty<double?>();

        public AttachedProperty<double?> TyreTemperatureFrontLeftOuter = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureFrontRightOuter = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearLeftOuter = new AttachedProperty<double?>();
        public AttachedProperty<double?> TyreTemperatureRearRightOuter = new AttachedProperty<double?>();

        public AttachedProperty<string> TyreTemperatureFrontLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureFrontRightColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearLeftColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearRightColour = new AttachedProperty<string>();

        public AttachedProperty<string> TyreTemperatureFrontLeftInnerColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureFrontRightInnerColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearLeftInnerColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearRightInnerColour = new AttachedProperty<string>();

        public AttachedProperty<string> TyreTemperatureFrontLeftMiddleColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureFrontRightMiddleColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearLeftMiddleColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearRightMiddleColour = new AttachedProperty<string>();

        public AttachedProperty<string> TyreTemperatureFrontLeftOuterColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureFrontRightOuterColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearLeftOuterColour = new AttachedProperty<string>();
        public AttachedProperty<string> TyreTemperatureRearRightOuterColour = new AttachedProperty<string>();

        public TyreData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            if (Plugin.UpdateAt10Fps)
            {
                TyreCompound.Value = GetTyreCompound();

                TyrePressureFrontLeft.Value = NewData.TyrePressureFrontLeft;
                TyrePressureFrontRight.Value = NewData.TyrePressureFrontRight;
                TyrePressureRearLeft.Value = NewData.TyrePressureRearLeft;
                TyrePressureRearRight.Value = NewData.TyrePressureRearRight;

                SetTyreTemperatures();
                SetTyrePressureColours();
                SetTyreTemperatureColours();
            }
        }

        protected override void Init(PluginManager pluginManager)
        {
            Plugin.AttachProperty("TyreData.TyreCompound", TyreCompound);

            Plugin.AttachProperty("TyreData.TyrePressureFrontLeft", TyrePressureFrontLeft);
            Plugin.AttachProperty("TyreData.TyrePressureFrontRight", TyrePressureFrontRight);
            Plugin.AttachProperty("TyreData.TyrePressureRearLeft", TyrePressureRearLeft);
            Plugin.AttachProperty("TyreData.TyrePressureRearRight", TyrePressureRearRight);

            Plugin.AttachProperty("TyreData.TyrePressureFrontLeftColour", TyrePressureFrontLeftColour);
            Plugin.AttachProperty("TyreData.TyrePressureFrontRightColour", TyrePressureFrontRightColour);
            Plugin.AttachProperty("TyreData.TyrePressureRearLeftColour", TyrePressureRearLeftColour);
            Plugin.AttachProperty("TyreData.TyrePressureRearRightColour", TyrePressureRearRightColour);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeft", TyreTemperatureFrontLeft);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRight", TyreTemperatureFrontRight);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeft", TyreTemperatureRearLeft);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRight", TyreTemperatureRearRight);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftInner", TyreTemperatureFrontLeftInner);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightInner", TyreTemperatureFrontRightInner);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftInner", TyreTemperatureRearLeftInner);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightInner", TyreTemperatureRearRightInner);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftMiddle", TyreTemperatureFrontLeftMiddle);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightMiddle", TyreTemperatureFrontRightMiddle);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftMiddle", TyreTemperatureRearLeftMiddle);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightMiddle", TyreTemperatureRearRightMiddle);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftOuter", TyreTemperatureFrontLeftOuter);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightOuter", TyreTemperatureFrontRightOuter);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftOuter", TyreTemperatureRearLeftOuter);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightOuter", TyreTemperatureRearRightOuter);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftColour", TyreTemperatureFrontLeftColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightColour", TyreTemperatureFrontRightColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftColour", TyreTemperatureRearLeftColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightColour", TyreTemperatureRearRightColour);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftInnerColour", TyreTemperatureFrontLeftInnerColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightInnerColour", TyreTemperatureFrontRightInnerColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftInnerColour", TyreTemperatureRearLeftInnerColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightInnerColour", TyreTemperatureRearRightInnerColour);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftMiddleColour", TyreTemperatureFrontLeftMiddleColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightMiddleColour", TyreTemperatureFrontRightMiddleColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftMiddleColour", TyreTemperatureRearLeftMiddleColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightMiddleColour", TyreTemperatureRearRightMiddleColour);

            Plugin.AttachProperty("TyreData.TyreTemperatureFrontLeftOuterColour", TyreTemperatureFrontLeftOuterColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureFrontRightOuterColour", TyreTemperatureFrontRightOuterColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearLeftOuterColour", TyreTemperatureRearLeftOuterColour);
            Plugin.AttachProperty("TyreData.TyreTemperatureRearRightOuterColour", TyreTemperatureRearRightOuterColour);
        }

        private string GetTyreCompound()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
            {
                if (AllGameData.AccRawGameData.Graphics.TyreCompound == "dry_compound")
                    return "Dry";
                else if (AllGameData.AccRawGameData.Graphics.TyreCompound == "wet_compound")
                    return "Wet";
            }

            return null;
        }

        private void SetTyrePressureColours()
        {
            ColourGradient gradient = GetTyrePressureColourGradient();

            if (gradient != null)
                SetTyrePressureColours(gradient);
            else
                SetTyrePressureColoursToWhite();
        }

        private ColourGradient GetTyrePressureColourGradient()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
            {
                string carClass = AccHelper.GetCarClass(AccHelper.GetPlayersCar(AllGameData.AccRawGameData).CarEntry.CarModelType);
                if (carClass.Contains("GT3"))
                    return TyreCompound.Value == "Dry" ? Settings.AccGt3DryTyrePressureGradient : Settings.AccGt3WetTyrePressureGradient;
                else if (carClass.Contains("GT4"))
                    return TyreCompound.Value == "Dry" ? Settings.AccGt4DryTyrePressureGradient : Settings.AccGt4WetTyrePressureGradient;
            }

            return null;
        }

        private void SetTyrePressureColours(ColourGradient gradient)
        {
            TyrePressureFrontLeftColour.Value = GetTyrePressureColour(TyrePressureFrontLeft.Value, gradient).ToHex();
            TyrePressureFrontRightColour.Value = GetTyrePressureColour(TyrePressureFrontRight.Value, gradient).ToHex();
            TyrePressureRearLeftColour.Value = GetTyrePressureColour(TyrePressureRearLeft.Value, gradient).ToHex();
            TyrePressureRearRightColour.Value = GetTyrePressureColour(TyrePressureRearRight.Value, gradient).ToHex();
        }

        private Color GetTyrePressureColour(double? value, ColourGradient gradient)
        {
            if (value == null)
                return Color.FromName("DimGray");

            return ColourHelper.GradientPick((double)value, gradient);
        }

        private void SetTyrePressureColoursToWhite()
        {
            TyrePressureFrontLeftColour.Value = "White";
            TyrePressureFrontRightColour.Value = "White";
            TyrePressureRearLeftColour.Value = "White";
            TyrePressureRearRightColour.Value = "White";
        }

        private void SetTyreTemperatures()
        {
            TyreTemperatureFrontLeft.Value = NewData.TyreTemperatureFrontLeft;
            TyreTemperatureFrontRight.Value = NewData.TyreTemperatureFrontRight;
            TyreTemperatureRearLeft.Value = NewData.TyreTemperatureRearLeft;
            TyreTemperatureRearRight.Value = NewData.TyreTemperatureRearRight;

            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
            {
                // 2022-01-24: These are currently empty?
                //float[] innerTemps = AllGameData.AccRawGameData.Physics.TyreTempI;
                //float[] middleTemps = AllGameData.AccRawGameData.Physics.TyreTempM;
                //float[] outerTemps = AllGameData.AccRawGameData.Physics.TyreTempO;

                //TyreTemperatureFrontLeftInner.Value = innerTemps[0];
                //TyreTemperatureFrontRightInner.Value = innerTemps[1];
                //TyreTemperatureRearLeftInner.Value = innerTemps[2];
                //TyreTemperatureRearRightInner.Value = innerTemps[3];

                //TyreTemperatureFrontLeftMiddle.Value = middleTemps[0];
                //TyreTemperatureFrontRightMiddle.Value = middleTemps[1];
                //TyreTemperatureRearLeftMiddle.Value = middleTemps[2];
                //TyreTemperatureRearRightMiddle.Value = middleTemps[3];

                //TyreTemperatureFrontLeftOuter.Value = outerTemps[0];
                //TyreTemperatureFrontRightOuter.Value = outerTemps[1];
                //TyreTemperatureRearLeftOuter.Value = outerTemps[2];
                //TyreTemperatureRearRightOuter.Value = outerTemps[3];

                TyreTemperatureFrontLeftInner.Value = NewData.TyreTemperatureFrontLeft;
                TyreTemperatureFrontRightInner.Value = NewData.TyreTemperatureFrontRight;
                TyreTemperatureRearLeftInner.Value = NewData.TyreTemperatureRearLeft;
                TyreTemperatureRearRightInner.Value = NewData.TyreTemperatureRearRight;

                TyreTemperatureFrontLeftMiddle.Value = NewData.TyreTemperatureFrontLeft;
                TyreTemperatureFrontRightMiddle.Value = NewData.TyreTemperatureFrontRight;
                TyreTemperatureRearLeftMiddle.Value = NewData.TyreTemperatureRearLeft;
                TyreTemperatureRearRightMiddle.Value = NewData.TyreTemperatureRearRight;

                TyreTemperatureFrontLeftOuter.Value = NewData.TyreTemperatureFrontLeft;
                TyreTemperatureFrontRightOuter.Value = NewData.TyreTemperatureFrontRight;
                TyreTemperatureRearLeftOuter.Value = NewData.TyreTemperatureRearLeft;
                TyreTemperatureRearRightOuter.Value = NewData.TyreTemperatureRearRight;
            }
            else
            {
                TyreTemperatureFrontLeftInner.Value = NewData.TyreTemperatureFrontLeftInner;
                TyreTemperatureFrontRightInner.Value = NewData.TyreTemperatureFrontRightInner;
                TyreTemperatureRearLeftInner.Value = NewData.TyreTemperatureRearLeftInner;
                TyreTemperatureRearRightInner.Value = NewData.TyreTemperatureRearRightInner;

                TyreTemperatureFrontLeftMiddle.Value = NewData.TyreTemperatureFrontLeftMiddle;
                TyreTemperatureFrontRightMiddle.Value = NewData.TyreTemperatureFrontRightMiddle;
                TyreTemperatureRearLeftMiddle.Value = NewData.TyreTemperatureRearLeftMiddle;
                TyreTemperatureRearRightMiddle.Value = NewData.TyreTemperatureRearRightMiddle;

                TyreTemperatureFrontLeftOuter.Value = NewData.TyreTemperatureFrontLeftOuter;
                TyreTemperatureFrontRightOuter.Value = NewData.TyreTemperatureFrontRightOuter;
                TyreTemperatureRearLeftOuter.Value = NewData.TyreTemperatureRearLeftOuter;
                TyreTemperatureRearRightOuter.Value = NewData.TyreTemperatureRearRightOuter;
            }
        }

        private void SetTyreTemperatureColours()
        {
            TyreTemperatureFrontLeftColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontLeft.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureFrontRightColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontRight.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearLeftColour.Value = GetTyreTemperatureColour(TyreTemperatureRearLeft.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearRightColour.Value = GetTyreTemperatureColour(TyreTemperatureRearRight.Value, Settings.TyreTemperatureGradient).ToHex();

            TyreTemperatureFrontLeftInnerColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontLeftInner.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureFrontRightInnerColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontRightInner.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearLeftInnerColour.Value = GetTyreTemperatureColour(TyreTemperatureRearLeftInner.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearRightInnerColour.Value = GetTyreTemperatureColour(TyreTemperatureRearRightInner.Value, Settings.TyreTemperatureGradient).ToHex();

            TyreTemperatureFrontLeftMiddleColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontLeftMiddle.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureFrontRightMiddleColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontRightMiddle.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearLeftMiddleColour.Value = GetTyreTemperatureColour(TyreTemperatureRearLeftMiddle.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearRightMiddleColour.Value = GetTyreTemperatureColour(TyreTemperatureRearRightMiddle.Value, Settings.TyreTemperatureGradient).ToHex();

            TyreTemperatureFrontLeftOuterColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontLeftOuter.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureFrontRightOuterColour.Value = GetTyreTemperatureColour(TyreTemperatureFrontRightOuter.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearLeftOuterColour.Value = GetTyreTemperatureColour(TyreTemperatureRearLeftOuter.Value, Settings.TyreTemperatureGradient).ToHex();
            TyreTemperatureRearRightOuterColour.Value = GetTyreTemperatureColour(TyreTemperatureRearRightOuter.Value, Settings.TyreTemperatureGradient).ToHex();
        }

        private Color GetTyreTemperatureColour(double? value, ColourGradient gradient)
        {
            if (value == null)
                return Color.FromName("DimGray");

            return ColourHelper.GradientPick((double)value, gradient);
        }
    }
}
