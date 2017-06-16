namespace GeodesicLibrary.Tools
{
    internal static class Converter
    {
        public static double DergeeToDecimalDegree(double degrees, double minits, double seconds)
        {
            return degrees + minits / 60 + seconds / 3600;
        }
    }
}