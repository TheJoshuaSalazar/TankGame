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
    class PowerUpHint : DrawableGameComponent
    {
        string textToDraw, secondToDraw;
        public string Hint;
        public string Continue;
        SpriteFont spriteFont, second;
        SpriteBatch spriteBatch;

        public PowerUpHint(TankGame game)
            : base(game)
        { }

        public override void Initialize()
        {
            Hint = " ";
            Continue = " ";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts/LargeFont");
            second = Game.Content.Load<SpriteFont>(@"Fonts/font");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/AmmoPackTexture"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 285, Game.Window.ClientBounds.Height / 2 - 200, 75, 75), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/HealthPackTexture"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 120, Game.Window.ClientBounds.Height / 2 - 200, 75, 75), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/ShieldPowerUp"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 + 65, Game.Window.ClientBounds.Height / 2 - 200, 75, 75), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/MovementPowerUp"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 + 225, Game.Window.ClientBounds.Height / 2 - 200, 75, 75), Color.White);

            spriteBatch.DrawString(spriteFont, "These are the Power - Ups in the game",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 235, Game.Window.ClientBounds.Height / 2 - 250), Color.White);

            spriteBatch.DrawString(spriteFont, "Ammo",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 275, Game.Window.ClientBounds.Height / 2 - 125), Color.White);

            spriteBatch.DrawString(spriteFont, "Health Pack",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 150, Game.Window.ClientBounds.Height / 2 - 125), Color.White);

            spriteBatch.DrawString(spriteFont, "Shield",
                new Vector2(Game.Window.ClientBounds.Width / 2 + 55, Game.Window.ClientBounds.Height / 2 - 125), Color.White);

            spriteBatch.DrawString(spriteFont, "Greater Move Distance",
                new Vector2(Game.Window.ClientBounds.Width / 2 + 165, Game.Window.ClientBounds.Height / 2 - 125), Color.White);

            spriteBatch.DrawString(spriteFont, "Run them over to obtain them",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 175, Game.Window.ClientBounds.Height / 2 - 75), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void clearScreen()
        {
            secondToDraw = "";
        }
    }
}
