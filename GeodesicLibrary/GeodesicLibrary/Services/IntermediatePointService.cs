using System;
using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;

namespace GeodesicLibrary.Services
{
    /// <summary>
    /// Рассчитывает недостающую координату промежуточной точки, 
    ///  если известны координаты концов отрезка, и широта либо долгота
    /// </summary>
    public class IntermediatePointService
    {
        private const double TOLERANCE = 0.00000000001;

        private readonly IEllipsoid _ellipsoid;

        public IntermediatePointService(IEllipsoid ellipsoid)
        {
            _ellipsoid = ellipsoid;
        }

        /// <summary>
        /// Рассчёт широты как функции долготы
        /// </summary>
        /// <param name="longitude">Долгота промежуточной точки</param>
        /// <param name="coord1">Начальная координата отрезка</param>
        /// <param name="coord2">Конечная координата отрезка</param>
        /// <returns>Широта промежуточной точки</returns>
        public double GetLatitude(double longitude, Point coord1, Point coord2)
        {
            longitude *= Math.PI / 180;
            return Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE
                ? GetLatitudeSpheroid(longitude, coord1, coord2)
                : GetLatitudeEllipsoid(longitude, coord1, coord2);
        }

        /// <summary>
        /// Рассчёт долготы как функции широты
        /// </summary>
        /// <param name="latitude">Широта промежуточной точки</param>
        /// <param name="coord1">Начальная координата отрезка</param>
        /// <param name="coord2">Конечная координата отрезка</param>
        /// <returns>Долгота промежуточной точки</returns>
        public double GetLongitude(double latitude, Point coord1, Point coord2)
        {
            latitude *= Math.PI / 180;
            return Math.Abs(_ellipsoid.EquatorialRadius - _ellipsoid.PolarRadius) < TOLERANCE
                ? GetLongitudeSpheroid(latitude, coord1, coord2)
                : GetLongitudeEllipsoid(latitude, coord1, coord2);
        }

        private double GetLatitudeSpheroid(double longitude, Point coord1, Point coord2)
        {
            return Math.Atan(Math.Tan(coord1.LatR) / Math.Sin(coord2.LonR - coord1.LonR) *
                                 Math.Sin(coord2.LonR - longitude) +
                                 Math.Tan(coord2.LatR) / Math.Sin(coord2.LonR - coord1.LonR) *
                                 Math.Sin(longitude - coord1.LonR)) * 180 / Math.PI;
        }

        private double GetLatitudeEllipsoid(double longitude, Point coord1, Point coord2)
        {
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

                if (IsBetween(first.LonR, coordM.LonR, longitude))
                    second = coordM;
                else if (IsBetween(second.LonR, coordM.LonR, longitude))
                    first = coordM;
            } while (Math.Abs(coordM.LonR - longitude) > TOLERANCE);
            return coordM.Latitude;
        }

        private double GetLongitudeSpheroid(double latitude, Point coord1, Point coord2)
        {
            // Разные знаки - т.е. разные полушария
            if (coord1.Longitude * coord2.Longitude < 0)
            {
                var min = Math.Min(coord1.Longitude, coord2.Longitude);
                var max = Math.Max(coord1.Longitude, coord2.Longitude);
                if (180 - max + 180 + min < 180)
                {
                    // Ближе через 180ый мередиан
                    // Проводим инверсию и решаем инвертированную задачу
                    double res = coord1.Longitude < 0
                        ? GetLongitude(latitude, new Point(coord1.Longitude + 180, coord1.Latitude),
                            new Point(coord2.Longitude - 180, coord2.Latitude))
                        : GetLongitude(latitude, new Point(coord1.Longitude - 180, coord1.Latitude),
                            new Point(coord2.Longitude + 180, coord2.Latitude));
                    // Корректируем инверсию
                    return res > 0 ? res - 180 : 180 + res;
                }
            }

            // Приводим задачу к диапазону координат от -90 до +90
            if (coord1.Longitude < -90 || coord2.Longitude < -90)
            {
                var delta = Math.Abs(Math.Min(coord1.Longitude, coord2.Longitude)) - 90;
                return
                    GetLonSpheroid(latitude, new Point(coord1.Longitude + delta, coord1.Latitude),
                        new Point(coord2.Longitude + delta, coord2.Latitude)) - delta;
            }
            if (coord1.Longitude > 90 || coord2.Longitude > 90)
            {
                var delta = Math.Max(coord1.Longitude, coord2.Longitude) - 90;
                return
                    GetLonSpheroid(latitude, new Point(coord1.Longitude - delta, coord1.Latitude),
                        new Point(coord2.Longitude - delta, coord2.Latitude)) + delta;
            }

            // Видимо задача и так в нужном диапазоне
            return GetLonSpheroid(latitude, coord1, coord2);
        }

