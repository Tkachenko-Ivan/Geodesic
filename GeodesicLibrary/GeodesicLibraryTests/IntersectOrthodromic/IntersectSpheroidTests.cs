using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
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
    [TestClass]
    public class IntersectSpheroidTests : IntersectTests
    {
        public sealed override IntersectService IntersectService { get; set; }

        public sealed override InverseProblemService InverseProblemService { get; set; }

        public IntersectSpheroidTests()
        {
            IntersectService = new IntersectService(new Spheroid());
            InverseProblemService = new InverseProblemService(new Spheroid());
        }
    }
}