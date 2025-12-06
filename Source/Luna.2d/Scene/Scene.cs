using Frent;
using Luna.Ecs;
using Luna.g2d;
using Luna.g2d.Providers;
using Luna.g2d.Renderer;

namespace Luna.SceneSystem
{
    public abstract class Scene
    {
        public World World { get; private set; }
        public SystemProvider Systems { get; private set; }

        public Camera2D MainCamera { get; private set; }

        public virtual void Start()
        {
            World = new World();
            Systems = new SystemProvider();

            // SISTEMAS QUE TODA CENA PRECISA
            Systems.AddSystem(new Camera2DSystem(), World);
            Systems.AddSystem(new SpriteRenderSystem(), World);

            OnLoad(); // cria entidades
        }

        protected abstract void OnLoad(); // aqui você monta a cena

        public void Update(float dt)
        {
            Systems.Update(dt);
            World.Update();
        }

        public void Render()
        {
            Systems.Render();
        }

        public Entity CreateEntity()
        {
            return World.Create();
        }
    }
}
