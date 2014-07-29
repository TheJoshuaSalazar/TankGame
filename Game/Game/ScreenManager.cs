using System;
using System.Collections.Generic;
using System.Text;
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
    public class ScreenManager : DrawableGameComponent
    {
        StartScreen startScreen;
        MoveHintScreen hintScreen;
        PowerUpHint powerUpHintScreen;
        GameOverScreen gameOverScreen;
        public enum GameState { POWERUPHINT, MOVEHINT, START, PLAYER1, PLAYER2, END }
        public GameState currentState;

        int takeTurn = 0;
        bool enterPressed;
        bool aPressed;
        bool debugPressed;
        bool hasShown = true;
        bool gameOverShown = true;

        public ScreenManager(TankGame game)
            : base(game)
        {  }

        public override void Initialize()
        {
            startScreen = new StartScreen((TankGame)Game, "Fonts/LargeFont");
            startScreen.SetData(GameState.START);
            currentState = GameState.START;

            base.Initialize();
        }

        public void LoadContent()
        {
            startScreen.LoadContent();

            base.LoadContent();
        }

        public void Draw()
        {
            startScreen.Draw(TankGame.spriteBatch);
            //if (powerUpHintScreen != null) 
            //    powerUpHintScreen.Draw(TankGame.spriteBatch);
            //if(gameOverScreen != null)
            //    gameOverScreen.Draw(TankGame.spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (currentState == GameState.START)
            {
                gameOverShown = true;
                startScreen.SetData(GameState.START);
                currentState = GameState.START;
                //Music-------------------------------------------
                if (((TankGame)Game).soundManager.endingPlaying)
                    ((TankGame)Game).soundManager.stopEnd();
                if(!((TankGame)Game).soundManager.introPlaying)
                    ((TankGame)Game).soundManager.playIntro();
                //------------------------------------------------
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.Enter) && !enterPressed) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                if (currentState == GameState.START)
                {
                    ChangeGameState(GameState.MOVEHINT);
                    hintScreen = new MoveHintScreen((TankGame)Game);
                    ((TankGame)Game).noDebug();
                    //Music---------------------------------------------
                    if (((TankGame)Game).soundManager.introPlaying)
                        ((TankGame)Game).soundManager.stopIntro();
                    if (!((TankGame)Game).soundManager.backgroundPlaying)
                        ((TankGame)Game).soundManager.playBackground();
                    //--------------------------------------------------
                    ((TankGame)Game).Components.Add(hintScreen);
                }
            }

            if((Keyboard.GetState().IsKeyDown(Keys.Z)) && !debugPressed)
            {
                ChangeGameState(GameState.PLAYER1);
                debugPressed = true;
                ((TankGame)Game).startDebug();
            }

            if (currentState == GameState.MOVEHINT)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) ||
                    Keyboard.GetState().IsKeyDown(Keys.D) ||
                    GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X == 1 ||
                    GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X == 1)
                {
                    ((TankGame)Game).Components.Remove(hintScreen);
                    startScreen.clearScreen();
                    ChangeGameState(GameState.PLAYER1);
                    doThings();
                }
            }

            if (((TankGame)Game).gameRunning && ((TankGame)Game).currentTank.moveLimit <= 1.5 && ((TankGame)Game).turnsTaken == 0 && hasShown)
            {
                ChangeGameState(GameState.POWERUPHINT);
                powerUpHintScreen = new PowerUpHint((TankGame)Game);
                ((TankGame)Game).Components.Add(powerUpHintScreen);
                hasShown = false; 
            }

            if (currentState == GameState.POWERUPHINT)
            {
                if (((TankGame)Game).currentTank.moveLimit <= 0.5 || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    ((TankGame)Game).Components.Remove(powerUpHintScreen);
                    ChangeGameState(GameState.PLAYER1);
                    startScreen.clearScreen();
                    doThings();
                }
            }

            if (currentState == GameState.PLAYER1 || currentState == GameState.START)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) & !aPressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    doThings();
                }
            }

            if (currentState == GameState.PLAYER2)
            {
                if (GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) & !aPressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    doThings();
                }
            }

            if (currentState == GameState.END && gameOverShown)
            {
                gameOverScreen = new GameOverScreen((TankGame)Game);
                //Music ----------------------------------------
                ((TankGame)Game).Components.Add(gameOverScreen);
                if (((TankGame)Game).soundManager.backgroundPlaying)
                    ((TankGame)Game).soundManager.stopBackground();
                if (!((TankGame)Game).soundManager.endingPlaying)
                    ((TankGame)Game).soundManager.playEnd();
                //----------------------------------------------
                gameOverShown = false; 
            }

            if (currentState == GameState.END)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                {
                    ((TankGame)Game).Components.Remove(gameOverScreen);
                    gameOverScreen.stopConfetti();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                enterPressed = false;
            }
            if (GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.A))
                aPressed = false;

            base.Update(gameTime);
        }

        public void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.POWERUPHINT:
                    currentState = GameState.POWERUPHINT;
                    startScreen.SetData(GameState.POWERUPHINT);
                    break;

                case GameState.MOVEHINT:
                    currentState = GameState.MOVEHINT;
                    startScreen.SetData(GameState.MOVEHINT);
                    break;

                case GameState.START:
                    currentState = GameState.START;
                    startScreen.SetData(GameState.START);
                    break;

                case GameState.PLAYER1:
                    currentState = GameState.PLAYER1;
                    startScreen.SetData(GameState.PLAYER1);
                    break;

                case GameState.PLAYER2:
                    currentState = GameState.PLAYER2;
                    startScreen.SetData(GameState.PLAYER2);
                    break;

                case GameState.END:
                    currentState = GameState.END;
                    startScreen.SetData(GameState.END);
                    break;
            }
        }

        public void updateTime(int turnTime, int moveLimit)
        {
            startScreen.screenTurnTime = turnTime;
            startScreen.screenMoveLimit = moveLimit.ToString();
        }

        private void doThings()
        {
            if (currentState == GameState.START)
            {
                ChangeGameState(GameState.PLAYER1);
            }

            else if ((currentState == GameState.PLAYER1 ||
                currentState == GameState.PLAYER2) &&
                !((TankGame)Game).gameRunning)
            {
               if(((TankGame)Game).powerUpManager.powerList.Count == 0)
                    ((TankGame)Game).powerUpManager.addPowerUps();
                ((TankGame)Game).changeTurn();
                takeTurn++;
                startScreen.clearScreen();
            }

            else if (currentState == GameState.END)
            {
                ChangeGameState(GameState.END);
            }

            enterPressed = true;
            aPressed = true;
        }
    }
}
