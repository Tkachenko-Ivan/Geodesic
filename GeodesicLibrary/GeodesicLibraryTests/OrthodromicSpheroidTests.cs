using GeodesicLibrary;
using GeodesicLibrary.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    /// <summary>
    /// Тестируется решение прямой и обратной геодезических задач на сфероиде. 
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
    [TestClass]
    public class OrthodromicSpheroidTests
    {
        /// <summary>
        /// Юго-Западное направление 
        /// </summary>
        [TestMethod]
        public void SouthWestDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444, 6367444);
            var distanceService = new DistanceService(6367444, 6367444);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(28, 7, 38);
            var lon2 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat2 = Converter.DergeeToDecimalDegree(13, 5, 46);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicSpheroidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicSpheroidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicSpheroidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Северо-Западное направление 
        /// </summary>
        [TestMethod]
        public void NorthWestDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(13, 5, 46);
            var lon2 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat2 = Converter.DergeeToDecimalDegree(28, 7, 38);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicSpheroidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicSpheroidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicSpheroidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Юго-Восточное направление 
        /// </summary>
        [TestMethod]
        public void SouthEastDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat1 = Converter.DergeeToDecimalDegree(28, 7, 38);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(13, 5, 46);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicSpheroidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicSpheroidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicSpheroidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Северо-Восточное направление 
        /// </summary>
        [TestMethod]
        public void NorthEastDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat1 = Converter.DergeeToDecimalDegree(13, 5, 46);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(28, 7, 38);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicSpheroidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicSpheroidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicSpheroidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Юг
        /// </summary>
        [TestMethod]
        public void SouthDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(28, 7, 38);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(13, 5, 46);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Север
        /// </summary>
        [TestMethod]
        public void NorthDirectionTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(13, 5, 46);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(28, 7, 38);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Запад (Средние широты)
        /// </summary>
        [TestMethod]
        public void WestDirectionMiddleTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(28, 7, 38);
            var lon2 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat2 = Converter.DergeeToDecimalDegree(28, 7, 38);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Восток (Средние широты)
        /// </summary>
        [TestMethod]
        public void EastDirectionMiddleTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat1 = Converter.DergeeToDecimalDegree(28, 7, 38);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(28, 7, 38);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Запад (Экватор)
        /// </summary>
        [TestMethod]
        public void WestDirectionEquatorTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat1 = Converter.DergeeToDecimalDegree(0, 0, 0);
            var lon2 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat2 = Converter.DergeeToDecimalDegree(0, 0, 0);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }

        /// <summary>
        /// Строго на Восток (Экватор)
        /// </summary>
        [TestMethod]
        public void EastDirectionEquatorTest()
        {
            var coordinatesService = new CoordinatesService(6367444.6571, 6367444.6571);
            var distanceService = new DistanceService(6367444.6571, 6367444.6571);

            var lon1 = Converter.DergeeToDecimalDegree(59, 36, 30);
            var lat1 = Converter.DergeeToDecimalDegree(0, 0, 0);
            var lon2 = Converter.DergeeToDecimalDegree(15, 25, 53);
            var lat2 = Converter.DergeeToDecimalDegree(0, 0, 0);

            // Решение обратной задачи
            var inverseAnswer = distanceService.OrthodromicEllipsoidDistance(lon1, lat1, lon2, lat2);

            // Решение прямой задачи 1
            var directAnswerForward = coordinatesService.DirectProblemSpheroid(lon1, lat1, inverseAnswer.ForwardAzimuth, inverseAnswer.Distance);
            var distance1 = distanceService.OrthodromicEllipsoidDistance(directAnswerForward.Longitude, directAnswerForward.Latitude, lon2, lat2).Distance;
            Assert.AreEqual(distance1, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerForward.Longitude, lon2, 0.000000001);
            Assert.AreEqual(directAnswerForward.Latitude, lat2, 0.000000001);
            Assert.AreEqual(inverseAnswer.ReverseAzimuth, directAnswerForward.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи 2
            var directAnswerReverse = coordinatesService.DirectProblemSpheroid(lon2, lat2, inverseAnswer.ReverseAzimuth, inverseAnswer.Distance);
            var distance2 = distanceService.OrthodromicEllipsoidDistance(directAnswerReverse.Longitude, directAnswerReverse.Latitude, lon1, lat1)
                .Distance;
            Assert.AreEqual(distance2, 0, 0.0006); // 0.06 мм
            Assert.AreEqual(directAnswerReverse.Longitude, lon1, 0.000000001);
            Assert.AreEqual(directAnswerReverse.Latitude, lat1, 0.000000001);
            Assert.AreEqual(inverseAnswer.ForwardAzimuth, directAnswerReverse.ReverseAzimuth, 0.000000001);
        }
    }
}