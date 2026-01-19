namespace Raytrc;

public struct ColorUtil
{
    public static double LinearToGamma(double linearComponent)
    {
        if (linearComponent > 0)
            return Math.Sqrt(linearComponent);
        return 0;
    }
    
    public static void WriteColor(StreamWriter writer, Color col)
    {
        var r = col.X;
        var g = col.Y;
        var b = col.Z;
        
        Interval intensity = new Interval(0.000, 0.999);

        r = LinearToGamma(r);
        g = LinearToGamma(g);
        b = LinearToGamma(b);
        
        var rbyte = (int)(256 * intensity.Clamp(r));
        var gbyte = (int)(256 * intensity.Clamp(g));
        var bbyte = (int)(256 * intensity.Clamp(b));
        
        writer.WriteLine(rbyte + " " + gbyte + " " + bbyte);
    }
}