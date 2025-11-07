namespace Luna.Editor
{
    public static class UIManager
    {
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static void OnResize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            foreach (var element in Elements)
                element.OnResize();
        }

        public static List<UIElement> Elements = new();
    }
}
