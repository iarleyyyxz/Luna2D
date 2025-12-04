using Frent;
using Luna.Ecs;
using Luna.g2d.Interfaces;
using Luna.g2d.Providers;
using Luna.g2d.Renderer;

namespace Luna.g2d.Scene
{
    public class Scene : IScene
    {
        private SystemProvider systems;

        private World World2D;

        private Camera2D Camera2D;

        public void Start()
        {
            World2D = new World();
            systems = new SystemProvider();
        }

        public void Stop()
        {
            
        }

        public void Update(float deltaTime)
        {
            
        }

        public Camera2D GetCamera() => Camera2D;

        public SystemProvider GetSystems() => systems;
        public World GetWorld() => World2D;

    }
}
