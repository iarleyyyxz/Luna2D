using System;
using SDL2;
using Luna.IO;
using Luna.Editor;
using Luna.Util;

public class Window
{
    private IntPtr _window;
    private IntPtr _renderer;
    public bool IsRunning { get; private set; }

    private UIButton button;
    private Menubar menubar; // ← agora é global e não recriad

    private Viewport viewport;

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

        Font.Init(_renderer, "assets/SORA-REGULAR.ttf", 16);

        // ✅ Criamos UI apenas 1 vez
        viewport = new Viewport

        {
            X = 250,
            Y = 40,
            Width = 400,
            Height = 320,
            Title = "Main Viewport"
        };

        button = new UIButton
        {
            X = 100,
            Y = 100,
            Width = 200,
            Height = 100,
            Text = "Click Me",
            OnClick = () => Console.WriteLine("Botão clicado!")
        };



        menubar = new Menubar { X = 0, Y = 0, Width = width, Height = 24 };

        var file = new Menu { Title = "File" };
        file.AddMenuItem(new MenuItem("New", () => Console.WriteLine("New clicked")));
        file.AddMenuItem(new MenuItem("Open"));
        file.AddMenuItem(new MenuItem("Exit", () => IsRunning = false));

        var edit = new Menu { Title = "Edit" };
        edit.AddMenuItem(new MenuItem("Undo"));
        edit.AddMenuItem(new MenuItem("Redo"));

        menubar.AddMenu(file);
        menubar.AddMenu(edit);


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
            viewport.Update();
            button.Update();
            menubar.Update();


            // ✅ Desenha UI
            viewport.Draw(_renderer);
            menubar.Draw(_renderer);
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
