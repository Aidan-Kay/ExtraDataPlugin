using GameReaderCommon;
using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin
{
    [PluginDescription("Description")]
    [PluginAuthor("Aidan Kay")]
    [PluginName("Extra Data Plugin")]
    public class ExtraDataPlugin : IPlugin, IDataPlugin, IWPFSettings
    {
        public PluginManager PluginManager { get; set; }

        public ExtraDataPluginSettings Settings;

        internal AllGameData AllGameData;

        internal Sections.SectionBase SessionData;
        internal Sections.SectionBase CarData;
        internal Sections.SectionBase FuelData;
        internal Sections.SectionBase TyreData;
        internal Sections.SectionBase LapData;
        internal Sections.SectionBase SectorData;
        internal Sections.SectionBase LeaderboardData;

        internal bool UpdateAt1Fps;
        internal bool UpdateAt2Fps;
        internal bool UpdateAt5Fps;
        internal bool UpdateAt10Fps;

        internal const long TicksFor1Fps = 10000000;
        internal const long TicksFor2Fps = 5000000;
        internal const long TicksFor5Fps = 2000000;
        internal const long TicksFor10Fps = 1000000;

        internal long LastRan1Fps;
        internal long LastRan2Fps;
        internal long LastRan5Fps;
        internal long LastRan10Fps;

        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin: ExtraDataPlugin");

            AllGameData = new AllGameData();

            SessionData = new Sections.SessionData(this);
            CarData = new Sections.CarData(this);
            FuelData = new Sections.FuelData(this);
            TyreData = new Sections.TyreData(this);
            LapData = new Sections.LapData(this);
            SectorData = new Sections.SectorData(this);
            LeaderboardData = new Sections.LeaderboardData(this);
        }

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            if (data.GameRunning && data.NewData != null && data.OldData != null)
            {
                AllGameData.GameData = data;
                AllGameData.AssignSpecificGameData();

                long nowTicks = DateTime.Now.Ticks;
                UpdateAt1Fps = nowTicks - LastRan1Fps >= TicksFor1Fps;
                UpdateAt2Fps = nowTicks - LastRan2Fps >= TicksFor2Fps;
                UpdateAt5Fps = nowTicks - LastRan5Fps >= TicksFor5Fps;
                UpdateAt10Fps = nowTicks - LastRan10Fps >= TicksFor10Fps;

                SessionData.Update();
                CarData.Update();
                FuelData.Update();
                TyreData.Update();
                LapData.Update();
                SectorData.Update();
                LeaderboardData.Update();

                if (UpdateAt1Fps)
                    LastRan1Fps = DateTime.Now.Ticks;
                if (UpdateAt2Fps)
                    LastRan2Fps = DateTime.Now.Ticks;
                if (UpdateAt5Fps)
                    LastRan5Fps = DateTime.Now.Ticks;
                if (UpdateAt10Fps)
                    LastRan10Fps = DateTime.Now.Ticks;
            }
            else
                ClearAllProperties();
        }

        public void ClearAllProperties()
        {
            SessionData = new Sections.SessionData(this);
            CarData = new Sections.CarData(this);
            FuelData = new Sections.FuelData(this);
            TyreData = new Sections.TyreData(this);
            LapData = new Sections.LapData(this);
            SectorData = new Sections.SectorData(this);
            LeaderboardData = new Sections.LeaderboardData(this);
        }

        public void End(PluginManager pluginManager) { }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager) => null;

        public void AttachProperty<T>(string propName, AttachedProperty<T> prop) => PluginManager.AttachProperty<T>(propName, typeof(ExtraDataPlugin), prop);

        public object GetPropertyValue(string propName) => PluginManager.GetPropertyValue(propName);
    }
}