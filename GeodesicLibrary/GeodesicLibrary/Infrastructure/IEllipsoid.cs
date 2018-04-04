namespace GeodesicLibrary.Infrastructure
{
    public interface IEllipsoid
    {
        double EquatorialRadius { get; }

        double PolarRadius { get; }

        /// <summary>
        /// Коэффициент полярного сжатия
        /// </summary>
        double F { get; }
    }
}