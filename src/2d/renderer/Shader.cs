// -----------------------------------------------------------------------------
// Luna 2D Game Engine
// Copyright (c) 2025 <iarleyyyx> (<github/iarleyyyx>)
//
// This file is part of the Luna 2D Game Engine.
//
// Licensed under the MIT License. You may obtain a copy of the License at:
// https://opensource.org/licenses/MIT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// -----------------------------------------------------------------------------
using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Luna.g2d.Renderer
{
    public class Shader : IDisposable
    {

        public int Handle { get; }

        public Shader(string vertPath, string fragPath)
        {
            string vertSrc = File.ReadAllText(vertPath);
            string fragSrc = File.ReadAllText(fragPath);

            int vert = CompileShader(ShaderType.VertexShader, vertSrc);
            int frag = CompileShader(ShaderType.FragmentShader, fragSrc);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vert);
            GL.AttachShader(Handle, frag);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0)
                throw new Exception($"Shader link error: {GL.GetProgramInfoLog(Handle)}");

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }

        public int CompileShader(ShaderType type, string src)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
                throw new Exception($"{type} compile error: {GL.GetShaderInfoLog(shader)}");

            return shader;
        }

        public void Use() => GL.UseProgram(Handle);
        public void Dispose() => GL.DeleteProgram(Handle);

        public int GetIntLocation(string name)
        {
            return GL.GetUniformLocation(Handle, name);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int loc = GetIntLocation(name);
            if (loc == -1)
            {
                Console.WriteLine($"[Shader] Warning: uniform '{name}' not found.");
                return;
            }

            GL.UniformMatrix4(loc, false, ref matrix);
        }


        public static implicit operator int(Shader v)
        {
            throw new NotImplementedException();
        }
    }
}