        /// <summary>
        /// Определяет долготу промежуточной точки.
        /// Работает в диапазоне от -90 до +90 для долготы.
        /// </summary>
        private double GetLonSpheroid(double latitude, Point coord1, Point coord2)
        {
            double a1 = Math.Tan(coord1.LatR) / Math.Sin(coord2.LonR - coord1.LonR);
            double a2 = Math.Tan(coord2.LatR) / Math.Sin(coord2.LonR - coord1.LonR);
            double b = a1 * Math.Sin(coord2.LonR) - a2 * Math.Sin(coord1.LonR);
            double c = a2 * Math.Cos(coord1.LonR) - a1 * Math.Cos(coord2.LonR);

            double lonfet = Math.Asin(Math.Tan(latitude) / Math.Sqrt(b * b + c * c));

            // В зависимости от значений c, b и угла - разные преобразования ответа

            if (c > 0 && b > 0)
                return (lonfet + Math.Atan(c / b)) * 180 / Math.PI - 90;
            if (c < 0 && b < 0)
                return -90 - (lonfet - Math.Atan(c / b)) * 180 / Math.PI;

            var angle = Math.Atan((coord2.LatR - coord1.LatR) / (coord2.LonR - coord1.LonR)) * 180 / Math.PI;

            if (Math.Abs(b) < 0.00000001 && angle > 0)
                return -90 + (lonfet + Math.Atan(c / b)) * 180 / Math.PI;
            if (Math.Abs(b) < 0.00000001 && angle < 0)
                return 90 - (lonfet - Math.Atan(c / b)) * 180 / Math.PI;

            if (c * angle > 0)
            {
                if (c > 0)
                    return (lonfet + Math.Atan(c / b)) * 180 / Math.PI + 90;
                return 90 - (lonfet - Math.Atan(c / b)) * 180 / Math.PI;
            }
            else
            {
                if (c > 0)
                    return -90 - (lonfet - Math.Atan(c / b)) * 180 / Math.PI;
                return -90 + (lonfet + Math.Atan(c / b)) * 180 / Math.PI;
            }
        }

        private double GetLongitudeEllipsoid(double latitude, Point coord1, Point coord2)
        {
            var inverce = new InverseProblemService(_ellipsoid);
            var direct = new DirectProblemService(_ellipsoid);
            Point coordM;
            Point first = coord1, second = coord2;
            do
            {
                var dist = inverce.OrthodromicDistance(first, second);
                coordM = direct.DirectProblem(first, dist.ForwardAzimuth, dist.Distance / 2).Сoordinate;

                if (Math.Abs(latitude - first.LatR) < TOLERANCE)
                    return first.Longitude;
                if (Math.Abs(latitude - second.LatR) < TOLERANCE)
                    return second.Longitude;

                if (IsBetween(first.LatR, coordM.LatR, latitude))
                    second = coordM;
                else if (IsBetween(second.LatR, coordM.LatR, latitude))
                    first = coordM;
            } while (Math.Abs(coordM.LatR - latitude) > TOLERANCE);
            return coordM.Longitude;
        }

        /// <summary>
        /// Проверяет утвержение о том, что временное значение находитя между первым и вторым
        /// </summary>
        /// <param name="first">Первое значение</param>
        /// <param name="second">Второе значение</param>
        /// <param name="temp">Проверяемое значение</param>
        private bool IsBetween(double first, double second, double temp)
        {
            return temp > second && temp < first || temp > first && temp < second;
        }
    }
}