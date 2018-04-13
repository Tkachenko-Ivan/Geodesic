using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.Azimuth
{
    [TestClass]
    public class AzimutSpheroidTests : AzimutTests
    {
        public sealed override InverseProblemService InverseProblemService { get; set; }

        public sealed override DirectProblemService DirectProblemService { get; set; }

        public AzimutSpheroidTests()
        {
            DirectProblemService = new DirectProblemService(new Spheroid());
            InverseProblemService = new InverseProblemService(new Spheroid());
        }
    }
}