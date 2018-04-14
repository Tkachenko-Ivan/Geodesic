using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class DirectAndInverseProblemTests
    {
        /// <summary>
        /// Простая проверка правильности рещения задач.
        ///     Проверка осуществляется в границах одного полушария, без пересечения экватора, нулевого меридиана и полюса
        /// </summary>
        [TestMethod]
        public void SimpleDirectInverseTest()
        {
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

        /// <summary>
        /// Проверка правильности решения задач при условии пересечения экватора или нулевого меридиана
        ///     проверка пересечения 180ого меридиана не выполняется
        /// </summary>
        [TestMethod]
        public void IntersectionDirectInverseTest()
        {
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

        /// <summary>
        /// Проверка правильности решения задач при условии пересечения 180ого меридиана
        /// </summary>
        [TestMethod]
        public void Intersection180DirectInverseTest()
        {
            // В северном полушарии
            var pointSouthWest = new Point(-170, 10);
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
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointSouthEast, pointNorthEast);
        }

        /// <summary>
        /// Проверка правильности решения задач при условии пересечения полюса
        /// </summary>
        [TestMethod]
        public void IntersectionPolarDirectInverseTest()
        {
            // TODO: Ещё придумтаь его надо
        }

        /// <summary>
        /// Выполнить проверку под разными углами наклона
        /// </summary>
        /// <remarks>
        /// Необходимо чтобы координаты образовывали квадрат, по крайней мере в прямоугольной проекции
        /// </remarks>
        /// <param name="pointSouthWest">Юго-Западный угол</param>
        /// <param name="pointNorthWest">Северо-Западный угол</param>
        /// <param name="pointSouthEast">Юго-Восточный угол</param>
        /// <param name="pointNorthEast">Северо-Восточный угол</param>
        private void AtDifferentAngles(Point pointSouthWest, Point pointNorthWest, Point pointSouthEast, Point pointNorthEast)
        {
            // На Сервер
            ProblemAssert(pointSouthEast, pointNorthEast);
            // На Северо-Восток
            ProblemAssert(pointSouthWest, pointNorthEast);
            // На Восток
            ProblemAssert(pointNorthWest, pointNorthEast);
            // На Юго-Восток
            ProblemAssert(pointNorthWest, pointSouthEast);
            // На Юг
            ProblemAssert(pointNorthWest, pointSouthWest);
            // На Юго-Запад
            ProblemAssert(pointNorthEast, pointSouthWest);
            // На Запад
            ProblemAssert(pointNorthEast, pointNorthWest);
            // На Северо-Запад
            ProblemAssert(pointSouthEast, pointNorthWest);
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
        private void ProblemAssert(Point point1, Point point2)
        {
            ProblemAssert(point1, point2, new Spheroid());
            ProblemAssert(point1, point2, new Ellipsoid());
        }

        private void ProblemAssert(Point point1, Point point2, IEllipsoid ellipsoid)
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