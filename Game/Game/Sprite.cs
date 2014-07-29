using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public struct Sprite
    {
        public Texture2D Texture;
        
        public Rectangle Position;
        public Vector2 vecPosition { get; set; }

        public void Draw(SpriteBatch spritebatch)
        {
            if (Texture != null)
                spritebatch.Draw(Texture, Position, Color.White);
        }

    }
}
