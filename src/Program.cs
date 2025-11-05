using System;
using Luna.Ecs;
using Luna.Preferences;
using Luna.Game;
class Program
{
    static void Main()
    {
        LGameWindow gameWindow = new LGameWindow(new GameSettings());
        gameWindow.Run();
    }
}