using System.Numerics;

namespace Raytrc;


public struct Vec3
{
    public double X;
    public double Y;
    public double Z;
    
    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vec3(Vec3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public static Vec3 operator *(double a, Vec3 b)
    {
        b.X *= a;
        b.Y *= a;
        b.Z *= a;
        return b;
    }

    public static Vec3 operator -(Vec3 b)
    {
        return new Vec3(-b.X, -b.Y, -b.Z);
    }
    
    public static Vec3 operator *(Vec3 b, double a)
    {
        b.X *= a;
        b.Y *= a;
        b.Z *= a;
        return b;
    }
    
    public static Vec3 operator *(Vec3 a, Vec3 b)
    {
        b.X *= a.X;
        b.Y *= a.Y;
        b.Z *= a.Z;
        return b;
    }

    public static Vec3 operator /(Vec3 b, double a)
    {
        return (1 / a) * b;
    }
    
    public static Vec3 operator +(Vec3 a, Vec3 b)
    {
        return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    public static Vec3 operator +(Vec3 a, double b)
    {
        return new Vec3(a.X + b, a.Y + b, a.Z + b);
    }
    
    public static Vec3 operator -(Vec3 a, Vec3 b)
    {
        return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
    
    public static double Dot(Vec3 a, Vec3 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }

    public static Vec3 Cross(Vec3 a, Vec3 b)
    {
        return new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
    }
    
    public double Length()
    {
        return Math.Sqrt(LengthSquared());
    }

    public double LengthSquared() 
    {
        return X*X + Y*Y + Z*Z;
    }

    public static Vec3 UnitVector(Vec3 v)
    {
        return v/v.Length();
    }

    public static Vec3 RandomVector()
    {
        return new Vec3(RandomDouble(),  RandomDouble(), RandomDouble());
    }
    
    public static Vec3 RandomVector(double min, double max)
    {
        return new Vec3(RandomDouble(min, max),  RandomDouble(min, max), RandomDouble(min, max));
    }
    
    public static Vec3 RandomUnitVector()
    {
        while (true)
        {
            Vec3 V = RandomVector(-1, 1);
            double len = V.LengthSquared();
            if (len <= 1 && len > 1e-160)
                return V/double.Sqrt(len);
        }
    }

    public static Vec3 RandomInUnitDisk()
    {
        while (true)
        {
            Vec3 V = new Vec3(RandomDouble(-1, 1), RandomDouble(-1, 1), 0);
            double len = V.LengthSquared();
            if (len < 1)
                return V;
        }
    }
    
    public static Vec3 RandomOnHemisphere(Vec3 normal)
    {
        Vec3 V = RandomUnitVector();
        if (Dot(V, normal) > 0)
            return V;
        return -V;
    }

    public static bool NearZero(Vec3 vec)
    {
        return (vec.X < 1e-8 &&  vec.Y < 1e-8 && vec.Z < 1e-8);
    }

    public static Vec3 Reflect(Vec3 vec, Vec3 normal)
    {
        return vec - 2 * normal * Dot(vec, normal);
    }

    public static Vec3 Refract(Vec3 uv, Vec3 normal, double relativeRefractionIndex)
    {
        var cos = Math.Min(Dot(-uv, normal), 1.0);
        Vec3 rOutPerp = relativeRefractionIndex * (uv + cos * normal);
        Vec3 rOutParrallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared())) * normal;
        return  rOutParrallel + rOutPerp;
    }
    // public double this[int  index]
    // {
    //     get => index switch
    //     {
    //         0 => X,
    //         1 => Y,
    //         _ => Z
    //     };
    //     set
    //     {
    //         switch (index)
    //         {
    //             case 0: X = value; break;
    //             case 1: Y = value; break;
    //             default: Z = value; break;
    //         }
    //     }
    // }
}
public struct Ray
{
    public Vec3 Origin;
    public Vec3 Dir;
    public Ray(Vec3 origin, Vec3 direction)
    {
        Origin = origin;
        Dir = direction;
    }
    
    public Vec3 At(double t)
    {
        return Origin + t * Dir;
    }
}