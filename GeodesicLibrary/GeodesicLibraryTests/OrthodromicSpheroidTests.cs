using GeodesicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    [TestClass]
    public class OrthodromicSpheroidTests : OrtodromicTests
    {
        public sealed override DistanceService DistanceService { get; set; }

        public sealed override CoordinatesService CoordinatesService { get; set; }

        public OrthodromicSpheroidTests()
        {
            CoordinatesService = new CoordinatesService(6367444, 6367444);
            DistanceService = new DistanceService(6367444, 6367444);
        }
    }
}