using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests
{
    [TestClass]
    public class IntermediatePointTests : Plan
    {
        [TestMethod]
        public void SimpleIntermediatePointTest()
        {
            //SimpleTest();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IntersectionIntermediatePointTest()
        {
            //IntersectionTest();
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Intersection180IntermediatePointTest()
        {
            //Intersection180Test();
            Assert.Inconclusive();
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
            //IntersectionPolarTest();
            Assert.Inconclusive();
        }

        public override void Tests(Point point1, Point point2, IEllipsoid ellipsoid)
        {
           /* var intermediatePointService = new IntermediatePointService(ellipsoid);
            var inverseProblemService = new InverseProblemService(ellipsoid);

            // Проверка на корректность
            Assert.IsTrue(lat >= lat1 && lat <= lat2 || lat <= lat1 && lat >= lat2);

            var coord1 = new Point(lon1, lat1);
            var coord2 = new Point(lon2, lat2);
            var iLon = intermediatePointService.GetLongitude(lat, coord1, coord2);
            var iLat = intermediatePointService.GetLatitude(iLon, coord1, coord2);

            // Сравнить изначальную широту, и получившуюся в результате рассчётов
            Assert.AreEqual(lat, iLat, 0.000000001);

            // Рассчитанная долгота должна быть между двух координат,
            //  кроме случая когда пересекает 180ый мередиан, 
            //  но ситуация с пересечением 180ого мередиана проверяется в отдельном тесте 
            Assert.IsTrue(iLon >= lon1 && iLon <= lon2 || iLon <= lon1 && iLon >= lon2);

            // Точка должна лежать точно на линии,
            //  значит при решении обратной геодезической задачи азимут не должен измениться
            var answer1 = inverseProblemService.OrthodromicDistance(coord1, coord2);
            var answer2 = inverseProblemService.OrthodromicDistance(coord1, new Point(iLon, iLat));
            Assert.AreEqual(answer1.ForwardAzimuth, answer2.ForwardAzimuth, 0.000000001, answer1.ToString());*/
        }
    }
}