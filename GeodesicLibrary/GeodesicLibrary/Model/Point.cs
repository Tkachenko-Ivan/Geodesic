using System;
using GeodesicLibrary.Tools;

namespace GeodesicLibrary.Model
{
    public class Point
    {
        private readonly double _longitude;

        private readonly double _latitude;

        public Point(double lon, double lat)
        {
            _longitude = lon;
            _latitude = lat;
        }

        public Point(double lonD, double lonM, double lonS, double latD, double latM, double latS)
            : this(Converter.DergeeToDecimalDegree(lonD, lonM, lonS), Converter.DergeeToDecimalDegree(latD, latM, latS))
        {

        }

        public double Longitude => _longitude;

        public double Latitude => _latitude;

        public double LonR => _longitude / 180 * Math.PI;

        public double LatR => _latitude / 180 * Math.PI;
    }

    public enum CardinalDirection
    {
        Eastern,
        Western,
        Northern,
        Southern
    }
}