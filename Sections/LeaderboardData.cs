using ACSharedMemory.ACC.Reader;
using GameReaderCommon;
using IRacingReader;
using iRacingSDK;
using ksBroadcastingNetwork.Structs; // ACC
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class LeaderboardData : SectionBase
    {
        bool OpponentCountChanged;
        bool OpponentOrderChanged;
        bool SessionChanged;
        bool StaticDriverDataLoaded;
        bool IsRace;

        private List<Opponent> Opponents;

        // Sort order mirrors NewData.Opponents
        public Driver[] Drivers = new Driver[50];

        public Dictionary<int, Driver> DriversByPosition = new Dictionary<int, Driver>();

        public Dictionary<int, RealtimeCarUpdate> AccCars;
        public iRacingSDK.SessionData._DriverInfo._Drivers[] IRacingDrivers;

        public LeaderboardData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            if (AllGameData.GameData.GameName == "IRacing")
                IRacingDrivers = AllGameData.IRacingRawData.SessionData.DriverInfo.CompetingDrivers;
            else if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                AccCars = AllGameData.AccRawData.Cars;

            Opponents = NewData.Opponents;
            OpponentCountChanged = NewData.Opponents.Count != OldData.Opponents.Count;

            if (!OpponentCountChanged)
                OpponentOrderChanged = HasOpponentOrderChanged();

            SessionChanged = NewData.SessionTypeName != OldData.SessionTypeName;
            IsRace = NewData.SessionTypeName.ToLower().Contains("race");

            if (SessionChanged)
                ResetProperties();

            UpdateDrivers();

            OpponentOrderChanged = false;
        }

        private bool HasOpponentOrderChanged()
        {
            for (int i = 0; i < Opponents.Count; i++)
                if (NewData.Opponents[i].CarNumber != OldData.Opponents[i].CarNumber)
                    return true;

            return false;
        }

        private void UpdateDrivers()
        {
            bool updateAll = OpponentCountChanged || OpponentOrderChanged || SessionChanged || !StaticDriverDataLoaded;

            // Mostly static data - no need to keep updating frequently
            if (updateAll)
            {
                for (int i = 0; i < Opponents.Count; i++)
                {
                    Opponent opponent = Opponents[i];

                    Drivers[i].Name = opponent.Name;
                    Drivers[i].IsPlayer = opponent.IsPlayer;
                    Drivers[i].CarName = opponent.CarName;
                    Drivers[i].CarNumber = opponent.CarNumber;
                    Drivers[i].CarClass = NewData.CarClass;

                    if (AllGameData.GameData.GameName == "IRacing")
                    {
                        iRacingSDK.SessionData._DriverInfo._Drivers driver = IRacingHelper.GetDriverByNumber(AllGameData.IRacingRawData, opponent.CarNumber);

                        if (driver != null)
                        {
                            Drivers[i].CarClassColour = IRacingHelper.GetCarClassColour(driver.CarClassColor);
                            Drivers[i].CarClassTextColour = "White";
                            Drivers[i].IRacingIRating = opponent.IRacing_IRating;
                            Drivers[i].IRacingLicenseText = driver.LicString;
                            Drivers[i].IRacingLicenseTextColour = IRacingHelper.GetLicenseTextColour(driver.LicString);
                            Drivers[i].IRacingLicenseColour = IRacingHelper.GetLicenseColour(driver.LicString);
                        }
                    }
                    else if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                    {
                        RealtimeCarUpdate car = AccHelper.GetCarByNumber(AllGameData.AccRawData, opponent.CarNumber);

                        if (car != null)
                        {
                            Drivers[i].CarName = AccHelper.GetCarName(car.CarEntry.CarModelType);
                            Drivers[i].CarClass = AccHelper.GetCarClass(car.CarEntry.CarModelType);
                            Drivers[i].CarClassColour = AccHelper.GetCarLicenseColour(car.CarEntry.CupCategory);
                            Drivers[i].CarClassTextColour = AccHelper.GetCarLicenseTextColour(car.CarEntry.CupCategory);
                        }
                    }
                }

                StaticDriverDataLoaded = true;
            }

            if (Plugin.UpdateAt1Fps || updateAll)
            {
                for (int i = 0; i < Opponents.Count; i++)
                {
                    Opponent opponent = Opponents[i];
                    Opponent oldOpponent = OldData.Opponents.Where(o => o.CarNumber == opponent.CarNumber).FirstOrDefault();

                    Drivers[i].IsConnected = opponent.IsConnected;
                    Drivers[i].LapsCompleted = GetLapsCompleted(opponent);
                    Drivers[i].GapToLeader = IsRace ? opponent.GaptoLeader : null;

                    Drivers[i].LastLapTime = CommonHelper.NullIf(opponent.LastLapTime, TimeSpan.Zero);
                    Drivers[i].LastLapColour = GetLastLapColour(Drivers[i]);
                    Drivers[i].BestLapTime = CommonHelper.NullIf(opponent.BestLapTime, TimeSpan.Zero);
                    Drivers[i].BestLapColour = GetBestLapColour(Drivers[i]);

                    Drivers[i].Sector1LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector1);
                    Drivers[i].Sector2LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector2);
                    Drivers[i].Sector3LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector3);

                    TimeSpan?[] bestOverallSectors = SectorFunctions.GetOverallBestSectorTimes(NewData);
                    Drivers[i].Sector1LastLapColour = GetLastLapSectorColour(Drivers[i].Sector1LastLapTime, Drivers[i].Sector1BestTime, bestOverallSectors[0]);
                    Drivers[i].Sector2LastLapColour = GetLastLapSectorColour(Drivers[i].Sector2LastLapTime, Drivers[i].Sector2BestTime, bestOverallSectors[1]);
                    Drivers[i].Sector3LastLapColour = GetLastLapSectorColour(Drivers[i].Sector3LastLapTime, Drivers[i].Sector3BestTime, bestOverallSectors[2]);

                    Drivers[i].Sector1BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector1);
                    Drivers[i].Sector2BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector2);
                    Drivers[i].Sector3BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector3);

                    Drivers[i].InPitLane = opponent.IsCarInPitLane;
                    Drivers[i].InPitBox = opponent.IsCarInPit;
                }

                SetPositions();
                //SetPositionInClass(Drivers);

                for (int i = 0; i < Opponents.Count; i++)
                    Drivers[i].IntervalGap = GetIntervalGap(Drivers[i]);

                for (int i = 0; i <= Opponents.Count; i++)
                    DriversByPosition[i + 1] = Drivers
                        .OrderBy(x => x.Position == 0)
                        .ThenBy(x => x.Position)
                        .ElementAt(i);
            }
        }

        protected override void Init(PluginManager pluginManager)
        {
            ResetProperties();
            AttachDelegates();
        }

        private void ResetProperties()
        {
            StaticDriverDataLoaded = false;

            for (int i = 0; i < Drivers.Length; i++)
            {
                Drivers[i] = new Driver();
                DriversByPosition[i + 1] = new Driver();
            }
        }

        private void AttachDelegates()
        {
            for (int i = 0; i < Drivers.Length; ++i)
            {
                int pos = i + 1;

                string propPrefix = $"LeaderboardData.Position{pos:00}.";

                Plugin.AttachDelegate($"{propPrefix}Name", () => DriversByPosition[pos].Name);

                Plugin.AttachDelegate($"{propPrefix}IsConnected", () => DriversByPosition[pos].IsConnected);
                Plugin.AttachDelegate($"{propPrefix}IsPlayer", () => DriversByPosition[pos].IsPlayer);

                Plugin.AttachDelegate($"{propPrefix}Position", () => DriversByPosition[pos].Position);
                Plugin.AttachDelegate($"{propPrefix}PositionInClass", () => DriversByPosition[pos].PositionInClass);

                Plugin.AttachDelegate($"{propPrefix}iRacing.iRating", () => DriversByPosition[pos].IRacingIRating);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseText", () => DriversByPosition[pos].IRacingLicenseText);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseTextColour", () => DriversByPosition[pos].IRacingLicenseTextColour);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseColour", () => DriversByPosition[pos].IRacingLicenseColour);

                Plugin.AttachDelegate($"{propPrefix}CarName", () => DriversByPosition[pos].CarName);
                Plugin.AttachDelegate($"{propPrefix}CarClass", () => DriversByPosition[pos].CarClass);
                Plugin.AttachDelegate($"{propPrefix}CarClassColour", () => DriversByPosition[pos].CarClassColour);
                Plugin.AttachDelegate($"{propPrefix}CarClassTextColour", () => DriversByPosition[pos].CarClassTextColour);

                Plugin.AttachDelegate($"{propPrefix}LapsCompleted", () => DriversByPosition[pos].LapsCompleted);

                Plugin.AttachDelegate($"{propPrefix}GapToLeader", () => DriversByPosition[pos].GapToLeader);
                Plugin.AttachDelegate($"{propPrefix}IntervalGap", () => DriversByPosition[pos].IntervalGap);

                Plugin.AttachDelegate($"{propPrefix}LastLapTime", () => DriversByPosition[pos].LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}LastLapColour", () => DriversByPosition[pos].LastLapColour);

                Plugin.AttachDelegate($"{propPrefix}BestLapTime", () => DriversByPosition[pos].BestLapTime);
                Plugin.AttachDelegate($"{propPrefix}BestLapColour", () => DriversByPosition[pos].BestLapColour);

                Plugin.AttachDelegate($"{propPrefix}Sector1LastLapTime", () => DriversByPosition[pos].Sector1LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}Sector2LastLapTime", () => DriversByPosition[pos].Sector2LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}Sector3LastLapTime", () => DriversByPosition[pos].Sector3LastLapTime);

                Plugin.AttachDelegate($"{propPrefix}Sector1BestTime", () => DriversByPosition[pos].Sector1BestTime);
                Plugin.AttachDelegate($"{propPrefix}Sector2BestTime", () => DriversByPosition[pos].Sector2BestTime);
                Plugin.AttachDelegate($"{propPrefix}Sector3BestTime", () => DriversByPosition[pos].Sector3BestTime);

                Plugin.AttachDelegate($"{propPrefix}Sector1LastLapColour", () => DriversByPosition[pos].Sector1LastLapColour);
                Plugin.AttachDelegate($"{propPrefix}Sector2LastLapColour", () => DriversByPosition[pos].Sector2LastLapColour);
                Plugin.AttachDelegate($"{propPrefix}Sector3LastLapColour", () => DriversByPosition[pos].Sector3LastLapColour);

                Plugin.AttachDelegate($"{propPrefix}InPitLane", () => DriversByPosition[pos].InPitLane);
                Plugin.AttachDelegate($"{propPrefix}InPitBox", () => DriversByPosition[pos].InPitBox);
            }
        }

        private void SetPositions()
        {
            Dictionary<int, object> positionBasis = new Dictionary<int, object>();

            if (IsRace)
                for (int i = 0; i < Opponents.Count; i++)
                    positionBasis[i] = Opponents[i].Position;
            else
                for (int i = 0; i < Opponents.Count; i++)
                    if (Opponents[i].BestLapTime != TimeSpan.Zero)
                        positionBasis[i] = (Opponents[i].BestLapTime, Opponents[i].Position);

            List<int> orderedPositions = positionBasis.OrderBy(pair => pair.Value).Select(pair => pair.Key).ToList();

            for (int i = 0; i < Opponents.Count; i++)
                Drivers[i].Position = orderedPositions.IndexOf(i) + 1;
        }

        public void SetPositionInClass(Driver[] drivers)
        {
            foreach (string carClass in drivers.Where(d => d.CarClass != null).Select(d => d.CarClass).Distinct())
            {
                var driversInClass = Drivers.Where(d => d.CarClass == carClass).OrderBy(d => d.Position).ToList();
                foreach (Driver driver in driversInClass)
                    driver.PositionInClass = driversInClass.IndexOf(driver) + 1;
            }
        }

        public Driver GetDriverByPosition(int position)
        {
            var driver = Drivers.Where(d => d.Position == position);
            return driver.Any() ? driver.First() : null;
        }

        public string GetLastLapColour(Driver driver)
        {
            if (driver.LastLapTime == null)
                return "DimGray";

            if (NewData.BestLapOpponent == null)
                return "White";

            TimeSpan? overallBestLapTime = CommonHelper.NullIf(NewData.BestLapOpponent.BestLapTime, TimeSpan.Zero);
            return driver.LastLapTime == overallBestLapTime ? "Magenta" : "White";
        }

        public string GetBestLapColour(Driver driver)
        {
            if (driver.BestLapTime == null)
                return "DimGray";

            if (NewData.BestLapOpponent == null)
                return "White";

            TimeSpan? overallBestLapTime = CommonHelper.NullIf(NewData.BestLapOpponent.BestLapTime, TimeSpan.Zero);
            return driver.BestLapTime == overallBestLapTime ? "Magenta" : "White";
        }

        public double? GetLapsCompleted(Opponent opponent)
        {
            if (!IsRace)
                return null;

            if (NewData.CurrentLap > 0 && NewData.CurrentLapTime > TimeSpan.Zero)
                return opponent.CurrentLap - 1 + opponent.TrackPositionPercent ?? 0;

            return null;
        }

        public double? GetIntervalGap(Driver driver)
        {
            if (!IsRace)
                return null;

            if (driver.Position == 1)
                return null;

            var carAhead = GetDriverByPosition(driver.Position - 1);
            return carAhead != null ? driver.GapToLeader - carAhead.GapToLeader : null;
        }

        public string GetLastLapSectorColour(TimeSpan? lastSector, TimeSpan? bestSector, TimeSpan? bestOverallSector)
        {
            if (lastSector == null) return "DimGray";
            if (lastSector == bestOverallSector) return "Magenta";
            if (bestSector == null) return "LimeGreen";
            if (lastSector == bestSector) return "LimeGreen";
            return "Yellow";
        }
    }
}
