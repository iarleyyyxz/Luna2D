using Frent; 

namespace Luna.g2d
{
    /// <summary>
    /// Defines the basic interface for a component of behavior (Behaviour)
    /// that can be attached to an entity within a <see cref="World"/>.
    /// This interface establishes the fundamental lifecycle of a component.
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// Called once when the behavior is initialized or attached to an entity.
        /// Used to set up variables and load resources.
        /// </summary>
        /// <param name="world">The instance of the world to which the entity belongs.</param>
        virtual void Init(World world) {}

        /// <summary>
        /// Called every frame to update the behavior's logic.
        /// This is where most time-dependent actions and calculations occur.
        /// </summary>
        /// <param name="dt">The time elapsed since the last frame, in seconds (Delta Time).</param>
        virtual void Update(float dt) {}

        /// <summary>
        /// Called every frame to draw or render the component.
        /// It should only contain drawing logic, separate from update logic.
        /// </summary>
        virtual void Render() {}

        /// <summary>
        /// Called when the behavior component should be disposed of and
        /// any allocated resources (e.g., textures, memory) must be released.
        /// </summary>
        void Dispose();
    }
}