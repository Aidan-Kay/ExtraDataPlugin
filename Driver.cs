using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin
{
    internal class Driver
    {
        public string Name;

        public bool IsConnected;
        public bool IsPlayer;

        public int Position;
        public int PositionInClass;

        public string CarName;
        public string CarNumber;

        public string CarClass;
        public string CarClassColour;
        public string CarClassTextColour;

        public double? IRacingIRating;
        public string IRacingLicenseText;
        public string IRacingLicenseTextColour;
        public string IRacingLicenseColour;

        public int? CurrentLap;
        public double? CurrentLapDistance;
        public double? LapsCompleted;

        public double? GapToLeader;
        public double? IntervalGap;

        public TimeSpan? LastLapTime;
        public string LastLapColour;

        public TimeSpan? BestLapTime;
        public string BestLapColour;

        public TimeSpan? Sector1LastLapTime;
        public TimeSpan? Sector2LastLapTime;
        public TimeSpan? Sector3LastLapTime;

        public TimeSpan? Sector1BestTime;
        public TimeSpan? Sector2BestTime;
        public TimeSpan? Sector3BestTime;

        public string Sector1LastLapColour;
        public string Sector2LastLapColour;
        public string Sector3LastLapColour;

        public bool InPitLane;
        public bool InPitBox;
    }
}
