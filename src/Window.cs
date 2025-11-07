using System;
using Luna.Editor;
using Luna.IO;
using SDL2;

namespace Luna
{
    public class Window
    {
        private IntPtr _window;
    private IntPtr _renderer;
    public bool IsRunning { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public string Title { get; private set; }

    public Window(string title, int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Title = title;

        // Inicializa o SDL (vídeo)
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            throw new Exception($"Error on init SDL: {SDL.SDL_GetError()}");
        }

        _window = SDL.SDL_CreateWindow(
            title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            width,
            height,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );

        if (_window == IntPtr.Zero)
        {
            throw new Exception($"Error on create Window: {SDL.SDL_GetError()}");
        }

        _renderer = SDL.SDL_CreateRenderer(
            _window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC
        );

        if (_renderer == IntPtr.Zero)
        {
            throw new Exception($"Error renderer: {SDL.SDL_GetError()}");
        }

        IsRunning = true;
    }


    public void Run()
    {
        while (IsRunning)
        {
            ProcessEvents();

            // Limpa a tela (cor preta)
            SDL.SDL_SetRenderDrawColor(_renderer, 0, 1, 2, 255);
            SDL.SDL_RenderClear(_renderer);

                // Aqui você desenha coisas...
                UIButton button = new UIButton
                {
                    X =  100,
                    Y = 100,
                    Width = 200,
                    Height = 100,
                    Text = "Click Me",
                };

            
                button.Draw(_renderer);

                
            SDL.SDL_RenderPresent(_renderer);
        }

        Quit();
    }

    private void ProcessEvents()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            Keyboard.ProcessEvent(e);
            Mouse.ProcessEvent(e);
            
            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                IsRunning = false;
        }
    }

    public void Quit()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
    }
}