using Luna.IO;
using SDL2;

namespace Luna.Editor
{
    public class UIButton : UIElement
    {

        public String Text { get; set; } = string.Empty;

        public Action? OnClick;
        public Action? OnHover;


        public bool IsMouseHovered = false;

        public override void Draw(IntPtr renderer)
        {

            SDL.SDL_Rect rect = new SDL.SDL_Rect { x = X, y = Y, w = Width, h = Height };
            SDL.SDL_SetRenderDrawColor(renderer, 50, 50, 50, 255);
            SDL.SDL_RenderFillRect(renderer, ref rect);

            int w, h;
            SDL.SDL_Color white = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
            IntPtr txt = Font.RenderText(Text, white, out w, out h);

            SDL.SDL_Rect textRect = new SDL.SDL_Rect
            {
                x = X + (Width / 2) - (w / 2),   // Centraliza X
                y = Y + (Height / 2) - (h / 2),  // Centraliza Y
                w = w,
                h = h
            };

            SDL.SDL_RenderCopy(renderer, txt, IntPtr.Zero, ref textRect);
            SDL.SDL_DestroyTexture(txt);

        }

        public override void Update()
        {
            // Handle button update logic here (e.g., mouse hover, click detection)
            bool isInside = Mouse.IsMouseOver(Mouse.X, Mouse.Y, X, Y, Width, Height);   

            if (isInside && !IsMouseHovered)
            {
                OnHover?.Invoke(); // chama só quando o mouse entra

            }

            IsMouseHovered = isInside;

            if (isInside && (
                Mouse.GetState(Mouse.Button.Left).Clicked ||
                Mouse.GetState(Mouse.Button.Right).Clicked
                ))
            {
                OnClick?.Invoke();
            }



        }
    }
}