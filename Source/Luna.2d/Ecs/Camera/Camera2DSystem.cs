using Frent;
using Frent.Core;
using Luna.Ecs;
using Luna.g2d;

public class Camera2DSystem : IBehaviour
{
    private World world;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Init(World w)
    {
        world = w;
    }

    public void Update(float dt)
    {
        foreach ((Ref<Transform2D> t, Ref<Camera2D> cam) in world
            .Query<Transform2D, Camera2D>()
            .Enumerate<Transform2D, Camera2D>())
        {
            if (cam.Value.IsMainCamera)
                cam.Value.UpdateCamera(t.Value.Position);
        }
    }
}
