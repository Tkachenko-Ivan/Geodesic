using System;
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

            if (Math.Abs(azimuth) < TOLERANCE && lon2 * lon1 < 0 && lat2 > 0 && lat1 > 0) // Через северный полюс
                return 360;
            if (Math.Abs(azimuth) < TOLERANCE && lon2 * lon1 < 0 && lat2 < 0 && lat1 < 0) // Через южный полюс
                return 180;

            if (Math.Abs(lon1 - lon2) > TOLERANCE)
            {
                bool reverse = false;
                if (lon1 * lon2 < 0)
                    // Одна из координат в западном полушарии, а вторая в восточном
                    if (Math.Abs(lon1) + Math.Abs(lon2) > 180)
                        reverse = true;

                var direct = lon1 < lon2 ? !reverse : reverse;

                if (Math.Abs(lat1 - lat2) < TOLERANCE && direct)
                {
                    if (lat1 > 0) // Северное полушарие
                        return 360 - Math.Abs(azimuth);
                    // Южное полушарие
                    return 180 + Math.Abs(azimuth);
                }
                if (Math.Abs(lat1 - lat2) < TOLERANCE && !direct)
                {
                    if (lat1 > 0) // Северное полушарие
                        return Math.Abs(azimuth);
                    // Южное полушарие
                    return 180 - Math.Abs(azimuth);
                }

                if (direct && lat2 > lat1)
                    return 360 - Math.Abs(azimuth);
                if (direct && lat2 < lat1)
                    return Math.Abs(azimuth) + 180;
                if (!direct && lat2 > lat1)
                    return Math.Abs(azimuth);
                if (!direct && lat2 < lat1)
                    return 180 - Math.Abs(azimuth);
            }

            if (Math.Abs(azimuth) < TOLERANCE && lat2 < lat1) // юг
                return 180;
            if (Math.Abs(azimuth) < TOLERANCE && lat2 > lat1) // север
                return 360;

            return Math.Abs(azimuth);
        }
    }
}
