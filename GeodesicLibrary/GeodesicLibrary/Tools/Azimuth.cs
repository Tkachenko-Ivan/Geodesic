using System;

namespace GeodesicLibrary.Tools
{
    internal class Azimuth
    {
        private const double TOLERANCE = 0.00000000001;

        public static double AzimuthRecovery(double lon1, double lat1, double lon2, double lat2, double azimuth)
        {
              if (Math.Abs(lat1 - lat2) < TOLERANCE && lon1 < lon2) // запад
                  return 360 - Math.Abs(azimuth);
              if (Math.Abs(lat1 - lat2) < TOLERANCE && lon1 > lon2) // восток
                  return Math.Abs(azimuth);

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
