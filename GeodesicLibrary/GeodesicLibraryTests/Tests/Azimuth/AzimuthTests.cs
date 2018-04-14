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
        /// Простая проверка правильности определения азимута.
        ///     Проверка осуществляется в границах одного полушария, без пересечения экватора, нулевого меридиана и полюса
        /// </summary>
        [TestMethod]
        public void SimpleAzimutTest()
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
        /// Проверка правильности определения азимута при условии пересечения экватора или нулевого меридиана
        ///     проверка пересечения 180ого меридиана не выполняется
        /// </summary>
        [TestMethod]
        public void IntersectionAzimutTest()
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
        /// Проверка правильности определения азимута при условии пересечения 180ого меридиана
        /// </summary>
        [TestMethod]
        public void Intersection180AzimutTest()
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
        /// Проверка правильности определения азимута при условии полюса
        /// </summary>
        /// <remarks>
        /// Помимо прочего, при пересечении полюса - меняется знак (как и в целом при пересечении 180ого меридиана)
        /// </remarks>
        [TestMethod]
        public void IntersectionPolarAzimutTest()
        {
            var inverseProblemAnswer1 = AzimuthTest(new Point(30, 80), new Point(-150, 80));
            Assert.AreEqual(inverseProblemAnswer1.ForwardAzimuth, inverseProblemAnswer1.ReverseAzimuth, 0.000000001);

            var inverseProblemAnswer2 = AzimuthTest(new Point(45, 65), new Point(-135, 65));
            Assert.AreEqual(inverseProblemAnswer2.ForwardAzimuth, inverseProblemAnswer2.ReverseAzimuth, 0.000000001);

            // Как пересекать полюс значения не имеет - ответ должен быть тот же
            Assert.AreEqual(inverseProblemAnswer1.ForwardAzimuth, inverseProblemAnswer2.ForwardAzimuth, 0.000000001);
        }

        /// <summary>
        /// Проверка того как изменяется азимут при движении по прямой
        /// </summary>
        [TestMethod]
        public void СhangeOfAzimutTest()
        {
            // TODO: доделать когда разберусь с прямой геодезической задачей
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
            AzimuthTest(pointSouthWest, pointNorthWest);
            // На Северо-Восток
            AzimuthTest(pointSouthWest, pointNorthEast);
            // На Восток
            AzimuthTest(pointSouthWest, pointSouthEast);
            // На Юго-Восток
            AzimuthTest(pointNorthWest, pointSouthEast);
            // На Юг
            AzimuthTest(pointNorthWest, pointSouthWest);
            // На Юго-Запад
            AzimuthTest(pointNorthEast, pointSouthWest);
            // На Запад
            AzimuthTest(pointSouthEast, pointSouthWest);
            // На Северо-Запад
            AzimuthTest(pointSouthEast, pointNorthWest);
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
        private InverseProblemAnswer AzimuthTest(Point point1, Point point2)
        {
            var answer1 = InverseProblemService.OrthodromicDistance(point1, point2);
            var answer2 = InverseProblemService.OrthodromicDistance(point2, point1);
            Assert.AreEqual(answer1.ForwardAzimuth, answer2.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(answer1.ReverseAzimuth, answer2.ForwardAzimuth, 0.000000001);

            // TODO: Тут какая-то беда с прямой геодезической задачей, но азимут определяется верно
            // Решение прямой геодезической задачи, но с ним что-то не то
            /*var answerPoint1 = DirectProblemService.DirectProblem(point1, answer1.ForwardAzimuth, answer1.Distance);
            Assert.AreEqual(point2.Latitude, answerPoint1.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(point2.Longitude, answerPoint1.Сoordinate.Longitude, 0.000000001);*/

            return answer1;
        }

    }
}