using GameReaderCommon;
using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class SessionData : SectionBase
    {
        public AttachedProperty<double> AirTemperature = new AttachedProperty<double>();
        public AttachedProperty<double> TrackTemperature = new AttachedProperty<double>();

        public AttachedProperty<bool> IsTimedSession = new AttachedProperty<bool>();
        public AttachedProperty<TimeSpan> SessionTimeRemaining = new AttachedProperty<TimeSpan>();
        public AttachedProperty<double?> EstimatedLapsRemaining = new AttachedProperty<double?>();

        public SessionData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            if (Plugin.UpdateAt10Fps)
            {
                AirTemperature.Value = NewData.AirTemperature;
                TrackTemperature.Value = NewData.RoadTemperature;

                IsTimedSession.Value = NewData.TotalLaps == 0;
                SessionTimeRemaining.Value = NewData.SessionTimeLeft;
                EstimatedLapsRemaining.Value = GetEstimatedLapsRemaining();
            }
        }

        protected override void Init(PluginManager pluginManager)
        {
            Plugin.AttachProperty("SessionData.AirTemperature", AirTemperature);
            Plugin.AttachProperty("SessionData.TrackTemperature", TrackTemperature);

            Plugin.AttachProperty("SessionData.IsTimedSession", IsTimedSession);
            Plugin.AttachProperty("SessionData.SessionTimeRemaining", SessionTimeRemaining);
            Plugin.AttachProperty("SessionData.EstimatedLapsRemaining", EstimatedLapsRemaining);
        }

        public double? GetEstimatedLapsRemaining()
        {
            double sessionTimeLeft = CommonHelper.TimeSpanToSeconds(NewData.SessionTimeLeft);

            if (sessionTimeLeft == 0)
                return null;

            double? referenceLapTime;

            double? averageLapTime = GetLastNLapsAverageTime(5);
            referenceLapTime = averageLapTime ?? CommonHelper.TimeSpanToSeconds(NewData.AllTimeBest);

            if (referenceLapTime == null)
                return null;

            return Math.Ceiling((double)(sessionTimeLeft / referenceLapTime));

            //var currentLapTime = Helper.TimeSpanToSeconds(NewData.CurrentLapTime);

            //if (NewData.CurrentLap == 1 && currentLapTime == 0)
            //    return Math.Ceiling((double)(sessionTimeLeft / referenceLapTime));
            //else
            //{
            //    var timeAtStartOfLap = sessionTimeLeft + currentLapTime;
            //    return Math.Ceiling((double)(timeAtStartOfLap / referenceLapTime)) - NewData.TrackPositionPercent;
            //}
        }

        public double? GetLastNLapsAverageTime(int laps)
        {
            return null;
        }
    }
}
