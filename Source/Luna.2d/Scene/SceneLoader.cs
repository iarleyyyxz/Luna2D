using Frent;
using Luna.Ecs;
using Luna.g2d;
using Luna.SceneSystem;
using OpenTK.Mathematics;

public static class SceneLoader
{
    public static Scene LoadInitial()
    {
        Scene scene = new MainScene();
        scene.Start();

        World world = scene.World;

        // camera
        Entity cam = world.Create();
        cam.Add(new Transform2D { Position = Vector2.Zero });
        cam.Add(new Camera2D
        {
            ProjectionSize = new(640, 360),
            Zoom = 1f,
            IsMainCamera = true
        });

        // teste sprite
        Entity e = world.Create();
        e.Add(new Transform2D { Position = new(100, 100), Scale = new(50, 50) });
        e.Add(new SpriteRenderer
        {
            Sprite = new Sprite2D("assets/textures/player.png"),
            Tint = new(1, 1, 1, 1)
        });

        return scene;
    }
}
