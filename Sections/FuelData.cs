using GameReaderCommon;
using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class FuelData : SectionBase
    {
        public AttachedProperty<double> CurrentFuel = new AttachedProperty<double>();
        public AttachedProperty<double> FuelLastLap = new AttachedProperty<double>();
        public AttachedProperty<double> FuelPerLap = new AttachedProperty<double>();
        public AttachedProperty<double> FuelLapsRemaining = new AttachedProperty<double>();

        public FuelData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            if (Plugin.UpdateAt10Fps)
            {
                CurrentFuel.Value = NewData.Fuel;
                FuelLastLap.Value = GetFuelLastLap();
                FuelPerLap.Value = GetFuelPerLap();
                FuelLapsRemaining.Value = GetFuelLapsRemaining();
            }
        }

        protected override void Init(PluginManager pluginManager)
        {
            Plugin.AttachProperty("FuelData.CurrentFuel", CurrentFuel);
            Plugin.AttachProperty("FuelData.FuelLastLap", FuelLastLap);
            Plugin.AttachProperty("FuelData.FuelPerLap", FuelPerLap);
            Plugin.AttachProperty("FuelData.FuelLapsRemaining", FuelLapsRemaining);
        }

        private double GetFuelLastLap() =>
            (double)Plugin.GetPropertyValue("DataCorePlugin.Computed.Fuel_LastLapConsumption");

        private double GetFuelPerLap()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                return AllGameData.AccRawData.Graphics.FuelXLap;

            return (double)Plugin.GetPropertyValue("DataCorePlugin.Computed.Fuel_LitersPerLap");
        }

        private double GetFuelLapsRemaining()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                return AllGameData.AccRawData.Graphics.fuelEstimatedLaps;

            return (double)Plugin.GetPropertyValue("DataCorePlugin.Computed.Fuel_RemainingLaps");
        }
    }
}
