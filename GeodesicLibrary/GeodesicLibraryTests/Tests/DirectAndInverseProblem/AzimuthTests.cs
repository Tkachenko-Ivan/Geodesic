using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class AzimutTests
    {
        public InverseProblemService InverseProblemService { get; set; }

        public DirectProblemService DirectProblemService { get; set; }

        public AzimutTests()
        {
            DirectProblemService = new DirectProblemService(new Spheroid());
            InverseProblemService = new InverseProblemService(new Spheroid());
        }

        /// <summary>
        /// Тестируется правильность определения азимута в западном и восточном полушариях
        /// </summary>
        /// <remarks>
        /// Критерий правильности определения азимута таков:
        ///     - сначала решается обратная геодезическая задача и определяется азимут и дистанция
        ///     - затем решается прямая геодезическая задача
        /// При правильном азимуте, точка из решения обратной задачи должна совпасть с точкой из решения прямой.
        /// </remarks>
        [TestMethod]
        public void AzimutTest()
        {
            var point11 = new Point(15, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);
            var point12 = new Point(25, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);

            var point21 = new Point(15, 0, 0, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);
            var point22 = new Point(25, 0, 0, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);

            var answer1 = InverseProblemService.OrthodromicDistance(point11, point12);
            var answer2 = InverseProblemService.OrthodromicDistance(point21, point22);

            var answerPoint1 = DirectProblemService.DirectProblem(point11, answer1.ForwardAzimuth, answer1.Distance);
            var answerPoint2 = DirectProblemService.DirectProblem(point21, answer2.ForwardAzimuth, answer2.Distance);

            Assert.AreEqual(point12.Latitude, answerPoint1.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point12.Longitude, answerPoint1.Сoordinate.Longitude, 0.000000001);

            Assert.AreEqual(point22.Latitude, answerPoint2.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point22.Longitude, answerPoint2.Сoordinate.Longitude, 0.000000001);
        }

    }
}