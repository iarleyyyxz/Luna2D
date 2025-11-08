using Luna.IO;
using Luna.Util;
using SDL2;
using System;

namespace Luna.Editor.UI
{
    public class UIInputLabel : UIElement
    {
        public string Text = "";
        public string Placeholder = "Enter text...";
        public bool IsFocused = false;

        private bool backspaceLock = false;
        private double blinkTimer = 0.0;
        private bool cursorVisible = true;

        public override void Update()
        {
            // Foco do input
            if (Mouse.IsClickedInside(X, Y, Width, Height))
                IsFocused = true;
            else if (Mouse.IsClicked())
                IsFocused = false;

            if (IsFocused)
            {
                string typed = Keyboard.GetTextInput();
                if (!string.IsNullOrEmpty(typed))
                    Text += typed;

                HandleBackspace();
            }

            BlinkCaret();
        }

        private void HandleBackspace()
        {
            bool isPressed = Keyboard.IsDown(SDL.SDL_Keycode.SDLK_BACKSPACE);

            if (isPressed && !backspaceLock && Text.Length > 0)
            {
                Text = Text.Remove(Text.Length - 1);
                backspaceLock = true;
            }
            else if (!isPressed)
            {
                backspaceLock = false; // libera para o próximo backspace
            }
        }

        private void BlinkCaret()
        {
            blinkTimer += Time.DeltaTime;
            if (blinkTimer >= 0.45)
            {
                cursorVisible = !cursorVisible;
                blinkTimer = 0.0;
            }
        }

        public override void Draw(IntPtr renderer)
        {
            SDL.SDL_Rect rect = new SDL.SDL_Rect { x = X, y = Y, w = Width, h = Height };

            SDL.SDL_SetRenderDrawColor(renderer, 45, 45, 45, 255);
            SDL.SDL_RenderFillRect(renderer, ref rect);

            SDL.SDL_SetRenderDrawColor(renderer, (byte)(IsFocused ? 120 : 30), 120, 255, 255);
            SDL.SDL_RenderDrawRect(renderer, ref rect);

            int textW, textH;
            IntPtr texture = Font.RenderText(Text, new SDL.SDL_Color { r = 240, g = 240, b = 240, a = 255 }, out textW, out textH);
            SDL.SDL_Rect trect = new SDL.SDL_Rect { x = X + 6, y = Y + (Height / 2 - textH / 2), w = textW, h = textH };
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref trect);
            SDL.SDL_DestroyTexture(texture);

            if (IsFocused && cursorVisible)
            {
                SDL.SDL_Rect caret = new SDL.SDL_Rect { x = X + 6 + textW + 1, y = Y + 4, w = 2, h = Height - 8 };
                SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                SDL.SDL_RenderFillRect(renderer, ref caret);
            }
        }
    }
}
