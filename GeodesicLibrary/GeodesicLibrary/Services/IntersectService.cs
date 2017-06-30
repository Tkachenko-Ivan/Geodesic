using System;
using System.Collections.Generic;
using System.Linq;
using GeodesicLibrary.Model;

namespace GeodesicLibrary.Services
{
    public class IntersectService
    {
        private const double TOLERANCE = 0.00000000001;

        private readonly IEllipsoid _ellipsoid;

        public IntersectService(IEllipsoid ellipsoid)
        {
            _ellipsoid = ellipsoid;
        }

        public Point IntersectOrthodromic(Point coord11, Point coord12, Point coord21, Point coord22)
        {
            var inLon = IntersectLongitude(coord11, coord12, coord21, coord22);
            var inLat1 = GetLatitude(inLon, coord11, coord12);
            var inLat2 = GetLatitude(inLon, coord21, coord22);
            return new Point(inLon, (inLat1 + inLat2) / 2);
        }

        public double GetLatitude(double longitude, Point coord1, Point coord2)
        {
            longitude *= Math.PI / 180;

            // Сфероид
            if (Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE)
                return Math.Atan(Math.Tan(coord1.LatR) / Math.Sin(coord2.LonR - coord1.LonR) *
                                 Math.Sin(coord2.LonR - longitude) +
                                 Math.Tan(coord2.LatR) / Math.Sin(coord2.LonR - coord1.LonR) *
                                 Math.Sin(longitude - coord1.LonR)) * 180 / Math.PI;

            // Эллипсоид вращения
            var inverce = new InverseProblemService(_ellipsoid);
            var direct = new DirectProblemService(_ellipsoid);
            Point coordM;
            Point first = coord1, second = coord2;
            do
            {
                var dist = inverce.OrthodromicDistance(first, second);
                coordM = direct.DirectProblem(first, dist.ForwardAzimuth, dist.Distance / 2).Сoordinate;

                if (Math.Abs(longitude - first.LonR) < TOLERANCE)
                    return first.Latitude;
                if (Math.Abs(longitude - second.LonR) < TOLERANCE)
                    return second.Latitude;

                if (longitude > first.LonR && longitude < coordM.LonR)
                    second = coordM;
                else if (longitude > coordM.LonR && longitude < second.LonR)
                    first = coordM;
            } while (Math.Abs(coordM.LonR - longitude) > TOLERANCE);
            return coordM.Latitude;
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

                var lat11 = GetLatitude(first, coord11, coord12);
                var lat12 = GetLatitude(first, coord21, coord22);

                var lat21 = GetLatitude(midLon, coord11, coord12);
                var lat22 = GetLatitude(midLon, coord21, coord22);

                var lat31 = GetLatitude(second, coord11, coord12);
                var lat32 = GetLatitude(second, coord21, coord22);

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

      /*  private double Lambda(double l, double u1, double u2)
        {
            double sinU1 = Math.Sin(u1), cosU1 = Math.Cos(u1);
            double sinU2 = Math.Sin(u2), cosU2 = Math.Cos(u2);

            double lambda = l, lambdaP, iterLimit = 100;

            do
            {
                var sinLambda = Math.Sin(lambda);
                var cosLambda = Math.Cos(lambda);

                var sinSigma = Math.Sqrt(cosU2 * sinLambda * (cosU2 * sinLambda)
                                            + (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda)
                                            * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (Math.Abs(sinSigma) < TOLERANCE)
                    return 0;

                var cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                var sigma = Math.Atan2(sinSigma, cosSigma);
                var sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                var cosSqAlpha = 1 - sinAlpha * sinAlpha;

                double cos2SigmaM;
                if (Math.Abs(cosSqAlpha) > TOLERANCE)
                    cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                else
                    cos2SigmaM = 0;

                double c = _ellipsoid.F / 16 * cosSqAlpha * (4 + _ellipsoid.F * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = l +
                         (1 - c) * _ellipsoid.F * sinAlpha *
                         (sigma + c * sinSigma * (cos2SigmaM + c * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            return lambda;
        }*/

    }
}
