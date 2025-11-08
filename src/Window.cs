using System;
using SDL2;
using Luna.IO;
using Luna.Editor;
using Luna.Util;
using Luna.Editor.UI;

public class Window
{
    private IntPtr _window;
    private IntPtr _renderer;
    public bool IsRunning { get; private set; }

    private UIInputLabel uIInputLabel;

    public int Width { get; set; }
    public int Height { get; set; }
    public string Title { get; set; }

    bool isFullscreen = false;

    public Window(string title, int width, int height)
    {
        Width = width;
        Height = height;
        Title = title;

        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            throw new Exception(SDL.SDL_GetError());

        _window = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |
        SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        _renderer = SDL.SDL_CreateRenderer(_window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        //SDL.SDL_SetWindowFullscreen(_window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        SDL.SDL_SetWindowFullscreen(_window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

        SDL.SDL_StartTextInput();

        Font.Init(_renderer, "assets/SORA-REGULAR.ttf", 16);

        // ✅ Criamos UI apenas 1 vez
        uIInputLabel = new UIInputLabel
        {
            X = 20,
            Y = 50,
            Width = 200,
            Height = 24
        };
    


        IsRunning = true;
    }

    public void Run()
    {
        while (IsRunning)
        {
            ProcessEvents();

            SDL.SDL_SetRenderDrawColor(_renderer, 0, 1, 2, 255);
            SDL.SDL_RenderClear(_renderer);

            // ✅ Atualiza UI
            Time.Update();
            uIInputLabel.Update();

            uIInputLabel.Draw(_renderer);


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

            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN && e.key.keysym.sym == SDL.SDL_Keycode.SDLK_F11)
            {
                ToggleFullscreen();
            }

            if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
            {
                if (e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED)
                {
                    int w, h;
                    SDL.SDL_GetWindowSize(_window, out w, out h);
                    Width = w;
                    Height = h;
                    UIManager.OnResize(Width, Height);
                }
            }

            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    IsRunning = false;
        }

        void ToggleFullscreen()
        {
            if (!isFullscreen)
            {
                SDL.SDL_SetWindowFullscreen(_window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
                isFullscreen = true;
            }
            else
            {
                SDL.SDL_SetWindowFullscreen(_window, 0);
                isFullscreen = false;
            }

        }
    }

    public void Quit()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}
