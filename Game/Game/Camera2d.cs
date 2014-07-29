using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class Camera2d
    {
        public Vector2 Origin;
        public float Zoom { get; set; }

        Viewport viewport;

        public Camera2d(GraphicsDevice graphicsDevice)
        {
            viewport = graphicsDevice.Viewport;
            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Zoom = 1.0f;
        }

        public Matrix getViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-position * parallax, 0.0f)) *
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void lookAt(Vector2 position)
        {
            this.position = position - new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
        }

        public Vector2 position;
    }
}
