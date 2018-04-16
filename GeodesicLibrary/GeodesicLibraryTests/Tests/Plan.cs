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
          /*  pointSouthWest = new Point(100, 10);
            pointNorthWest = new Point(100, 30);
            pointNorth = new Point(110, 30);
            pointNorthEast = new Point(120, 30);
            pointEast = new Point(120, 20);
            pointSouthEast = new Point(120, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);*/

            // Северо-Западное полушарие
            pointSouthWest = new Point(-30, 10);
            pointNorthWest = new Point(-30, 30);
            pointNorth = new Point(-20, 30);
            pointNorthEast = new Point(-10, 30);
            pointEast = new Point(-10, 20);
            pointSouthEast = new Point(-10, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
          /*  pointSouthWest = new Point(-120, 10);
            pointNorthWest = new Point(-120, 30);
            pointNorth = new Point(-110, 30);
            pointNorthEast = new Point(-100, 30);
            pointEast = new Point(-100, 20);
            pointSouthEast = new Point(-100, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);*/

            // Юго-Восточное полушарие
            pointSouthWest = new Point(10, -30);
            pointNorthWest = new Point(10, -10);
            pointNorth = new Point(20, -10);
            pointNorthEast = new Point(30, -10);
            pointEast = new Point(30, -20);
            pointSouthEast = new Point(30, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
           /* pointSouthWest = new Point(100, -30);
            pointNorthWest = new Point(100, -10);
            pointNorth = new Point(110, -10);
            pointNorthEast = new Point(120, -10);
            pointEast = new Point(120, -20);
            pointSouthEast = new Point(120, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);*/

            // Юго-Западное полушарие
            pointSouthWest = new Point(-30, -30);
            pointNorthWest = new Point(-30, -10);
            pointNorth = new Point(-20, -10);
            pointNorthEast = new Point(-10, -10);
            pointEast = new Point(-10, -20);
            pointSouthEast = new Point(-10, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
           /* pointSouthWest = new Point(-120, -30);
            pointNorthWest = new Point(-120, -10);
            pointNorth = new Point(-110, -10);
            pointNorthEast = new Point(-100, -10);
            pointEast = new Point(-100, -20);
            pointSouthEast = new Point(-100, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);*/
        }

        /// <summary>
        /// Тестирование работы при пересечении экватора или нулевого меридиана.
        /// Тестирование пересечения 180ого меридиана в отдельном методе.
        /// </summary>
        protected void IntersectionTest()
        {
            // Пересечение нулевого меридиана в Северном полушарии
            var pointSouthWest = new Point(-5, 10);
            var pointNorthWest = new Point(-5, 30);
            var pointNorth = new Point(5, 30);
            var pointNorthEast = new Point(15, 30);
            var pointEast = new Point(15, 20);
            var pointSouthEast = new Point(15, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-15, 10);
            pointNorthWest = new Point(-15, 30);
            pointNorth = new Point(-5, 30);
            pointNorthEast = new Point(5, 30);
            pointEast = new Point(5, 20);
            pointSouthEast = new Point(5, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение нулевого меридиана в Южном полушарии
            pointSouthWest = new Point(-5, -30);
            pointNorthWest = new Point(-5, -10);
            pointNorth = new Point(5, -10);
            pointNorthEast = new Point(15, -10);
            pointEast = new Point(15, -20);
            pointSouthEast = new Point(15, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-15, -30);
            pointNorthWest = new Point(-15, -10);
            pointNorth = new Point(-5, -10);
            pointNorthEast = new Point(5, -10);
            pointEast = new Point(5, -20);
            pointSouthEast = new Point(5, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение экватора в Восточном полушарии
            pointSouthWest = new Point(10, -15);
            pointNorthWest = new Point(10, 5);
            pointNorth = new Point(20, 5);
            pointNorthEast = new Point(30, 5);
            pointEast = new Point(30, -5);
            pointSouthEast = new Point(30, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(10, -5);
            pointNorthWest = new Point(10, 15);
            pointNorth = new Point(20, 15);
            pointNorthEast = new Point(30, 15);
            pointEast = new Point(30, 5);
            pointSouthEast = new Point(30, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение экватора в Западном полушарии
            pointSouthWest = new Point(-30, -15);
            pointNorthWest = new Point(-30, 5);
            pointNorth = new Point(-20, 5);
            pointNorthEast = new Point(-10, 5);
            pointEast = new Point(-10, -5);
            pointSouthEast = new Point(-10, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-30, -5);
            pointNorthWest = new Point(-30, 15);
            pointNorth = new Point(-20, 15);
            pointNorthEast = new Point(-10, 15);
            pointEast = new Point(-10, 5);
            pointSouthEast = new Point(-10, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение экватора и нулевого меридиана
            pointSouthWest = new Point(-5, -15);
            pointNorthWest = new Point(-5, 5);
            pointNorth = new Point(5, 5);
            pointNorthEast = new Point(15, 5);
            pointEast = new Point(15, -5);
            pointSouthEast = new Point(15, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-5, -5);
            pointNorthWest = new Point(-5, 15);
            pointNorth = new Point(5, 15);
            pointNorthEast = new Point(15, 15);
            pointEast = new Point(15, 5);
            pointSouthEast = new Point(15, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-15, -15);
            pointNorthWest = new Point(-15, 5);
            pointNorth = new Point(-5, 5);
            pointNorthEast = new Point(5, 5);
            pointEast = new Point(5, -5);
            pointSouthEast = new Point(5, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-15, -5);
            pointNorthWest = new Point(-15, 15);
            pointNorth = new Point(-5, 15);
            pointNorthEast = new Point(5, 15);
            pointEast = new Point(5, 5);
            pointSouthEast = new Point(5, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
        }

        /// <summary>
        /// Тестирование работы при пересечении 180ого меридиана
        /// </summary>
        protected void Intersection180Test()
        {
            // Пересечение 180 меридиана в Северном полушарии
            var pointSouthWest = new Point(175, 10);
            var pointNorthWest = new Point(175, 30);
            var pointNorth = new Point(-175, 30);
            var pointNorthEast = new Point(-165, 30);
            var pointEast = new Point(-165, 20);
            var pointSouthEast = new Point(-165, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(165, 10);
            pointNorthWest = new Point(165, 30);
            pointNorth = new Point(175, 30);
            pointNorthEast = new Point(-175, 30);
            pointEast = new Point(-175, 20);
            pointSouthEast = new Point(-175, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение 180 меридиана в Южном полушарии
            pointSouthWest = new Point(175, -30);
            pointNorthWest = new Point(175, -10);
            pointNorth = new Point(-175, -10);
            pointNorthEast = new Point(-165, -10);
            pointEast = new Point(-165, -20);
            pointSouthEast = new Point(-165, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(165, -30);
            pointNorthWest = new Point(165, -10);
            pointNorth = new Point(175, -10);
            pointNorthEast = new Point(-175, -10);
            pointEast = new Point(-175, -20);
            pointSouthEast = new Point(-175, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);

            // Пересечение 180 меридиана У Экватора
            pointSouthWest = new Point(175, -15);
            pointNorthWest = new Point(175, 5);
            pointNorth = new Point(-175, 5);
            pointNorthEast = new Point(-165, 5);
            pointEast = new Point(-165, -5);
            pointSouthEast = new Point(-165, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(165, -15);
            pointNorthWest = new Point(165, 5);
            pointNorth = new Point(175, 5);
            pointNorthEast = new Point(-175, 5);
            pointEast = new Point(-175, -5);
            pointSouthEast = new Point(-175, -15);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(175, -5);
            pointNorthWest = new Point(175, 15);
            pointNorth = new Point(-175, 15);
            pointNorthEast = new Point(-165, 15);
            pointEast = new Point(-165, 5);
            pointSouthEast = new Point(-165, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(165, -5);
            pointNorthWest = new Point(165, 15);
            pointNorth = new Point(175, 15);
            pointNorthEast = new Point(-175, 15);
            pointEast = new Point(-175, 5);
            pointSouthEast = new Point(-175, -5);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
        }

        /// <summary>
        /// Тестирование работы когда координаты по долготе или широте отличаются более чем на 90 градусов,
        ///     т.е. линия лежит в 2 полушариях и занимает значительную их часть
        /// </summary>
        protected void LongLineTest()
        {
            // Северное через нулевой меридиан
            var pointSouthWest = new Point(-50, 10);
            var pointNorthWest = new Point(-50, 30);
            var pointNorth = new Point(20, 30);
            var pointNorthEast = new Point(80, 30);
            var pointEast = new Point(80, 20);
            var pointSouthEast = new Point(80, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-80, 10);
            pointNorthWest = new Point(-80, 30);
            pointNorth = new Point(-10, 30);
            pointNorthEast = new Point(50, 30);
            pointEast = new Point(50, 20);
            pointSouthEast = new Point(50, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            // Северное через 180ый меридиан
            pointSouthWest = new Point(145, 10);
            pointNorthWest = new Point(145, 30);
            pointNorth = new Point(-175, 30);
            pointNorthEast = new Point(-135, 30);
            pointEast = new Point(-135, 20);
            pointSouthEast = new Point(-135, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(135, 10);
            pointNorthWest = new Point(135, 30);
            pointNorth = new Point(175, 30);
            pointNorthEast = new Point(-145, 30);
            pointEast = new Point(-145, 20);
            pointSouthEast = new Point(-145, 10);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            // Южное через нулевой меридиан
            pointSouthWest = new Point(-50, -30);
            pointNorthWest = new Point(-50, -10);
            pointNorth = new Point(20, -10);
            pointNorthEast = new Point(80, -10);
            pointEast = new Point(80, -20);
            pointSouthEast = new Point(80, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-80, -30);
            pointNorthWest = new Point(-80, -10);
            pointNorth = new Point(-10, -10);
            pointNorthEast = new Point(50, -10);
            pointEast = new Point(50, -20);
            pointSouthEast = new Point(50, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            // Южное через 180ый меридиан
            pointSouthWest = new Point(145, -30);
            pointNorthWest = new Point(145, -10);
            pointNorth = new Point(-175, -10);
            pointNorthEast = new Point(-135, -10);
            pointEast = new Point(-135, -20);
            pointSouthEast = new Point(-135, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(135, -30);
            pointNorthWest = new Point(135, -10);
            pointNorth = new Point(175, -10);
            pointNorthEast = new Point(-145, -10);
            pointEast = new Point(-145, -20);
            pointSouthEast = new Point(-145, -30);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            // Восточное через экватор
            pointSouthWest = new Point(10, -55);
            pointNorthWest = new Point(10, 45);
            pointNorth = new Point(20, 45);
            pointNorthEast = new Point(30, 45);
            pointEast = new Point(30, -5);
            pointSouthEast = new Point(30, -55);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(10, -45);
            pointNorthWest = new Point(10, 55);
            pointNorth = new Point(20, 55);
            pointNorthEast = new Point(30, 55);
            pointEast = new Point(30, 5);
            pointSouthEast = new Point(30, -45);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            // Западное через экватор
            pointSouthWest = new Point(-30, -55);
            pointNorthWest = new Point(-30, 45);
            pointNorth = new Point(-20, 45);
            pointNorthEast = new Point(-10, 45);
            pointEast = new Point(-10, -5);
            pointSouthEast = new Point(-10, -55);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
            pointSouthWest = new Point(-30, -45);
            pointNorthWest = new Point(-30, 55);
            pointNorth = new Point(-20, 55);
            pointNorthEast = new Point(-10, 55);
            pointEast = new Point(-10, 5);
            pointSouthEast = new Point(-10, -45);
            AtDifferentAngles(pointSouthWest, pointNorthWest, pointNorth, pointNorthEast, pointEast, pointSouthEast);
        }

        /// <summary>
        /// Тестирование при пересечении полюса
        /// </summary>
        protected void IntersectionPolarTest()
        {
            // Северный полюс
            var point1 = new Point(50, 70);
            var point2 = new Point(-130, 50);
            Tests(point1, point2, new Spheroid());
            Tests(point1, point2, new Ellipsoid());
            point1 = new Point(120, 70);
            point2 = new Point(-60, 50);
            Tests(point1, point2, new Spheroid());
            Tests(point1, point2, new Ellipsoid());

            // Южный полюс
            point1 = new Point(50, -70);
            point2 = new Point(-130, -50);
            Tests(point1, point2, new Spheroid());
            Tests(point1, point2, new Ellipsoid());
            point1 = new Point(120, -70);
            point2 = new Point(-60, -50);
            Tests(point1, point2, new Spheroid());
            Tests(point1, point2, new Ellipsoid());
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
