using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class AzimutDialTests : Plan
    {

        [TestMethod]
        public void SimpleAzimutDialTest()
        {
            SimpleTest();
        }


        [TestMethod]
        public void IntersectionAzimutDialTest()
        {
            IntersectionTest();
        }


        [TestMethod]
        public void Intersection180AzimutDialTest()
        {
            Intersection180Test();
        }

        /// <summary>
        /// Тестирется правильность определения азимута относительно различных точек,
        ///  как при движении часовой стрелки по циферблату
        /// </summary>
        /// <remarks>
        /// Логика в том, что при движении по часовй стрелке значение азимута должно уменьшаться
        /// </remarks>
        public override void AtDifferentAngles(Point pointSouthWest, Point pointNorthWest, Point pointNorth, Point pointNorthEast, Point pointEast, Point pointSouthEast, IEllipsoid ellipsoid)
        {
            // На 12 часов
            var a01 = TestsAzimuth(pointSouthEast, pointNorthEast, ellipsoid);
            Assert.AreEqual(360, a01);
            var a02 = TestsAzimuth(pointSouthWest, pointNorthWest, ellipsoid);
            Assert.AreEqual(360, a02);
            // На 1 час
            var a03 = TestsAzimuth(pointSouthWest, pointNorth, ellipsoid);
            // На 1,5 часа
            var a04 = TestsAzimuth(pointSouthWest, pointNorthEast, ellipsoid);
            // На 2 часа
            var a05 = TestsAzimuth(pointSouthWest, pointEast, ellipsoid);
            // На 3 часа
            var a06 = TestsAzimuth(pointSouthWest, pointSouthEast, ellipsoid);
            var a07 = TestsAzimuth(pointNorthWest, pointNorthEast, ellipsoid);
            // На 4 часа
            var a08 = TestsAzimuth(pointNorthWest, pointEast, ellipsoid);
            // На 4,5 часа
            var a09 = TestsAzimuth(pointNorthWest, pointSouthEast, ellipsoid);
            // На 5 часов
            var a10 = TestsAzimuth(pointNorth, pointSouthEast, ellipsoid);
            // На 6 часов
            var a11 = TestsAzimuth(pointNorthWest, pointSouthWest, ellipsoid);
            Assert.AreEqual(180, a11);
            var a12 = TestsAzimuth(pointNorthEast, pointSouthEast, ellipsoid);
            Assert.AreEqual(180, a12);
            // На 7 часов
            var a13 = TestsAzimuth(pointNorth, pointSouthWest, ellipsoid);
            // На 7,5 часов
            var a14 = TestsAzimuth(pointNorthEast, pointSouthWest, ellipsoid);
            // На 8 часов
            var a15 = TestsAzimuth(pointEast, pointSouthWest, ellipsoid);
            // На 9 часов
            var a16 = TestsAzimuth(pointNorthEast, pointNorthWest, ellipsoid);
            var a17 = TestsAzimuth(pointSouthEast, pointSouthWest, ellipsoid);
            // На 10 часов
            var a18 = TestsAzimuth(pointEast, pointNorthWest, ellipsoid);
            // На 10,5 часов
            var a19 = TestsAzimuth(pointSouthEast, pointNorthWest, ellipsoid);
            // На 11 часов
            var a20 = TestsAzimuth(pointSouthEast, pointNorth, ellipsoid);

            Assert.IsTrue(a03 < a02);
            Assert.IsTrue(a04 < a03);
            Assert.IsTrue(a05 < a04);
            Assert.IsTrue(a06 < a05);

            Assert.IsTrue(a08 < a07);
            Assert.IsTrue(a09 < a08);
            Assert.IsTrue(a10 < a09);
            Assert.IsTrue(a11 < a10);

            Assert.IsTrue(a13 < a12);
            Assert.IsTrue(a14 < a13);
            Assert.IsTrue(a15 < a14);
            Assert.IsTrue(a16 < a15);

            Assert.IsTrue(a18 < a17);
            Assert.IsTrue(a19 < a18);
            Assert.IsTrue(a20 < a19);
        }

        public double TestsAzimuth(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            var inverseProblemService = new InverseProblemService(ellipsoid);
            return inverseProblemService.OrthodromicDistance(point1, point2).ForwardAzimuth;
        }

        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            // Не используется и не должен
            throw new System.NotImplementedException();
        }
    }
}