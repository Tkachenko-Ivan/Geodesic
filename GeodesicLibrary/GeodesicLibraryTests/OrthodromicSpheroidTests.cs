using GeodesicLibrary;
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
            DirectProblemService = new DirectProblemService(6367444, 6367444);
            InverseProblemService = new InverseProblemService(6367444, 6367444);
        }
    }
}