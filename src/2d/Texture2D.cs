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
using StbImageSharp;

namespace Luna.g2d
{
    public class Texture2D : IDisposable
    {

        public int Handle { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Texture2D(string path)
        {
            Handle = GL.GenTexture();
            Bind();


            using (Stream stream = File.OpenRead(path))
            {
                var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                Width = image.Width;
                Height = image.Height;

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    image.Data
                );
            }

            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Unbind();
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}