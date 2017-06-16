using System;
using GeodesicLibrary.Model;

namespace GeodesicLibrary
{
    public class IntersectService
    {
        private const double TOLERANCE = 0.00000000001;

        private double EquatorialRadius { get; }

        private double PolarRadius { get; }

        private double F => (EquatorialRadius - PolarRadius) / PolarRadius;

        public IntersectService(double equatorialRadius, double polarRadius)
        {
            EquatorialRadius = equatorialRadius;
            PolarRadius = polarRadius;
        }

        public Coordinate IntersectOrthodromic(double lon11, double lat11, double lon12,
            double lat12, double lon21, double lat21, double lon22, double lat22)
        {
            var inLon = IntersectLongitude(lon11, lat11, lon12, lat12, lon21, lat21, lon22, lat22);
            var inLat1 = GetLatitude(inLon, lon11, lat11, lon12, lat12);
            var inLat2 = GetLatitude(inLon, lon21, lat21, lon22, lat22);
            return new Coordinate(inLon, (inLat1 + inLat2) / 2);
        }

        private double GetLatitude(double longitude, double lon1, double lat1, double lon2, double lat2)
        {
            lon1 *= Math.PI / 180;
            lat1 *= Math.PI / 180;
            lon2 *= Math.PI / 180;
            lat2 *= Math.PI / 180;
            longitude *= Math.PI / 180;

            if (Math.Abs(EquatorialRadius - PolarRadius) < TOLERANCE)
                return Math.Atan(Math.Tan(lat1) / Math.Sin(lon2 - lon1) * Math.Sin(lon2 - longitude) +
                                 Math.Tan(lat2) / Math.Sin(lon2 - lon1) * Math.Sin(longitude - lon1)) * 180 / Math.PI;

            double u1 = Math.Atan((1 - F) * Math.Tan(lat1));
            double u2 = Math.Atan((1 - F) * Math.Tan(lat2));
            double lambda = Lambda(lon2 - lon1, u1, u2);
            double lambdaD1 = Lambda(longitude - lon1, u1, u2);
            double lambdaD2 = Lambda(lon2 - longitude, u1, u2);
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

            if (Math.Abs(EquatorialRadius - PolarRadius) < TOLERANCE)
            {
                double dl1 = Math.Sin(lon12 - lon11);
                double dl2 = Math.Sin(lon22 - lon21);

                // Первая ортодрома
                double a1s = Math.Tan(lat11) / dl1 * Math.Sin(lon12);
                double a2s = Math.Tan(lat12) / dl1 * Math.Cos(lon11);
                double a1ss = Math.Tan(lat11) / dl1 * Math.Cos(lon12);
                double a2ss = Math.Tan(lat12) / dl1 * Math.Sin(lon11);

                // Вторая ортодрома
                double a3s = Math.Tan(lat21) / dl2 * Math.Sin(lon22);
                double a4s = Math.Tan(lat22) / dl2 * Math.Cos(lon21);
                double a3ss = Math.Tan(lat21) / dl2 * Math.Cos(lon22);
                double a4ss = Math.Tan(lat22) / dl2 * Math.Sin(lon21);

                double b1 = a1s - a2ss - a3s + a4ss;
                double b2 = a2s - a1ss + a3ss - a4s;

                return Math.Atan(-b1 / b2) * 180 / Math.PI;
            }
            else
            {
                double u11 = Math.Atan((1 - F) * Math.Tan(lat11));
                double u12 = Math.Atan((1 - F) * Math.Tan(lat12));
                double u21 = Math.Atan((1 - F) * Math.Tan(lat21));
                double u22 = Math.Atan((1 - F) * Math.Tan(lat22));

                double lambda1 = Lambda(lon12 - lon11, u11, u12);
                double lambda2 = Lambda(lon22 - lon21, u21, u22);

                double dl1 = Math.Sin(lambda1);
                double dl2 = Math.Sin(lambda2);

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

                double c = F / 16 * cosSqAlpha * (4 + F * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = l +
                         (1 - c) * F * sinAlpha *
                         (sigma + c * sinSigma * (cos2SigmaM + c * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            return lambda;
        }

    }
}
