﻿using GeodesicLibrary;
using GeodesicLibrary.Model;
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
            var point1 = new Point(22, 36, 30, 13, 5, 46);
            var point2 = new Point(27, 25, 53, 15, 7, 38);
            var point3 = new Point(20, 36, 30, 17, 5, 46);
            var point4 = new Point(26, 25, 53, 13, 7, 38);

            var intersectCoord = IntersectService.IntersectOrthodromic(point1,
                point2, point3, point4);

            var firstOrtodrom = InverseProblemService.OrthodromicDistance(point1, point2);
            var secondOrtodrom = InverseProblemService.OrthodromicDistance(point3, point4);

            var firstOrtodrom2 = InverseProblemService.OrthodromicDistance(point1, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));
            var secondOrtodrom2 = InverseProblemService.OrthodromicDistance(point3, new Point(intersectCoord.Longitude,
                intersectCoord.Latitude));

            // Точнее пока не получается (на эллипсоиде, на сфероиде всё ништяк)
            Assert.AreEqual(firstOrtodrom.ForwardAzimuth, firstOrtodrom2.ForwardAzimuth, 0.005);
            Assert.AreEqual(secondOrtodrom.ForwardAzimuth, secondOrtodrom2.ForwardAzimuth, 0.005);
        }
    }
}