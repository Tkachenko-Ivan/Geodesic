using GeodesicLibrary;
using GeodesicLibrary.Model;
using GeodesicLibrary.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests
{
    /// <summary>
    /// Тестируется правильность вычисления точки пересечения двух ортодром
    /// </summary>
    /// <remarks>
    /// Методика тестирования следующая:
    ///     - находим координаты точки пересечения двух ортодром заданных начальной и конечной координатой
    ///     - решаем обратную геодезическую задачу для обоих ортодром - определяем прямой азимут
    ///     - решаем обратную геодезическую задачу от начальной точки до найденной точки пересечения (для обоих ортодром)
    ///     - сравниваем азимуты, если точка пересечения найдена верно, азимуты не должны поменяться
    /// </remarks>
    public abstract class IntersectTests
    {
        public abstract IntersectService IntersectService { get; set; }

        public abstract InverseProblemService InverseProblemService { get; set; }

        [TestMethod]
        public void PointIntersectTest()
        {
            var lon11 = Converter.DergeeToDecimalDegree(22, 36, 30);
            var lat11 = Converter.DergeeToDecimalDegree(13, 5, 46);
            var lon12 = Converter.DergeeToDecimalDegree(27, 25, 53);
            var lat12 = Converter.DergeeToDecimalDegree(15, 7, 38);
            var lon21 = Converter.DergeeToDecimalDegree(20, 36, 30);
            var lat21 = Converter.DergeeToDecimalDegree(17, 5, 46);
            var lon22 = Converter.DergeeToDecimalDegree(26, 25, 53);
            var lat22 = Converter.DergeeToDecimalDegree(13, 7, 38);

            var intersectCoord = IntersectService.IntersectOrthodromic(lon11, lat11, lon12, lat12, lon21, lat21, lon22, lat22);

            var firstOrtodrom = InverseProblemService.OrthodromicDistance(lon11, lat11, lon12, lat12);
            var secondOrtodrom = InverseProblemService.OrthodromicDistance(lon21, lat21, lon22, lat22);

            var firstOrtodrom2 = InverseProblemService.OrthodromicDistance(lon11, lat11, intersectCoord.Longitude, intersectCoord.Latitude);
            var secondOrtodrom2 = InverseProblemService.OrthodromicDistance(lon21, lat21, intersectCoord.Longitude, intersectCoord.Latitude);

            // Точнее пока не получается (на эллипсоиде, на сфероиде всё ништяк)
            Assert.AreEqual(firstOrtodrom.ForwardAzimuth, firstOrtodrom2.ForwardAzimuth, 0.005);
            Assert.AreEqual(secondOrtodrom.ForwardAzimuth, secondOrtodrom2.ForwardAzimuth, 0.005);
        }
    }
}