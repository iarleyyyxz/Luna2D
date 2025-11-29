using System.Numerics;
using Frent;

namespace Luna.g2d
{
    public struct Camera2D
    {
        public float Zoom;
        public Vector2 ProjectionSize;    
        public bool IsMainCamera;

        public bool FollowTarget;
        public Entity Target;
        public float FollowSmoothing;    

        public Matrix4x4 View;
        public Matrix4x4 Projection;
        public Matrix4x4 InverseView;
        public Matrix4x4 InverseProjection;
        public Matrix4x4 ViewProjection;
    }
}
