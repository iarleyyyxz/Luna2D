using SDL2;

namespace Luna.IO
{

    /// <summary>
    /// Mouse input helper for SDL2-based engine.
    /// Usage:
    ///  - Call Mouse.ProcessEvent(e) for each SDL event in your event loop (to capture wheel & event-based info).
    ///  - Call Mouse.Update() once per frame (to update per-frame button states & deltas).
    /// </summary>
    public static class Mouse
    {
        // public types
        public enum Button { Left = 1, Middle = 2, Right = 3, X1 = 4, X2 = 5 }

        public struct ButtonState
        {
            public bool Pressed;      // true in frame when button went down
            public bool Released;     // true in frame when button went up
            public bool Held;         // true while button is held
            public bool Clicked;      // shorthand for Pressed (or Pressed+Released based on logic)
            public bool DoubleClicked;
            public bool Dragging;     // true when dragging with this button
            public int DragStartX;    // screen coords where drag started
            public int DragStartY;
        }

        // events
        public static event Action<int, int>? OnMove; // x,y
        public static event Action<Button>? OnButtonDown;
        public static event Action<Button>? OnButtonUp;
        public static event Action<Button>? OnClick;
        public static event Action<Button>? OnDoubleClick;
        public static event Action<Button, int, int>? OnDragStart; // button, startX, startY
        public static event Action<Button, int, int, int, int>? OnDrag; // button, startX, startY, curX, curY
        public static event Action<Button, int, int, int, int>? OnDragEnd; // button, startX, startY, endX, endY
        public static event Action<int, int>? OnWheel; // x,y (SDL wheel x/y, typically y for vertical)

        // properties (read-only from outside)
        public static int X { get; private set; }
        public static int Y { get; private set; }
        public static int DeltaX { get; private set; }
        public static int DeltaY { get; private set; }

        // wheel values accumulated during the frame; reset on Update()
        public static int WheelX { get; private set; }
        public static int WheelY { get; private set; }

        // button states map (Button -> ButtonState)
        private static readonly Dictionary<Button, ButtonState> _states = new()
        {
            { Button.Left, new ButtonState() },
            { Button.Middle, new ButtonState() },
            { Button.Right, new ButtonState() },
            { Button.X1, new ButtonState() },
            { Button.X2, new ButtonState() }
        };

        // internal trackers
        private static readonly Dictionary<Button, bool> _wasHeld = new()
        {
            { Button.Left, false },
            { Button.Middle, false },
            { Button.Right, false },
            { Button.X1, false },
            { Button.X2, false }
        };

        private static readonly Dictionary<Button, long> _lastClickTime = new()
        {
            { Button.Left, 0 },
            { Button.Middle, 0 },
            { Button.Right, 0 },
            { Button.X1, 0 },
            { Button.X2, 0 }
        };

        private static readonly Dictionary<Button, (int X, int Y)> _dragStartPos = new()
        {
            { Button.Left, (0,0) },
            { Button.Middle, (0,0) },
            { Button.Right, (0,0) },
            { Button.X1, (0,0) },
            { Button.X2, (0,0) }
        };

        // configuration
        /// <summary>Max millis between clicks to count as double-click (default 400ms)</summary>
        public static int DoubleClickTimeMs { get; set; } = 400;

        /// <summary>Minimum movement (pixels) while holding before starting a drag (default 6px)</summary>
        public static int DragStartThreshold { get; set; } = 6;

