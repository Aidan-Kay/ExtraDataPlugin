using GameReaderCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AidanKay.ExtraDataPlugin
{
    public static class SectorFunctions
    {
        public static TimeSpan?[] GetOverallBestSectorTimes(StatusDataBase data)
        {
            List<double?> bestSector1s = new List<double?>();
            List<double?> bestSector2s = new List<double?>();
            List<double?> bestSector3s = new List<double?>();

            foreach (Opponent o in data.Opponents)
            {
                bestSector1s.Add(o.BestSector1);
                bestSector2s.Add(o.BestSector2);
                bestSector3s.Add(o.BestSector3);
            }

            return new TimeSpan?[]
            {
                CommonHelper.ToNullableTimeSpan(bestSector1s.Where(t => t.HasValue).Min()),
                CommonHelper.ToNullableTimeSpan(bestSector2s.Where(t => t.HasValue).Min()),
                CommonHelper.ToNullableTimeSpan(bestSector3s.Where(t => t.HasValue).Min())
            };
        }
    }
}
