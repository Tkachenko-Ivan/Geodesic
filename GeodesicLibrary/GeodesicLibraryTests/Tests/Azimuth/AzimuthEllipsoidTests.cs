using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.Azimuth
{
    [TestClass]
    public class AzimutEllipsoidTests : AzimutTests
    {
        public sealed override InverseProblemService InverseProblemService { get; set; }

        public sealed override DirectProblemService DirectProblemService { get; set; }

        public AzimutEllipsoidTests()
        {
            DirectProblemService = new DirectProblemService(new Ellipsoid());
            InverseProblemService = new InverseProblemService(new Ellipsoid());
        }
    }
}