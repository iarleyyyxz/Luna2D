using Luna.Ecs;
using Luna.g2d.Renderer;

namespace Luna.g2d.Scene
{
    public class Scene
    {
        public string Name { get; private set; }
        public List<GameObject> gameObjects = new();

        public Scene(string scName)
        {
            Name = scName;
        }

        public void AddGameObject(GameObject go)
        {
            gameObjects.Add(go);
        }

        public void RemoveGameObject(GameObject go)
        {
            gameObjects.Remove(go);
        }

        public void Start()
        {
            foreach (var go in gameObjects)
                go.Start();
        }

        public void Update(float dt, int screenW, int screenH)
        {
            Renderer2D.Begin();

            foreach (var go in gameObjects) 
            {
                go.Update(dt);
                go.GetComponent<SpriteRenderer>().Update(dt);
            }
                

            Renderer2D.End(screenW, screenH);
        }

        public void Unload()
        {
            foreach (var go in gameObjects)
                go.OnDestroy();

            gameObjects.Clear();
        }
    }
}
