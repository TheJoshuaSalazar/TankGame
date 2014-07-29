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
    public class StartScreen : DrawableGameComponent
    {
        string textToDraw;
        public int screenTurnTime;
        public string screenMoveLimit;
        SpriteFont spriteFont;
        SpriteFont titleFont;
        SpriteBatch spriteBatch;
        int blinkTIme;
        int currentBlinkTIme;
        bool blink;
        Texture2D startScreenPic;
        bool onStartScreen;
        string title;
        Random ran;

        List<string> titleNames = new List<string>();

        public StartScreen(TankGame game, String type)
            : base(game)
        {
            blink = true;
            spriteFont = Game.Content.Load<SpriteFont>(@type);
            addToTitleList();
        }

        public override void Initialize()
        {
            screenTurnTime = 9;
            screenMoveLimit = " ";
            base.Initialize();
        }

        public void Update(int elapsedMs)
        {
            blinkTIme = 500;
            currentBlinkTIme += elapsedMs;

            if (screenTurnTime >= 9)
                blink = true;

            if (currentBlinkTIme > blinkTIme && screenTurnTime <= 3)
            {
                blink = !blink;
                currentBlinkTIme -= blinkTIme;
            }
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            startScreenPic = Game.Content.Load<Texture2D>(@"Images\StartScreen");
            titleFont = Game.Content.Load<SpriteFont>(@"Fonts\title");
            
            randomizeTitle();

            base.LoadContent();
        }

        public void Draw(SpriteBatch batch)
        {
            if(blink)
            batch.DrawString(spriteFont, "Time Remaining: " + screenTurnTime,
                new Vector2(((TankGame)Game).camera.position.X + 400,
                    ((TankGame)Game).camera.position.Y + 585),
                Color.White);

            batch.DrawString(spriteFont, "Move Limit: " + screenMoveLimit,
                new Vector2(((TankGame)Game).camera.position.X + 645,
                    ((TankGame)Game).camera.position.Y + 585),
                Color.White);

            if (onStartScreen)
            {
                batch.Draw(startScreenPic, new Vector2(((TankGame)Game).camera.position.X, ((TankGame)Game).camera.position.Y), Color.White);
                batch.DrawString(titleFont, title, new Vector2(((TankGame)Game).camera.position.X + 180, ((TankGame)Game).camera.position.Y + 652), Color.Black);
                batch.DrawString(titleFont, title, new Vector2(((TankGame)Game).camera.position.X + 184, ((TankGame)Game).camera.position.Y + 656), Color.Red);
            }

            batch.DrawString(spriteFont,
                textToDraw,
                new Vector2(((TankGame)Game).camera.position.X + 420, 
                            Game.Window.ClientBounds.Height),
                    Color.Red);

        }

        public void SetData(ScreenManager.GameState GameState)
        {

            switch (GameState)
            {
                case ScreenManager.GameState.MOVEHINT:
                    textToDraw = "";
                    onStartScreen = false;     
                    break;

                case ScreenManager.GameState.POWERUPHINT:
                    textToDraw = "";
                    break;

                case ScreenManager.GameState.START:
                    textToDraw = "Press ENTER or (A) to begin\nPress Z to enter Debug";
                    onStartScreen = true;
                    break;

                case ScreenManager.GameState.PLAYER1:
                    textToDraw = "Player 1 press ENTER or (A) to begin";
                    onStartScreen = false;
                    break;

                case ScreenManager.GameState.PLAYER2:
                    textToDraw = "Player 2 press ENTER or (A) to begin";
                    onStartScreen = false;

                    break;

                case ScreenManager.GameState.END:
                    textToDraw = "";

                    break;
            }
        }

        public void clearScreen()
        {
            textToDraw = "";
        }

        public void addToTitleList()
        {
            titleNames.Add("TAAAAAAAAAANK!!!!!");
            titleNames.Add("Screen of Tanks");
            titleNames.Add("Can't tank this");
            titleNames.Add("Game of Tanks");
            titleNames.Add("TonyDazzleTank");
            titleNames.Add("Art of Tanks");
            titleNames.Add("Tank me to hell");
            titleNames.Add("Lord of the Tanks");
            titleNames.Add("Jank Tanks");
            titleNames.Add("Way of the Tank");
            titleNames.Add("Tanks and Ammo");
            titleNames.Add("Tankception");
            titleNames.Add("Girls und Tanks");
            titleNames.Add("Don't tank me bro");
            titleNames.Add("Pimp my tank");
            titleNames.Add("Tank up or shut up");
            titleNames.Add("Tanklander");
            titleNames.Add("Tank club");
            titleNames.Add("Men in Tanks II");
            titleNames.Add("Grand theft tanks");
            titleNames.Add("TankCraft");
            titleNames.Add("Dawn of the tanks");
            titleNames.Add("Tank hard");
            titleNames.Add("Kingdom Tanks");
            titleNames.Add("Fullmetal Tank");
            titleNames.Add("Call of Tank");
            titleNames.Add("Tankzone");
            titleNames.Add("Tank Assault");
            titleNames.Add("Battletank 3");
            titleNames.Add("Tank Killer");
            titleNames.Add("Rise of tanks");
            titleNames.Add("Tank Force");
            titleNames.Add("Metal Tank Solid");
            titleNames.Add("Tank Gaiden");
            titleNames.Add("Bro, do you even tank");
            titleNames.Add("Like a tank");
            titleNames.Add("Nyantank");
            titleNames.Add("2-D Tanks");
            titleNames.Add("Tanks, but no tanks");
            titleNames.Add("Tank Tank Revolution");
            titleNames.Add("Untankable");
            titleNames.Add("Shingeki no Tank");
            titleNames.Add("Planet of the Tanks");
            titleNames.Add("Tank you very much");
            titleNames.Add("God of Tanks");
            titleNames.Add("Tank's Creed");
            titleNames.Add("Dat Tank");
            titleNames.Add("Tank Raider");
            titleNames.Add("T.A.N.K");
            titleNames.Add("Tankicide");
            titleNames.Add("Iron Tank");
            titleNames.Add("We will tank you");
            titleNames.Add("Y U NO TANK");
            titleNames.Add("You only tank once");
        }

        public void randomizeTitle()
        {
            ran = new Random();
            int num = ran.Next(0, titleNames.Count);
            title = titleNames[num];
        }
    }
}
