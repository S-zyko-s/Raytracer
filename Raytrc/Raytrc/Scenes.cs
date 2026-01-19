namespace Raytrc;

public class Scenes
{
    public static void Scene1()
    {
        HittableList world = new HittableList();

        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
        var materialLeft = new Dielectric(1.5);
        var materialBubble = new Dielectric(1/1.5);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);
        
        world.Objects.Add(new Sphere(new Point3(0,-100.5,-1), 100, materialGround));
        world.Objects.Add(new Sphere(new Point3(0,0,-1.2), 0.5, materialCenter));
        world.Objects.Add(new Sphere(new Point3(-1,0,-1), 0.5, materialLeft));
        world.Objects.Add(new Sphere(new Point3(-1,0,-1), 0.4, materialBubble));
        world.Objects.Add(new Sphere(new Point3(1,0,-1), 0.5, materialRight));
        
        Camera cam = new Camera();

        var time = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
        cam.Path = $"Scene1-{time}.ppm";
        
        cam.AspectRatio = 16.0 / 9.0;
        cam.ImageWidth = 600;
        cam.LookFrom = new Point3(-2, 2, 1);
        cam.LookAt = new Point3(0, 0, -1);
        cam.Vup = new Vec3(0, 1, 0);
        cam.Vfov = 20;

        cam.DefocusAngle = 10.0;
        cam.FocusDistance = 3.4;

