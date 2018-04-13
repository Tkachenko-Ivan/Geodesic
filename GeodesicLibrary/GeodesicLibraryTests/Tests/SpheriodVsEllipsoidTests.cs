using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    /// <summary>
    /// Тестирование соответствия решений на эллипсоиде и на сфероиде
    /// </summary>
    /// <remarks>
    /// Смысл в том, что расчёт на эллипсоиде - универсальный, а сфероид лишь частный случай,  
    ///   однако решение на эллипсоиде сложное, и в случае сфероида, его можно упростить.
    /// Всё же, на сфероиде, результаты должны совпадать, упращаем мы расчёт или нет.
    /// Поэтому, в тестах, для сфероида, принудительно вызывается расчёт 
    ///   как для эллипсоида, и расчёт как для сфероида, потом сравниваются результаты.
    /// </remarks>
    [TestClass]
    public class SpheriodVsEllipsoidTests
    {
        /// <summary>
        /// Сравнение прямой и обратной геодезической задач
        /// </summary>
        [TestMethod]
        public void CompareDirectAndInverseProblemTest()
        {
            // На средних широтах
            EllipsoidSpheroidCompare(new Point(59.3, 13.1), new Point(15.1, 28.1));
            // На экваторе
            EllipsoidSpheroidCompare(new Point(59.3, 0), new Point(15.1, 0));
        }

        /// <summary>
        /// Сравнивает результаты полученные с помощью работы алгоритма для сферы, 
        /// с результатами полученными с помощью алгоритма эллипсоида
        /// </summary>
        private void EllipsoidSpheroidCompare(Point point1, Point point2)
        {
            var directProblemService = new DirectProblemService(new Spheroid());
            var inverseProblemService = new InverseProblemService(new Spheroid());

            PrivateObject distance = new PrivateObject(inverseProblemService);
            PrivateObject coordinates = new PrivateObject(directProblemService);

            // Решение обратной задачи
            var byEllipsoidInverse =
                (InverseProblemAnswer)distance.Invoke("OrthodromicEllipsoidDistance", point1, point2);
            var bySpheroidInverse =
                (InverseProblemAnswer)distance.Invoke("OrthodromicSpheroidDistance", point1, point2);
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