using System.Security.Cryptography;

namespace Raytrc;

public class BvhNode : Hittable
{
    public BvhNode(HittableList list) : this(list.Objects, 0, list.Objects.Count)
    {
    }

    public BvhNode(List<Hittable> obj, int start, int end)
    {
        Bbox = AABB.Empty;
        for (int objIndex=start; objIndex < end; objIndex++)
            Bbox = new AABB(Bbox, obj[objIndex].BoundingBox());

        int axis = Bbox.LongestAxis();
        
        int objSpan = end - start;

        if (objSpan == 1)
        {
            Left = Right = obj[start];
        } else if (objSpan == 2)
        {
            Left = obj[start];
            Right =  obj[start+1];
        }
        else
        {
            obj.Sort(start, end - start, Comparer<Hittable>.Create((a, b) =>
            {
                Interval aVal, bVal;
                
                bVal = b.BoundingBox().AxisInterval(axis);
                aVal = a.BoundingBox().AxisInterval(axis);

                if (aVal.Max>bVal.Max) return -1;
                if (aVal.Max<bVal.Max) return 1;
                return 0;
            }));
            
            var mid = start + objSpan / 2;
            Left = new BvhNode(obj, start, mid);
            Right = new BvhNode(obj, mid, end);
        }
        
        Bbox = new AABB(Left.BoundingBox(), Right.BoundingBox());
    }

    public override bool Hit(Ray r, Interval rayT, out HitRecord rec)
    {
        rec = new HitRecord();
        HitRecord leftRec;
        if (!Bbox.Hit(r, rayT))
            return false;

        bool hitLeft = Left.Hit(r, rayT, out leftRec);
        bool hitRight = Right.Hit(r, new Interval(rayT.Min, hitLeft ? leftRec.T : rayT.Max), out rec);
        if (hitLeft && !hitRight){ rec = leftRec; }
        
        return hitLeft || hitRight;
    }

    public override AABB BoundingBox()
    {
        return Bbox;
    }
    
    private Hittable Left;
    private Hittable Right;
    private AABB Bbox;
}