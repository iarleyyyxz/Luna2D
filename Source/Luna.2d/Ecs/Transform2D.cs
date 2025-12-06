using OpenTK.Mathematics;
using Frent;
using Frent.Components;

namespace Luna.Ecs
{
    public struct Transform2D : IEntityComponent
    {
        public Vector2 Position;

        public float Rotation;

        public Vector2 Scale;

        public void Update(Entity self)
        {
            
        }
    }
}