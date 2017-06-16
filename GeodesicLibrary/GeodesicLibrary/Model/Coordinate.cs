using System.Runtime.CompilerServices;

namespace GeodesicLibrary.Model
{
    public class Coordinate
    {
        public Coordinate(double lon, double lat)
        {
            Longitude = lon;
            Latitude = lat;
        }

        public double Longitude { get; }

        public double Latitude { get; }
    }
}
