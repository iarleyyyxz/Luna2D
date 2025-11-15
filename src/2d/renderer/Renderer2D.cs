using OpenTK.Mathematics;

namespace Luna.g2d.Renderer
{
    public static class Renderer2D
    {
        private static SpriteBatch2D batch = new SpriteBatch2D();

        public static void Begin()
        {
            batch.Begin();
        }

        public static void Draw(Sprite2D sprite)
        {
            batch.DrawSprite(
                sprite.Texture,
                sprite.Position,
                sprite.Size,
                sprite.Rotation,
                sprite.Color,
                sprite.UV0,
                sprite.UV1,
                sprite.FlipX,
                sprite.FlipY,
                sprite.Layer
            );
        }

        public static void End(int screenW, int screenH)
        {
            batch.End(screenW, screenH);
        }
    }
}
