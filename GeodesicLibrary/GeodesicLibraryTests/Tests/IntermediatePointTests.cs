using System;
using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class IntermediatePointTests : Plan
    {
        [TestMethod]
        public void SimpleIntermediatePointTest()
        {
            SimpleTest();
        }

        [TestMethod]
        public void IntersectionIntermediatePointTest()
        {
            IntersectionTest();
        }

        [TestMethod]
        public void Intersection180IntermediatePointTest()
        {
            Intersection180Test();
        }

        [TestMethod]
        public void LongLineIntermediatePointTest()
        {
            //LongLineTest();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IntersectionPolarIntermediatePointTest()
        {
            var intermediatePointService = new IntermediatePointService(new Spheroid());

            // Северный полюс
            var point1 = new Point(50, 70);
            var point2 = new Point(-130, 50);
            var lat = (point1.Latitude + point2.Latitude) / 2;
            try
            {
                intermediatePointService.GetLongitude(lat, point1, point2);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "При переходе через полюс, невозможно однозначно определить долготу по широте");
            }
            try
            {
                intermediatePointService.GetLatitude(50, point1, point2);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "При переходе через полюс, невозможно однозначно определить широту по долготе");
            }

            // Южный полюс
            point1 = new Point(50, -70);
            point2 = new Point(-130, -50);
            lat = (point1.Latitude + point2.Latitude) / 2;
            try
            {
                intermediatePointService.GetLongitude(lat, point1, point2);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "При переходе через полюс, невозможно однозначно определить долготу по широте");
            }
            try
            {
                intermediatePointService.GetLatitude(50, point1, point2);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "При переходе через полюс, невозможно однозначно определить широту по долготе");
            }
        }

        /// <summary>
        /// Тестирование ситуаций когда, значения широты или долготы у двух точек совпадает
        /// </summary>
        [TestMethod]
        public void RightAngleIntermediatePointTest()
        {
            Assert.Inconclusive();
        }

        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
            var intermediatePointService = new IntermediatePointService(ellipsoid);
            var inverseProblemService = new InverseProblemService(ellipsoid);

            if (Math.Abs(point1.Latitude - point2.Latitude) < 0.000000001 ||
                Math.Abs(point1.Longitude - point2.Longitude) < 0.000000001)
                return;

            // По серёдке возьмём
            var lat = (point1.Latitude + point2.Latitude) / 2;

            var iLon = intermediatePointService.GetLongitude(lat, point1, point2);
            var iLat = intermediatePointService.GetLatitude(iLon, point1, point2);

            // Сравнить изначальную широту, и получившуюся в результате рассчётов
            Assert.AreEqual(lat, iLat, 0.000000001);

            // Рассчитанная долгота должна быть между двух координат,
            //  кроме случая когда пересекает 180ый меридиан
            if (Math.Abs(point1.Longitude) + Math.Abs(point2.Longitude) < 180 && point2.Longitude * point1.Longitude < 0)
                Assert.IsTrue(iLon >= point1.Longitude && iLon <= point2.Longitude ||
                              iLon <= point1.Longitude && iLon >= point2.Longitude);


            // Точка должна лежать точно на линии,
            //  значит при решении обратной геодезической задачи азимут не должен измениться
            var answer1 = inverseProblemService.OrthodromicDistance(point1, point2);
            var answer2 = inverseProblemService.OrthodromicDistance(point1, new Point(iLon, iLat));
            Assert.AreEqual(answer1.ForwardAzimuth, answer2.ForwardAzimuth, 0.0000001, answer1.ToString());
        }
    }
}