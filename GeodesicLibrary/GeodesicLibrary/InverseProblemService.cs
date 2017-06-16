using System;
using GeodesicLibrary.Model;
using GeodesicLibrary.Tools;

namespace GeodesicLibrary
{
    public class InverseProblemService
    {
        private const double TOLERANCE = 0.00000000001;

        private double EquatorialRadius { get; }

        private double PolarRadius { get; }

        private double F => (EquatorialRadius - PolarRadius) / PolarRadius;

        public InverseProblemService(double equatorialRadius, double polarRadius)
        {
            EquatorialRadius = equatorialRadius;
            PolarRadius = polarRadius;
        }

        public InverseProblemAnswer OrthodromicDistance(Point coord1, Point coord2)
        {
            return Math.Abs(EquatorialRadius - PolarRadius) < TOLERANCE
                ? OrthodromicSpheroidDistance(coord1, coord2)
                : OrthodromicEllipsoidDistance(coord1, coord2);
        }

        /// <summary>
        /// Решение обратной геодезической задачи на элипсоиде
        /// </summary>
        private InverseProblemAnswer OrthodromicEllipsoidDistance(Point coord1, Point coord2)
        {
            double l = (coord2.LonR - coord1.LonR); // Разность геодезических долгот

            double u1 = Math.Atan((1 - F) * Math.Tan(coord1.LatR)); // Приведённая широта
            double u2 = Math.Atan((1 - F) * Math.Tan(coord2.LatR));
            double sinU1 = Math.Sin(u1), cosU1 = Math.Cos(u1);
            double sinU2 = Math.Sin(u2), cosU2 = Math.Cos(u2);

            double lambda = l, lambdaP;
            double cosSqAlpha, sinSigma, cosSigma, cos2SigmaM, sigma;
            int iterLimit = 100;

            do
            {
                var sinLambda = Math.Sin(lambda);
                var cosLambda = Math.Cos(lambda);

                sinSigma =
                    Math.Sqrt(Math.Pow(cosU2 * sinLambda, 2) + Math.Pow(cosU1 * sinU2 - sinU1 * cosU2 * cosLambda, 2));

                if (sinSigma == 0)
                    return new InverseProblemAnswer(0, 0, 0);

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan(sinSigma / cosSigma);
                var sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;

                if (cosSqAlpha != 0)
                    cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                else
                    cos2SigmaM = 0;

                double c = F / 16 * cosSqAlpha * (4 + F * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = l +
                         (1 - c) * F * sinAlpha *
                         (sigma + c * sinSigma * (cos2SigmaM + c * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0)
                return new InverseProblemAnswer(0, 0, 0);

            double uSq = cosSqAlpha * (Math.Pow(EquatorialRadius, 2) - Math.Pow(PolarRadius, 2)) /
                         Math.Pow(PolarRadius, 2);
            double a = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double b = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = b * sinSigma *
                                (cos2SigmaM +
                                 b / 4 *
                                 (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                                  b / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) *
                                  (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double s = PolarRadius * a * (sigma - deltaSigma);

            var a1 = Math.Atan(cosU2 * Math.Sin(lambda) / (cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(lambda))) * 180 /
                     Math.PI;
            var a2 = Math.Atan(cosU1 * Math.Sin(lambda) / (-sinU1 * cosU2 + cosU1 * sinU2 * Math.Cos(lambda))) * 180 /
                     Math.PI;
            a1 = Azimuth.AzimuthRecovery(coord1, coord2, a1);
            a2 = Azimuth.AzimuthRecovery(coord2, coord1, a2);

            return new InverseProblemAnswer(a1, a2, s);
        }

        /// <summary>
        /// Решение обратной геодезической задачи на сфероиде
        /// </summary>
        private InverseProblemAnswer OrthodromicSpheroidDistance(Point coord1, Point coord2)
        {
            var ω = coord2.LonR - coord1.LonR;

            var cosSigma = Math.Sin(coord1.LatR) * Math.Sin(coord2.LatR) + Math.Cos(coord1.LatR) * Math.Cos(coord2.LatR) * Math.Cos(ω);

            double s;
            if (cosSigma < 0)
                s = PolarRadius * (Math.PI - Math.Abs(Math.Acos(cosSigma)));
            else if (Math.Abs(cosSigma - 1) < TOLERANCE)
                s = 0;
            else
                s = PolarRadius * Math.Acos(cosSigma);

            var a1 =
                Math.Atan(Math.Cos(coord2.LatR) * Math.Sin(ω) /
                          (Math.Cos(coord1.LatR) * Math.Sin(coord2.LatR) - Math.Sin(coord1.LatR) * Math.Cos(coord2.LatR) * Math.Cos(ω))) * 180 /
                Math.PI;
            var a2 =
                Math.Atan(Math.Cos(coord1.LatR) * Math.Sin(ω) /
                          (Math.Cos(coord1.LatR) * Math.Sin(coord2.LatR) * Math.Cos(ω) - Math.Sin(coord1.LatR) * Math.Cos(coord2.LatR))) * 180 /
                Math.PI;

            a1 = Azimuth.AzimuthRecovery(coord1, coord2, a1);
            a2 = Azimuth.AzimuthRecovery(coord2, coord1, a2);

            return new InverseProblemAnswer(a1, a2, s);
        }
    }
}
