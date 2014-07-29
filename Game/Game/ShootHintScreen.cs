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
    class ShootHintScreen : DrawableGameComponent
    {
        string textToDraw, secondToDraw;
        public string Hint;
        public string Continue;
        SpriteFont spriteFont, second;
        SpriteBatch spriteBatch;

        public ShootHintScreen(TankGame game)
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
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts/font");
            second = Game.Content.Load<SpriteFont>(@"Fonts/font");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/spaceBar"), new Rectangle(200, 200, 400, 400), Color.White);

            spriteBatch.DrawString(spriteFont, "Press and Hold the space bar to shoot a bullet",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 175, Game.Window.ClientBounds.Height / 2 - 275), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void clearScreen()
        {
            secondToDraw = "";
        }
    }
}
