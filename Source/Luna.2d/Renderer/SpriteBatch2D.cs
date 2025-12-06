using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Luna.Core;

namespace Luna.g2d.Renderer
{
    public class SpriteBatch2D
    {
        const int MaxSprites = 2000;
        const int VERTEX_SIZE = 10; 
        const int FLOAT_SIZE = sizeof(float);
        const int QUAD_FLOATS = VERTEX_SIZE * 4;

        private float[] vertices = new float[MaxSprites * QUAD_FLOATS];

        private int vao, vbo, ebo;
        private int spriteCount = 0;

        private List<Texture2D> textureSlots = new();
        private Shader shader;

        public SpriteBatch2D()
        {
            shader = ResourceManager.LoadShader(
                "assets/shaders/batch.vert",
                "assets/shaders/batch.frag");

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.Use();

            int texLoc = GL.GetUniformLocation(shader.Handle, "uTextures");
            if (texLoc != -1)
            {
                int[] units = new int[16];
                for (int i = 0; i < units.Length; i++)
                    units[i] = i;

                GL.Uniform1(texLoc, units.Length, units);
            }

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * FLOAT_SIZE, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            // === Index Buffer ===
            uint[] indices = new uint[MaxSprites * 6];
            uint offset = 0;
            for (int i = 0; i < indices.Length; i += 6)
            {
                indices[i] = offset + 0;
                indices[i + 1] = offset + 1;
                indices[i + 2] = offset + 2;
                indices[i + 3] = offset + 2;
                indices[i + 4] = offset + 3;
                indices[i + 5] = offset + 0;
                offset += 4;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            int stride = VERTEX_SIZE * FLOAT_SIZE;

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 2 * FLOAT_SIZE);

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, stride, 4 * FLOAT_SIZE);

            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, stride, 8 * FLOAT_SIZE);

            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 1, VertexAttribPointerType.Float, false, stride, 9 * FLOAT_SIZE);

            GL.BindVertexArray(0);
        }

        // ────────────────────────────────────────────

        public void Begin()
        {
            spriteCount = 0;
            textureSlots.Clear();
        }

        private int GetTextureSlot(Texture2D tex)
        {
            int index = textureSlots.IndexOf(tex);
            if (index != -1) return index;

            textureSlots.Add(tex);
            return textureSlots.Count - 1;
        }

        public void DrawSprite(
            Texture2D tex, Vector2 pos, Vector2 size, float rot, Vector4 color,
            Vector2 uv0, Vector2 uv1, bool flipX, bool flipY, int layer)
        {
            if (spriteCount >= MaxSprites)
                return;

            int texID = GetTextureSlot(tex);

            // === UV ===
            Vector2 uvBL = new(uv0.X, uv1.Y);
            Vector2 uvBR = new(uv1.X, uv1.Y);
            Vector2 uvTR = new(uv1.X, uv0.Y);
            Vector2 uvTL = new(uv0.X, uv0.Y);

            if (flipX) (uvBL.X, uvBR.X, uvTR.X, uvTL.X) = (uvBR.X, uvBL.X, uvTL.X, uvTR.X);
            if (flipY) (uvBL.Y, uvBR.Y, uvTR.Y, uvTL.Y) = (uvTR.Y, uvTL.Y, uvBR.Y, uvBL.Y);

            // === Model Matrix ===
            System.Numerics.Matrix4x4 model =
                System.Numerics.Matrix4x4.CreateScale(size.X, size.Y, 1) *
                System.Numerics.Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(rot)) *
                System.Numerics.Matrix4x4.CreateTranslation(pos.X, pos.Y, layer);

            System.Numerics.Vector4 p0 = System.Numerics.Vector4.Transform(new 
            System.Numerics.Vector4(-0.5f, -0.5f, 0, 1), model);
            System.Numerics.Vector4 p1 = System.Numerics.Vector4.Transform(new 
            System.Numerics.Vector4(+0.5f, -0.5f, 0, 1), model);
            System.Numerics.Vector4 p2 = System.Numerics.Vector4.Transform(new 
            System.Numerics.Vector4(+0.5f, +0.5f, 0, 1), model);
            System.Numerics.Vector4 p3 = System.Numerics.Vector4.Transform(new 
            System.Numerics.Vector4(-0.5f, +0.5f, 0, 1), model);

            int idx = spriteCount * QUAD_FLOATS;

            AddVertex(idx + VERTEX_SIZE * 0, p0, uvBL, color, texID, layer);
            AddVertex(idx + VERTEX_SIZE * 1, p1, uvBR, color, texID, layer);
            AddVertex(idx + VERTEX_SIZE * 2, p2, uvTR, color, texID, layer);
            AddVertex(idx + VERTEX_SIZE * 3, p3, uvTL, color, texID, layer);

            spriteCount++;
        }

        private void AddVertex(int index, System.Numerics.Vector4 pos, Vector2 uv, Vector4 color, int texID, int layer)
        {
            vertices[index]     = pos.X;
            vertices[index + 1] = pos.Y;

            vertices[index + 2] = uv.X;
            vertices[index + 3] = uv.Y;

            vertices[index + 4] = color.X;
            vertices[index + 5] = color.Y;
            vertices[index + 6] = color.Z;
            vertices[index + 7] = color.W;

            vertices[index + 8] = texID;
            vertices[index + 9] = layer;
        }

        // ────────────────────────────────────────────

        public void End(Matrix4 viewProjection)
        {
            Flush(viewProjection);
        }

        private void Flush(Matrix4 projection)
        {
            if (spriteCount == 0)
                return;

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero,
                spriteCount * QUAD_FLOATS * FLOAT_SIZE, vertices);

            shader.Use();
            shader.SetMatrix4("uProjection", projection);

            for (int i = 0; i < textureSlots.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                textureSlots[i].Bind();
            }

            GL.DrawElements(PrimitiveType.Triangles, spriteCount * 6,
                DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);

            spriteCount = 0;
            textureSlots.Clear();
        }
    }
}
