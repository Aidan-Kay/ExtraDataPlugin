using ACSharedMemory.ACC.Reader;
using System.Linq;

namespace AidanKay.ExtraDataPlugin
{
    internal static class AccHelper
    {
        public static ksBroadcastingNetwork.Structs.RealtimeCarUpdate GetPlayersCar(ACCRawData accRawData)
        {
            int carId = accRawData.Graphics.PlayerCarID;
            return accRawData.Cars[carId];
        }

        public static ksBroadcastingNetwork.Structs.RealtimeCarUpdate GetCarByPosition(ACCRawData accRawData, int position)
        {
            var car = accRawData.Cars.Where(c => c.Value.RacePosition == position);
            return car.Count() > 0 ? car.First().Value : null;
        }

        public static string GetCarName(byte carModelType)
        {
            switch (carModelType)
            {
                case 0: return "Porsche 991 GT3 R 2018";
                case 1: return "Mercedes AMG GT3 2015";
                case 2: return "Ferrari 488 GT3 2018";
                case 3: return "Audi R8 LMS 2015";
                case 4: return "Lamborghini Huracan GT3 2015";
                case 5: return "McLaren 650S GT3 2015";
                case 6: return "Nissan GTR Nismo GT3 2018";
                case 7: return "BMW M6 GT3 2017";
                case 8: return "Bentley Continental GT3 2018";
                case 9: return "Porsche 991 II GT3 Cup 2017";
                case 10: return "Nissan GTR Nismo GT3 2015";
                case 11: return "Bentley Continental GT3 2015";
                case 12: return "Aston Martin Vantage V12 GT3 2013";
                case 13: return "Lamborghini Gallardo G3 Reiter 2017";
                case 14: return "Emil Frey Jaguar G3 2012";
                case 15: return "Lexus RCF GT3 2016";
                case 16: return "Lamborghini Huracan GT3 EVO 2019";
                case 17: return "Honda NSX GT3 2017";
                case 18: return "Lamborghini Huracan ST 2015";
                case 19: return "Audi R8 LMS Evo 2019";
                case 20: return "Aston Martin V8 Vantage GT3 2019";
                case 21: return "Honda NSX GT3 Evo 2019";
                case 22: return "McLaren 720S GT3 2019";
                case 23: return "Porsche 911 II GT3 R 2019";
                case 24: return "Ferrari 488 GT3 Evo 2020";
                case 25: return "Mercedes AMG GT3 Evo 2020";
                case 50: return "Alpine A110 GT4 2018";
                case 51: return "Aston Martin Vantage AMR GT4 2018";
                case 52: return "Audi R8 LMS GT4 2016";
                case 53: return "BMW M4 GT42 018";
                case 55: return "Chevrolet Camaro GT4 R 2017";
                case 56: return "Ginetta G55 GT4 2012";
                case 57: return "Ktm Xbow GT4 2016";
                case 58: return "Maserati Gran Turismo MC GT4 2016";
                case 59: return "McLaren 570s GT4 2016";
                case 60: return "Mercedes AMG GT4 2016";
                case 61: return "Porsche 718 Cayman GT4 MR 2019";
            }

            return null;
        }

        public static string GetCarClass(byte carModelType)
        {

            switch (carModelType)
            {
                case 0: return "GT3 - 2018";
                case 1: return "GT3 - 2018";
                case 2: return "GT3 - 2018";
                case 3: return "GT3 - 2018";
                case 4: return "GT3 - 2018";
                case 5: return "GT3 - 2018";
                case 6: return "GT3 - 2018";
                case 7: return "GT3 - 2018";
                case 8: return "GT3 - 2018";
                case 9: return "GT3 - 2018";
                case 10: return "GT3 - 2018";
                case 11: return "GT3 - 2018";
                case 12: return "GT3 - 2018";
                case 13: return "GT3 - 2018";
                case 14: return "GT3 - 2018";
                case 15: return "GT3 - 2018";
                case 16: return "GT3 - 2019";
                case 17: return "GT3 - 2018";
                case 18: return "GT3 - 2018";
                case 19: return "GT3 - 2019";
                case 20: return "GT3 - 2019";
                case 21: return "GT3 - 2019";
                case 22: return "GT3 - 2019";
                case 23: return "GT3 - 2019";
                case 24: return "GT3 - 2020";
                case 25: return "GT3 - 2020";
                case 50: return "GT4";
                case 51: return "GT4";
                case 52: return "GT4";
                case 53: return "GT4";
                case 55: return "GT4";
                case 56: return "GT4";
                case 57: return "GT4";
                case 58: return "GT4";
                case 59: return "GT4";
                case 60: return "GT4";
                case 61: return "GT4";
                default: return "-";
            }
        }

        public static string GetCarLicenseColour(byte cupCategory)
        {
            switch (cupCategory)
            {
                case 0: return "#FFFFFF"; // PRO
                case 1: return "#000000"; // PRO-AM
                case 2: return "#FF0000"; // AM
                case 3: return "#7C7C7C"; // Silver
                case 4: return "#0C9E00"; // National
                default: return "#FFFFFF";
            }
        }

        public static string GetCarLicenseTextColour(byte cupCategory)
        {
            switch (cupCategory)
            {
                case 0: return "#000000"; // PRO
                case 1: return "#FFFFFF"; // PRO-AM
                case 2: return "#FFFFFF"; // AM
                case 3: return "#FFFFFF"; // Silver
                case 4: return "#FFFFFF"; // National
                default: return "#000000";
            }
        }
    }
}
