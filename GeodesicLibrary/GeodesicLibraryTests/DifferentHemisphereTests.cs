using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    /// <summary>
    /// Тестируются случаи когда точки находятся в разных полушариях,
    ///     например одна в западном, другая в восточном, северном/южном.
    /// Проверяется правильность определения путевых углов и рассчёта расстояний.
    /// </summary>
    [TestClass]
    public class DifferentHemisphereTests
    {
        public InverseProblemService InverseProblemService { get; set; }

        public DirectProblemService DirectProblemService { get; set; }

        public DifferentHemisphereTests()
        {
            DirectProblemService = new DirectProblemService(new Spheroid());
            InverseProblemService = new InverseProblemService(new Spheroid());
        }

        /// <summary>
        /// Тестирование пересечения экватора с севера на юг
        /// </summary>
        /// <remarks>
        /// Методика следующая:
        ///     - Берутся три точки лежащие на одной долготе 
        ///         (потому что в этом случае заранее известно на какой долготе линия пересечёт экватор):
        ///         две из них по разные строны от экватора (point1, point2),
        ///         и одна на экваторе (point0)
        ///     - Решается обратная геодезическая задача: 
        ///         * от точки в северном полушарии до точки на экваторе
        ///         * от точки на экваторе до точки в южном полушарии
        ///         * от точки в северном полушарии до точки в южном полушарии
        ///     - Проводятся сравнения:
        ///         * Прямой и обратный путевой угол должены быть вычислены одинакого во всех трёх решениях
        ///         * Дистанция от точки в северном полушарии, до точки в южном, должна быть равна сумме дистанций от этих точек до экватора
        /// </remarks>
        [TestMethod]
        public void NorthernSouthernHemisphereTest()
        {
            var point0 = new Point(20, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);
            var point1 = new Point(20, 0, 0, CardinalLongitude.E, 15, 0, 0, CardinalLatitude.N);
            var point2 = new Point(20, 0, 0, CardinalLongitude.E, 15, 0, 0, CardinalLatitude.S);

            var answerFirst = InverseProblemService.OrthodromicDistance(point1, point0);
            var answerSecond = InverseProblemService.OrthodromicDistance(point0, point2);
            var answerAll = InverseProblemService.OrthodromicDistance(point1, point2);

            Assert.AreEqual(answerFirst.ForwardAzimuth, answerAll.ForwardAzimuth, 0.00000001);
            Assert.AreEqual(answerSecond.ReverseAzimuth, answerAll.ReverseAzimuth, 0.00000001);
            Assert.AreEqual(answerSecond.ForwardAzimuth, answerFirst.ForwardAzimuth, 0.00000001);
            Assert.AreEqual(answerSecond.ReverseAzimuth, answerFirst.ReverseAzimuth, 0.00000001);
            Assert.AreEqual(answerSecond.Distance, answerFirst.Distance, 0.00000001);
            Assert.AreEqual(answerAll.Distance, answerSecond.Distance + answerFirst.Distance, 0.00000001);
        }

        /// <summary>
        /// Пересечение нулевого меридиана с востока на запад через нулевой меридиан
        /// </summary>
        [TestMethod]
        public void EasternWesternHemisphereTest()
        {
            var point0 = new Point(0, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);
            var point1 = new Point(15, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);
            var point2 = new Point(15, 0, 0, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);

            var answerFirst = InverseProblemService.OrthodromicDistance(point1, point0);
            var answerSecond = InverseProblemService.OrthodromicDistance(point0, point2);
            var answerAll = InverseProblemService.OrthodromicDistance(point1, point2);

            Assert.AreEqual(answerFirst.ForwardAzimuth, answerAll.ForwardAzimuth, 0.000000001);
            Assert.AreEqual(answerSecond.ReverseAzimuth, answerAll.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(answerSecond.ForwardAzimuth, answerFirst.ForwardAzimuth, 0.000000001);
            Assert.AreEqual(answerSecond.ReverseAzimuth, answerFirst.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(answerSecond.Distance, answerFirst.Distance, 0.000000001);
            Assert.AreEqual(answerAll.Distance, answerSecond.Distance + answerFirst.Distance, 0.000000001);
        }

        /// <summary>
        /// Пересечение нулевого меридиана с востока на запад через 180ый меридиан
        /// </summary>
        /// <remarks>
        /// В тесте устанавливается разница в 30 градусов, поэтому дистанция должна быть равна
        ///     любой другой разнице в 30 градусов на этой широте.
        /// Азимуты у решения задачи в восточном полушарии и азимуты у задачи в западном полушарии, вероятно, должны измениться
        /// </remarks>
        [TestMethod]
        public void EasternWesternHemisphere180Test()
        {
            var point0 = new Point(180, 0, 0, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);
            var point1 = new Point(165, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N);
            var point2 = new Point(165, 0, 0, CardinalLongitude.W, 0, 0, 0, CardinalLatitude.N);

            var answerFirst = InverseProblemService.OrthodromicDistance(point1, point0);
            var answerSecond = InverseProblemService.OrthodromicDistance(point0, point2);
            var answerAll = InverseProblemService.OrthodromicDistance(point1, point2);
            var answerAny =
                InverseProblemService.OrthodromicDistance(
                    new Point(135, 0, 0, CardinalLongitude.E, 0, 0, 0, CardinalLatitude.N), point1);

            Assert.AreEqual(answerAll.Distance, answerAny.Distance, 0.00000001);
            Assert.AreEqual(answerSecond.Distance, answerFirst.Distance, 0.00000001);
            Assert.AreEqual(answerAll.Distance, answerSecond.Distance + answerFirst.Distance, 0.00000001);

            Assert.AreEqual(answerFirst.ForwardAzimuth, answerSecond.ReverseAzimuth, 0.00000001);
            Assert.AreEqual(answerFirst.ReverseAzimuth, answerSecond.ForwardAzimuth, 0.00000001);
        }

        /// <summary>
        /// Переход с восточного полушария в западное через полюс
        /// </summary>
        /// <remarks>
        /// При переходе через полюс прямой обратный азимуты (у общей задачи) должны совпадать.
        /// У задачи ДО полюса обратный азимут долженн быть равен 180 (т.к. долгота не меняется).
        /// У задачи ОТ полюса обратный азимут долженн быть равен 360 (т.к. долгота не меняется).
        /// </remarks>
        [TestMethod]
        public void EasternWesternHemispherePolarTest()
        {
            var point0 = new Point(90, 0, 0, CardinalLongitude.E, 90, 0, 0, CardinalLatitude.N);
            var point1 = new Point(90, 0, 0, CardinalLongitude.E, 80, 0, 0, CardinalLatitude.N);
            var point2 = new Point(90, 0, 0, CardinalLongitude.W, 80, 0, 0, CardinalLatitude.N);

            var answerFirst = InverseProblemService.OrthodromicDistance(point1, point0);
            var answerSecond = InverseProblemService.OrthodromicDistance(point0, point2);
            var answerAll = InverseProblemService.OrthodromicDistance(point1, point2);

            Assert.AreEqual(answerSecond.Distance, answerFirst.Distance, 0.00000001);
            Assert.AreEqual(answerAll.Distance, answerSecond.Distance + answerFirst.Distance, 0.00000001);
            Assert.AreEqual(answerAll.ForwardAzimuth, answerAll.ReverseAzimuth, 0.00000001);
            Assert.AreEqual(answerFirst.ReverseAzimuth, 180, 0.00000001);
            Assert.AreEqual(answerSecond.ReverseAzimuth, 360, 0.00000001);
            Assert.AreEqual(answerAll.ForwardAzimuth, answerFirst.ForwardAzimuth, 0.00000001);
            Assert.AreEqual(answerAll.ReverseAzimuth, answerSecond.ReverseAzimuth, 0.00000001);
        }
    }
}