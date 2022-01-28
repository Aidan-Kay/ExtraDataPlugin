using GameReaderCommon;
using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class LapData : SectionBase
    {
        public AttachedProperty<bool> CurrentLapIsValid = new AttachedProperty<bool>();

        public AttachedProperty<TimeSpan?> CurrentLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> CurrentLapColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> LastLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> LastLapColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> SessionBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> PreviousSessionBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> SessionBestLapColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> AllTimeBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> PreviousAllTimeBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> AllTimeBestLapColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> OverallBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> PreviousOverallBestLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> OverallBestLapColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> OptimalLapTime = new AttachedProperty<TimeSpan?>();

        public AttachedProperty<double?> LastLapDeltaToSessionBest = new AttachedProperty<double?>();
        public AttachedProperty<double?> LastLapDeltaToAllTimeBest = new AttachedProperty<double?>();

        public AttachedProperty<double?> PredictedLapDeltaToSessionBest = new AttachedProperty<double?>();

        public LapData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            // Order is intentional (dependencies)

            SetSessionBestLapTimes();
            SessionBestLapColour.Value = GetSessionBestLapColour();

            SetAllTimeBestLapTimes();
            AllTimeBestLapColour.Value = GetAllTimeBestLapColour();

            SetOverallBestLapTimes();
            OverallBestLapColour.Value = GetOverallBestLapColour();

            CurrentLapIsValid.Value = IsCurrentLapValid();

            CurrentLapTime.Value = GetCurrentLapTime();
            CurrentLapColour.Value = GetCurrentLapColour();

            LastLapTime.Value = GetLastLapTime();
            LastLapColour.Value = GetLastLapColour();

            LastLapDeltaToSessionBest.Value = GetLastLapDeltaToSessionBest();
            LastLapDeltaToAllTimeBest.Value = GetLastLapDeltaToAllTimeBest();

            PredictedLapDeltaToSessionBest.Value = GetPredictedLapDeltaToSessionBest();
        }

        protected override void Init(PluginManager pluginManager)
        {
            Plugin.AttachProperty("LapData.CurrentLapIsValid", CurrentLapIsValid);

            Plugin.AttachProperty("LapData.CurrentLapTime", CurrentLapTime);
            Plugin.AttachProperty("LapData.CurrentLapColour", CurrentLapColour);

            Plugin.AttachProperty("LapData.LastLapTime", LastLapTime);
            Plugin.AttachProperty("LapData.LastLapColour", LastLapColour);

            Plugin.AttachProperty("LapData.SessionBestLapTime", SessionBestLapTime);
            Plugin.AttachProperty("LapData.PreviousSessionBestLapTime", PreviousSessionBestLapTime);
            Plugin.AttachProperty("LapData.SessionBestLapColour", SessionBestLapColour);

            Plugin.AttachProperty("LapData.AllTimeBestLapTime", AllTimeBestLapTime);
            Plugin.AttachProperty("LapData.PreviousAllTimeBestLapTime", PreviousAllTimeBestLapTime);
            Plugin.AttachProperty("LapData.AllTimeBestLapColour", AllTimeBestLapColour);

            Plugin.AttachProperty("LapData.OverallBestLapTime", OverallBestLapTime);
            Plugin.AttachProperty("LapData.PreviousOverallBestLapTime", PreviousOverallBestLapTime);
            Plugin.AttachProperty("LapData.OverallBestLapColour", OverallBestLapColour);

            Plugin.AttachProperty("LapData.OptimalLapTime", OptimalLapTime);

            Plugin.AttachProperty("LapData.LastLapDeltaToSessionBest", LastLapDeltaToSessionBest);
            Plugin.AttachProperty("LapData.LastLapDeltaToAllTimeBest", LastLapDeltaToAllTimeBest);

            Plugin.AttachProperty("LapData.PredictedLapDeltaToSessionBest", PredictedLapDeltaToSessionBest);
        }

        private bool IsCurrentLapValid()
        {
            if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                return (int)Plugin.GetPropertyValue("GameRawData.Graphics.isValidLap") == 1;

            else if (AllGameData.GameData.GameName == "IRacing")
            {
                if (NewData.CurrentLapTime == TimeSpan.Zero || NewData.LastLapTime != OldData.LastLapTime)
                    return true;

                if (!CurrentLapIsValid.Value)
                    return false;

                AllGameData.IRacingRawData.Telemetry.TryGetValue("PlayerCarTeamIncidentCount", out object incCountObj);
                AllGameData.IRacingOldRawData.Telemetry.TryGetValue("PlayerCarTeamIncidentCount", out object oldIncCountObj);

                int incCount = Convert.ToInt32(incCountObj);
                int oldIncCount = Convert.ToInt32(oldIncCountObj);

                if (AllGameData.IRacingRawData.Telemetry.SessionFlags.ToString().Contains("furled") || incCount != oldIncCount)
                    return false;
            }

            return true;
        }

        private TimeSpan? GetCurrentLapTime() =>
            CommonHelper.NullIf(NewData.CurrentLapTime, TimeSpan.Zero);

        private string GetCurrentLapColour()
        {
            if (CurrentLapTime.Value == null)
                return "DimGray";

            if (!CurrentLapIsValid.Value)
                return "Red";

            return "White";
        }

        private TimeSpan? GetLastLapTime() =>
            CommonHelper.NullIf(NewData.LastLapTime, TimeSpan.Zero);

        private string GetLastLapColour()
        {
            TimeSpan? lastLap = LastLapTime.Value;
            TimeSpan? sessionBestLap = SessionBestLapTime.Value;
            TimeSpan? overallBestLap = OverallBestLapTime.Value;

            if (lastLap == null) return "DimGray";
            if (overallBestLap == null) return "Magenta";
            if (lastLap == overallBestLap) return "Magenta";
            if (sessionBestLap == null) return "LimeGreen";
            if (lastLap == sessionBestLap) return "LimeGreen";
            return "White";
        }

        private string GetSessionBestLapColour()
        {
            TimeSpan? sessionBestLap = SessionBestLapTime.Value;
            TimeSpan? overallBestLap = OverallBestLapTime.Value;

            if (sessionBestLap == null) return "DimGray";
            if (overallBestLap == null) return "Magenta";
            if (sessionBestLap == overallBestLap) return "Magenta";
            return "LimeGreen";
        }

        private string GetAllTimeBestLapColour()
        {
            if (AllTimeBestLapTime.Value == null) return "DimGray";
            return "Magenta";
        }

        private string GetOverallBestLapColour()
        {
            if (OverallBestLapTime.Value == null) return "DimGray";
            return "Magenta";
        }

        private void SetSessionBestLapTimes()
        {
            SessionBestLapTime.Value = CommonHelper.NullIf(NewData.BestLapTime, TimeSpan.Zero);

            if (CommonHelper.IsDifferent(NewData.BestLapTime, OldData.BestLapTime))
                PreviousSessionBestLapTime.Value = CommonHelper.NullIf(OldData.BestLapTime, TimeSpan.Zero);
        }

        private void SetAllTimeBestLapTimes()
        {
            AllTimeBestLapTime.Value = CommonHelper.NullIf(NewData.AllTimeBest, TimeSpan.Zero);

            if (CommonHelper.IsDifferent(NewData.AllTimeBest, OldData.AllTimeBest))
                PreviousAllTimeBestLapTime.Value = CommonHelper.NullIf(OldData.AllTimeBest, TimeSpan.Zero);
        }

        private void SetOverallBestLapTimes()
        {
            if (NewData.BestLapOpponent == null)
            {
                OverallBestLapTime.Value = null;
                PreviousOverallBestLapTime.Value = null;
                return;
            }

            OverallBestLapTime.Value = CommonHelper.NullIf(NewData.BestLapOpponent.BestLapTime, TimeSpan.Zero);

            if (OldData.BestLapOpponent == null)
                return;

            if (CommonHelper.IsDifferent(NewData.BestLapOpponent.BestLapTime, OldData.BestLapOpponent.BestLapTime))
                PreviousOverallBestLapTime.Value = CommonHelper.NullIf(OldData.BestLapOpponent.BestLapTime, TimeSpan.Zero);
        }

        private double? GetLastLapDeltaToSessionBest()
        {
            double? lastTime = CommonHelper.TimeSpanToSeconds(LastLapTime.Value);
            double? bestTime = CommonHelper.TimeSpanToSeconds(SessionBestLapTime.Value);
            double? oldBestTime = CommonHelper.TimeSpanToSeconds(PreviousSessionBestLapTime.Value);

            if (lastTime == null || bestTime == null)
                return null;

            if (lastTime == bestTime)
            {
                if (oldBestTime == null)
                    return null;
                return lastTime - oldBestTime;
            }
            else
                return lastTime - bestTime;
        }

        private double? GetLastLapDeltaToAllTimeBest()
        {
            double? lastTime = CommonHelper.TimeSpanToSeconds(LastLapTime.Value);
            double? bestTime = CommonHelper.TimeSpanToSeconds(AllTimeBestLapTime.Value);
            double? oldBestTime = CommonHelper.TimeSpanToSeconds(PreviousAllTimeBestLapTime.Value);

            if (lastTime == null || bestTime == null)
                return null;

            if (lastTime == bestTime)
            {
                if (oldBestTime == null)
                    return null;
                return lastTime - oldBestTime;
            }
            else
                return lastTime - bestTime;
        }

        private double? GetPredictedLapDeltaToSessionBest()
        {
            double? estimatedTime = CommonHelper.TimeSpanToSeconds((TimeSpan)Plugin.GetPropertyValue("PersistantTrackerPlugin.EstimatedLapTime"));
            double? bestTime = CommonHelper.TimeSpanToSeconds(SessionBestLapTime.Value);

            if (estimatedTime == null || bestTime == null)
                return null;

            return estimatedTime - bestTime;
        }
    }
}
