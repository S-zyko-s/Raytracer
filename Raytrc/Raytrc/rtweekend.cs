namespace Raytrc;

public struct Rtweekend
{
    public const double Infinity =  double.PositiveInfinity;
    public const double Pi = 3.1415926535897932385;
    
    public static double DegreeToRadian(double angle)
    {
        return Pi * angle / 180.0;
    }

    public static double RandomDouble()
    {
        return Random.Shared.NextDouble();
    }

    public static double RandomDouble(double min, double max)
    {
        return min + RandomDouble() * (max - min);
    }
}