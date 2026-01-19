namespace Raytrc;

public class Camera
{
    /* Public Camera Parameters Here */

    public string Path = "image.ppm";
    
    public double AspectRatio = 1.0;
    public int ImageWidth = 100;
    public int SamplesPerPixel = 100;
    public int MaxDepth = 50;
    
    public Point3 LookFrom = new Point3(0, 0, 0);
    public Point3 LookAt = new Point3(0, 0, -1);
    public Vec3 Vup = new Vec3(0, 1, 0);
    public double Vfov = 90;
    
    public double DefocusAngle = 0;
    public double FocusDistance = 10;

    public Color Background = new Color(0.2, 0.2, 0.2);
    public void Render(HittableList world) {
        Initialize();
        
        using var writer = new StreamWriter(Path);
        writer.Write("P3\n" + ImageWidth + " " + _imageHeight + "\n255\n");


        for (int j = 0; j < _imageHeight; j++)
        {
            Console.WriteLine("Progress status: " + j + "/" + _imageHeight);
            Color[] row = new Color[ImageWidth];
            Parallel.For(0, ImageWidth, i =>
            {
            Color pixelColor = new Color(0, 0, 0);
            for (int sample = 0; sample < SamplesPerPixel; sample++)
            {
                Ray r = GetRay(i, j);
                pixelColor += RayColor(r, MaxDepth, world);
            }

            row[i] = pixelColor;
        });
            for (int i = 0; i < ImageWidth; i++)
            {
                ColorUtil.WriteColor(writer, _pixelSamplesScale * row[i]);
            }
        }
        Console.WriteLine("Rendering Completed");
    }
    
    /* Private Camera Variables Here */
    private int    _imageHeight;   // Rendered image height
    private double _pixelSamplesScale;
    private Point3 _center;         // Camera center
    private Point3 _pixel00Loc;    // Location of pixel 0, 0
    private Vec3   _pixelDeltaU;  // Offset to pixel to the right
    private Vec3 _pixelDeltaV;
    private Vec3 _u, _v, _w;
    private Vec3 _defocusDiskU; 
    private Vec3 _defocusDiskV;
    
    void Initialize() {
        _imageHeight = (int)(ImageWidth/AspectRatio);
        if (_imageHeight == 0){_imageHeight = 1;}
        
        _pixelSamplesScale = 1.0 / SamplesPerPixel;
        
        _center = LookFrom;
        
        var theta = DegreeToRadian(Vfov);
        var h = Math.Tan(theta/2);
        var viewportHeight = 2 * h * FocusDistance;
        var viewportWidth = viewportHeight * ((double)ImageWidth/_imageHeight);

        _w = Vec3.UnitVector(LookFrom-LookAt);
        _u = Vec3.UnitVector(Vec3.Cross(Vup, _w));
        _v = Vec3.Cross(_w, _u);
        
        var viewportU = viewportWidth * _u;
        var viewportV = viewportHeight * -_v;
        
        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;
        
        var viewportUpperLeft = _center - FocusDistance * _w - viewportU/2 - viewportV/2;
        _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);
        
        var defocusRadius = FocusDistance * Math.Tan(DegreeToRadian(DefocusAngle/2));
        _defocusDiskU = _u * defocusRadius;
        _defocusDiskV = _v * defocusRadius;
        
    }

    Color RayColor(Ray r, int depth, HittableList world){
        HitRecord rec;
        if (depth <= 0)
            return new Color(0, 0, 0);
        
        
        if (world.Hit(r, new Interval(0.001, Infinity), out rec))
        {
            Ray scattered;
            Color attenuation;
            if (rec.Material.Scatter(r, rec, out attenuation, out scattered))
                return attenuation * RayColor(scattered, depth-1, world);
            // return rec.Material.Emitted();
        }

        // return Background;
        Vec3 unitDirection = Vec3.UnitVector(r.Dir);
        var a = 0.5 * (unitDirection.Y + 1);
        return (1-a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }

    Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixelSample = _pixel00Loc
                            + ((i + offset.X) * _pixelDeltaU)
                            + ((j + offset.Y) * _pixelDeltaV);

        var rayOrigin = (DefocusAngle <= 0) ? _center : DefocusDiskSample();
        var rayDirection = pixelSample - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }

    Point3 DefocusDiskSample()
    {
        var p = Vec3.RandomInUnitDisk();
        return _center + (p.X * _defocusDiskU) + (p.Y * _defocusDiskV);
    }
    
    Vec3 SampleSquare()
    {
        return new Vec3(RandomDouble() - 0.5, RandomDouble() - 0.5, 0);
    }
}