namespace Raytrc;

class Sphere : Hittable {
    public Sphere(Point3 center, double radius, Material material)
    {
        Center = center;
        Radius = Math.Max(0, radius);
        Material = material;
        var rvec = new Vec3(Radius, Radius, Radius);
        Bbox = new AABB(center - rvec, center + rvec);
    }

    public override bool Hit(Ray r, Interval rayT, out HitRecord rec){
        rec = new HitRecord();
        Vec3 oc = Center - r.Origin;
        var a = r.Dir.LengthSquared();
        var h = Vec3.Dot(r.Dir, oc);
        var c = oc.LengthSquared() - Radius*Radius;

        var discriminant = h*h - a*c;
        if (discriminant < 0)
            return false;

        var sqrtd = Math.Sqrt(discriminant);

        // Find the nearest root that lies in the acceptable range.
        var root = (h - sqrtd) / a;
        if (!rayT.Surrounds(root)) {
            root = (h + sqrtd) / a;
            if (!rayT.Surrounds(root)) 
                return false;
        }

        rec.T = root;
        rec.Point = r.At(rec.T);
        Vec3 outwardNormal = (rec.Point - Center) / Radius;
        rec.SetFaceNormal(r, outwardNormal);
        rec.Material = Material;
        
        return true;
    }

    public override AABB BoundingBox()
    {
        return Bbox;
    }

    public Vec3 Center;
    public double Radius;
    public Material Material;
    public AABB Bbox;
}