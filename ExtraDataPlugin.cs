using AidanKay.ExtraDataPlugin.Sections;
using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AidanKay.ExtraDataPlugin
{
    [PluginDescription("Description")]
    [PluginAuthor("Aidan Kay")]
    [PluginName("Extra Data Plugin")]
    public class ExtraDataPlugin : IPlugin, IDataPlugin, IWPFSettings
    {
        public PluginManager PluginManager { get; set; }

        internal AllGameData AllGameData = new AllGameData();

        internal List<SectionBase> Sections = new List<SectionBase>();

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

            InitSections();
        }

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            //System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (data.GameRunning && data.NewData != null && data.OldData != null)
            {
                AllGameData.GameData = data;
                AllGameData.AssignSpecificGameData();

                long nowTicks = DateTime.Now.Ticks;
                UpdateAt1Fps = nowTicks - LastRan1Fps >= TicksFor1Fps;
                UpdateAt2Fps = nowTicks - LastRan2Fps >= TicksFor2Fps;
                UpdateAt5Fps = nowTicks - LastRan5Fps >= TicksFor5Fps;
                UpdateAt10Fps = nowTicks - LastRan10Fps >= TicksFor10Fps;

                Parallel.ForEach(Sections, section =>
                    section.DataUpdate()
                );

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

            //stopwatch.Stop();
            //double time = (double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond;
        }

        private void InitSections()
        {
            Sections.Add(new SessionData(this));
            Sections.Add(new CarData(this));
            Sections.Add(new FuelData(this));
            Sections.Add(new TyreData(this));
            Sections.Add(new LapData(this));
            Sections.Add(new SectorData(this));
            Sections.Add(new LeaderboardData(this));
        }

        public void ClearAllProperties()
        {
            Sections.Clear();
            InitSections();
        }

        public void End(PluginManager pluginManager) { }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager) => null;

        public void AttachProperty<T>(string propName, AttachedProperty<T> prop) => PluginManager.AttachProperty<T>(propName, typeof(ExtraDataPlugin), prop);

        public void AttachDelegate<T>(string propName, Func<T> valueProvider) => PluginManager.AttachDelegate<T>(propName, typeof(ExtraDataPlugin), valueProvider);

        public object GetPropertyValue(string propName) => PluginManager.GetPropertyValue(propName);
    }
}