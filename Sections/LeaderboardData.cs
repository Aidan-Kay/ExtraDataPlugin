using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Linq;

namespace AidanKay.ExtraDataPlugin.Sections
{
    internal class LeaderboardData : SectionBase
    {
        public Driver[] Drivers = new Driver[Settings.LeaderboardNumberOfDrivers];

        public LeaderboardData(ExtraDataPlugin extraDataPlugin) : base(extraDataPlugin) { }

        public override void DataUpdate()
        {
            UpdateDrivers();
        }

        protected override void AttachProperties(PluginManager pluginManager)
        {
            for (int i = 0; i < Drivers.Length; i++)
            {
                Drivers[i] = new Driver();

                string propPrefix = $"LeaderboardData.Position{i + 1:00}.";

                Plugin.AttachProperty($"{propPrefix}Position", Drivers[i].Position);
                Plugin.AttachProperty($"{propPrefix}PositionInClass", Drivers[i].PositionInClass);

                Plugin.AttachProperty($"{propPrefix}CarName", Drivers[i].CarName);
                Plugin.AttachProperty($"{propPrefix}CarClass", Drivers[i].CarClass);
                Plugin.AttachProperty($"{propPrefix}CarClassColour", Drivers[i].CarClassColour);
                Plugin.AttachProperty($"{propPrefix}CarClassTextColour", Drivers[i].CarClassTextColour);

                Plugin.AttachProperty($"{propPrefix}IntervalGap", Drivers[i].IntervalGap);

                Plugin.AttachProperty($"{propPrefix}LapsCompleted", Drivers[i].LapsCompleted);

                Plugin.AttachProperty($"{propPrefix}InPitLane", Drivers[i].InPitLane);
                Plugin.AttachProperty($"{propPrefix}InPitBox", Drivers[i].InPitBox);

                Plugin.AttachProperty($"{propPrefix}LastLapTime", Drivers[i].LastLapTime);

                Plugin.AttachProperty($"{propPrefix}Sector1LastLapTime", Drivers[i].Sector1LastLapTime);
                Plugin.AttachProperty($"{propPrefix}Sector2LastLapTime", Drivers[i].Sector2LastLapTime);
                Plugin.AttachProperty($"{propPrefix}Sector3LastLapTime", Drivers[i].Sector3LastLapTime);

                Plugin.AttachProperty($"{propPrefix}Sector1BestTime", Drivers[i].Sector1BestTime);
                Plugin.AttachProperty($"{propPrefix}Sector2BestTime", Drivers[i].Sector2BestTime);
                Plugin.AttachProperty($"{propPrefix}Sector3BestTime", Drivers[i].Sector3BestTime);

                Plugin.AttachProperty($"{propPrefix}Sector1LastLapColour", Drivers[i].Sector1LastLapColour);
                Plugin.AttachProperty($"{propPrefix}Sector2LastLapColour", Drivers[i].Sector2LastLapColour);
                Plugin.AttachProperty($"{propPrefix}Sector3LastLapColour", Drivers[i].Sector3LastLapColour);
            }
        }

