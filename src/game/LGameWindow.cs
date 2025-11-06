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
using OpenTK.Windowing.Desktop;

using Luna.Preferences;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.ES11;
using Luna.g2d;

namespace Luna.Game
{
    public class LGameWindow
    {

        private GameSettings settings;
        private GameWindow gameWindow;
        private Texture2D texture2D = null;

        public LGameWindow(GameSettings gmConf)
        {
            this.settings = gmConf;
            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = NativeWindowSettings.Default;

            gws.IsMultiThreaded = false;

            SetFrequency(settings.RenderFrequency, settings.UpdateFrequency);

            nws.APIVersion = Version.Parse("4.6.6");
 
            nws.Size = new Vector2i(settings.Width, settings.Height);
            nws.Title = settings.GameTitle;

            gameWindow = new GameWindow(gws, nws);

            Load();
        }


        public void SetFrequency(double render, double update)
        {
            this.settings.RenderFrequency = render;
            this.settings.UpdateFrequency = update;
        }


        public void Run()
        {
            gameWindow.Run();
        }

        public void Load()
        {
            gameWindow.Load += () =>
            {
                GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);

                // Load the scene, resources, etc.
            };

            gameWindow.RenderFrame += (FrameEventArgs args) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                texture2D.Bind();

                gameWindow.SwapBuffers();
            };

        }

        public void Update(float dt)
        {
             gameWindow.UpdateFrame += (FrameEventArgs args) =>
            {
                // Loop the scenes, game, scripts, etc.
            };
            
        }
        
        public void OnDestroy()
        {
            
        }
    }
}