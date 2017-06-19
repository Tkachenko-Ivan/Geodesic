using System;
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

        private double GetLatitude(double longitude, Point coord1, Point coord2)
        {
            longitude *= Math.PI / 180;

            if (Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE)
                return Math.Atan(Math.Tan(coord1.LatR) / Math.Sin(coord2.LonR - coord1.LonR) * Math.Sin(coord2.LonR - longitude) +
                                 Math.Tan(coord2.LatR) / Math.Sin(coord2.LonR - coord1.LonR) * Math.Sin(longitude - coord1.LonR)) * 180 / Math.PI;

            double u1 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord1.LatR));
            double u2 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord2.LatR));
            double lambda = Lambda(coord2.LonR - coord1.LonR, u1, u2);
            double lambdaD1 = Lambda(longitude - coord1.LonR, u1, u2);
            double lambdaD2 = Lambda(coord2.LonR - longitude, u1, u2);
            return Math.Atan(Math.Tan(u1) / Math.Sin(lambda) * Math.Sin(lambdaD2) / (1 - _ellipsoid.F) +
                             Math.Tan(u2) / Math.Sin(lambda) * Math.Sin(lambdaD1) / (1 - _ellipsoid.F)) * 180 / Math.PI;
        }

        private double IntersectLongitude(Point coord11, Point coord12, Point coord21, Point coord22)
        {
            if (Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE)
            {
                double dl1 = Math.Sin(coord12.LonR - coord11.LonR);
                double dl2 = Math.Sin(coord22.LonR - coord21.LonR);

                // Первая ортодрома
                double a1s = Math.Tan(coord11.LatR) / dl1 * Math.Sin(coord12.LonR);
                double a2s = Math.Tan(coord12.LatR) / dl1 * Math.Cos(coord11.LonR);
                double a1ss = Math.Tan(coord11.LatR) / dl1 * Math.Cos(coord12.LonR);
                double a2ss = Math.Tan(coord12.LatR) / dl1 * Math.Sin(coord11.LonR);

                // Вторая ортодрома
                double a3s = Math.Tan(coord21.LatR) / dl2 * Math.Sin(coord22.LonR);
                double a4s = Math.Tan(coord22.LatR) / dl2 * Math.Cos(coord21.LonR);
                double a3ss = Math.Tan(coord21.LatR) / dl2 * Math.Cos(coord22.LonR);
                double a4ss = Math.Tan(coord22.LatR) / dl2 * Math.Sin(coord21.LonR);

                double b1 = a1s - a2ss - a3s + a4ss;
                double b2 = a2s - a1ss + a3ss - a4s;

                return Math.Atan(-b1 / b2) * 180 / Math.PI;
            }
            else
            {
                double u11 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord11.LatR));
                double u12 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord12.LatR));
                double u21 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord21.LatR));
                double u22 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord22.LatR));

                double lambda1 = Lambda(coord12.LonR - coord11.LonR, u11, u12);
                double lambda2 = Lambda(coord22.LonR - coord21.LonR, u21, u22);

                double dl1 = Math.Sin(lambda1);
                double dl2 = Math.Sin(lambda2);

                // Первая ортодрома
                double a1s = Math.Tan(u11) / dl1 * Math.Sin(coord12.LonR);
                double a2s = Math.Tan(u12) / dl1 * Math.Cos(coord11.LonR);
                double a1ss = Math.Tan(u11) / dl1 * Math.Cos(coord12.LonR);
                double a2ss = Math.Tan(u12) / dl1 * Math.Sin(coord11.LonR);

                // Вторая ортодрома
                double a3s = Math.Tan(u21) / dl2 * Math.Sin(coord22.LonR);
                double a4s = Math.Tan(u22) / dl2 * Math.Cos(coord21.LonR);
                double a3ss = Math.Tan(u21) / dl2 * Math.Cos(coord22.LonR);
                double a4ss = Math.Tan(u22) / dl2 * Math.Sin(coord21.LonR);

                double b1 = a1s - a2ss - a3s + a4ss;
                double b2 = a2s - a1ss + a3ss - a4s;

                return Math.Atan(-b1 / b2) * 180 / Math.PI;
            }
        }

        private double Lambda(double l, double u1, double u2)
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
        }

    }
}
