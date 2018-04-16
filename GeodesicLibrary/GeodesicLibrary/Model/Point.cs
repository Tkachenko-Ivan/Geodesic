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
            var message = "";
            if (lon > 180)
                lon = -(360 - lon);
            if (lon < -180)
                lon = 360 + lon;
            if (Math.Abs(lon) > 180)
                message += $"Значение долготы должно находиться в интервале [-180;180], текущее значение долготы: {lon}.";
            if (Math.Abs(lat) > 90)
                message += (message == string.Empty ? "" : "\n") + $"Значение широты должно находиться в интервале [-90;90], текущее значение широты: {lat}.";
            if (message != string.Empty)
                throw new Exception(message);

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
        public Point(int lonD, int lonM, double lonS, CardinalLongitude lonCardinal, int latD, int latM,
            double latS, CardinalLatitude latCardinal)
        {
            var message = "";
            if (lonM > 60 || lonM < 0 || latM > 60 || latM < 0)
                message += "Минуты должны быть в интевале [0;60].";
            if (lonS > 60 || lonS < 0 || latS > 60 || latS < 0)
                message += (message == string.Empty ? "" : "\n") + "Секунды должны быть в интевале [0;60].";
            if (message != string.Empty)
                throw new Exception(message);

            _longitude = lonCardinal == CardinalLongitude.W
                ? Converter.DergeeToDecimalDegree(-lonD, -lonM, -lonS)
                : Converter.DergeeToDecimalDegree(lonD, lonM, lonS);

            _latitude = latCardinal == CardinalLatitude.S
                ? Converter.DergeeToDecimalDegree(-latD, -latM, -latS)
                : Converter.DergeeToDecimalDegree(latD, latM, latS);

            if (Math.Abs(_longitude) > 180)
                message += $"Значение долготы должно находиться в интервале [-180;180], текущее значение долготы: {_longitude}.";
            if (Math.Abs(_latitude) > 90)
                message += (message == string.Empty ? "" : "\n") + $"Значение широты должно находиться в интервале [-90;90], текущее значение широты: {_latitude}.";
            if (message != string.Empty)
                throw new Exception(message);
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