using Luna.g2d;
using Luna.g2d.Renderer;

namespace Luna.Core
{

    public enum ResourceType
    {
        Texture,
        Shader,
        Sound,
        Font,
        Script,
        Sprite2D,
        Animation2D,
        Unknown
    }


    public static class ResourceManager
    {

        private static Dictionary<string, Texture2D> textures = new();
        private static Dictionary<string, Shader> shaders = new();

        /// <summary>
        /// Gets the resource type based on the file extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ResourceType GetResourceTypeFromExtension(string extension)
        {
            return extension.ToLower() switch
            {
                ".png" => ResourceType.Texture,
                ".jpg" => ResourceType.Texture,
                ".jpeg" => ResourceType.Texture,
                ".bmp" => ResourceType.Texture,
                ".shader" => ResourceType.Shader,
                ".glsl" => ResourceType.Shader,
                ".frag" => ResourceType.Shader,
                ".vert" => ResourceType.Shader,
                ".ogg" => ResourceType.Sound,
                ".wav" => ResourceType.Sound,
                ".mp3" => ResourceType.Sound,
                ".ttf" => ResourceType.Font,
                ".otf" => ResourceType.Font,
                ".luna" => ResourceType.Script,
                ".sprite2d" => ResourceType.Sprite2D,
                ".anim" => ResourceType.Animation2D,
                _ => ResourceType.Unknown,
            };
        }

        /// <summary>
        /// Loads a texture from the specified path. If the texture is already loaded, 
        /// it returns the existing texture handle.
        /// </summary>
       public static Texture2D LoadTexture(string path)
        {
            if (textures.ContainsKey(path))
                return textures[path];

            Texture2D texture = new Texture2D(path);
            textures[path] = texture;
            return texture;
        }

        /// <summary>
        /// Gets a loaded texture by its path. Returns null if the texture is not found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path)
        {
            textures.TryGetValue(path, out var tex);
            return tex;
        }

        /// <summary>
        /// Unloads a texture by its path.
        /// </summary>
        public static void UnloadTexture(string path)
        {
            if (textures.ContainsKey(path))
            {
                textures[path].Dispose(); 
                textures.Remove(path);
            }
        }

        public static Shader LoadShader(string vertPath, string fragPath)
        {
            string key = vertPath + "|" + fragPath;

            if (shaders.ContainsKey(key))
                return shaders[key];

            Shader shader = new Shader(vertPath, fragPath);
            shaders[key] = shader;

            return shader;
        }

        public static Shader GetShader(string vertPath, string fragPath)
        {
            string key = vertPath + "|" + fragPath;
            shaders.TryGetValue(key, out var shader);
            return shader;
        }

        public static void UnloadShader(string vertPath, string fragPath)
        {
            string key = vertPath + "|" + fragPath;

            if (shaders.ContainsKey(key))
            {
                shaders[key].Dispose();
                shaders.Remove(key);
            }
        }

        
        /// <summary>
        /// Unloads all loaded resources.
        /// </summary>
        public static void UnloadAll()
        {
            foreach (var tex in textures.Values)
                tex.Dispose();

            textures.Clear();
        }
    }
}
