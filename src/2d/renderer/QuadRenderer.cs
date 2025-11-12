using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class GLQuadRenderer
{
    int vao, vbo, shader;
    int uColor, uMVP;

    public GLQuadRenderer()
    {
        float[] vertices = {
            // X,   Y
            0f, 0f,
            1f, 0f,
            1f, 1f,
            0f, 1f
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 4, vertices, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * 4, 0);

        GL.BindVertexArray(0);

        string vs = @"
        #version 330 core
        layout(location = 0) in vec2 aPos;
        uniform mat4 uMVP;
        void main() { gl_Position = uMVP * vec4(aPos, 0.0, 1.0); }
        ";

        string fs = @"
        #version 330 core
        uniform vec4 uColor;
        out vec4 FragColor;
        void main() { FragColor = uColor; }
        ";

        shader = Compile(vs, fs);

        uColor = GL.GetUniformLocation(shader, "uColor");
        uMVP = GL.GetUniformLocation(shader, "uMVP");
    }

    int Compile(string vs, string fs)
    {
        int v = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(v, vs);
        GL.CompileShader(v);

        int f = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(f, fs);
        GL.CompileShader(f);

        int p = GL.CreateProgram();
        GL.AttachShader(p, v);
        GL.AttachShader(p, f);
        GL.LinkProgram(p);

        GL.DeleteShader(v);
        GL.DeleteShader(f);

        return p;
    }

    public void DrawQuad(float x, float y, float w, float h, float r, float g, float b, float a, int sw, int sh)
    {
        GL.UseProgram(shader);

        // Matriz ortho correta 2D (0,0 top-left igual SDL)
        Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(0, sw, sh, 0, -1, 1);
        Matrix4 model = Matrix4.CreateScale(w, h, 1) * Matrix4.CreateTranslation(x, y, 0);
        Matrix4 mvp = model * ortho;

        GL.UniformMatrix4(uMVP, false, ref mvp);
        GL.Uniform4(uColor, r, g, b, a);

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4); // ✅ CORE COMPATÍVEL
        GL.BindVertexArray(0);
    }
}
