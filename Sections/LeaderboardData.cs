using GameReaderCommon;
using ksBroadcastingNetwork.Structs; // ACC
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Driver[] Drivers = new Driver[Settings.LeaderboardNumberOfDrivers];

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
                Parallel.For(0, Opponents.Count, i =>
                {
                    Opponent opponent = Opponents[i];

                    Drivers[i].Name = opponent.Name;
                    Drivers[i].IsPlayer = opponent.IsPlayer;
                    Drivers[i].Position = opponent.Position;
                    Drivers[i].PositionInClass = opponent.PositionInClass;
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
                });

                StaticDriverDataLoaded = true;
            }

            if (Plugin.UpdateAt5Fps || updateAll)
            {
                Parallel.For(0, Opponents.Count, i =>
                {
                    Opponent opponent = Opponents[i];

                    Drivers[i].IsConnected = opponent.IsConnected;

                    // ACC current lap starts at 0
                    Drivers[i].CurrentLap = AllGameData.GameData.GameName == "AssettoCorsaCompetizione" ? opponent.CurrentLap : opponent.CurrentLap - 1;
                    Drivers[i].CurrentLapDistance = opponent.TrackPositionPercent;
                    Drivers[i].LapsCompleted = GetLapsCompleted(opponent, Drivers[i].CurrentLap);

                    Drivers[i].GapToLeader = IsRace ? opponent.GaptoLeader : null;
                    Drivers[i].IntervalGap = GetIntervalGap(Drivers[i]);

                    Drivers[i].LastLapTime = CommonHelper.NullIf(opponent.LastLapTime, TimeSpan.Zero);
                    Drivers[i].LastLapColour = GetLastLapColour(Drivers[i]);
                    Drivers[i].BestLapTime = CommonHelper.NullIf(opponent.BestLapTime, TimeSpan.Zero);
                    Drivers[i].BestLapColour = GetBestLapColour(Drivers[i]);

                    Drivers[i].Sector1LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector1);
                    Drivers[i].Sector2LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector2);
                    Drivers[i].Sector3LastLapTime = CommonHelper.MillisecondsToTimeSpan(opponent.LastLapSector3);

                    TimeSpan?[] bestOverallSectors = SectorData.GetOverallBestSectorTimes(NewData);
                    Drivers[i].Sector1LastLapColour = GetLastLapSectorColour(Drivers[i].Sector1LastLapTime, Drivers[i].Sector1BestTime, bestOverallSectors[0]);
                    Drivers[i].Sector2LastLapColour = GetLastLapSectorColour(Drivers[i].Sector2LastLapTime, Drivers[i].Sector2BestTime, bestOverallSectors[1]);
                    Drivers[i].Sector3LastLapColour = GetLastLapSectorColour(Drivers[i].Sector3LastLapTime, Drivers[i].Sector3BestTime, bestOverallSectors[2]);

                    Drivers[i].Sector1BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector1);
                    Drivers[i].Sector2BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector2);
                    Drivers[i].Sector3BestTime = CommonHelper.MillisecondsToTimeSpan(opponent.BestLapSector3);

                    Drivers[i].InPitLane = opponent.IsCarInPitLane;
                    Drivers[i].InPitBox = opponent.IsCarInPit;
                });

                // If setting positions manually i.e. using lap distance, will use the below. Prop delegates would be e.g. DriversByPosition[i + 1].Name

                //SetPositions();
                //SetPositionInClass();

                //var orderedDrivers = Drivers
                //    .OrderBy(x => x.Position == 0)
                //    .ThenBy(x => x.Position);

                //for (int i = 0; i < Opponents.Count; i++)
                //    DriversByPosition[i + 1] = orderedDrivers.ElementAt(i);
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
                string propPrefix = $"LeaderboardData.Position{i + 1:00}.";

                Plugin.AttachDelegate($"{propPrefix}Name", () => Drivers[i].Name);

                Plugin.AttachDelegate($"{propPrefix}IsConnected", () => Drivers[i].IsConnected);
                Plugin.AttachDelegate($"{propPrefix}IsPlayer", () => Drivers[i].IsPlayer);

                Plugin.AttachDelegate($"{propPrefix}Position", () => Drivers[i].Position);
                Plugin.AttachDelegate($"{propPrefix}PositionInClass", () => Drivers[i].PositionInClass);

                Plugin.AttachDelegate($"{propPrefix}iRacing.iRating", () => Drivers[i].IRacingIRating);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseText", () => Drivers[i].IRacingLicenseText);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseTextColour", () => Drivers[i].IRacingLicenseTextColour);
                Plugin.AttachDelegate($"{propPrefix}iRacing.LicenseColour", () => Drivers[i].IRacingLicenseColour);

                Plugin.AttachDelegate($"{propPrefix}CarName", () => Drivers[i].CarName);
                Plugin.AttachDelegate($"{propPrefix}CarClass", () => Drivers[i].CarClass);
                Plugin.AttachDelegate($"{propPrefix}CarClassColour", () => Drivers[i].CarClassColour);
                Plugin.AttachDelegate($"{propPrefix}CarClassTextColour", () => Drivers[i].CarClassTextColour);

                Plugin.AttachDelegate($"{propPrefix}CurrentLap", () => Drivers[i].CurrentLap);
                Plugin.AttachDelegate($"{propPrefix}CurrentLapDistance", () => Drivers[i].CurrentLapDistance);
                Plugin.AttachDelegate($"{propPrefix}LapsCompleted", () => Drivers[i].LapsCompleted);

                Plugin.AttachDelegate($"{propPrefix}GapToLeader", () => Drivers[i].GapToLeader);
                Plugin.AttachDelegate($"{propPrefix}IntervalGap", () => Drivers[i].IntervalGap);

                Plugin.AttachDelegate($"{propPrefix}LastLapTime", () => Drivers[i].LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}LastLapColour", () => Drivers[i].LastLapColour);

                Plugin.AttachDelegate($"{propPrefix}BestLapTime", () => Drivers[i].BestLapTime);
                Plugin.AttachDelegate($"{propPrefix}BestLapColour", () => Drivers[i].BestLapColour);

                Plugin.AttachDelegate($"{propPrefix}Sector1LastLapTime", () => Drivers[i].Sector1LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}Sector2LastLapTime", () => Drivers[i].Sector2LastLapTime);
                Plugin.AttachDelegate($"{propPrefix}Sector3LastLapTime", () => Drivers[i].Sector3LastLapTime);

                Plugin.AttachDelegate($"{propPrefix}Sector1BestTime", () => Drivers[i].Sector1BestTime);
                Plugin.AttachDelegate($"{propPrefix}Sector2BestTime", () => Drivers[i].Sector2BestTime);
                Plugin.AttachDelegate($"{propPrefix}Sector3BestTime", () => Drivers[i].Sector3BestTime);

                Plugin.AttachDelegate($"{propPrefix}Sector1LastLapColour", () => Drivers[i].Sector1LastLapColour);
                Plugin.AttachDelegate($"{propPrefix}Sector2LastLapColour", () => Drivers[i].Sector2LastLapColour);
                Plugin.AttachDelegate($"{propPrefix}Sector3LastLapColour", () => Drivers[i].Sector3LastLapColour);

                Plugin.AttachDelegate($"{propPrefix}InPitLane", () => Drivers[i].InPitLane);
                Plugin.AttachDelegate($"{propPrefix}InPitBox", () => Drivers[i].InPitBox);
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

        public void SetPositionInClass()
        {
            foreach (string carClass in Drivers.Where(d => d.CarClass != null).Select(d => d.CarClass).Distinct())
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

        public double? GetLapsCompleted(Opponent opponent, int? currentLap)
        {
            if (!IsRace || currentLap == null)
                return null;

            return currentLap + opponent.TrackPositionPercent ?? 0;
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

        public string GetLastLapColour(Driver driver)
        {
            if (driver.LastLapTime == null)
                return Settings.NullValueColour;

            if (NewData.BestLapOpponent == null)
                return Settings.GeneralValueColour;

            TimeSpan? overallBestLapTime = CommonHelper.NullIf(NewData.BestLapOpponent.BestLapTime, TimeSpan.Zero);
            return driver.LastLapTime == overallBestLapTime ? Settings.OverallBestTimeColour : Settings.GeneralValueColour;
        }

        public string GetBestLapColour(Driver driver)
        {
            if (driver.BestLapTime == null)
                return Settings.NullValueColour;

            if (NewData.BestLapOpponent == null)
                return Settings.GeneralValueColour;

            TimeSpan? overallBestLapTime = CommonHelper.NullIf(NewData.BestLapOpponent.BestLapTime, TimeSpan.Zero);
            return driver.BestLapTime == overallBestLapTime ? Settings.OverallBestTimeColour : Settings.GeneralValueColour;
        }

        public string GetLastLapSectorColour(TimeSpan? lastSector, TimeSpan? bestSector, TimeSpan? bestOverallSector)
        {
            if (lastSector == null) return Settings.NullValueColour;
            if (lastSector == bestOverallSector) return Settings.OverallBestTimeColour;
            if (bestSector == null) return Settings.PersonalBestTimeColour;
            if (lastSector == bestSector) return Settings.PersonalBestTimeColour;
            return Settings.SectorTimeNotImprovedColour;
        }
    }
}
