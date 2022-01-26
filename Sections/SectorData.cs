using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class SectorData : SectionBase
    {
        public AttachedProperty<TimeSpan?> SectorLastTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<double?> SectorLastDeltaToSessionBest = new AttachedProperty<double?>();

        public AttachedProperty<TimeSpan?> Sector1LastTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> Sector1LastTimeColour = new AttachedProperty<string>();
        public AttachedProperty<TimeSpan?> Sector2LastTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> Sector2LastTimeColour = new AttachedProperty<string>();
        public AttachedProperty<TimeSpan?> Sector3LastTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<string> Sector3LastTimeColour = new AttachedProperty<string>();

        public AttachedProperty<TimeSpan?> Sector1BestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector1PreviousBestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector2BestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector2PreviousBestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector3BestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector3PreviousBestTime = new AttachedProperty<TimeSpan?>();

        public AttachedProperty<TimeSpan?> Sector1OverallBestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector2OverallBestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector3OverallBestTime = new AttachedProperty<TimeSpan?>();

        public AttachedProperty<double?> Sector1DeltaToSessionBest = new AttachedProperty<double?>();
        public AttachedProperty<double?> Sector2DeltaToSessionBest = new AttachedProperty<double?>();
        public AttachedProperty<double?> Sector3DeltaToSessionBest = new AttachedProperty<double?>();

        public SectorData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            SetLastSectorTimes();
            SetPersonalBestSectorTimes();
            SetOverallBestSectorTimes();
            SetLastSectorDeltaToPersonalBests();
            SetLastSectorColours();
        }

        protected override void Init(PluginManager pluginManager)
        {
            Plugin.AttachProperty("SectorData.SectorLastTime", SectorLastTime);
            Plugin.AttachProperty("SectorData.SectorLastDeltaToSessionBest", SectorLastDeltaToSessionBest);

            Plugin.AttachProperty("SectorData.Sector1LastTime", Sector1LastTime);
            Plugin.AttachProperty("SectorData.Sector1LastTimeColour", Sector1LastTimeColour);
            Plugin.AttachProperty("SectorData.Sector2LastTime", Sector2LastTime);
            Plugin.AttachProperty("SectorData.Sector2LastTimeColour", Sector2LastTimeColour);
            Plugin.AttachProperty("SectorData.Sector3LastTime", Sector3LastTime);
            Plugin.AttachProperty("SectorData.Sector3LastTimeColour", Sector3LastTimeColour);

            Plugin.AttachProperty("SectorData.Sector1BestTime", Sector1BestTime);
            Plugin.AttachProperty("SectorData.Sector1PreviousBestTime", Sector1PreviousBestTime);
            Plugin.AttachProperty("SectorData.Sector2BestTime", Sector2BestTime);
            Plugin.AttachProperty("SectorData.Sector2PreviousBestTime", Sector2PreviousBestTime);
            Plugin.AttachProperty("SectorData.Sector3BestTime", Sector3BestTime);
            Plugin.AttachProperty("SectorData.Sector3PreviousBestTime", Sector3PreviousBestTime);

            Plugin.AttachProperty("SectorData.Sector1OverallBestTime", Sector1OverallBestTime);
            Plugin.AttachProperty("SectorData.Sector2OverallBestTime", Sector2OverallBestTime);
            Plugin.AttachProperty("SectorData.Sector3OverallBestTime", Sector3OverallBestTime);

            Plugin.AttachProperty("SectorData.Sector1DeltaToSessionBest", Sector1DeltaToSessionBest);
            Plugin.AttachProperty("SectorData.Sector2DeltaToSessionBest", Sector2DeltaToSessionBest);
            Plugin.AttachProperty("SectorData.Sector3DeltaToSessionBest", Sector3DeltaToSessionBest);
        }

        public int? GetCurrentSector()
        {
            return CommonHelper.NullIf(NewData.CurrentSectorIndex, 0);
        }

        public int? GetPreviousSector()
        {
            int? currentSector = GetCurrentSector();
            if (currentSector == null) return null;

            if (currentSector == 1) return 3;
            if (currentSector == 2) return 1;
            if (currentSector == 3) return 2;
            return null;
        }

        public void SetLastSectorTimes()
        {
            SectorLastTime.Value = GetLastSectorTime();
            Sector1LastTime.Value = GetLastSectorTime(1);
            Sector2LastTime.Value = GetLastSectorTime(2);
            Sector3LastTime.Value = GetLastSectorTime(3);
        }

        public TimeSpan? GetLastSectorTime()
        {
            int? currentSector = GetCurrentSector();
            if (currentSector == null) return null;
            return GetLastSectorTime((int)currentSector);
        }

        public TimeSpan? GetLastSectorTime(int sectorNum)
        {
            TimeSpan? sectorTime = null;
            if (NewData.CurrentSectorIndex <= sectorNum)
            {
                if (sectorNum == 1) sectorTime = NewData.Sector1LastLapTime;
                if (sectorNum == 2) sectorTime = NewData.Sector2LastLapTime;
                if (sectorNum == 3) sectorTime = NewData.Sector3LastLapTime;
            }
            else
            {
                if (sectorNum == 1) sectorTime = NewData.Sector1Time;
                if (sectorNum == 2) sectorTime = NewData.Sector2Time;
            }

            return sectorTime;
        }

        public void SetPersonalBestSectorTimes()
        {
            Sector1BestTime.Value = NewData.Sector1BestTime;
            Sector2BestTime.Value = NewData.Sector2BestTime;
            Sector3BestTime.Value = NewData.Sector3BestTime;

            if (CommonHelper.IsDifferent(NewData.Sector1BestTime, OldData.Sector1BestTime))
                Sector1PreviousBestTime.Value = OldData.Sector1BestTime;

            if (CommonHelper.IsDifferent(NewData.Sector2BestTime, OldData.Sector2BestTime))
                Sector2PreviousBestTime.Value = OldData.Sector2BestTime;

            if (CommonHelper.IsDifferent(NewData.Sector3BestTime, OldData.Sector3BestTime))
                Sector3PreviousBestTime.Value = OldData.Sector3BestTime;
        }

        public void SetOverallBestSectorTimes()
        {
            List<double?> bestSector1s = new List<double?>();
            List<double?> bestSector2s = new List<double?>();
            List<double?> bestSector3s = new List<double?>();

            foreach (Opponent o in NewData.Opponents)
            {
                bestSector1s.Add(o.BestSector1);
                bestSector2s.Add(o.BestSector2);
                bestSector3s.Add(o.BestSector3);
            }

            Sector1OverallBestTime.Value = CommonHelper.ToNullableTimeSpan(bestSector1s.Where(t => t.HasValue).Min());
            Sector2OverallBestTime.Value = CommonHelper.ToNullableTimeSpan(bestSector2s.Where(t => t.HasValue).Min());
            Sector3OverallBestTime.Value = CommonHelper.ToNullableTimeSpan(bestSector3s.Where(t => t.HasValue).Min());
        }

        public void SetLastSectorDeltaToPersonalBests()
        {
            Sector1DeltaToSessionBest.Value = CommonHelper.RoundDelta(GetLastSectorDeltaToPersonalBest(Sector1LastTime.Value, Sector1BestTime.Value, Sector1PreviousBestTime.Value));
            Sector2DeltaToSessionBest.Value = CommonHelper.RoundDelta(GetLastSectorDeltaToPersonalBest(Sector2LastTime.Value, Sector2BestTime.Value, Sector2PreviousBestTime.Value));
            Sector3DeltaToSessionBest.Value = CommonHelper.RoundDelta(GetLastSectorDeltaToPersonalBest(Sector3LastTime.Value, Sector3BestTime.Value, Sector3PreviousBestTime.Value));

            int? previousSector = GetPreviousSector();
            if (previousSector == 1) SectorLastDeltaToSessionBest.Value = Sector1DeltaToSessionBest.Value;
            if (previousSector == 2) SectorLastDeltaToSessionBest.Value = Sector2DeltaToSessionBest.Value;
            if (previousSector == 3) SectorLastDeltaToSessionBest.Value = Sector3DeltaToSessionBest.Value;
        }

        public double? GetLastSectorDeltaToPersonalBest(TimeSpan? sectorLastTime, TimeSpan? sectorBestTime, TimeSpan? oldSectorBestTime)
        {
            if (sectorLastTime == null || sectorBestTime == null) return null;

            double? lastTime = CommonHelper.TimeSpanToSeconds(sectorLastTime);
            double? bestTime = CommonHelper.TimeSpanToSeconds(sectorBestTime);
            double? oldBestTime = CommonHelper.TimeSpanToSeconds(oldSectorBestTime);

            if (lastTime == bestTime)
            {
                if (oldBestTime == null) return null;
                return lastTime - oldBestTime;
            }
            else
                return lastTime - bestTime;
        }

        private void SetLastSectorColours()
        {
            Sector1LastTimeColour.Value = GetLastSectorColour(1, Sector1LastTime.Value, Sector1BestTime.Value, Sector1OverallBestTime.Value);
            Sector2LastTimeColour.Value = GetLastSectorColour(2, Sector2LastTime.Value, Sector2BestTime.Value, Sector2OverallBestTime.Value);
            Sector3LastTimeColour.Value = GetLastSectorColour(3, Sector3LastTime.Value, Sector3BestTime.Value, Sector3OverallBestTime.Value);
        }

        private string GetLastSectorColour(int sectorIndex, TimeSpan? sectorLastTime, TimeSpan? sectorBestTime, TimeSpan? sectorOverallBestTime)
        {
            if (GetCurrentSector() == null || !sectorLastTime.HasValue) return "DimGray";

            bool isCurrentLap = sectorIndex < GetCurrentSector();

            if (!sectorOverallBestTime.HasValue) return isCurrentLap ? "Fuchsia" : "#FF5A005A";

            // Bug where opponent.BestSectorN is 1ms higher
            if (sectorLastTime <= sectorOverallBestTime.Value.Add(TimeSpan.FromMilliseconds(1))) return isCurrentLap ? "Fuchsia" : "#FF5A005A";
            if (!sectorBestTime.HasValue || sectorLastTime <= sectorBestTime) return isCurrentLap ? "LimeGreen" : "#FF004600";
            return isCurrentLap ? "Yellow" : "#FF4F4F00";
        }
    }
}
