using GeodesicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    [TestClass]
    public class OrthodromicEllipsoidTests : OrtodromicTests
    {
        public sealed override InverseProblemService InverseProblemService { get; set; }

        public sealed override DirectProblemService DirectProblemService { get; set; }

        public OrthodromicEllipsoidTests()
        {
            DirectProblemService = new DirectProblemService(6378137, 6356752.3142);
            InverseProblemService = new InverseProblemService(6378137, 6356752.3142);
        }
    }
}