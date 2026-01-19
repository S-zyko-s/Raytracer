namespace Raytrc;

public struct HitRecord {
    public Point3 Point;
    public Vec3 Normal;
    public double T;
    public bool FrontFace;
    public Material Material;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Dir, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}

public class Hittable {
    public virtual bool Hit(Ray r, Interval rayT, out HitRecord rec)
    {
        rec = new HitRecord();
        return false;
    }

    public virtual AABB BoundingBox()
    {
        return new AABB();
    }
}