using System;

namespace GeodesicLibrary
{
    class CoordinatesService
    {
        private double EquatorialRadius { get; }

        private double PolarRadius { get; }

        private double F => (EquatorialRadius - PolarRadius) / PolarRadius;

        public CoordinatesService(double equatorialRadius, double polarRadius)
        {
            EquatorialRadius = equatorialRadius;
            PolarRadius = polarRadius;
        }

        /// <summary>
        /// Решение прямой геодезической задачи
        /// </summary>
        public Tuple<double, double, double> DirectProblemEllipsoid(double lon1, double lat1, double a1, double s)
        {
            a1 = a1 * Math.PI / 180;

            double u1 = Math.Atan((1 - F) * Math.Tan(lat1 * Math.PI / 180));
            double sigma1 = Math.Atan(Math.Tan(u1) / Math.Cos(a1));
            var sinAlpha = Math.Cos(u1) * Math.Sin(a1);

            var cosSqAlpha = 1 - sinAlpha * sinAlpha;

            double uSq = cosSqAlpha * (Math.Pow(EquatorialRadius, 2) - Math.Pow(PolarRadius, 2)) /
                         Math.Pow(PolarRadius, 2);
            double a = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double b = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            var si = s / PolarRadius / a;
            double sigma = si;

            double sigmaP, cos2SigmaM;
            do
            {
                var sinSigma = Math.Sin(sigma);
                var cosSigma = Math.Cos(sigma);

                cos2SigmaM = Math.Cos(2 * sigma1 + sigma);
                double deltaSigma = b * sinSigma *
                                    (cos2SigmaM +
                                     b / 4 *
                                     (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                                      b / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) *
                                      (-3 + 4 * cos2SigmaM * cos2SigmaM)));
                sigmaP = sigma;
                sigma = si + deltaSigma;
            } while (Math.Abs(sigma - sigmaP) > 1e-12);

            var sinU2 = Math.Sin(u1) * Math.Cos(sigma) + Math.Cos(u1) * Math.Sin(sigma) * Math.Cos(a1);
            var lat2 = sinU2 /
                       ((1 - F) *
                        Math.Sqrt(sinAlpha * sinAlpha +
                                  Math.Pow(
                                      Math.Sin(u1) * Math.Sin(sigma) - Math.Cos(u1) * Math.Cos(sigma) * Math.Cos(a1), 2)));
            lat2 = Math.Atan(lat2);

            // Разность долгот
            var lamda = Math.Atan(Math.Sin(sigma) * Math.Sin(a1) /
                              (Math.Cos(u1) * Math.Cos(sigma) - Math.Sin(u1) * Math.Sin(sigma) * Math.Cos(a1)));

            double c = F / 16 * cosSqAlpha * (4 + F * (4 - 3 * cosSqAlpha));

