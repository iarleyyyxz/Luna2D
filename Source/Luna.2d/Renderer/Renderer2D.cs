using Luna.Ecs;
using Luna.g2d;
using Luna.g2d.Renderer;
using OpenTK.Mathematics;

public static class Renderer2D
{
    private static SpriteBatch2D batch = new SpriteBatch2D();
    private static Matrix4 VP;

    public static void Begin(Matrix4 viewProjection)
    {
        VP = viewProjection;
        batch.Begin();
    }

    public static void Draw(Sprite2D sprite, Transform2D transform, Vector4 tint)
    {
        batch.DrawSprite(
            sprite.Texture,
            transform.Position,
            transform.Scale,
            transform.Rotation,
            tint,
            sprite.UV0,
            sprite.UV1,
            false,
            false,
            0
        );
    }

    public static void End()
    {
        batch.End(VP);
    }
}
