using System;

namespace Luna.Ecs
{
    public class GameObject
    {
        public string Name { get; set; }
        List<Component> components = new List<Component>();

        public bool IsActive { get; set; } = true;

        public GameObject(string name)
        {
            this.Name = name;

            
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T c = new T { owner = this };
            components.Add(c);
            c.Start();
            return c;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (var c in components) if (c is T) return (T)c;
            return null;
        }

        public void Start() => components.ForEach(c => c.Start());
        public void Update(float dt) => components.ForEach(c => c.Update(dt));
        public void OnDestroy() => components.ForEach(c => c.OnDestroy());
    
    }
}