using SimHub.Plugins;
using System;

namespace AidanKay.ExtraDataPlugin
{
    internal class Driver
    {
        public AttachedProperty<int> Position = new AttachedProperty<int>();
        public AttachedProperty<int> PositionInClass = new AttachedProperty<int>();

        public AttachedProperty<string> CarName = new AttachedProperty<string>();

        public AttachedProperty<string> CarClass = new AttachedProperty<string>();
        public AttachedProperty<string> CarClassColour = new AttachedProperty<string>();
        public AttachedProperty<string> CarClassTextColour = new AttachedProperty<string>();

        public AttachedProperty<double?> IntervalGap = new AttachedProperty<double?>();

        public AttachedProperty<double> LapsCompleted = new AttachedProperty<double>();

        public AttachedProperty<bool> InPitLane = new AttachedProperty<bool>();
        public AttachedProperty<bool> InPitBox = new AttachedProperty<bool>();

        public AttachedProperty<TimeSpan?> LastLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector1LastLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector2LastLapTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector3LastLapTime = new AttachedProperty<TimeSpan?>();

        public AttachedProperty<TimeSpan?> Sector1BestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector2BestTime = new AttachedProperty<TimeSpan?>();
        public AttachedProperty<TimeSpan?> Sector3BestTime = new AttachedProperty<TimeSpan?>();

        public AttachedProperty<string> Sector1LastLapColour = new AttachedProperty<string>();
        public AttachedProperty<string> Sector2LastLapColour = new AttachedProperty<string>();
        public AttachedProperty<string> Sector3LastLapColour = new AttachedProperty<string>();
    }
}
