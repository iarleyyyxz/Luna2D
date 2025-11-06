using System;
using Luna.Ecs;
using Luna.Preferences;
using Luna.Game;
using SDL2;
using Luna;
class Program
{
    static void Main()
    {
        // LGameWindow gameWindow = new LGameWindow(new GameSettings());
        // gameWindow.Run();
        Window window = new Window("Luna Game Engine", 800, 600);
        window.Run();
    }
}