        cam.Render(world);
    }

    public static void Scene2()
    {
        HittableList world = new HittableList();
        
        var ground_material = new Lambertian(new Color(0.5, 0.5, 0.5));
            world.Objects.Add(new Sphere(new Point3(0,-1000,0), 1000, ground_material));
        
    for (int a = -11; a < 11; a++) {
        for (int b = -11; b < 11; b++) {
            var choose_mat = RandomDouble();
            Point3 center = new Point3(a + 0.9*RandomDouble(), 0.2, b + 0.9*RandomDouble());

            if ((center - new Point3(4, 0.2, 0)).Length() > 0.9) {
                Material sphere_material;

                if (choose_mat < 0.6) {
                    // diffuse
                    var albedo = Color.RandomVector() * Color.RandomVector();
                    sphere_material = new Lambertian(albedo);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else if (choose_mat < 0.95) {
                    // metal
                    var albedo = Color.RandomVector(0.5, 1);
                    var fuzz = RandomDouble(0, 0.5);
                    sphere_material = new Metal(albedo, fuzz);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else {
                    // glass
                    sphere_material = new Dielectric(1.5);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                }
            }
        }
    }

    var material1 = new Dielectric(1.5);
    world.Objects.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

    var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
    world.Objects.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

    var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
    world.Objects.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

    world = new HittableList(new BvhNode(world));
    Camera cam = new Camera();

    var time = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
    cam.Path = $"Scene2-{time}.ppm";
    
    cam.AspectRatio      = 16.0 / 9.0;
    cam.ImageWidth       = 1200;
    cam.SamplesPerPixel  = 500;
    cam.MaxDepth         = 50;

    cam.Vfov     = 20;
    cam.LookFrom = new Point3(13,2,3);
    cam.LookAt   = new Point3(0,0,0);
    cam.Vup      = new Vec3(0,1,0);
    
    cam.DefocusAngle = 0.6;
    cam.FocusDistance    = 10.0;
    
    cam.Render(world);
    }
    
    public static void Scene3()
    {
        HittableList world = new HittableList();
        
        var ground_material = new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Objects.Add(new Sphere(new Point3(0,-1000,0), 1000, ground_material));

    for (int a = -11; a < 11; a = a+2) {
        for (int b = -11; b < 11; b = b+2) {
            var choose_mat = RandomDouble();
            Point3 center = new Point3(a + 0.9*RandomDouble(), 0.2, b + 0.9*RandomDouble());

            if ((center - new Point3(4, 0.2, 0)).Length() > 0.9) {
                Material sphere_material;

                if (choose_mat < 0.2) {
                    // diffuse
                    var albedo = Color.RandomVector() * Color.RandomVector();
                    sphere_material = new Lambertian(albedo);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else if (choose_mat < 0.6) {
                    // metal
                    var albedo = Color.RandomVector(0.5, 1);
                    var fuzz = RandomDouble(0, 0.5);
                    sphere_material = new Metal(albedo, fuzz);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else {
                    // glass
                    sphere_material = new Dielectric(1.5);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                }
            }
        }
    }

    var material1 = new Dielectric(1.5);
    world.Objects.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

    var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
    world.Objects.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

    var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
    world.Objects.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

    var material4 = new DiffuseLight(new Color(0.7, 0.6, 0.5)*10);
    world.Objects.Add(new Sphere(new Point3(4, 1, 5), 1.0, material4));
    
    
    world = new HittableList(new BvhNode(world));
    Camera cam = new Camera();
    
    var time = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
    cam.Path = $"Scene3-{time}.ppm";

    cam.AspectRatio      = 16.0 / 9.0;
    cam.ImageWidth       = 800;
    cam.SamplesPerPixel  = 300;
    cam.MaxDepth         = 50;

    cam.Vfov     = 20;
    cam.LookFrom = new Point3(13,2,3);
    cam.LookAt   = new Point3(0,0,0);
    cam.Vup      = new Vec3(0,1,0);

    cam.DefocusAngle = 0.6;
    cam.FocusDistance    = 10.0;
    cam.Background = new Color(0, 0, 0);

    cam.Render(world);
    }
    
    public static void Scene4()
    {
        HittableList world = new HittableList();
        
        var ground_material = new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Objects.Add(new Sphere(new Point3(0,-1000,0), 1000, ground_material));

    for (int a = -11; a < 11; a = a+2) {
        for (int b = -11; b < 11; b = b+2) {
            var choose_mat = RandomDouble();
            Point3 center = new Point3(a + 0.9*RandomDouble(), 0.2, b + 0.9*RandomDouble());

            if ((center - new Point3(4, 0.2, 0)).Length() > 0.9) {
                Material sphere_material;

                if (choose_mat < 0.2) {
                    // diffuse
                    var albedo = Color.RandomVector() * Color.RandomVector();
                    sphere_material = new Lambertian(albedo);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else if (choose_mat < 0.6) {
                    // metal
                    var albedo = Color.RandomVector(0.5, 1);
                    var fuzz = RandomDouble(0, 0.5);
                    sphere_material = new Metal(albedo, fuzz);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                } else {
                    // glass
                    sphere_material = new Dielectric(1.5);
                    world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                }
            }
        }
    }

    var material1 = new Dielectric(1.5);
    world.Objects.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

    var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
    world.Objects.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

    var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
    world.Objects.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

    var material4 = new DiffuseLight(new Color(0.7, 0.6, 0.5)*10);
    world.Objects.Add(new Sphere(new Point3(4, 7, 0), 1.0, material4));
    
    
    world = new HittableList(new BvhNode(world));
    Camera cam = new Camera();

    var time = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
    cam.Path = $"Scene4-{time}.ppm";
    
    cam.AspectRatio      = 16.0 / 9.0;
    cam.ImageWidth       = 600;
    cam.SamplesPerPixel  = 300;
    cam.MaxDepth         = 50;

    cam.Vfov     = 20;
    cam.LookFrom = new Point3(13,2,3);
    cam.LookAt   = new Point3(0,0,0);
    cam.Vup      = new Vec3(0,1,0);

    cam.DefocusAngle = 0.6;
    cam.FocusDistance    = 10.0;
    cam.Background = new Color(0, 0, 0);

    cam.Render(world);
    }
    
    public static void Scene5()
    {
        HittableList world = new HittableList();
        
        var ground_material = new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Objects.Add(new Sphere(new Point3(0,-1000,0), 1000, ground_material));

        for (int a = -11; a < 11; a++) {
            for (int b = -11; b < 11; b++) {
                var choose_mat = RandomDouble();
                Point3 center = new Point3(a + 0.9*RandomDouble(), 0.2, b + 0.9*RandomDouble());

                if ((center - new Point3(4, 0.2, 0)).Length() > 0.9) {
                    Material sphere_material;

                    if (choose_mat < 0.2) {
                        // diffuse
                        var albedo = Color.RandomVector() * Color.RandomVector();
                        sphere_material = new Lambertian(albedo);
                        world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                    } else if (choose_mat < 0.6) {
                        // metal
                        var albedo = Color.RandomVector(0.5, 1);
                        var fuzz = RandomDouble(0, 0.5);
                        sphere_material = new Metal(albedo, fuzz);
                        world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                    } else {
                        // glass
                        sphere_material = new Dielectric(1.5);
                        world.Objects.Add(new Sphere(center, 0.2, sphere_material));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5);
        world.Objects.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

        var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
        world.Objects.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

        var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
        world.Objects.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

        var material4 = new DiffuseLight(new Color(1, 0, 0)*10);
        world.Objects.Add(new Sphere(new Point3(2.5, 7, 0), 1.0, material4));
        
        var material5 = new DiffuseLight(new Color(0, 1, 0)*10);
        world.Objects.Add(new Sphere(new Point3(6.5, 7, 0), 1.0, material5));
        
        var material6 = new DiffuseLight(new Color(0, 0, 1)*10);
        world.Objects.Add(new Sphere(new Point3(4.5, 7, 3.5), 1.0, material6));
        
        
        world = new HittableList(new BvhNode(world));
        Camera cam = new Camera();

        var time = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
        cam.Path = $"Scene5-{time}.ppm";
        
        cam.AspectRatio      = 16.0 / 9.0;
        cam.ImageWidth       = 1600;
        cam.SamplesPerPixel  = 1000;
        cam.MaxDepth         = 50;

        cam.Vfov     = 20;
        cam.LookFrom = new Point3(13,2,3);
        cam.LookAt   = new Point3(0,0,0);
        cam.Vup      = new Vec3(0,1,0);

        cam.DefocusAngle = 0.6;
        cam.FocusDistance    = 10.0;
        cam.Background = new Color(0.03, 0.03, 0.03);

        cam.Render(world);
    }
}