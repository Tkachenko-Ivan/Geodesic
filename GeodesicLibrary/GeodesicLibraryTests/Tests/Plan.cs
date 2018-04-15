using GeodesicLibrary.Infrastructure;
using GeodesicLibrary.Model;
using GeodesicLibraryTests.Model;

namespace GeodesicLibraryTests.Tests
{
    public abstract class Plan
    {
        /// <summary>
        /// Тестирование в границах одного полушария без пересечения экватора или нулевого меридиана.
        /// </summary>
        protected void SimpleTest()
        {
            // Северо-Восточное полушарие
            var pointSouthWest = new Point(10, 10);
            var pointNorthWest = new Point(10, 30);
            var pointNorth = new Point(20, 30);
            var pointNorthEast = new Point(30, 30);
            var pointEast = new Point(30, 20);
            var pointSouthEast = new Point(30, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Северо-Западное полушарие
            pointSouthWest = new Point(-30, 10);
            pointNorthWest = new Point(-30, 30);
            pointNorth = new Point(-20, 30);
            pointNorthEast = new Point(-10, 30);
            pointEast = new Point(-10, 20);
            pointSouthEast = new Point(-10, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Юго-Восточное полушарие
            pointSouthWest = new Point(10, -30);
            pointNorthWest = new Point(10, -10);
            pointNorth = new Point(20, -10);
            pointNorthEast = new Point(30, -10);
            pointEast = new Point(30, -20);
            pointSouthEast = new Point(30, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Юго-Западное полушарие
            pointSouthWest = new Point(-30, -30);
            pointNorthWest = new Point(-30, -10);
            pointNorth = new Point(-20, -10);
            pointNorthEast = new Point(-10, -10);
            pointEast = new Point(-10, -20);
            pointSouthEast = new Point(-10, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
        }

        /// <summary>
        /// Тестирование работы при пересечении экватора или нулевого меридиана.
        /// Тестирование пересечения 180ого меридиана в отдельном методе.
        /// </summary>
        protected void IntersectionTest()
        {
        }

        /// <summary>
        /// Тестирование работы при пересечении 180ого меридиана
        /// </summary>
        protected void Intersection180Test()
        {
        }

        /// <summary>
        /// Тестирование работы когда координаты по долготе или широте отличаются более чем на 90 градусов,
        ///     т.е. линия лежит в 2 полушариях и занимает значительную их часть
        /// </summary>
        protected void LongLineTest()
        {
        }

        /// <summary>
        /// Тестирование при пересечении полюса
        /// </summary>
        protected void IntersectionPolarTest()
        {
        }

        /// <summary>
        /// Выполнить проверку под разными углами наклона
        /// </summary>
        /// <remarks>
        /// Необходимо чтобы координаты образовывали квадрат, по крайней мере в прямоугольной проекции
        /// </remarks>
        private void AtDifferentAngles(
            Point pointSouthWest, Point pointNorthWest, Point pointNorth, Point pointNorthEast, Point pointEast, Point pointSouthEast)
        {
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast, new Spheroid());
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast, new Ellipsoid());
        }

        public virtual void AtDifferentAngles(Point pointSouthWest, Point pointNorthWest, Point pointNorth, Point pointNorthEast, Point pointEast, Point pointSouthEast, IEllipsoid ellipsoid)
        {
            // На 12 часов
            Tests(pointSouthEast, pointNorthEast, ellipsoid);
            Tests(pointSouthWest, pointNorthWest, ellipsoid);
            // На 1 час
            Tests(pointSouthWest, pointNorth, ellipsoid);
            // На 1,5 часа
            Tests(pointSouthWest, pointNorthEast, ellipsoid);
            // На 2 часа
            Tests(pointSouthWest, pointEast, ellipsoid);
            // На 3 часа
            Tests(pointSouthWest, pointSouthEast, ellipsoid);
            Tests(pointNorthWest, pointNorthEast, ellipsoid);
            // На 4 часа
            Tests(pointNorthWest, pointEast, ellipsoid);
            // На 4,5 часа
            Tests(pointNorthWest, pointSouthEast, ellipsoid);
            // На 5 часов
            Tests(pointNorth, pointSouthEast, ellipsoid);
            // На 6 часов
            Tests(pointNorthWest, pointSouthWest, ellipsoid);
            Tests(pointNorthEast, pointSouthEast, ellipsoid);
            // На 7 часов
            Tests(pointNorth, pointSouthWest, ellipsoid);
            // На 7,5 часов
            Tests(pointNorthEast, pointSouthWest, ellipsoid);
            // На 8 часов
            Tests(pointEast, pointSouthWest, ellipsoid);
            // На 9 часов
            Tests(pointNorthEast, pointNorthWest, ellipsoid);
            Tests(pointSouthEast, pointSouthWest, ellipsoid);
            // На 10 часов
            Tests(pointEast, pointNorthWest, ellipsoid);
            // На 10,5 часов
            Tests(pointSouthEast, pointNorthWest, ellipsoid);
            // На 11 часов
            Tests(pointSouthEast, pointNorth, ellipsoid);
        }

        public abstract void Tests(Point point1, Point point2, IEllipsoid ellipsoid);
    }
}
