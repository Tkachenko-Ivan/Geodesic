using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.IntermediatePoint
{
    /// <summary>
    /// Тестируется определение долготы как функции широты или широты как функции долготы
    /// </summary>
    public abstract class IntermediatePointTests
    {
        public abstract IntermediatePointService IntermediatePointService { get; set; }

        [TestMethod]
        public void GetLonByLat()
        {
        }
    }
}