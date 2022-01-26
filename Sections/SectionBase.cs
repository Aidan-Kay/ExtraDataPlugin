using GameReaderCommon;
using SimHub.Plugins;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal abstract class SectionBase
    {
        protected readonly ExtraDataPlugin Plugin;

        protected AllGameData AllGameData { get => Plugin.AllGameData; }
        protected StatusDataBase NewData { get => Plugin.AllGameData.GameData.NewData; }
        protected StatusDataBase OldData { get => Plugin.AllGameData.GameData.OldData; }

        public SectionBase(ExtraDataPlugin extraDataPlugin)
        {
            Plugin = extraDataPlugin;
            Init(extraDataPlugin.PluginManager);
        }

        protected abstract void Init(PluginManager pluginManager);

        public abstract void DataUpdate();
    }
}