namespace Luna.Editor
{
    public abstract class UIElement
    {

        public int X, Y, Width, Height;

        public bool Visible = true;

        public abstract void Draw(IntPtr renderer);

        public virtual void Update()
        {
        }
    }
}