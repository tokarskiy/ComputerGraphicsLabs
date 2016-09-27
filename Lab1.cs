using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenGL_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var figuresList = new List<IRenderable>
            {
                new Triangle(0.0f, 5.0f, 1.0f, 6.0f, 1.0f, 5.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(1.0f, 5.0f, 1.0f, 3.0f, 3.0f, 3.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(0.0f, 5.0f, 1.0f, 6.0f, 1.0f, 5.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(0.5f, 3.0f, 1.0f, 3.5f, 1.0f, 2.5f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(2.0f, 3.0f, 4.0f, 3.0f, 4.0f, 1.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(0.0f, 5.0f, 1.0f, 6.0f, 1.0f, 5.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Triangle(1.0f, 0.0f, 3.0f, 2.0f, 5.0f, 0.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Rectangle(1.0f, 2.0f, 2.0f, 3.0f, 3.0f, 2.0f, 2.0f, 1.0f) { Color = Color4.Yellow, BorderColor = Color4.Black },
                new Rectangle(4.0f, 1.0f, 6.0f, 1.0f, 7.0f, 0.0f, 5.0f, 0.0f) { Color = Color4.Yellow, BorderColor = Color4.Black }
            };

            using (var scene = new Scene(figuresList))
            {
                scene.BackgroundColor = new Color4(163, 73, 164, 0);
                scene.PlusKeyOffset = new Vector2 { X = 1.0f, Y = 0.5f } / 10;
                scene.MinusKeyOffset = new Vector2 { X = -1.0f, Y = -0.5f } / 10;
                GL.LineWidth(2);
                scene.Run();
            }
            
        }
    }
    
    
    public class Scene : GameWindow
    {
        private Vector2 offset;
        private List<IRenderable> figuresList;
        public Vector2 PlusKeyOffset { get; set; }
        public Vector2 MinusKeyOffset { get; set; }
        public Color4 BackgroundColor { get; set; }

        public Scene(List<IRenderable> figuresList)
            : base(600, 600, GraphicsMode.Default, "Lab1")
        {
            offset = new Vector2 { X = 0, Y = 0 };
            this.figuresList = figuresList;
            PlusKeyOffset = new Vector2 { X = 0, Y = 0 };
            MinusKeyOffset = new Vector2 { X = 0, Y = 0 };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
            GL.Ortho(-10, 10, -10, 10, -1, 1);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[Key.Up])
            {
                offset.Y += 0.1f;
            }
            if (Keyboard[Key.Down])
            {
                offset.Y -= 0.1f;
            }
            if (Keyboard[Key.Left])
            {
                offset.X -= 0.1f;
            }
            if (Keyboard[Key.Right])
            {
                offset.X += 0.1f;
            }
            if (Keyboard[Key.Plus] || Keyboard[Key.KeypadPlus])
            {
                offset += PlusKeyOffset;
            }
            if (Keyboard[Key.Minus] || Keyboard[Key.KeypadMinus])
            {
                offset += MinusKeyOffset;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            figuresList.ForEach(x => x.Render(offset));
            SwapBuffers();
        }
    }
    
    
    public interface IRenderable
    {
        void Render();
        void Render(Vector2 offset);
    }
    
    
    public class Rectangle : IRenderable
    {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly Vector2 c;
        private readonly Vector2 d;
        public Color4 Color { get; set; }
        public Color4 BorderColor { get; set; }
        public System.Boolean HasBorder { get; set; }
        public System.Boolean OnlyBorder { get; set; }

        public Rectangle(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            a = new Vector2 { X = x1, Y = y1 };
            b = new Vector2 { X = x2, Y = y2 };
            c = new Vector2 { X = x3, Y = y3 };
            d = new Vector2 { X = x4, Y = y4 };
            HasBorder = true;
            OnlyBorder = false;
        }

        public void Render()
        {
            if (!OnlyBorder)
            {
                GL.Color4(Color.R, Color.G, Color.B, Color.A);
                GL.Begin(PrimitiveType.Polygon);
                {
                    GL.Vertex2(a);
                    GL.Vertex2(b);
                    GL.Vertex2(c);
                    GL.Vertex2(d);
                }
                GL.End();
            }

            if (HasBorder)
            {
                GL.Color4(BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(a);
                    GL.Vertex2(b);
                    GL.Vertex2(c);
                    GL.Vertex2(d);
                }
                GL.End();
            }
        }

        public void Render(Vector2 offset)
        {
            if (!OnlyBorder)
            {
                GL.Color4(Color.R, Color.G, Color.B, Color.A);
                GL.Begin(PrimitiveType.Polygon);
                {
                    GL.Vertex2(a + offset);
                    GL.Vertex2(b + offset);
                    GL.Vertex2(c + offset);
                    GL.Vertex2(d + offset);
                }
                GL.End();
            }

            if (HasBorder)
            {
                GL.Color4(BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(a + offset);
                    GL.Vertex2(b + offset);
                    GL.Vertex2(c + offset);
                    GL.Vertex2(d + offset);
                }
                GL.End();
            }
        }
    }
    
    
    public class Triangle : IRenderable
    {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly Vector2 c;
        public Color4 Color { get; set; }
        public Color4 BorderColor { get; set; }
        public System.Boolean HasBorder { get; set; }
        public System.Boolean OnlyBorder { get; set; }

        public Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            a = new Vector2 { X = x1, Y = y1 };
            b = new Vector2 { X = x2, Y = y2 };
            c = new Vector2 { X = x3, Y = y3 };
            HasBorder = true;
            OnlyBorder = false;
        }

        public void Render()
        {
            if (!OnlyBorder)
            {
                GL.Color4(Color.R, Color.G, Color.B, Color.A);
                GL.Begin(PrimitiveType.Triangles);
                {
                    GL.Vertex2(a);
                    GL.Vertex2(b);
                    GL.Vertex2(c);
                }
                GL.End();
            }

            if (HasBorder)
            {
                GL.Color4(BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(a);
                    GL.Vertex2(b);
                    GL.Vertex2(c);
                }
                GL.End();
            }
        }

        public void Render(Vector2 offset)
        {
            if (!OnlyBorder)
            {
                GL.Color4(Color.R, Color.G, Color.B, Color.A);
                GL.Begin(PrimitiveType.Triangles);
                {
                    GL.Vertex2(a + offset);
                    GL.Vertex2(b + offset);
                    GL.Vertex2(c + offset);
                }
                GL.End();
            }

            if (HasBorder)
            {
                GL.Color4(BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(a + offset);
                    GL.Vertex2(b + offset);
                    GL.Vertex2(c + offset);
                }
                GL.End();
            }
        }
    }
}
