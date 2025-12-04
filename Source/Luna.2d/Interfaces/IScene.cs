using Frent;
using Luna.Ecs;
using Luna.g2d.Providers;

namespace Luna.g2d.Interfaces
{
    /// <summary>
    /// Defines the fundamental interface for a Scene within the game or application,
    /// managing the lifecycle (start, update, stop) and providing access
    /// to its core components, such as the <see cref="World"/> (for ECS entities),
    /// the <see cref="Camera2D"/>, and the system providers.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Called once to initialize the scene and load all its resources.
        /// This is the entry point for the scene's logic.
        /// </summary>
        void Start();

        /// <summary>
        /// Called every frame to update the logic of the scene and all its systems
        /// and entities.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last frame, in seconds (Delta Time).</param>
        void Update(float deltaTime);

        /// <summary>
        /// Called when the scene is being shut down or unloaded.
        /// It should be used to release resources and clean up the scene state.
        /// </summary>
        void Stop();

        /// <summary>
        /// Returns the instance of the <see cref="World"/>, which is the container for
        /// all entities (ECS - Entity Component System) within this scene.
        /// </summary>
        /// <returns>The scene's World instance.</returns>
        World GetWorld();

        /// <summary>
        /// Returns the instance of the <see cref="Camera2D"/> used to render
        /// the scene, controlling the view and perspective.
        /// </summary>
        /// <returns>The scene's Camera2D instance.</returns>
        Camera2D GetCamera();
        
        /// <summary>
        /// Returns the provider responsible for managing and accessing all the active systems
        /// (such as physics, rendering, logic systems) in the scene.
        /// </summary>
        /// <returns>The scene's SystemProvider instance.</returns>
        SystemProvider GetSystems();
    }
}