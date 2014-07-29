using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Game
{
    class GameOverScreen : DrawableGameComponent
    {
        TankGame game;
        string textToDraw, secondToDraw;
        public string Hint;
        public string Continue;
        SpriteFont spriteFont, second;
        SpriteBatch spriteBatch;
        Random random = new Random();

        Fireworks fireWorks;
        Confetti confetti;
        ConfettiDrop confettiDrop;

        float fireworksGenerationPeriod = 0;
        float confettiGenerationPeriod = 0;
        float confettiDropGenerationPeriod = 0;


        public GameOverScreen(TankGame game)
            : base(game)
        {
            this.game = game;
        }

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

        public override void Update(GameTime gameTime)
        {
            if (fireworksGenerationPeriod <= 0)
            {
                generateFireworks(random.Next(50, TankGame.FIELD_WIDTH - 50),
                    random.Next(300, TankGame.FIELD_HEIGHT - 700));

                fireworksGenerationPeriod = (float)random.NextDouble() / 6;
            }
            else
            {
                fireworksGenerationPeriod -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(confettiGenerationPeriod <= 0)
            {
                generateConfetti(game.currentTank.getTankPos().X + random.Next(-120, 120),
                    game.currentTank.getTankPos().Y + random.Next(-120, 120));

                confettiGenerationPeriod = .5f;
            }
            else
            {
                confettiGenerationPeriod -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (confettiDropGenerationPeriod <= 0)
            {
                generateConfettiDrop(24);

                confettiDropGenerationPeriod = 10;
            }
            else
            {
                confettiDropGenerationPeriod -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/Banner"),
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 300, Game.Window.ClientBounds.Height / 2 - 300, 600, 300), Color.White);

            spriteBatch.DrawString(spriteFont, "WINNER  WINNER  WINNER",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 150, Game.Window.ClientBounds.Height / 2 - 225), Color.Black);

            spriteBatch.DrawString(spriteFont, "Right Shift to play again",
                new Vector2(Game.Window.ClientBounds.Width / 2 - 160, Game.Window.ClientBounds.Height / 2 - 100), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void clearScreen()
        {
            secondToDraw = "";
        }

        public void generateConfetti(float x, float y)
        {
            confetti = new Confetti(game, new Vector2(x, y));
            //confetti = new Confetti(game, new Vector2(random.Next(50, TankGame.FIELD_WIDTH - 50),
            //    random.Next(100, TankGame.FIELD_HEIGHT - 400)));
            confetti.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            confetti.UpdateOrder = 100;
            confetti.DrawOrder = 100;
            confetti.Visible = true;
        }

        public void generateFireworks(float x, float y)
        {
            fireWorks = new Fireworks(game, new Vector2(x, y));
            fireWorks.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            fireWorks.UpdateOrder = 100;
            fireWorks.DrawOrder = 100;
            fireWorks.Visible = true;
        }

        public void generateConfettiDrop(int numDrops)
        {
            for (int i = 0; i < numDrops; i++)
            {
                confettiDrop = new ConfettiDrop(game, new Vector2(random.Next(50, TankGame.FIELD_WIDTH - 50), random.Next(200, 300)));
                confettiDrop.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

                confettiDrop.UpdateOrder = 100;
                confettiDrop.DrawOrder = 100;
                confettiDrop.Visible = true;
            }
        }

        public void stopConfetti()
        {
            confetti.Destroy();
            confetti.Enabled = false;
        }
    }
}
