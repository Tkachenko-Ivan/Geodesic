using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class DirectAndInverseProblemEllipsoidTests : DirectAndInverseProblemTests
    {
        public sealed override InverseProblemService InverseProblemService { get; set; }

        public sealed override DirectProblemService DirectProblemService { get; set; }

        public DirectAndInverseProblemEllipsoidTests()
        {
            DirectProblemService = new DirectProblemService(new Ellipsoid());
            InverseProblemService = new InverseProblemService(new Ellipsoid());
        }
    }
}