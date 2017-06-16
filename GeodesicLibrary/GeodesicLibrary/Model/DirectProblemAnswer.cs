namespace GeodesicLibrary.Model
{
    public class DirectProblemAnswer
    {
        public DirectProblemAnswer(Point сoordinate, double reverseAzimuth)
        {
            Сoordinate = сoordinate;
            ReverseAzimuth = reverseAzimuth;
        }

        public Point Сoordinate { get; }

        public double ReverseAzimuth { get; }
    }
}