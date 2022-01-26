using IRacingReader;
using iRacingSDK;
using System.Linq;

namespace AidanKay.ExtraDataPlugin
{
    internal static class IRacingHelper
    {
        public static iRacingSDK.SessionData._DriverInfo._Drivers GetDriverByNumber(IRacingReader.DataSampleEx iRacingRawData, string carNumber)
        {
            var driver = iRacingRawData.SessionData.DriverInfo.CompetingDrivers.Where(d => d.CarNumber == carNumber);
            return driver.Count() > 0 ? driver.First() : null;
        }

        public static string GetCarClassColour(string carClassColour)
        {
            return carClassColour == "0xffffff" ? "#FF33CEFF" : carClassColour.Replace("0x", "#FF");
        }

        public static string GetLicenseColour(string licString)
        {
            if (licString.Contains("A"))
                return "#FF0153DB";
            if (licString.Contains("B"))
                return "#FF00C702";
            if (licString.Contains("C"))
                return "#FFFEEC04";
            if (licString.Contains("D"))
                return "#FFFC8A27";
            return !licString.Contains("R") ? "Black" : "#FFB40800";
        }

        public static string GetLicenseTextColour(string licString)
        {
            return licString.Contains("B") || licString.Contains("C") || licString.Contains("D") ? "Black" : "White";
        }
    }
}
