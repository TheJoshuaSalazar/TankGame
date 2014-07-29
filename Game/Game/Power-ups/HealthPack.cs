using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Game
{
    public class HealthPack : Microsoft.Xna.Framework.GameComponent
    {
        Texture2D healthPackImage;
        Rectangle healthPackRectangle;
        SpriteBatch batcher;

        public HealthPack(TankGame game, SpriteBatch batch)
            : base(game)
        {
            batcher = batch;
            healthPackImage = Game.Content.Load<Texture2D>(@"Images/HealthPackTexture");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void drawHealth(Vector2 position)
        {
            healthPackRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            batcher.Draw(healthPackImage, healthPackRectangle, Color.White);
        }

        public void removeHealth()
        {
            batcher.Dispose();
        }
    }
}
