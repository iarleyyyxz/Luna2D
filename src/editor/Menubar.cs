using SDL2;

namespace Luna.Editor
{
    public class Menubar : UIElement
    {
        public List<Menu> Menus = new();

        public void AddMenu(Menu menu) => Menus.Add(menu);

        public override void Draw(IntPtr renderer)
        {
            // Fundo da barra superior
            SDL.SDL_Rect bar = new SDL.SDL_Rect { x = X, y = Y, w = Width, h = Height };
            SDL.SDL_SetRenderDrawColor(renderer, 20, 20, 20, 255);
            SDL.SDL_RenderFillRect(renderer, ref bar);

            // Desenha cada menu
            int offsetX = 0;
            foreach (var menu in Menus)
            {
                menu.X = X + offsetX;
                menu.Y = Y;
                menu.Width = 110;
                menu.Height = Height;

                menu.Draw(renderer);
                offsetX += 110;
            }
        }

        public override void Update()
        {
            foreach (var menu in Menus)
                menu.Update();
        }

        public override void OnResize()
        {
            Width = UIManager.ScreenWidth;
        }

    }
    
}