            var l = lamda - (1 - c) * F * sinAlpha *
                        (sigma +
                         c * Math.Sin(sigma) * (cos2SigmaM + c * Math.Cos(sigma) * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            var lon2 = -l + lon1 * Math.PI / 180;

            var a2 = Math.Atan(sinAlpha / (-Math.Sin(u1) * Math.Sin(sigma) + Math.Cos(u1) * Math.Cos(sigma) * Math.Cos(a1))) * 180 / Math.PI;

            return Tuple.Create(lon2 * 180 / Math.PI, lat2 * 180 / Math.PI, a2);
        }

        #region Пересечение двух ортодром

        public Tuple<double, double> IntersectOrthodromicEllipsoid(double lon11, double lat11, double lon12,
            double lat12, double lon21, double lat21, double lon22, double lat22)
        {
            var inLon = IntersectLongitude(22, 13, 27, 15, 20, 17, 26, 13);
            var inLat1 = GetLatitude(inLon, 22, 13, 27, 15);
            var inLat2 = GetLatitude(inLon, 20, 17, 26, 13);
            return Tuple.Create(inLon, (inLat1 + inLat2) / 2);
        }

        private double GetLatitude(double longitude, double lon1, double lat1, double lon2, double lat2)
        {
            lon1 *= Math.PI / 180;
            lat1 *= Math.PI / 180;
            lon2 *= Math.PI / 180;
            lat2 *= Math.PI / 180;
            longitude *= Math.PI / 180;

            double l = lon2 - lon1; // Смещение по X в радианах
            double ld1 = longitude - lon1; // Смещение по X в радианах
            double ld2 = lon2 - longitude; // Смещение по X в радианах

            double u1 = Math.Atan((1 - F) * Math.Tan(lat1));
            double u2 = Math.Atan((1 - F) * Math.Tan(lat2));

            double lambda = Lambda(l, u1, u2);
            double lambdaD1 = Lambda(ld1, u1, u2);
            double lambdaD2 = Lambda(ld2, u1, u2);

            return Math.Atan(Math.Tan(u1) / Math.Sin(lambda) * Math.Sin(lambdaD2) / (1 - F) +
                             Math.Tan(u2) / Math.Sin(lambda) * Math.Sin(lambdaD1) / (1 - F)) * 180 / Math.PI;
        }

        private double IntersectLongitude(double lon11, double lat11, double lon12, double lat12, double lon21, double lat21, double lon22, double lat22)
        {
            lon11 *= Math.PI / 180;
            lat11 *= Math.PI / 180;
            lon12 *= Math.PI / 180;
            lat12 *= Math.PI / 180;

            lon21 *= Math.PI / 180;
            lat21 *= Math.PI / 180;
            lon22 *= Math.PI / 180;
            lat22 *= Math.PI / 180;

            double u11 = Math.Atan((1 - F) * Math.Tan(lat11));
            double u12 = Math.Atan((1 - F) * Math.Tan(lat12));
            double u21 = Math.Atan((1 - F) * Math.Tan(lat21));
            double u22 = Math.Atan((1 - F) * Math.Tan(lat22));

            double lambda1 = Lambda(lon12 - lon11, u11, u12);
            double lambda2 = Lambda(lon22 - lon21, u21, u22);

            double dl1 = Math.Sin(lambda1) * (1 - F);
            double dl2 = Math.Sin(lambda2) * (1 - F);

            // Первая ортодрома
            double a1s = Math.Tan(u11) / dl1 * Math.Sin(lon12);
            double a2s = Math.Tan(u12) / dl1 * Math.Cos(lon11);
            double a1ss = Math.Tan(u11) / dl1 * Math.Cos(lon12);
            double a2ss = Math.Tan(u12) / dl1 * Math.Sin(lon11);

            // Вторая ортодрома
            double a3s = Math.Tan(u21) / dl2 * Math.Sin(lon22);
            double a4s = Math.Tan(u22) / dl2 * Math.Cos(lon21);
            double a3ss = Math.Tan(u21) / dl2 * Math.Cos(lon22);
            double a4ss = Math.Tan(u22) / dl2 * Math.Sin(lon21);

            double b1 = a1s - a2ss - a3s + a4ss;
            double b2 = a2s - a1ss + a3ss - a4s;

            return Math.Atan(-b1 / b2) * 180 / Math.PI;
        }

        private double Lambda(double l, double u1, double u2)
        {
            double sinU1 = Math.Sin(u1), cosU1 = Math.Cos(u1);
            double sinU2 = Math.Sin(u2), cosU2 = Math.Cos(u2);

            double lambda = l, lambdaP, iterLimit = 100;
            double cosSqAlpha, sinSigma, cosSigma, cos2SigmaM, sigma;

            do
            {
                var sinLambda = Math.Sin(lambda);
                var cosLambda = Math.Cos(lambda);

                sinSigma = Math.Sqrt(cosU2 * sinLambda * (cosU2 * sinLambda)
                                            + (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda)
                                            * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0)
                    return 0;

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
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

            return lambda;
        }

        #endregion
    }
}
