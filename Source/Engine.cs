using Luna.SceneSystem;

public static class Engine
{
    public static Scene CurrentScene;

    public static void Init()
    {
        CurrentScene = SceneLoader.LoadInitial();

        // registra sistemas
        CurrentScene.Systems.AddSystem(new Camera2DSystem(), CurrentScene.World);
        CurrentScene.Systems.AddSystem(new SpriteRenderSystem(), CurrentScene.World);
    }

    public static void Update(float dt)
    {
        CurrentScene.Update(dt);
    }

    public static void Render()
    {
        CurrentScene.Systems.Render();
    }
}
