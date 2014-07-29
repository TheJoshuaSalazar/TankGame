using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class Layer
    {
        private readonly Camera2d camera;
        public Vector2 Parallax;
        public List<Sprite> Sprites;

        public Layer(Camera2d camera)
        {
            this.camera = camera;
            Parallax = Vector2.One;
            Sprites = new List<Sprite>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.getViewMatrix(Parallax));

            foreach (Sprite sprite in Sprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
