using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{

    [TestClass]
    public class IntersectTests
    {
        [TestMethod]
        public void SimpleDirectInverseTest()
        {
            Assert.Inconclusive();
            // Северо-Восточное полушарие
            var pointSouthWest = new Point(15, 10);
            var pointNorthWest = new Point(15, 25);
            var pointSouthEast = new Point(30, 10);
            var pointNorthEast = new Point(30, 25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Северо-Западное полушарие
            pointSouthWest = new Point(-45, 10);
            pointNorthWest = new Point(-45, 25);
            pointSouthEast = new Point(-30, 10);
            pointNorthEast = new Point(-30, 25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Юго-Восточное полушарие
            pointSouthWest = new Point(15, -40);
            pointNorthWest = new Point(15, -25);
            pointSouthEast = new Point(30, -40);
            pointNorthEast = new Point(30, -25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Юго-Западное полушарие
            pointSouthWest = new Point(-45, -40);
            pointNorthWest = new Point(-45, -25);
            pointSouthEast = new Point(-30, -40);
            pointNorthEast = new Point(-30, -25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);
        }

        [TestMethod]
        public void IntersectionDirectInverseTest()
        {
            Assert.Inconclusive();
            // Северное полушарие: пересечение нулевого меридиана
            var pointSouthWest = new Point(-15, 10);
            var pointNorthWest = new Point(-15, 25);
            var pointSouthEast = new Point(15, 10);
            var pointNorthEast = new Point(15, 25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Южное полушарие: пересечение нулевого меридиана
            pointSouthWest = new Point(-15, -25);
            pointNorthWest = new Point(-15, -10);
            pointSouthEast = new Point(15, -25);
            pointNorthEast = new Point(15, -10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Восточное полушарие: пересечение экватора
            pointSouthWest = new Point(15, -10);
            pointNorthWest = new Point(15, 10);
            pointSouthEast = new Point(30, -10);
            pointNorthEast = new Point(30, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Западное полушарие: пересечение экватора
            pointSouthWest = new Point(-45, -10);
            pointNorthWest = new Point(-45, 10);
            pointSouthEast = new Point(-15, -10);
            pointNorthEast = new Point(-15, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // Пересечение экватора и нелувого меридина
            pointSouthWest = new Point(-15, -10);
            pointNorthWest = new Point(-15, 10);
            pointSouthEast = new Point(15, -10);
            pointNorthEast = new Point(15, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);
        }

        [TestMethod]
        public void Intersection180DirectInverseTest()
        {
            Assert.Inconclusive();
            // TODO: Здесь вообще зависает
            // В северном полушарии
         /*   var pointSouthWest = new Point(-170, 10);
            var pointNorthWest = new Point(-170, 25);
            var pointSouthEast = new Point(170, 10);
            var pointNorthEast = new Point(170, 25);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // В южном полушарии
            pointSouthWest = new Point(-170, -25);
            pointNorthWest = new Point(-170, -10);
            pointSouthEast = new Point(170, -25);
            pointNorthEast = new Point(170, -10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);

            // На экваторе
            pointSouthWest = new Point(-170, -10);
            pointNorthWest = new Point(-170, 10);
            pointSouthEast = new Point(170, -10);
            pointNorthEast = new Point(170, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);*/
        }

        /// <summary>
        /// Проверка правильности решения задач при условии пересечения полюса
        /// </summary>
        [TestMethod]
        public void IntersectionPolarDirectInverseTest()
        {
            Assert.Inconclusive();
        }

        private void AtDifferentAngles(Point pointSouthWest, Point pointNorthWest, Point pointSouthEast, Point pointNorthEast)
        {
            PointIntersectTest(pointNorthWest, pointSouthEast, pointNorthEast, pointSouthWest);
            PointIntersectTest(pointNorthWest, pointSouthEast,pointSouthWest , pointNorthEast);
            PointIntersectTest(pointSouthEast, pointNorthWest,  pointNorthEast, pointSouthWest);
            PointIntersectTest(pointSouthEast, pointNorthWest, pointSouthWest, pointNorthEast); 
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
        public void PointIntersectTest(Point point1, Point point2, Point point3, Point point4)
        {
            PointIntersectTest(point1, point2, point3, point4, new Spheroid());
            PointIntersectTest(point1, point2, point3, point4, new Ellipsoid());
        }

        private void PointIntersectTest(Point point1, Point point2, Point point3, Point point4, IEllipsoid ellipsoid)
        {
            var intersectService = new IntersectService(ellipsoid);
            var inverseProblemService = new InverseProblemService(ellipsoid);

            var intersectCoord = intersectService.IntersectOrthodromic(point1,
                point2, point3, point4);

            var firstOrtodrom = inverseProblemService.OrthodromicDistance(point1, point2);
            var secondOrtodrom = inverseProblemService.OrthodromicDistance(point3, point4);

            var firstOrtodrom2 = inverseProblemService.OrthodromicDistance(point1, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));
            var secondOrtodrom2 = inverseProblemService.OrthodromicDistance(point3, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));

            Assert.AreEqual(firstOrtodrom.ForwardAzimuth, firstOrtodrom2.ForwardAzimuth, 0.00000001);
            Assert.AreEqual(secondOrtodrom.ForwardAzimuth, secondOrtodrom2.ForwardAzimuth, 0.00000001);
        }
    }
}