namespace GeodesicLibrary.Tools
{
    internal static class Converter
    {
        /// <summary>
        /// Конвертирует градусы.минуты.секунды в десятичные градусы
        /// </summary>
        public static double DergeeToDecimalDegree(double degrees, double minits, double seconds)
        {
            return degrees + minits / 60 + seconds / 3600;
        }
    }
}