        // helper: get current ticks (ms)
        private static long NowMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// Call once per SDL event from your main loop so mouse wheel events are captured.
        /// Example:
        ///    while(SDL.SDL_PollEvent(out var e) == 1) { Mouse.ProcessEvent(e); ... }
        /// </summary>
        public static void ProcessEvent(SDL.SDL_Event e)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                    // e.wheel.x / e.wheel.y
                    WheelX += e.wheel.x;
                    WheelY += e.wheel.y;
                    OnWheel?.Invoke(e.wheel.x, e.wheel.y);
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    {
                        var b = MapSdlButtonToEnum(e.button.button);
                        if (b.HasValue)
                        {
                            // mark pressed immediately; Update will reconcile Held/Pressed
                            var btn = b.Value;
                            var s = _states[btn];
                            s.Pressed = true;
                            s.Held = true;
                            s.Released = false;
                            s.Clicked = true;
                            _states[btn] = s;

                            OnButtonDown?.Invoke(btn);

                            // record time for double click detection
                            long now = NowMs();
                            if (now - _lastClickTime[btn] <= DoubleClickTimeMs)
                            {
                                s.DoubleClicked = true;
                                _states[btn] = s;
                                OnDoubleClick?.Invoke(btn);
                                // reset last click so triple-click doesn't trigger
                                _lastClickTime[btn] = 0;
                            }
                            else
                            {
                                s.DoubleClicked = false;
                                _states[btn] = s;
                                _lastClickTime[btn] = now;
                            }

                            // record potential drag start pos (actual drag begins when pointer moves beyond threshold)
                            _dragStartPos[btn] = (X, Y);
                        }
                    }
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    {
                        var b = MapSdlButtonToEnum(e.button.button);
                        if (b.HasValue)
                        {
                            var btn = b.Value;
                            var s = _states[btn];
                            s.Released = true;
                            s.Held = false;
                            // Click event: we consider Click happened on button up (common UI behavior)
                            // but we kept Clicked true on DOWN as well; dispatch click here for consistency.
                            OnButtonUp?.Invoke(btn);

                            // If we are dragging, end drag
                            if (s.Dragging)
                            {
                                s.Dragging = false;
                                var start = _dragStartPos[btn];
                                OnDragEnd?.Invoke(btn, start.X, start.Y, X, Y);
                            }
                            else
                            {
                                // If Released and small movement -> Click
                                OnClick?.Invoke(btn);
                            }

                            // reset Pressed/Clicked for next frame
                            s.Pressed = false;
                            s.Clicked = false;
                            s.DoubleClicked = s.DoubleClicked; // keep for reference this frame
                            _states[btn] = s;
                        }
                    }
                    break;

                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    {
                        // update X/Y and delta immediately so other systems reading events get current pos
                        int mx = e.motion.x;
                        int my = e.motion.y;
                        // update delta based on last known X,Y
                        DeltaX = mx - X;
                        DeltaY = my - Y;
                        X = mx;
                        Y = my;

                        OnMove?.Invoke(X, Y);

                        // handle potential start of drag for held buttons
                        foreach (var kv in new List<Button>(_states.Keys))
                        {
                            var s = _states[kv];
                            if (s.Held && !s.Dragging)
                            {
                                var start = _dragStartPos[kv];
                                int dx = Math.Abs(X - start.X);
                                int dy = Math.Abs(Y - start.Y);
                                if (dx >= DragStartThreshold || dy >= DragStartThreshold)
                                {
                                    s.Dragging = true;
                                    _states[kv] = s;
                                    OnDragStart?.Invoke(kv, start.X, start.Y);
                                }
                            }

                            // if currently dragging, emit OnDrag
                            if (s.Dragging)
                                OnDrag?.Invoke(kv, _dragStartPos[kv].X, _dragStartPos[kv].Y, X, Y);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Call once per frame (after processing SDL events). This updates Held/Pressed/Released toggles,
        /// clears per-frame flags like Wheel, Released, Pressed, DoubleClicked (as appropriate).
        /// </summary>
        public static void Update()
        {
            // Reset deltas unless motion event updated them earlier this frame
            // but keep them for one frame - user can read DeltaX/DeltaY then they will be reset.
            // We'll reset here so next frame starts at zero.
            DeltaX = 0;
            DeltaY = 0;

            // Update absolute mouse position and button mask
            uint stateMask = SDL.SDL_GetMouseState(out int mx, out int my);
            // If ProcessEvent handled motion this frame, mx/my may be same - that's fine
            X = mx;
            Y = my;

            // For each tracked button, compute Held based on mask; set Pressed/Released appropriately
            foreach (var b in new List<Button>(_states.Keys))
            {
                bool isHeldNow = (stateMask & SDL.SDL_BUTTON((uint)b)) != 0;
                var s = _states[b];

                // Pressed set true in ProcessEvent (DOWN) or here if we missed event
                if (isHeldNow && !_wasHeld[b])
                {
                    s.Pressed = true;
                    s.Held = true;
                    OnButtonDown?.Invoke(b);
                }
                else
                {
                    s.Pressed = false; // only true in the frame of transition
                }

                if (!isHeldNow && _wasHeld[b])
                {
                    s.Released = true;
                    s.Held = false;
                    OnButtonUp?.Invoke(b);

                    // If it was dragging, end drag
                    if (s.Dragging)
                    {
                        s.Dragging = false;
                        var start = _dragStartPos[b];
                        OnDragEnd?.Invoke(b, start.X, start.Y, X, Y);
                    }
                    else
                    {
                        // if not dragging, raise click
                        OnClick?.Invoke(b);
                    }
                }
                else
                {
                    s.Released = false; // only true the frame of release
                }

                s.Held = isHeldNow;
                _states[b] = s;
                _wasHeld[b] = isHeldNow;
            }

            // clear wheel after a frame (so consumers must read it during the same frame)
            WheelX = 0;
            WheelY = 0;

            // clear per-frame 'Pressed/Released/Clicked/DoubleClicked' if desired:
            // Keep DoubleClicked true only the frame it occurred. We'll clear it here.
            foreach (var k in new List<Button>(_states.Keys))
            {
                var s = _states[k];
                s.DoubleClicked = false;
                s.Pressed = false;
                s.Released = false;
                s.Clicked = false;
                _states[k] = s;
            }
        }

        /// <summary>Returns the current ButtonState snapshot for given button.</summary>
        public static ButtonState GetState(Button button) => _states[button];

        /// <summary>Helper: whether specified button is currently held down.</summary>
        public static bool IsHeld(Button b) => _states[b].Held;

        /// <summary>Helper: whether this button started pressed this frame.</summary>
        public static bool IsPressed(Button b) => _states[b].Pressed;

        /// <summary>Helper: whether this button was released this frame.</summary>
        public static bool IsReleased(Button b) => _states[b].Released;

        /// <summary>Helper: whether this button is currently dragging.</summary>
        public static bool IsDragging(Button b) => _states[b].Dragging;

        /// <summary>Helper: true if the mouse is over on position (mx,my) within given rectangle.</summary>
        public static bool IsMouseOver(int mx, int my, int x, int y, int width, int height)
        {
            return mx >= x && mx < x + width && my >= y && my < y + height;
        }

        public static bool IsClickedInside(int x, int y, int width, int height)
        {
            foreach (Button b in Enum.GetValues(typeof(Button)))
            {
                var state = GetState(b);
                if (state.Clicked && IsMouseOver(X, Y, x, y, width, height))
                    return true;
            }
            return false;
        }

        public static bool IsClicked()
        {
            foreach (Button b in Enum.GetValues(typeof(Button)))
            {
                var state = GetState(b);
                if (state.Clicked)
                    return true;
            }
            return false;
        }

        // internal helper: maps SDL button numeric to our enum
        private static Button? MapSdlButtonToEnum(uint sdlButton)
        {
            return sdlButton switch
            {
                SDL.SDL_BUTTON_LEFT => Button.Left,
                SDL.SDL_BUTTON_MIDDLE => Button.Middle,
                SDL.SDL_BUTTON_RIGHT => Button.Right,
                SDL.SDL_BUTTON_X1 => Button.X1,
                SDL.SDL_BUTTON_X2 => Button.X2,
                _ => null
            };


        }
    }
}