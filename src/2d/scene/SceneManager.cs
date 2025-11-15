namespace Luna.g2d.Scene
{
    public class SceneManager
    {
        private static Dictionary<string, Scene> scenes = new();

        private static Scene currentScene;

        public static Scene CurrentScene => currentScene;

        public static void AddScene(Scene scene)
        {
            scenes[scene.Name] = scene;
        }

        public static void LoadScene(string name)
        {
            if (!scenes.ContainsKey(name))
            {
                Console.WriteLine($"[SceneManager] Scene '{name}' Not Found!");
                return;
            }

            currentScene?.Unload();

            currentScene = scenes[name];
            currentScene.Start();

            Console.WriteLine($"[SceneManager] Scene loaded: {name}");
        }

        public static void Update(float dt, int sw, int sh)
        {
            currentScene?.Update(dt, sw, sh);
        }

        /*public static void Render(SpriteBatch batch)
        {
            currentScene?.Render(batch);
        }*/
    }
        
}
