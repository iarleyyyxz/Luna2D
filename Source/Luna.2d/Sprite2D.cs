using Luna.Core;
using OpenTK.Mathematics;

namespace Luna.g2d
{
    public class Sprite2D
    {
        public Texture2D Texture;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = Vector2.One; // in pixels
        public float Rotation = 0f; // degrees
        public bool FlipX = true;
        public bool FlipY = true;
        public Vector4 Color = Vector4.One; // rgba tint
        public int Layer = 0;

        // Custom UVs (0..1) - useful for spritesheets
        public Vector2 UV0 = new(0, 0);
        public Vector2 UV1 = new(1, 1);

        public Sprite2D(Texture2D texture)
        {
            Texture = texture;
            Size = new Vector2(texture.Width, texture.Height);
        }

        public Sprite2D(string path)
        {
            Texture = ResourceManager.LoadTexture(path);
            Size = new Vector2(Texture.Width, Texture.Height);
        }
    }
}