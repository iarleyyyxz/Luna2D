using System.Numerics;
using Frent;
using Frent.Core;
using Luna.g2d;
using Luna.g2d.Interfaces;
using Luna.Util;

namespace Luna.Ecs
{
    public class Camera2DSystem : IBehaviour
    {
        private World world;

        public void Init(World world)
        {
            this.world = world;
        }

        public void Update(float dt)
        {
            foreach ((Ref<Transform2D> transform, Ref<Camera2D> camera) 
                in world.Query<Transform2D, Camera2D>().Enumerate<Transform2D, Camera2D>())
            {
                ref Transform2D t = ref transform.Value;
                ref Camera2D c = ref camera.Value;

                if (c.FollowTarget && c.Target.IsAlive)
                {
                    if (c.Target.TryGet(out Ref<Transform2D> targetTransform))
                    {
                        if (c.FollowSmoothing > 0)
                        {
                            t.Position = Vector2.Lerp(
                                t.Position,
                                targetTransform.Value.Position,
                                1f - MathF.Exp(-c.FollowSmoothing * dt)
                            );
                        }
                        else
                        {
                            t.Position = targetTransform.Value.Position;
                        }
                    }
                }

                c.Projection = CreateProjectionMatrix(c);

                Matrix4x4.Invert(c.Projection, out c.InverseProjection);

                c.View = CreateViewMatrix(t.Position);

                Matrix4x4.Invert(c.View, out c.InverseView);

                c.ViewProjection = c.View * c.Projection;
            }
        }

        private Matrix4x4 CreateProjectionMatrix(Camera2D c)
        {
            float w = c.ProjectionSize.X * c.Zoom;
            float h = c.ProjectionSize.Y * c.Zoom;

            return Matrix4x4.CreateOrthographicOffCenter(
                0, w,
                h, 0,
                -1f, 100f
            );
        }

        private Matrix4x4 CreateViewMatrix(Vector2 cameraPos)
        {
            return Matrix4x4.CreateTranslation(
                -cameraPos.X,
                -cameraPos.Y,
                0f
            );
        }

        public void Dispose() { }
    }
}
