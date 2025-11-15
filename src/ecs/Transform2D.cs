using OpenTK.Mathematics;

namespace Luna.Ecs
{
    public class Transform2D : Component
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Scale = new Vector2(1, 1);
        public float Rotation = 0f;     // graus

        public void Translate(Vector2 delta) => Position += delta;
    }
}
