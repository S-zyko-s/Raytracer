using Microsoft.VisualBasic.CompilerServices;

namespace Raytrc;

public class Material
{
    public virtual bool Scatter(
    Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered
        ) {
            scattered = new Ray(rec.Point, rec.Normal);
            attenuation = new Color(0, 0, 0);
            return false;
        }
    
    public virtual Color Emitted(
    ) {
        return new Color(0, 0, 0);
    }
}

class Lambertian : Material
{
    public Lambertian(Color albedo)
    {
        _albedo =  albedo;
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        Vec3 scatterDirection =  rec.Normal + Vec3.RandomUnitVector();
        if (Vec3.NearZero(scatterDirection))
        {
            scatterDirection = rec.Normal;
        }
        scattered = new Ray(rec.Point, scatterDirection);
        attenuation = _albedo;
        return true;
    }
    
    private Color _albedo; 
}

class Metal : Material
{
    public Metal(Color albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz;
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(rIn.Dir, rec.Normal);
        reflected = Vec3.UnitVector(reflected) + _fuzz* Vec3.RandomUnitVector();
        attenuation = _albedo;
        scattered = new Ray(rec.Point, reflected);
        return Vec3.Dot(scattered.Dir, rec.Normal) > 0;
    }

    private Color _albedo;
    private double _fuzz;
}

class Dielectric : Material
{
    public Dielectric(double refractionIndex)
    {
        _refractionIndex = refractionIndex;
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        attenuation =  new Color(1, 1, 1);
        double ri = rec.FrontFace ? (1 / _refractionIndex) : _refractionIndex;
        
        Vec3 unitDir = Vec3.UnitVector(rIn.Dir);
        
        var cos = Math.Min(Vec3.Dot(-unitDir, rec.Normal), 1.0);
        var sin = Math.Sqrt(1.0 - cos * cos);

        bool cannotRefract = ri * sin > 1.0;
        Vec3 changed;
        
        if (cannotRefract || Reflectance(cos, ri) > RandomDouble())
        {
            changed = Vec3.Reflect(unitDir, rec.Normal);
        }
        else
        {
            changed = Vec3.Refract(unitDir, rec.Normal, ri);
        }
        
        scattered = new Ray(rec.Point, changed);
        return true;
    }

    static double Reflectance(double cos, double relativeRefractionIndex)
    {
        var r0 = (1 - relativeRefractionIndex) / (1 + relativeRefractionIndex);
        r0 *= r0;
        return r0 + (1-r0) * Math.Pow((1-cos), 5);
    }
    
    private double _refractionIndex;
}

class DiffuseLight : Material
{
    public DiffuseLight(Color color)
    {
        _color = color;
    }

    public override Color Emitted()
    {
        return new Color(_color);
    }
    
    private Color _color;
}