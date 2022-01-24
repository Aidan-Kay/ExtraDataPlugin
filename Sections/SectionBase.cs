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
            AttachProperties(extraDataPlugin.PluginManager);
        }

        protected abstract void AttachProperties(PluginManager pluginManager);

        public abstract void Update();
    }
}