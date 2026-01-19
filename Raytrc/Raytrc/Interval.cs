using System.Xml;

namespace Raytrc;

public struct Interval
{
    public double Min, Max;
    
    public Interval()
    {
        Min = +Infinity;
        Max = -Infinity;
    }

    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public Interval(Interval a, Interval b)
    {
        Min = a.Min <= b.Min ? a.Min : b.Min;
        Max = a.Max >= b.Max ? a.Max : b.Max;
    }
    
    public double Size()
    {
        return Max - Min;
    }
    
    public bool Contains(double x)
    {
        return Min <= x && x <= Max;
    }
    
    public bool Surrounds(double x)
    {
        return Min < x && x < Max;
    }

    public double Clamp(double x)
    {
        if (x < Min) return Min;
        if (x > Max) return Max;
        return x;
    }

    public Interval Expand(double delta)
    {
        var padding = delta / 2;
        return new Interval(Min - padding, Max + padding);
    }
    
    public static Interval Empty = new Interval();
    public static Interval Universe = new  Interval(-Infinity, +Infinity);
}