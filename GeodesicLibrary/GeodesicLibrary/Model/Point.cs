using System;
using GeodesicLibrary.Tools;

namespace GeodesicLibrary.Model
{
    public class Point
    {
        private readonly double _longitude;

        private readonly double _latitude;

        /// <summary>
        /// Задание координат точки через десятичные градусы
        /// </summary>
        /// <param name="lon">Долгота, восточная принимает положительные значения, западня - отрицательные</param>
        /// <param name="lat">Широта, северная принимает положительные значения, южная - отрицательные</param>
        public Point(double lon, double lat)
        {
            _longitude = lon;
            _latitude = lat;
        }

        /// <summary>
        /// Задание координат точки через градусы минуты и секунды
        /// </summary>
        /// <param name="lonD">Долгота, градусы</param>
        /// <param name="lonM">Долгота, минуты</param>
        /// <param name="lonS">Долгота, секунды</param>
        /// <param name="lonCardinal">Западное/Восточное полушарие</param>
        /// <param name="latD">Широта, градусы</param>
        /// <param name="latM">Широта, минуты</param>
        /// <param name="latS">Широта, секунды</param>
        /// <param name="latCardinal">Северное/Южное получшарие</param>
        public Point(double lonD, double lonM, double lonS, CardinalLongitude lonCardinal, double latD, double latM,
            double latS, CardinalLatitude latCardinal)
        {
            // TODO: защита от дурака

            _longitude = lonCardinal == CardinalLongitude.W
                ? Converter.DergeeToDecimalDegree(-lonD, -lonM, -lonS)
                : Converter.DergeeToDecimalDegree(lonD, lonM, lonS);

            _latitude = latCardinal == CardinalLatitude.S
                ? Converter.DergeeToDecimalDegree(-latD, -latM, -latS)
                : Converter.DergeeToDecimalDegree(latD, latM, latS);
        }

        /// <summary>
        /// Долгота, десятичные градусы
        /// </summary>
        public double Longitude => _longitude;

        /// <summary>
        /// Широта, десятичные градусы
        /// </summary>
        public double Latitude => _latitude;

        /// <summary>
        /// Долгота, радианы
        /// </summary>
        public double LonR => _longitude / 180 * Math.PI;

        /// <summary>
        /// Широта, радианы
        /// </summary>
        public double LatR => _latitude / 180 * Math.PI;
    }

    public enum CardinalLatitude
    {
        N, // Northern
        S // Southern
    }

    public enum CardinalLongitude
    {
        E, // Eastern
        W // Western
    }
}