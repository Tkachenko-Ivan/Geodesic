using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class AzimutTests : Plan
    {

        [TestMethod]
        public void SimpleAzimutTest()
        {
            SimpleTest();
        }


        [TestMethod]
        public void IntersectionAzimutTest()
        {
            IntersectionTest();
        }


        [TestMethod]
        public void Intersection180AzimutTest()
        {
            Intersection180Test();
        }

        [TestMethod]
        public void LongLineAzimutTest()
        {
            LongLineTest();
        }

        [TestMethod]
        public void IntersectionPolarAzimutTest()
        {
            IntersectionPolarTest();
        }

        /// <summary>
        /// Проверка того как изменяется азимут при движении по прямой
        /// </summary>
        [TestMethod]
        public void СhangeOfAzimutLineTest()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Изменение азимута при движении по окружности
        /// </summary>
        [TestMethod]
        public void СhangeOfAzimutCircleTest()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Тестируется правильность определения азимута
        /// </summary>
        /// <remarks>
        /// Первый критерий правильности определения азимута таков:
        ///     - сначала решается обратная геодезическая задача для определения азимута из первой точки до второй
        ///     - затем решается обратная геодезическая задача для определения азимута из второй точки до первой
        ///     - сравниваются прямые и обратные азимуты двух решений
        /// Второй критерий:
        ///     - сначала решается обратная геодезическая задача для определения азимута и дистанции
        ///     - из первой точки по заданной дистанции мы решаем прямую геодезическую задача: должны попасть во вторую точку
        ///     - и наоборот 
        /// </remarks>
        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            var inverseProblemService = new InverseProblemService(ellipsoid);
            var directProblemService = new DirectProblemService(ellipsoid);

            var answer1 = inverseProblemService.OrthodromicDistance(point1, point2);
            var answer2 = inverseProblemService.OrthodromicDistance(point2, point1);
            Assert.AreEqual(answer1.ForwardAzimuth, answer2.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(answer1.ReverseAzimuth, answer2.ForwardAzimuth, 0.000000001);

            var answerPoint1 = directProblemService.DirectProblem(point1, answer1.ForwardAzimuth, answer1.Distance);
            Assert.AreEqual(point2.Latitude, answerPoint1.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point2.Longitude, answerPoint1.Сoordinate.Longitude, 0.000000001);

            var answerPoint2 = directProblemService.DirectProblem(point2, answer2.ForwardAzimuth, answer2.Distance);
            Assert.AreEqual(point1.Latitude, answerPoint2.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point1.Longitude, answerPoint2.Сoordinate.Longitude, 0.000000001);
        }
    }
}