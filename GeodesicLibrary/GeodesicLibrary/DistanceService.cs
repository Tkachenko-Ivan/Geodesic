using System;
using GeodesicLibrary.Model;
using GeodesicLibrary.Tools;

namespace GeodesicLibrary
{
    public class DistanceService
    {
        private double EquatorialRadius { get; }

        private double PolarRadius { get; }

        private double F => (EquatorialRadius - PolarRadius) / PolarRadius;

        public DistanceService(double equatorialRadius, double polarRadius)
        {
            EquatorialRadius = equatorialRadius;
            PolarRadius = polarRadius;
        }

        /// <summary>
        /// Решение обратной геодезической задачи
        /// </summary>
        public InverseProblemAnswer OrthodromicEllipsoidDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double l = (lon2 - lon1) * Math.PI / 180; // Разность геодезических долгот

            double u1 = Math.Atan((1 - F) * Math.Tan(lat1 * Math.PI / 180)); // Приведённая широта
            double u2 = Math.Atan((1 - F) * Math.Tan(lat2 * Math.PI / 180));
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

            a1 = Azimuth.AzimuthRecovery(lon1, lat1, lon2, lat2, a1);
            a2 = Azimuth.AzimuthRecovery(lon2, lat2, lon1, lat1, a2);

            return new InverseProblemAnswer(a1, a2, s);
        }
    }
}
