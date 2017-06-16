using GeodesicLibrary;
using GeodesicLibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    /// <summary>
    /// Тестирование соответствия решений подхода для расчётов на эллипсоиде и подхода для расчётов на сфероиде
    /// </summary>
    /// <remarks>
    /// Смысл в том, что расчёт на эллипсоиде - универсальный, а сфероид лишь частный случай,  
    ///   однако решение сложное, и в случае сфероида, его можно упростить.
    /// Всё же, на сфероиде, результаты должны совпадать, упращаем мы расчёт или нет.
    /// Поэтому, в тестах, для сфероида, принудительно вызывается расчёт 
    ///   как для эллипсоида, и расчёт как для сфероида, потом сравниваются результаты.
    /// </remarks>
    [TestClass]
    public class OrthodromicSpheroidVsEllipsoidTests
    {
        public InverseProblemService InverseProblemService { get; set; }

        public DirectProblemService DirectProblemService { get; set; }

        public OrthodromicSpheroidVsEllipsoidTests()
        {
            DirectProblemService = new DirectProblemService(6367444, 6367444);
            InverseProblemService = new InverseProblemService(6367444, 6367444);
        }

        /// <summary>
        /// На средних широтах
        /// </summary>
        [TestMethod]
        public void MiddleLatitudeTest()
        {
            var point1 = new Point(59, 36, 30, 13, 5, 46);
            var point2 = new Point(15, 25, 53, 28, 7, 38);

            PrivateObject distance = new PrivateObject(InverseProblemService);
            PrivateObject coordinates = new PrivateObject(DirectProblemService);

            // Решение обратной задачи
            var byEllipsoidInverse =
                (InverseProblemAnswer) distance.Invoke("OrthodromicEllipsoidDistance", point1, point2);
            var bySpheroidInverse =
                (InverseProblemAnswer) distance.Invoke("OrthodromicSpheroidDistance", point1, point2);
            Assert.AreEqual(byEllipsoidInverse.Distance, bySpheroidInverse.Distance, 0.0006); // 0.06 мм
            Assert.AreEqual(byEllipsoidInverse.ForwardAzimuth, bySpheroidInverse.ForwardAzimuth, 0.000000001);
            Assert.AreEqual(byEllipsoidInverse.ReverseAzimuth, bySpheroidInverse.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи
            var byEllipsoidDirect =
                (DirectProblemAnswer)
                coordinates.Invoke("DirectProblemEllipsoid", point1, byEllipsoidInverse.ForwardAzimuth,
                    byEllipsoidInverse.Distance);
            var bySpheroidDirect =
                (DirectProblemAnswer)
                coordinates.Invoke("DirectProblemSpheroid", point1, bySpheroidInverse.ForwardAzimuth,
                    bySpheroidInverse.Distance);
            Assert.AreEqual(byEllipsoidDirect.ReverseAzimuth, bySpheroidDirect.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(byEllipsoidDirect.Сoordinate.Latitude, bySpheroidDirect.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(byEllipsoidDirect.Сoordinate.Longitude, bySpheroidDirect.Сoordinate.Longitude, 0.000000001);
        }

        /// <summary>
        /// На экваторе
        /// </summary>
        [TestMethod]
        public void EquatorLatitudeTest()
        {
            var point1 = new Point(59, 36, 30, 0, 0, 0);
            var point2 = new Point(15, 25, 53, 0, 0, 0);

            PrivateObject distance = new PrivateObject(InverseProblemService);
            PrivateObject coordinates = new PrivateObject(DirectProblemService);

            // Решение обратной задачи
            var byEllipsoidInverse =
                (InverseProblemAnswer) distance.Invoke("OrthodromicEllipsoidDistance", point1, point2);
            var bySpheroidInverse =
                (InverseProblemAnswer) distance.Invoke("OrthodromicSpheroidDistance", point1, point2);
            Assert.AreEqual(byEllipsoidInverse.Distance, bySpheroidInverse.Distance, 0.0006); // 0.06 мм
            Assert.AreEqual(byEllipsoidInverse.ForwardAzimuth, bySpheroidInverse.ForwardAzimuth, 0.000000001);
            Assert.AreEqual(byEllipsoidInverse.ReverseAzimuth, bySpheroidInverse.ReverseAzimuth, 0.000000001);

            // Решение прямой задачи
            var byEllipsoidDirect =
                (DirectProblemAnswer)
                coordinates.Invoke("DirectProblemEllipsoid", point1, byEllipsoidInverse.ForwardAzimuth,
                    byEllipsoidInverse.Distance);
            var bySpheroidDirect =
                (DirectProblemAnswer)
                coordinates.Invoke("DirectProblemSpheroid", point1, bySpheroidInverse.ForwardAzimuth,
                    bySpheroidInverse.Distance);
            Assert.AreEqual(byEllipsoidDirect.ReverseAzimuth, bySpheroidDirect.ReverseAzimuth, 0.000000001);
            Assert.AreEqual(byEllipsoidDirect.Сoordinate.Latitude, bySpheroidDirect.Сoordinate.Latitude, 0.000000001);
            Assert.AreEqual(byEllipsoidDirect.Сoordinate.Longitude, bySpheroidDirect.Сoordinate.Longitude, 0.000000001);
        }
    }
}