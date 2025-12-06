using Frent;
using Frent.Core;
using Luna.Ecs;
using Luna.g2d;

public class SpriteRenderSystem : IBehaviour
{
    private World world;
    private Camera2D mainCamera;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Init(World world)
    {
        this.world = world;
    }

    public void Render()
    {
        // acha a câmera principal
        foreach ((Ref<Transform2D> t, Ref<Camera2D> cam)
            in world.Query<Transform2D, Camera2D>().Enumerate<Transform2D, Camera2D>())
        {
            if (cam.Value.IsMainCamera)
                mainCamera = cam.Value;
        }

        if (mainCamera.ViewProjection == default)
            return;

        Renderer2D.Begin(mainCamera.ViewProjection);

        foreach ((Ref<Transform2D> t, Ref<SpriteRenderer> sprite)
            in world.Query<Transform2D, SpriteRenderer>()
                .Enumerate<Transform2D, SpriteRenderer>())
        {
            Renderer2D.Draw(
                sprite.Value.Sprite,
                t.Value,
                sprite.Value.Tint
            );
        }

        Renderer2D.End();
    }
}
