using SDL2;
using System.Runtime.InteropServices;

namespace Luna.IO
{
    public static class Font
    {
        private static IntPtr _font = IntPtr.Zero; // <--- Aqui! É IntPtr, não TTF_Font
        private static IntPtr _renderer = IntPtr.Zero;

        public static void Init(IntPtr sdlRenderer, string fontFile, int fontSize)
        {
            _renderer = sdlRenderer;

            if (SDL_ttf.TTF_Init() != 0)
                throw new Exception("Error on initialize SDL_ttf: " + SDL.SDL_GetError());

            _font = SDL_ttf.TTF_OpenFont(fontFile, fontSize);

            if (_font == IntPtr.Zero)
                throw new Exception("Error on load font: " + SDL.SDL_GetError());
        }

        public static IntPtr RenderText(string text, SDL.SDL_Color color, out int width, out int height)
        {
            IntPtr surface = SDL_ttf.TTF_RenderUTF8_Blended(_font, text, color);

            if (surface == IntPtr.Zero)
            {
                width = height = 0;
                return IntPtr.Zero;
            }

            // ← Aqui estamos pegando os valores width/height da surface retornada
            var surf = Marshal.PtrToStructure<SDL.SDL_Surface>(surface);
            width = surf.w;
            height = surf.h;

            IntPtr texture = SDL.SDL_CreateTextureFromSurface(_renderer, surface);
            SDL.SDL_FreeSurface(surface);

            return texture;
        }
    }
}
