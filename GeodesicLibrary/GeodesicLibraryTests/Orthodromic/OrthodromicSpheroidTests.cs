using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    [TestClass]
    public class OrthodromicSpheroidTests : OrtodromicTests
    {
        public sealed override InverseProblemService InverseProblemService { get; set; }

        public sealed override DirectProblemService DirectProblemService { get; set; }

        public OrthodromicSpheroidTests()
        {
            DirectProblemService = new DirectProblemService(new Ellipsoid());
            InverseProblemService = new InverseProblemService(new Ellipsoid());
        }
    }
}