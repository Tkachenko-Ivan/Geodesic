using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.Azimuth
{
    public abstract class AzimutTests
    {
        public abstract InverseProblemService InverseProblemService { get; set; }

        public abstract DirectProblemService DirectProblemService { get; set; }

        /// <summary>
        /// Тестируется правильность определения азимута
        /// </summary>
        /// <remarks>
        /// Критерий правильности определения азимута таков:
        ///     - сначала решается обратная геодезическая задача и определяется азимут и дистанция
        ///     - затем решается прямая геодезическая задача
        /// При правильном азимуте, точка из решения обратной задачи должна совпасть с точкой из решения прямой.
        /// </remarks>
        [TestMethod]
        public void SimpleAzimutTest()
        {
            // Восточное полушарие - экватор
            AzimuthTest(new Point(15, 0), new Point(25, 0));
            // Западное полушарие - экватор
            AzimuthTest(new Point(-15, 0), new Point(-25, 0));
        }

        private void AzimuthTest(Point point1, Point point2)
        {
            var answer1 = InverseProblemService.OrthodromicDistance(point1, point2);
            var answerPoint1 = DirectProblemService.DirectProblem(point1, answer1.ForwardAzimuth, answer1.Distance);
            Assert.AreEqual(point2.Latitude, answerPoint1.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point2.Longitude, answerPoint1.Сoordinate.Longitude, 0.000000001);
        }

    }
}