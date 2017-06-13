namespace GeodesicLibrary.Model
{
    public class InverseProblemAnswer
    {
        public InverseProblemAnswer(double forward, double reverse, double distance)
        {
            ForwardAzimuth = forward;
            ReverseAzimuth = reverse;
            Distance = distance;
        }

        public double ForwardAzimuth { get; }

        public double ReverseAzimuth { get; }

        public double Distance { get; }
    }
}