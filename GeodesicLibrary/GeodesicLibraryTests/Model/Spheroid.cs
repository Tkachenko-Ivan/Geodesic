using GeodesicLibrary.Model;

namespace GeodesicLibraryTests.Model
{
    public class Spheroid : IEllipsoid
    {
        public double EquatorialRadius => 6367444;

        public double PolarRadius => 6367444;

        public double F => 0;
    }
}