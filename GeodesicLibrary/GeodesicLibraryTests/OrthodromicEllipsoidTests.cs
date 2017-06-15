using GeodesicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    [TestClass]
    public class OrthodromicEllipsoidTests : OrtodromicTests
    {
        public sealed override DistanceService DistanceService { get; set; }

        public sealed override CoordinatesService CoordinatesService { get; set; }

        public OrthodromicEllipsoidTests()
        {
            CoordinatesService = new CoordinatesService(6378137, 6356752.3142);
            DistanceService = new DistanceService(6378137, 6356752.3142);
        }
    }
}