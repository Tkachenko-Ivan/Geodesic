using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class DirectAndInverseProblemTests : Plan
    {

        [TestMethod]
        public void SimpleDirectInverseTest()
        {
            SimpleTest();
        }

        [TestMethod]
        public void IntersectionDirectInverseTest()
        {
            IntersectionTest();
        }

        [TestMethod]
        public void Intersection180DirectInverseTest()
        {
            Intersection180Test();
        }

        [TestMethod]
        public void IntersectionPolarDirectInverseTest()
        {
            IntersectionPolarTest();
        }
       
        /// <summary>
        /// Тестирование решения прямой и обратной задач
        /// </summary>
        /// <remarks>
        /// Тест состоит из нескольких частей:
        ///     1. Задаются координаты между которыми рассчитывается ортодромическая дистанция и определяется азимут 
        ///         - решается обратная геодезическая задача
        ///     2. По первой координате, дистанции и прямому направлению (азимуту) находим вторую координату 
        ///         - решается прямая геодезическая задача
        ///     3. Выполняется проверка 
        ///         - вторая координата из условий обратной задачи должна совпасть с координатой из решения прямой задачи, 
        ///         - обратный азимут из решения обратной задачи, должен совпадать с обратным азимутом из решения прямой задачи
        ///     4. По второй координате, дистанции и обратному направлению (азимуту) находим первую координату
        ///         - решаетая прямая геодезическая задача
        ///     5. Выполняется проверка 
        ///         - первая координата из условий обратной задачи должна совпасть с координатой из решения прямой задачи
        ///         - прямой азимут из решения обратной задачи, должен совпадать с обратным азимутом из решения прямой задачи
        /// </remarks>
        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            var inverseProblemService = new InverseProblemService(ellipsoid);
            var directProblemService = new DirectProblemService(ellipsoid);

            // Решение обратной задачи
            var inverseAnswer = inverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = directProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                inverseProblemService.OrthodromicDistance(
                    new Point(directAnswerForward.Сoordinate.Longitude, directAnswerForward.Сoordinate.Latitude),
                    point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = directProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                inverseProblemService.OrthodromicDistance(
                        new Point(directAnswerReverse.Сoordinate.Longitude, directAnswerReverse.Сoordinate.Latitude),
                        point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001); 
        }
    }
}