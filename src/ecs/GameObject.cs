using System.Collections.Generic;

namespace Luna.Ecs
{
    public class GameObject
    {
        public string Name { get; set; }
        List<Component> components = new();

        public bool IsActive { get; set; } = true;

        public GameObject(string name)
        {
            Name = name;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T c = new T { owner = this };
            components.Add(c);
            return c; // Start será chamado na Scene.Start()
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (var c in components)
                if (c is T t) return t;

            return null;
        }

        public void Start()
        {
            foreach (var c in components)
                c.Start();
        }

        public void Update(float dt)
        {
            if (!IsActive) return;
            foreach (var c in components)
                c.Update(dt);
        }

        public void OnDestroy()
        {
            foreach (var c in components)
                c.OnDestroy();
        }
    }
}
