namespace GeodesicLibrary.Model
{
    public class DirectProblemAnswer
    {
        public DirectProblemAnswer(double longitude, double latitude, double reverseAzimuth)
        {
            Longitude = longitude;
            Latitude = latitude;
            ReverseAzimuth = reverseAzimuth;
        }

        public double Longitude { get; }

        public double Latitude { get; }

        public double ReverseAzimuth { get; }
    }
}