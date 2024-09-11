using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class IntersectTests : Plan
    {
        [TestMethod]
        public void SimpleIntersectTest()
        {
            SimpleTest();
        }


        [TestMethod]
        public void IntersectionIntersectTest()
        {
            IntersectionTest();
        }


        [TestMethod]
        public void Intersection180IntersectTest()
        {
            Intersection180Test();
        }

        [TestMethod]
        public void IntersectionPolarAzimutTest()
        {
            Assert.Inconclusive();

            // Северный полюс
            var point11 = new Point(50, 70);
            var point12 = new Point(-130, 50);
            var point21 = new Point(120, 70);
            var point22 = new Point(-60, 50);
            Tests(point11, point12, point21, point22, new Spheroid());
            Tests(point11, point12, point21, point22, new Ellipsoid());
        }

        public override void AtDifferentAngles(Point pointSouthWest, Point pointNorthWest, Point pointNorth,
            Point pointNorthEast,
            Point pointEast, Point pointSouthEast, IEllipsoid ellipsoid)
        {
            Tests(pointSouthWest, pointNorth, pointNorthWest, pointEast, ellipsoid);
            Tests(pointNorth, pointSouthWest, pointNorthWest, pointEast, ellipsoid);
            Tests(pointSouthWest, pointNorth, pointEast, pointNorthWest, ellipsoid);
            Tests(pointNorth, pointSouthWest, pointEast, pointNorthWest, ellipsoid);

            Tests(pointSouthWest, pointNorthEast, pointNorthWest, pointEast, ellipsoid);
            Tests(pointNorthEast, pointSouthWest, pointNorthWest, pointEast, ellipsoid);
            Tests(pointSouthWest, pointNorthEast, pointEast, pointNorthWest, ellipsoid);
            Tests(pointNorthEast, pointSouthWest, pointEast, pointNorthWest, ellipsoid);
            Tests(pointSouthWest, pointNorthEast, pointNorth, pointSouthEast, ellipsoid);
            Tests(pointNorthEast, pointSouthWest, pointNorth, pointSouthEast, ellipsoid);
            Tests(pointSouthWest, pointNorthEast, pointSouthEast, pointNorth, ellipsoid);
            Tests(pointNorthEast, pointSouthWest, pointSouthEast, pointNorth, ellipsoid);

            Tests(pointSouthWest, pointEast, pointNorth, pointSouthEast, ellipsoid);
            Tests(pointEast, pointSouthWest, pointNorth, pointSouthEast, ellipsoid);
            Tests(pointSouthWest, pointEast, pointSouthEast, pointNorth, ellipsoid);
            Tests(pointEast, pointSouthWest, pointSouthEast, pointNorth, ellipsoid);
        }

        /// <summary>
        /// Тестируется правильность вычисления точки пересечения двух ортодром
        /// </summary>
        /// <remarks>
        /// Методика тестирования следующая:
        ///     - находим координаты точки пересечения двух ортодром заданных начальной и конечной координатой
        ///     - решаем обратную геодезическую задачу для обоих ортодром - определяем прямой азимут
        ///     - решаем обратную геодезическую задачу от начальной точки до найденной точки пересечения (для обоих ортодром)
        ///     - сравниваем азимуты, если точка пересечения найдена верно, азимуты не должны поменяться
        /// </remarks>
        public void Tests(Point point1, Point point2, Point point3, Point point4, IEllipsoid ellipsoid)
        {
            var intersectService = new IntersectService(ellipsoid);
            var inverseProblemService = new InverseProblemService(ellipsoid);

            var intersectCoord = intersectService.IntersectOrthodromic(point1, point2, point3, point4);

            var firstOrtodrom = inverseProblemService.OrthodromicDistance(point1, point2);
            var secondOrtodrom = inverseProblemService.OrthodromicDistance(point3, point4);

            var firstOrtodrom2 = inverseProblemService.OrthodromicDistance(point1, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));
            var secondOrtodrom2 = inverseProblemService.OrthodromicDistance(point3, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));

            Assert.AreEqual(firstOrtodrom.ForwardAzimuth, firstOrtodrom2.ForwardAzimuth, 0.0000001);
            Assert.AreEqual(secondOrtodrom.ForwardAzimuth, secondOrtodrom2.ForwardAzimuth, 0.0000001);
        }

        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            // Не используется и не должен
            throw new System.NotImplementedException();
        }
    }
}