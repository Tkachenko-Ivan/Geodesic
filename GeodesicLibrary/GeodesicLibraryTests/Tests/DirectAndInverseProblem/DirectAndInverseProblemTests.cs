using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    /// <summary>
    /// Тестируется решение прямой и обратной геодезических задач. 
    /// Для тестирования правильности отпределения азимута тестируются решения для всех сторон света и полушарий
    /// </summary>
    public abstract class DirectAndInverseProblemTests
    {
        public abstract InverseProblemService InverseProblemService { get; set; }

        public abstract DirectProblemService DirectProblemService { get; set; }

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
        [TestMethod]
        public void SimpleDirectInverseTest()
        {
            // Северо-Восточное полушарие
            AtDifferentAngles(new Point(15.3, 13.1), new Point(59.4, 13.1), new Point(15.3, 28.1), new Point(59.4, 28.1),
                new Point(15.3, 0), new Point(59.4, 0));
            // Юго-Восточное полушарие
            /*AtDifferentAngles(new Point(15.3, -28.1), new Point(59.4, -28.1), new Point(15.3, -13.1), new Point(59.4, -13.1),
                new Point(15.3, 0), new Point(59.4, 0));*/
            // Северо-Западное полушарие
            AtDifferentAngles(new Point(-59.4, 13.1), new Point(-15.3, 13.1), new Point(-59.4, 28.1), new Point(-15.3, 28.1),
                new Point(-59.4, 0), new Point(-15.3, 0));
            // Юго-Западное полушарие
           /* AtDifferentAngles(new Point(-59.4, -28.1), new Point(-15.3, -28.1), new Point(-59.4, -13.1), new Point(-15.3, -13.1),
                new Point(-59.4, 0), new Point(-15.3, 0));*/

            // TODO: для южного полушария неправильно вычисляется азимут
            // TODO: Переход из одного полушария в другое
        }

        /// <summary>
        /// Выполняет расчёт под разными углами
        /// </summary>
        /// <param name="pointSw">Юго-Западная точка</param>
        /// <param name="pointSe">Юго-Восточная точка</param>
        /// <param name="pointNw">Северо-Западная точка</param>
        /// <param name="pointNe">Северо-Восточная точка</param>
        /// <param name="pointW">Западная точка на экваторе</param>
        /// <param name="pointE">Восточная точка на экваторе</param>
        private void AtDifferentAngles(Point pointSw, Point pointSe, Point pointNw, Point pointNe, Point pointW, Point pointE)
        {
            // Юго-Западное направление 
            ProblemAssert(pointNe, pointSw);
            // Северо-Западное направление
            ProblemAssert(pointSe, pointNw);
            // Юго-Восточное направление
            ProblemAssert(pointNw, pointSe);
            // Северо-Восточное направление 
            ProblemAssert(pointSw, pointNe);
            // Строго на Юг
            ProblemAssert(pointNw, pointSw);
            // Строго на Север
            ProblemAssert(pointSe, pointNe);
            // Строго на Запад (Средние широты)
            ProblemAssert(pointNe, pointNw);
            // Строго на Восток (Средние широты)
            ProblemAssert(pointNw, pointNe);
            // Строго на Запад (Экватор)
            ProblemAssert(pointE, pointW);
            // Строго на Восток (Экватор)
            ProblemAssert(pointW, pointE);
        }

        private void ProblemAssert(Point point1, Point point2)
        {
            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(
                    new Point(directAnswerForward.Сoordinate.Longitude, directAnswerForward.Сoordinate.Latitude),
                    point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(
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