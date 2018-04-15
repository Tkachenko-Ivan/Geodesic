﻿using System;
using GeodesicLibrary.Model;

namespace GeodesicLibrary.Tools
{
    internal class Azimuth
    {
        private const double TOLERANCE = 0.00000000001;

        public static double AzimuthRecovery(Point coord1, Point coord2, double azimuth)
        {
            var result = AzimuthR(coord1, coord2, azimuth);
            return result < TOLERANCE ? 360 : result;
        }

        private static double AzimuthR(Point coord1, Point coord2, double azimuth)
        {
            var lon1 = coord1.Longitude;
            var lat1 = coord1.Latitude;
            var lon2 = coord2.Longitude;
            var lat2 = coord2.Latitude;

            // ТОDO: Изменение полушария с северного на южное или наоборот
            if (Math.Abs(lat1 - lat2) < TOLERANCE && lon1 < lon2) // запад
            {
                if (lat1 > 0) // Северное полушарие
                    return 360 - Math.Abs(azimuth);
                else // Южное полушарие
                    return 180 + Math.Abs(azimuth);
            }
            if (Math.Abs(lat1 - lat2) < TOLERANCE && lon1 > lon2) // восток
            {
                if (lat1 > 0) // Северное полушарие
                    return Math.Abs(azimuth);
                else // Южное полушарие
                    return 180 - Math.Abs(azimuth);
            }

            if (lon2 > lon1 && lat2 > lat1) // северо-запад
                return 360 - Math.Abs(azimuth);
            if (lon2 > lon1 && lat2 < lat1) // юго-запад
                return Math.Abs(azimuth) + 180;
            if (lon2 < lon1 && lat2 > lat1) // северо-восток
                return Math.Abs(azimuth);
            if (lon2 < lon1 && lat2 < lat1) // юго-восток
                return 180 - Math.Abs(azimuth);

            if (Math.Abs(azimuth) < TOLERANCE && lat2 < lat1) // юг
                azimuth = 180;
            if (Math.Abs(azimuth) < TOLERANCE && lat2 > lat1) // север
                azimuth = 360;

            return Math.Abs(azimuth);
        }
    }
}
