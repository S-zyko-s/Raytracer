using System.Drawing;


namespace Raytrc;

public struct AABB
{
    public Interval X, Y, Z;

    public AABB()
    {
    }

    public AABB(Interval x, Interval y, Interval z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public AABB(Vec3 a, Vec3 b)
    {
        X = (a.X<=b.X) ? new Interval(a.X, b.X) :  new Interval(b.X, a.X);
        Y = (a.Y<=b.Y) ? new Interval(a.Y, b.Y) :  new Interval(b.Y, a.Y);
        Z = (a.Z<=b.Z) ? new Interval(a.Z, b.Z) :  new Interval(b.Z, a.Z);
    }

    public AABB(AABB box1, AABB box2)
    {
        X = new Interval(box1.X, box2.X);
        Y = new Interval(box1.Y, box2.Y);
        Z = new Interval(box1.Z, box2.Z);
    }

    public Interval AxisInterval(int n)
    {
        if (n == 1) return Y;
        if (n == 2) return Z;
        return X;
    }

    public bool Hit(Ray r, Interval rayT)
    {
        Vec3 RayOrigin = r.Origin;
        Vec3 RayDir = r.Dir;

        for (int axis = 0; axis < 3; axis++)
        {
            Interval ax = AxisInterval(axis);
            double ROdob;
            double RDdob;
            switch (axis)
            {
                case 0:
                    ROdob = RayOrigin.X;
                    RDdob =  RayDir.X;
                    break;
                case 1:
                    ROdob = RayOrigin.Y;
                    RDdob =  RayDir.Y;
                    break;
                default:
                    ROdob = RayOrigin.Z;
                    RDdob =  RayDir.Z;
                    break;
            }
            double adinv = 1 / RDdob;
            
            var t0 = (ax.Min - ROdob) * adinv;
            var t1 = (ax.Max - ROdob) * adinv;

            if (t0 < t1)
            {
                if (t0 > rayT.Min) rayT.Min = t0;
                if (t1 < rayT.Max) rayT.Max = t1;
            }
            else
            {
                if (t1 > rayT.Min) rayT.Min = t1;
                if (t0 < rayT.Max) rayT.Max = t0;
            }
            
            if (rayT.Max <= rayT.Min)
                return false;
        }
        return true;
    }

    public int LongestAxis()
    {
        if (X.Size() > Y.Size())
        {
            return X.Size() > Z.Size() ? 0 : 2;
        }
        else
        {
            return Y.Size() > Z.Size() ? 1 : 2;
        }
    }
    
    public static AABB Empty = new AABB();
    public static AABB Universe = new AABB(Interval.Universe, Interval.Universe, Interval.Universe);
}