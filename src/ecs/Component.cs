using System;

namespace Luna.Ecs
{
    public abstract class Component
    {

        public GameObject? owner;

        public virtual void Start() { }
        public virtual void Update(float dt) { }
        public virtual void OnDestroy() { }
    }
}