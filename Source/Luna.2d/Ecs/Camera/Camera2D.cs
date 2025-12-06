using OpenTK.Mathematics;

public struct Camera2D
{
    public Vector2 ProjectionSize;
    public float Zoom;

    public Matrix4 View;
    public Matrix4 Projection;
    public Matrix4 ViewProjection;

    public bool IsMainCamera;

    public void UpdateCamera(Vector2 position)
    {
        View = Matrix4.CreateTranslation(-position.X, -position.Y, 0);

        Projection = Matrix4.CreateOrthographic(
            ProjectionSize.X / Zoom,
            ProjectionSize.Y / Zoom,
            -1f,
            1f
        );

        ViewProjection = View * Projection;
    }
}
