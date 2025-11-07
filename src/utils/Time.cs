namespace Luna.Util
{
    public static class Time
    {
        private static uint last;
        public static float DeltaTime { get; private set; }

        public static void Update()
        {
            uint now = SDL2.SDL.SDL_GetTicks();
            DeltaTime = (now - last) / 1000f;
            last = now;
        }
    }
}