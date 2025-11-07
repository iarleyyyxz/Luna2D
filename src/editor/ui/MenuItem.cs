namespace Luna.Editor
{
    public struct MenuItem
    {
        public string Title;
        public Action? OnSelect;

        public MenuItem(string title, Action? onSelect = null)
        {
            Title = title;
            OnSelect = onSelect;
        }

        public void Trigger() => OnSelect?.Invoke();
    }
}
