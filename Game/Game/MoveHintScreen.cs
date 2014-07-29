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
    class MoveHintScreen : DrawableGameComponent
    {
        string textToDraw, secondToDraw;
        public string Hint;
        public string Continue; 
        SpriteFont spriteFont, second;
        SpriteBatch spriteBatch;

        public MoveHintScreen(TankGame game)
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
            second = Game.Content.Load<SpriteFont>(@"Fonts/LargeFont");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/Contoller"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 125, Game.Window.ClientBounds.Height / 2 - 215, 250, 150), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/SpaceBar"), 
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 145, Game.Window.ClientBounds.Height / 2 - 25, 300, 75), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/leftArrow"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 + 70, Game.Window.ClientBounds.Height / 2 - 210, 50, 15), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/leftArrow"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 100, Game.Window.ClientBounds.Height / 2 - 180, 25, 15), Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/rightArrow"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 55, Game.Window.ClientBounds.Height / 2 - 180, 25, 15), Color.White);

            spriteBatch.DrawString(spriteFont, "Left stick moves the tank",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 150, Game.Window.ClientBounds.Height / 2 - 300), Color.White);

            spriteBatch.DrawString(spriteFont, "Right bumper to switch weapons",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 180, Game.Window.ClientBounds.Height / 2 - 255), Color.White);

            spriteBatch.DrawString(spriteFont, "Space or Right trigger to shoot",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 190, Game.Window.ClientBounds.Height / 2 - 65), Color.White);

            spriteBatch.DrawString(spriteFont, "Move to Continue",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 100, Game.Window.ClientBounds.Height / 2 + 65), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void clearScreen()
        {
            secondToDraw = "";
        }
    }
}
