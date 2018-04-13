using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.IntermediatePoint
{
    [TestClass]
    public class IntermediatePointSpheroidTests : IntermediatePointTests
    {
        public sealed override IntermediatePointService IntermediatePointService { get; set; }

        public sealed override InverseProblemService InverseProblemService { get; set; }

        public IntermediatePointSpheroidTests()
        {
            IntermediatePointService = new IntermediatePointService(new Spheroid());
            InverseProblemService = new InverseProblemService(new Spheroid());
        }
    }
}