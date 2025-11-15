using Luna.g2d;
using Luna.g2d.Renderer;
using System.Numerics;

namespace Luna.Ecs
{
    public class SpriteRenderer : Component
    {
        public Sprite2D Sprite;

        public override void Start()
        {
            if (owner.GetComponent<Transform2D>() == null)
                owner.AddComponent<Transform2D>();

            if (Sprite == null)
                Console.WriteLine($"[SpriteRenderer] Warning: GameObject '{owner.Name}' has no sprite!");
        }

        public override void Update(float dt)
        {
            Console.WriteLine($"Drawing sprite at {Sprite.Position} size {Sprite.Size}");
            if (Sprite == null)
                return;

            var t = owner.GetComponent<Transform2D>();

            Sprite.Position = new OpenTK.Mathematics.Vector2(t.Position.X, t.Position.Y);
            Sprite.Rotation = t.Rotation;
            Sprite.Size = t.Scale * Sprite.Size;


            Renderer2D.Draw(Sprite);
        }
    }
}
