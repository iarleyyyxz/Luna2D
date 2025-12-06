using Luna.SceneSystem;
using Luna.g2d;
using System.Numerics;
using Luna.Ecs;

public class MainScene : Scene
{
    protected override void OnLoad()
    {
        // CAMERA
        var cam = CreateEntity();
        cam.Add(new Transform2D { Position = new(0, 0) });
        cam.Add(new Camera2D
        {
            Zoom = 1f,
            ProjectionSize = new(640, 360),
            IsMainCamera = true
        });

        // SPRITE
        var sprite = CreateEntity();
        sprite.Add(new Transform2D { Position = new(120, 80) });
        sprite.Add(new SpriteRenderer()
        {
            Sprite = new Sprite2D("assets/textures/player.png"),
            Tint = new(1,1,1,1)
        });
    }
}
