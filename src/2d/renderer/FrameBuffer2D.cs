using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using SDL2;


namespace Luna.Renderer
{
    public class FrameBuffer2D
{
    public int FBO;
    public int ColorTex;
    public int Width, Height;
    public IntPtr SDLTexture;
    private IntPtr sdlRenderer;

    public FrameBuffer2D(IntPtr renderer, int width, int height)
    {
        sdlRenderer = renderer;
        Resize(width, height);
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        if (FBO != 0) GL.DeleteFramebuffer(FBO);
        if (ColorTex != 0) GL.DeleteTexture(ColorTex);
        if (SDLTexture != IntPtr.Zero) SDL.SDL_DestroyTexture(SDLTexture);

        // Criar textura do OpenGL
        ColorTex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, ColorTex);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        // Criar FBO e anexar textura
        FBO = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorTex, 0);

        var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        Console.WriteLine("FBO Status: " + status);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        // Criar textura SDL que vai receber pixels do OpenGL
        SDLTexture = SDL.SDL_CreateTexture(sdlRenderer,
            SDL.SDL_PIXELFORMAT_RGBA8888,
            (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            width, height);
    }

    public void Bind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
    public void Unbind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        public void ReadToSDLTexture()
        {
            byte[] pixels = new byte[Width * Height * 4];

            // copia os pixels do OpenGL
            
            GL.BindTexture(TextureTarget.Texture2D, ColorTex);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // atualiza a SDL texture
            unsafe
            {
                fixed (byte* p = pixels)
                {
                    SDL.SDL_UpdateTexture(SDLTexture, IntPtr.Zero, (IntPtr)p, Width * 4);
                }
            }
        }


    public void DrawSDL(int x, int y, int w, int h)
    {
        SDL.SDL_Rect dst = new SDL.SDL_Rect { x = x, y = y, w = w, h = h };
        SDL.SDL_RenderCopy(sdlRenderer, SDLTexture, IntPtr.Zero, ref dst);
    }
}

}
