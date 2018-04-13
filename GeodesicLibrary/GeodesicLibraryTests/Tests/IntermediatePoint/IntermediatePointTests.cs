using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.IntermediatePoint
{
    /// <summary>
    /// Тестируется определение долготы как функции широты или широты как функции долготы
    /// </summary>
    public abstract class IntermediatePointTests
    {
        public abstract IntermediatePointService IntermediatePointService { get; set; }

        public abstract InverseProblemService InverseProblemService { get; set; }

        [TestMethod]
        public void GetLonByLat()
        {
            // Первая четверть: I
            AtDifferentAngles(22, 10, 30, 50, 10, 30, 50);

            // Вторая четверть: II
            AtDifferentAngles(22, -50, -30, -10, 10, 30, 50);

            // Третья четверть: III
            AtDifferentAngles(-38, -50, -30, -10, -50, -30, -10);

            // Четвёртая четверть: IV
            AtDifferentAngles(-38, 10, 30, 50, -50, -30, -10);

            // Пересечение I и IV
            AtDifferentAngles(2, 10, 30, 50, -10, 10, 30);
            AtDifferentAngles(-2, 10, 30, 50, -10, 10, 30);

            // Пересечение II и III
            AtDifferentAngles(2, -50, -30, -10, -10, 10, 30);
            AtDifferentAngles(-2, -50, -30, -10, -10, 10, 30);

            // Пересечение I и II
            AtDifferentAngles(22, -10, 10, 30, 10, 30, 50);

            // Пересечение III и IV
            AtDifferentAngles(-38, -10, 10, 30, -50, -30, -10);

            // Пересечение II и IV или I и III
            AtDifferentAngles(2, -10, 10, 30, -10, 10, 30);
            AtDifferentAngles(-2, -50, -30, -10, -10, 10, 30);

            // TODO: добавить тест когда линия занимает более 90 градусов по долготе
        }

        // TODO: добавить тест для ситуации когда происходит пересечеие 180ого меридиана

        /// <summary>
        /// Тестирование при условии что линия пересекает начало координат: точку (0; 0)
        /// </summary>
        [TestMethod]
        public void IntersectOriginCoordinates()
        {
            Intersect(3, -10, -10, 10, 10);
            Intersect(3, 10, 10, -10, -10);
            Intersect(3, -10, 10, 10, -10);
            Intersect(3, 10, -10, -10, 10);

            Intersect(-3, -10, -10, 10, 10);
            Intersect(-3, 10, 10, -10, -10);
            Intersect(-3, -10, 10, 10, -10);
            Intersect(-3, 10, -10, -10, 10);
        }

        /// <summary>
        /// Комбинирует три значеия разными способами, 
        ///     присваивая их долготе или широте точек отрезков,
        ///     для того чтобы протестировать работу под разными углами наклона
        /// </summary>
        private void AtDifferentAngles(double lat, double x1, double x2, double x3, double y1, double y2, double y3)
        {
            // Линия возрастает по X и по Y с разной интенсивностью
            Intersect(lat, x1, y1, x2, y3);
            Intersect(lat, x1, y1, x2, y2);
            Intersect(lat, x1, y1, x3, y2);
            // Линия возрастает по X но убывает по Y с разной интенсивностью
            Intersect(lat, x1, y2, x3, y1);
            Intersect(lat, x1, y2, x2, y1);
            Intersect(lat, x1, y3, x2, y1);
            // Линия убывает по X и по Y с разной интенсивностью
            Intersect(lat, x2, y3, x1, y1);
            Intersect(lat, x2, y2, x1, y1);
            Intersect(lat, x3, y2, x1, y1);
            // Линия убывает по X но возрастает по Y с разной интенсивностью
            Intersect(lat, x3, y1, x1, y2);
            Intersect(lat, x2, y1, x1, y2);
            Intersect(lat, x2, y1, x1, y3);
        }

        /// <summary>
        /// Выполняет рассчёт долготы по двум координатам и заданной широте.
        /// Для проверки выполняет и обратный рассчёт:
        ///     вычислет широту по тем же координатам и полученной долготе
        /// </summary>
        /// <param name="lat">Изначальное значение широты</param>
        /// <param name="lon1">Долгота первой точки</param>
        /// <param name="lat1">Широта перовй точки</param>
        /// <param name="lon2">Долгота второй точки</param>
        /// <param name="lat2">Широта второй точки</param>
        private void Intersect(double lat, double lon1, double lat1, double lon2, double lat2)
        {
            // Проверка на корректность
            Assert.IsTrue(lat >= lat1 && lat <= lat2 || lat <= lat1 && lat >= lat2);

            var coord1 = new Point(lon1, lat1);
            var coord2 = new Point(lon2, lat2);
            var iLon = IntermediatePointService.GetLongitude(lat, coord1, coord2);
            var iLat = IntermediatePointService.GetLatitude(iLon, coord1, coord2);

            // Сравнить изначальную широту, и получившуюся в результате рассчётов
            Assert.AreEqual(lat, iLat, 0.000000001);

            // Рассчитанная долгота должна быть между двух координат,
            //  кроме случая когда пересекает 180ый мередиан, 
            //  но ситуация с пересечением 180ого мередиана проверяется в отдельном тесте 
            Assert.IsTrue(iLon >= lon1 && iLon <= lon2 || iLon <= lon1 && iLon >= lon2);

            // Точка должна лежать точно на линии,
            //  значит при решении обратной геодезической задачи азимут не должен измениться
            var answer1 = InverseProblemService.OrthodromicDistance(coord1, coord2);
            var answer2 = InverseProblemService.OrthodromicDistance(coord1, new Point(iLon, iLat));
            Assert.AreEqual(answer1.ForwardAzimuth, answer2.ForwardAzimuth, 0.000000001, answer1.ToString());
        }
    }
}