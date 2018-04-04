using System;
using System.Collections.Generic;
using System.Linq;
using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;

namespace GeodesicLibrary.Services
{
    /// <summary>
    /// Рассчитывает точку пересечения ортодром
    /// </summary>
    public class IntersectService
    {
        private const double TOLERANCE = 0.00000000001;

        private readonly IEllipsoid _ellipsoid;

        private readonly IntermediatePointService _interPoint;

        public IntersectService(IEllipsoid ellipsoid)
        {
            _ellipsoid = ellipsoid;
            _interPoint = new IntermediatePointService(ellipsoid);
        }

        public Point IntersectOrthodromic(Point coord11, Point coord12, Point coord21, Point coord22)
        {
            var inLon = IntersectLongitude(coord11, coord12, coord21, coord22);
            var inLat1 = _interPoint.GetLatitude(inLon, coord11, coord12);
            var inLat2 = _interPoint.GetLatitude(inLon, coord21, coord22);
            return new Point(inLon, (inLat1 + inLat2) / 2);
        }

        private double IntersectLongitude(Point coord11, Point coord12, Point coord21, Point coord22)
        {
            // Сфероид
            if (Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE)
            {
                double dl1 = Math.Sin(coord12.LonR - coord11.LonR);
                double dl2 = Math.Sin(coord22.LonR - coord21.LonR);

                // Первая ортодрома
                double a1S = Math.Tan(coord11.LatR) / dl1 * Math.Sin(coord12.LonR);
                double a2S = Math.Tan(coord12.LatR) / dl1 * Math.Cos(coord11.LonR);
                double a1Ss = Math.Tan(coord11.LatR) / dl1 * Math.Cos(coord12.LonR);
                double a2Ss = Math.Tan(coord12.LatR) / dl1 * Math.Sin(coord11.LonR);

                // Вторая ортодрома
                double a3S = Math.Tan(coord21.LatR) / dl2 * Math.Sin(coord22.LonR);
                double a4S = Math.Tan(coord22.LatR) / dl2 * Math.Cos(coord21.LonR);
                double a3Ss = Math.Tan(coord21.LatR) / dl2 * Math.Cos(coord22.LonR);
                double a4Ss = Math.Tan(coord22.LatR) / dl2 * Math.Sin(coord21.LonR);

                double b1 = a1S - a2Ss - a3S + a4Ss;
                double b2 = a2S - a1Ss + a3Ss - a4S;

                return Math.Atan(-b1 / b2) * 180 / Math.PI;
            }

            // Эллипсоид вращения
            var longs =
                new List<double> { coord11.Longitude, coord12.Longitude, coord21.Longitude, coord22.Longitude }
                    .OrderBy(
                        s => s).ToList();
            double first = longs[1], second = longs[2];
            double midLon;
            do
            {
                midLon = (first + second) / 2;

                var lat11 = _interPoint.GetLatitude(first, coord11, coord12);
                var lat12 = _interPoint.GetLatitude(first, coord21, coord22);

                var lat21 = _interPoint.GetLatitude(midLon, coord11, coord12);
                var lat22 = _interPoint.GetLatitude(midLon, coord21, coord22);

                var lat31 = _interPoint.GetLatitude(second, coord11, coord12);
                var lat32 = _interPoint.GetLatitude(second, coord21, coord22);

                if ((lat21 - lat22) * (lat31 - lat32) < 0)
                    first = midLon;
                else if ((lat21 - lat22) * (lat11 - lat12) < 0)
                    second = midLon;
                else if (Math.Abs(lat11 - lat12) < TOLERANCE)
                    return first;
                else if (Math.Abs(lat21 - lat22) < TOLERANCE)
                    return midLon;
                else if (Math.Abs(lat31 - lat32) < TOLERANCE)
                    return second;

            } while (Math.Abs(first - second) > TOLERANCE);
            return midLon;
        }
    }
}
