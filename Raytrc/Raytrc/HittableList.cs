namespace Raytrc;

public class HittableList : Hittable
{
    public List<Hittable> Objects = new List<Hittable>();

    public HittableList() {}
    public HittableList(Hittable obj) 
    {
        Objects.Add(obj);
        Bbox = new AABB(Bbox, obj.BoundingBox());
    }
    
    
    public override bool Hit(Ray r, Interval rayT, out HitRecord rec) {
        HitRecord tempRec;
        bool hitAnything = false;
        var closestSoFar = rayT.Max;
        rec = new HitRecord();
        foreach (Hittable obj in Objects) {
            if (obj.Hit(r, new Interval(rayT.Min, closestSoFar), out tempRec)) {
                hitAnything = true; 
                closestSoFar = tempRec.T;
                rec = tempRec;
            }
        }
        
        return hitAnything;
    }

    public override AABB BoundingBox()
    {
        return Bbox;
    }
    
    private AABB Bbox;
}