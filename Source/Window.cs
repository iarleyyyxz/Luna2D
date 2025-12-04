using System;
using SDL2;
using Luna.IO;
using Luna.Util;
using OpenTK.Graphics.OpenGL4;
using Luna.Renderer;
using Luna.g2d;
using System.Numerics;
using Luna.g2d.Scene;

public class Window
{
    private IntPtr _window;
    private IntPtr _renderer;

    public bool IsRunning { get; private set; }

    public static int Width { get; set; }
    public static int Height { get; set; }
    public string Title { get; set; }

    private bool isFullscreen = true;

    private FrameBuffer2D framebuffer;

    // Debug object

    public Window(string title, int width, int height)
    {
        Width = width;
        Height = height;
        Title = title;

        InitSDL();
        InitOpenGL();

        LoadFonts();
        CreateFrameBuffer();
       // CreateScene();
      //  CreateUI();

        IsRunning = true;
    }

    private void InitSDL()
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            throw new Exception(SDL.SDL_GetError());

        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);

        _window = SDL.SDL_CreateWindow(
            Title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            Width,
            Height,
            SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL |
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |
            SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
        );

        IntPtr glContext = SDL.SDL_GL_CreateContext(_window);
        if (glContext == IntPtr.Zero)
            throw new Exception("GL Context Error: " + SDL.SDL_GetError());

        SDL.SDL_GL_MakeCurrent(_window, glContext);
        SDL.SDL_GL_SetSwapInterval(1);

        GL.LoadBindings(new OpenTKBindings());

        SetWindowIcon("assets/icons/LunaIcon.bmp");

        _renderer = SDL.SDL_CreateRenderer(
            _window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC
        );

        SDL.SDL_SetWindowFullscreen(_window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        SDL.SDL_StartTextInput();
    }

    private void InitOpenGL()
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    private void LoadFonts()
    {
        Font.Init(_renderer, "assets/SORA-REGULAR.ttf", 16);
    }

    private void CreateFrameBuffer()
    {
        framebuffer = new FrameBuffer2D(_renderer, 640, 360);
    }

    public void Run()
    {
        while (IsRunning)
        {
            ProcessEvents();

            // --- Render Game World no Framebuffer via OpenGL ---
            framebuffer.Bind();
            GL.Viewport(0, 0, framebuffer.Width, framebuffer.Height);

            GL.ClearColor(0.5f, 0.5f, 1.0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

           

            framebuffer.Unbind();
            framebuffer.ReadToSDLTexture(); // Copia framebuffer para SDL Texture

            // --- Render UI via SDL_Renderer ---
            SDL.SDL_SetRenderDrawColor(_renderer, 25, 25, 25, 255);
            SDL.SDL_RenderClear(_renderer);


            SDL.SDL_RenderPresent(_renderer);

            Time.Update();
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

            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN &&
                e.key.keysym.sym == SDL.SDL_Keycode.SDLK_F11)
            {
                ToggleFullscreen();
            }

            if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT &&
                e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED)
            {
                SDL.SDL_GetWindowSize(_window, out int w, out int h);
                Width = w;
                Height = h;
            }
        }
    }

    private void ToggleFullscreen()
    {
        if (!isFullscreen)
        {
            SDL.SDL_SetWindowFullscreen(_window,
                (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            isFullscreen = true;
        }
        else
        {
            SDL.SDL_SetWindowFullscreen(_window, 0);
            isFullscreen = false;
        }
    }

    public void Quit()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
        Font.Quit();
    }

    public static IntPtr LoadTexture(IntPtr renderer, string filePath)
    {
        IntPtr surface = SDL_image.IMG_Load(filePath);
        if (surface == IntPtr.Zero)
            throw new Exception("Texture load error: " + SDL.SDL_GetError());

        IntPtr tex = SDL.SDL_CreateTextureFromSurface(renderer, surface);
        SDL.SDL_FreeSurface(surface);

        if (tex == IntPtr.Zero)
            throw new Exception("Texture create error: " + SDL.SDL_GetError());

        return tex;
    }

    private void SetWindowIcon(string path)
    {
        IntPtr icon = SDL.SDL_LoadBMP(path);
        if (icon == IntPtr.Zero)
        {
            Console.WriteLine("Icon load error: " + SDL.SDL_GetError());
            return;
        }

        SDL.SDL_SetWindowIcon(_window, icon);
        SDL.SDL_FreeSurface(icon);
    }
}