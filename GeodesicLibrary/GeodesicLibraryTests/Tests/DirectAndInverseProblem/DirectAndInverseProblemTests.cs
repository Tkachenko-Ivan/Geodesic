using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    /// <summary>
    /// Тестируется решение прямой и обратной геодезических задач. 
    /// Для тестирования правильности отпределения азимута тестируются решения для всех сторон света
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
    public abstract class DirectAndInverseProblemTests
    {
        public abstract InverseProblemService InverseProblemService { get; set; }

        public abstract DirectProblemService DirectProblemService { get; set; }

        /// <summary>
        /// Юго-Западное направление 
        /// </summary>
        [TestMethod]
        public void SouthWestDirectionTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
            var point2 = new Point(59, 36, 30, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Северо-Западное направление 
        /// </summary>
        [TestMethod]
        public void NorthWestDirectionTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);
            var point2 = new Point(59, 36, 30, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Юго-Восточное направление 
        /// </summary>
        [TestMethod]
        public void SouthEastDirectionTest()
        {
            var point1 = new Point(59, 36, 30, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Северо-Восточное направление 
        /// </summary>
        [TestMethod]
        public void NorthEastDirectionTest()
        {
            var point1 = new Point(59, 36, 30, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Юг
        /// </summary>
        [TestMethod]
        public void SouthDirectionTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Север
        /// </summary>
        [TestMethod]
        public void NorthDirectionTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Запад (Средние широты)
        /// </summary>
        [TestMethod]
        public void WestDirectionMiddleTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
            var point2 = new Point(59, 36, 30, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1, point2);

            // Решение прямой задачи 1
            var directAnswerForward = DirectProblemService.DirectProblem(point1,
                inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerForward.Сoordinate.Longitude,
                    directAnswerForward.Сoordinate.Latitude), point2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Сoordinate.Longitude, point2.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerForward.Сoordinate.Latitude, point2.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = DirectProblemService.DirectProblem(point2,
                inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 =
                InverseProblemService.OrthodromicDistance(new Point(directAnswerReverse.Сoordinate.Longitude,
                        directAnswerReverse.Сoordinate.Latitude), point1)
                    .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Сoordinate.Longitude, point1.Longitude, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Сoordinate.Latitude, point1.Latitude, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Восток (Средние широты)
        /// </summary>
        [TestMethod]
        public void EastDirectionMiddleTest()
        {
            var point1 = new Point(59, 36, 30, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1,
                point2);

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

        /// <summary>
        /// Строго на Запад (Экватор)
        /// </summary>
        [TestMethod]
        public void WestDirectionEquatorTest()
        {
            var point1 = new Point(15, 25, 53, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);
            var point2 = new Point(59, 36, 30, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1,
                point2);

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

        /// <summary>
        /// Строго на Восток (Экватор)
        /// </summary>
        [TestMethod]
        public void EastDirectionEquatorTest()
        {
            var point1 = new Point(59, 36, 30, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);
            var point2 = new Point(15, 25, 53, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);

            // Решение обратной задачи
            var inverseAnswer = InverseProblemService.OrthodromicDistance(point1,
                point2);

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