        private void UpdateDrivers()
        {
            if (Plugin.UpdateAt5Fps)
            {
                for (int i = 0; i < Drivers.Length; i++)
                {
                    Opponent opponent = NewData.OpponentAtPosition(i + 1, false, false, false);

                    if (opponent == null)
                        return;

                    // TODO:
                    // iR car classes & colours
                    // iR license text
                    // iR inc count
                    // pit count

                    Drivers[i].Position.Value = opponent.Position;
                    Drivers[i].CarName.Value = opponent.CarName;
                    Drivers[i].IntervalGap.Value = GetIntervalGap(Drivers[i], opponent);
                    Drivers[i].LapsCompleted.Value = GetLapsCompleted(opponent);
                    Drivers[i].InPitLane.Value = opponent.IsCarInPitLane;
                    Drivers[i].InPitBox.Value = opponent.IsCarInPit;

                    Drivers[i].LastLapTime.Value = CommonHelper.NullIf(opponent.LastLapTime, TimeSpan.Zero);

                    SetLastLapSectorTimes(Drivers[i], opponent);
                    SetBestSectorTimes(Drivers[i], opponent);

                    TimeSpan?[] bestOverallSectors = SectorFunctions.GetOverallBestSectorTimes(NewData);
                    Drivers[i].Sector1LastLapColour.Value = GetLastLapSectorColour(Drivers[i].Sector1LastLapTime.Value, Drivers[i].Sector1BestTime.Value, bestOverallSectors[0]);
                    Drivers[i].Sector2LastLapColour.Value = GetLastLapSectorColour(Drivers[i].Sector2LastLapTime.Value, Drivers[i].Sector2BestTime.Value, bestOverallSectors[1]);
                    Drivers[i].Sector3LastLapColour.Value = GetLastLapSectorColour(Drivers[i].Sector3LastLapTime.Value, Drivers[i].Sector3BestTime.Value, bestOverallSectors[2]);

                    if (AllGameData.GameData.GameName == "AssettoCorsaCompetizione")
                    {
                        ksBroadcastingNetwork.Structs.RealtimeCarUpdate car = AccHelper.GetCarByPosition(AllGameData.AccRawGameData, opponent.Position);
                        Drivers[i].CarName.Value = AccHelper.GetCarName(car.CarEntry.CarModelType);
                        Drivers[i].CarClass.Value = AccHelper.GetCarClass(car.CarEntry.CarModelType);
                        Drivers[i].CarClassColour.Value = AccHelper.GetCarLicenseColour(car.CarEntry.CupCategory);
                        Drivers[i].CarClassTextColour.Value = AccHelper.GetCarLicenseTextColour(car.CarEntry.CupCategory);
                    }
                    else
                    {
                        Drivers[i].CarClass.Value = NewData.CarClass;
                    }
                }

                SetPositionInClass(Drivers);
            }
        }

        public double? GetIntervalGap(Driver driver, Opponent thisCar)
        {
            if (driver.Position.Value == 1)
                return null;

            Opponent carAhead = NewData.OpponentAtPosition(driver.Position.Value - 1, false, false, false);
            return thisCar.GaptoLeader - carAhead.GaptoLeader;
        }

        public double GetLapsCompleted(Opponent opponent)
        {
            if (NewData.CurrentLap > 0 && NewData.CurrentLapTime > TimeSpan.Zero)
                return opponent.CurrentLap + opponent.TrackPositionPercent ?? 0;
            return 0;
        }

        public void SetLastLapSectorTimes(Driver driver, Opponent thisCar)
        {
            driver.Sector1LastLapTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.LastLapSector1);
            driver.Sector2LastLapTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.LastLapSector2);
            driver.Sector3LastLapTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.LastLapSector3);
        }

        public string GetLastLapSectorColour(TimeSpan? lastSector, TimeSpan? bestSector, TimeSpan? bestOverallSector)
        {
            if (lastSector == null) return "DimGray";
            if (lastSector == bestOverallSector) return "Magenta";
            if (bestSector == null) return "LimeGreen";
            if (lastSector == bestSector) return "LimeGreen";
            return "Yellow";
        }

        public void SetBestSectorTimes(Driver driver, Opponent thisCar)
        {
            driver.Sector1BestTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.BestLapSector1);
            driver.Sector2BestTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.BestLapSector2);
            driver.Sector3BestTime.Value = CommonHelper.MillisecondsToTimeSpan(thisCar.BestLapSector3);
        }

        public void SetPositionInClass(Driver[] drivers)
        {
            foreach (string carClass in drivers.Where(d => d.CarClass.Value != null).Select(d => d.CarClass.Value).Distinct())
            {
                var driversInClass = Drivers.Where(d => d.CarClass.Value == carClass).OrderBy(d => d.Position.Value).ToList();
                foreach (Driver driver in driversInClass)
                    driver.PositionInClass.Value = driversInClass.IndexOf(driver) + 1;
            }
        }
    }
}
