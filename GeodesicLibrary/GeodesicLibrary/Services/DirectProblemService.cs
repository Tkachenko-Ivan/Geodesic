using System;
using GeodesicLibrary.Model;
using GeodesicLibrary.Tools;

namespace GeodesicLibrary.Services
{
    public class DirectProblemService
    {
        private const double TOLERANCE = 0.00000000001;

        private readonly IEllipsoid _ellipsoid;

        public DirectProblemService(IEllipsoid ellipsoid)
        {
            _ellipsoid = ellipsoid;
        }

        public DirectProblemAnswer DirectProblem(Point coord, double a1, double s)
        {
            return Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE
                ? DirectProblemSpheroid(coord, a1, s)
                : DirectProblemEllipsoid(coord, a1, s);
        }

        /// <summary>
        /// Решение прямой геодезической задачи на эллипсоиде
        /// </summary>
        private DirectProblemAnswer DirectProblemEllipsoid(Point coord, double a1, double s)
        {
            a1 = a1 * Math.PI / 180;

            double u1 = Math.Atan((1 - _ellipsoid.F) * Math.Tan(coord.LatR));
            double sigma1 = Math.Atan(Math.Tan(u1) / Math.Cos(a1));
            var sinAlpha = Math.Cos(u1) * Math.Sin(a1);

            var cosSqAlpha = 1 - sinAlpha * sinAlpha;

            double uSq = cosSqAlpha * (Math.Pow(_ellipsoid.EquatorialRadius, 2) - Math.Pow(_ellipsoid.PolarRadius, 2)) /
                         Math.Pow(_ellipsoid.PolarRadius, 2);
            double a = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double b = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            var si = s / _ellipsoid.PolarRadius / a;
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
                       ((1 - _ellipsoid.F) *
                        Math.Sqrt(sinAlpha * sinAlpha +
                                  Math.Pow(
                                      Math.Sin(u1) * Math.Sin(sigma) - Math.Cos(u1) * Math.Cos(sigma) * Math.Cos(a1), 2)));
            lat2 = Math.Atan(lat2);

            // Разность долгот
            var lamda = Math.Atan(Math.Sin(sigma) * Math.Sin(a1) /
                              (Math.Cos(u1) * Math.Cos(sigma) - Math.Sin(u1) * Math.Sin(sigma) * Math.Cos(a1)));

            double c = _ellipsoid.F / 16 * cosSqAlpha * (4 + _ellipsoid.F * (4 - 3 * cosSqAlpha));

            var l = lamda - (1 - c) * _ellipsoid.F * sinAlpha *
                        (sigma +
                         c * Math.Sin(sigma) * (cos2SigmaM + c * Math.Cos(sigma) * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            var lon2 = -l + coord.LonR;

            lon2 = lon2 * 180 / Math.PI;
            lat2 = lat2 * 180 / Math.PI;

            var a2 = -Math.Atan(sinAlpha / (-Math.Sin(u1) * Math.Sin(sigma) + Math.Cos(u1) * Math.Cos(sigma) * Math.Cos(a1))) * 180 / Math.PI;
            a2 = Azimuth.AzimuthRecovery(new Point(lon2, lat2), coord, a2);

            return new DirectProblemAnswer(new Point(lon2, lat2), a2);
        }

        /// <summary>
        /// Решение прямой геодезической задачи на сфероиде
        /// </summary>
        private DirectProblemAnswer DirectProblemSpheroid(Point coord, double a1, double s)
        {
            a1 = a1 * Math.PI / 180;
            var lat1 = coord.LatR;

            var sigma = s / _ellipsoid.PolarRadius;
            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(sigma) + Math.Cos(lat1) * Math.Sin(sigma) * Math.Cos(a1));

            var lambda = Math.Atan(Math.Sin(sigma) * Math.Sin(a1) /
                         (Math.Cos(sigma) * Math.Cos(lat1) - Math.Sin(sigma) * Math.Sin(lat1) * Math.Cos(a1)));
            var lon2 = - lambda + coord.LonR;

            var a2 = -Math.Atan(Math.Cos(lat1) * Math.Sin(a1) /
                     (Math.Cos(lat1) * Math.Cos(sigma) * Math.Cos(a1) - Math.Sin(lat1) * Math.Sin(sigma))) * 180 / Math.PI;
            a2 = Azimuth.AzimuthRecovery(new Point(lon2 * 180 / Math.PI, lat2 * 180 / Math.PI), new Point(coord.Longitude, lat1 * 180 / Math.PI), a2);

            return new DirectProblemAnswer(new Point(lon2* 180 / Math.PI, lat2 * 180 / Math.PI), a2);
        }
    }
}