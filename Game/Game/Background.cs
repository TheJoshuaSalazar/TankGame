using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    public class Background : DrawableGameComponent
    {
        TankGame game;
        public List<Layer> layers;
        public Color[] terrainTextureData;

        public Background(TankGame game)
            : base(game)
        {
            this.game = game;
        }

        protected override void LoadContent()
        {
            layers = new List<Layer>
            {
                new Layer(game.camera) { Parallax = Vector2.One },
                new Layer(game.camera) { Parallax = Vector2.One }
            };

            layers[0].Sprites.Add(new Sprite { Texture = Game.Content.Load<Texture2D>(@"Images/tempBack"), Position = new Rectangle(0, 40, 2048, 1536) });
            layers[1].Sprites.Add(new Sprite { Texture = Game.Content.Load<Texture2D>(@"Images/TempBackGround"), Position = new Rectangle(0, 40, 2048, 1536) });

            terrainTextureData = new Color[(layers[1].Sprites[0].Texture.Width) * (layers[1].Sprites[0].Texture.Height)];
            layers[1].Sprites[0].Texture.GetData(terrainTextureData);

            base.LoadContent();
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Layer layer in layers)
            {
                layer.Draw(batch);
            }
        }

    }
}
