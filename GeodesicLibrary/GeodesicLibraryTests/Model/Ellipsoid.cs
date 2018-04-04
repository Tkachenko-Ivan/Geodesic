using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;

namespace GeodesicLibraryTests.Model
{
    public class Ellipsoid : IEllipsoid
    {
        public double EquatorialRadius => 6378137;

        public double PolarRadius => 6356752.3142;

        public double F => (EquatorialRadius - PolarRadius) / PolarRadius;
    }
}