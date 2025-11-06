using Luna.IO;
using SDL2;

namespace Luna.Editor
{
    public class UIButton : UIElement
    {

        public String Text;

        public Action? OnClick;
        public Action? OnHover;


        public bool IsMouseHovered = false;

        public override void Draw(IntPtr renderer)
        {

            SDL.SDL_Rect rect = new SDL.SDL_Rect { x = X, y = Y, w = Width, h = Height };
            SDL.SDL_SetRenderDrawColor(renderer, 50, 50, 50, 255);
            SDL.SDL_RenderFillRect(renderer, ref rect);

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