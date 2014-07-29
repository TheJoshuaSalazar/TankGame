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
using System.Diagnostics;
using DPSF;

namespace Game
{
    public class TankGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public Tank player1Tank;
        Vector2 tankPos;
        Vector2 turretPos;

        public Tank player2Tank;
        Vector2 tankPos2;
        Vector2 turretPos2;

        public Tank currentTank;

        public UIManager uiManager;
        Background background;
        ScreenManager screenManager;
        public PowerUpManager powerUpManager;

        public SoundManager soundManager;

        public Camera2d camera;
        Vector2 currentCameraLerpPosition;
        Vector2 lastTargetPosition;
        float cameraLerpPercent = 1;
        bool lerpCamera = false;

        public const int SCREEN_WIDTH = 1200;
        public const int SCREEN_HEIGHT = 800;
        public const int FIELD_WIDTH = 2048;
        public const int FIELD_HEIGHT = 1536;
        public const int FIELD_HEIGHT_OFFSET = 120;
        public const float GRAVITY = .0981f;

        public float power;
        public int turnsTaken = 0;

        public float shotPause;
        public float moveLimit;
        bool chargingShot;
        bool chargingShot2;
        bool shotFired;
        bool shotCollided;
        bool turnOver;
        public bool gameRunning;
        bool rightSwap;
        bool leftSwap;
        bool debugGame;

        float countdownTimer = 0;

        public GamePadState controller;

        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.Window.Title = "Tank Diggity";

            uiManager = new UIManager(this);
            Components.Add(uiManager);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            soundManager = new SoundManager(this);
            Components.Add(soundManager);
            
            tankPos = new Vector2(60, 785);
            turretPos = new Vector2(tankPos.X + 45, tankPos.Y + 25);
            player1Tank = new Tank(this, tankPos,(float)Math.PI / 2);
            Components.Add(player1Tank);

            tankPos2 = new Vector2(2048 - 140, 785);
            turretPos2 = new Vector2(tankPos2.X + 45, tankPos.Y + 10);
            player2Tank = new Tank(this, tankPos2,-(float)Math.PI / 2);
            Components.Add(player2Tank);

            currentTank = player1Tank;

            camera = new Camera2d(graphics.GraphicsDevice);

            background = new Background(this);
            Components.Add(background);          

            powerUpManager = new PowerUpManager(this);
            Components.Add(powerUpManager);

            power = 0;

            chargingShot = false;
            chargingShot2 = false;
            shotFired = false;
            shotCollided = false;
            turnOver = false;
            gameRunning = false;
            rightSwap = false;
            leftSwap = false;
            debugGame = false;

            controller = GamePad.GetState(PlayerIndex.One);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            screenManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (!gameRunning)
            //{
            //    if (!soundManager.introPlaying)
            //        soundManager.playIntro();
            //}
            //else
            //{
            //    if (!soundManager.backgroundPlaying)
            //        soundManager.playBackground();
            //}

            if (turnsTaken == 0 &! shotFired)
            {
                cameraFollow(currentTank.getTankPos());
            }

            player1Tank.checkAngle();
            player2Tank.checkAngle2();

            if (currentTank == player1Tank)
            {
                currentTank.turretPosition = new Vector2(currentTank.tankPosition.X + 45, currentTank.tankPosition.Y + 10);
                controller = GamePad.GetState(PlayerIndex.One);
                currentTank.recieveController(GamePad.GetState(PlayerIndex.One));
            }

            if (currentTank == player2Tank)
            {
                currentTank.turretPosition = new Vector2(currentTank.tankPosition.X + 30, currentTank.tankPosition.Y + 10);
                controller = GamePad.GetState(PlayerIndex.Two);
                currentTank.recieveController(GamePad.GetState(PlayerIndex.Two));
            }
            
            if ((currentTank.tankPosition.X > 180 && currentTank.tankPosition.X < 468) || (currentTank.tankPosition.X > 1160 && currentTank.tankPosition.X < 1260))
            {
                currentTank.turretPosition.X = currentTank.tankPosition.X + 50;
                currentTank.turretPosition.Y = currentTank.tankPosition.Y + 15;
            }
            else if ((currentTank.tankPosition.X > 705 && currentTank.tankPosition.X < 950) || (currentTank.tankPosition.X > 1540 && currentTank.tankPosition.X < 1795))
            {
                currentTank.turretPosition.X = currentTank.tankPosition.X + 30;
                currentTank.turretPosition.Y = currentTank.tankPosition.Y + 15;
            }
            else
            {
                currentTank.turretPosition.X = currentTank.tankPosition.X + 40;
                currentTank.turretPosition.Y = currentTank.tankPosition.Y + 10;
            }

            if (controller.Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            screenManager.Update(gameTime);

            player1Tank.tempSpeed = 1f;
            player2Tank.tempSpeed = 1f;

            if (player1Tank.intersectPixels(background.layers[1].Sprites[0].Position, background.terrainTextureData))
            {
                player1Tank.tempSpeed = 0;
            }

            if (player2Tank.intersectPixels(background.layers[1].Sprites[0].Position, background.terrainTextureData))
            {
                player2Tank.tempSpeed = 0;
            }

            if (gameRunning)
            {
                //if (soundManager.introPlaying)
                //    soundManager.stopIntro();

                uiManager.Update(gameTime);
                background.Update(gameTime);

                takeTurn(gameTime);

                tankCollision(player1Tank, player2Tank);
                powerUpManager.PowerUpCollision(player1Tank, player2Tank);

                if (isGameOver() || Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    gameRunning = false;
                    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                    GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
                    screenManager.ChangeGameState(ScreenManager.GameState.END);
                }

                if (currentTank.turnTime <= 5 && currentTank.turnTime > 0)
                {
                    if(countdownTimer >= 1)
                    {
                        soundManager.timeBeep.Play();
                        countdownTimer = 0;
                    }
                    else
                    {
                        countdownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                }
            }

            if (screenManager.currentState == ScreenManager.GameState.END)
                resetSetting();

            uiManager.moveFloatingText();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(59, 100, 147));

            background.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.getViewMatrix(Vector2.One));

            uiManager.Draw(spriteBatch);
            powerUpManager.Draw();

            player1Tank.Draw(spriteBatch);
            player2Tank.Draw(spriteBatch);
            screenManager.Draw();
                       
            base.Draw(gameTime);

            spriteBatch.End();
        }

        public void cameraFollow(Vector2 target)
        {
            if (target.X - SCREEN_WIDTH / 2 <= 0)
            {
                target.X += Math.Abs(target.X - SCREEN_WIDTH / 2);
            }

            if (target.X + SCREEN_WIDTH / 2 >= FIELD_WIDTH)
            {
                target.X -= (target.X + SCREEN_WIDTH / 2) - FIELD_WIDTH;
            }

            if (target.Y - SCREEN_HEIGHT / 2 <= 0)
            {
                target.Y += Math.Abs(target.Y - SCREEN_HEIGHT / 2);
            }

            if ((target.Y + SCREEN_HEIGHT / 2) + FIELD_HEIGHT_OFFSET >= FIELD_HEIGHT)
            {
                target.Y -= (target.Y + SCREEN_HEIGHT / 2) + FIELD_HEIGHT_OFFSET - FIELD_HEIGHT;
            }

            if (lerpCamera)
            {
                cameraLerpPercent = 0;
                lerpCamera = false;
            }

            if (cameraLerpPercent < 1)
            {
                cameraLerpPercent += .02f;
                currentCameraLerpPosition = Vector2.Lerp(lastTargetPosition, new Vector2((int)target.X, (int)target.Y + FIELD_HEIGHT_OFFSET), cameraLerpPercent);
                camera.lookAt(currentCameraLerpPosition);
            }
            else
            {
                camera.lookAt(new Vector2((int)target.X, (int)target.Y + FIELD_HEIGHT_OFFSET));
                currentCameraLerpPosition = Vector2.Lerp(lastTargetPosition, new Vector2((int)target.X, (int)target.Y + FIELD_HEIGHT_OFFSET), cameraLerpPercent);
                lastTargetPosition = new Vector2((int)target.X, (int)target.Y + FIELD_HEIGHT_OFFSET);
                cameraLerpPercent = 1;
            }

            uiManager.player1TextPosition.X = currentCameraLerpPosition.X - 395;
            uiManager.player1TextPosition.Y = currentCameraLerpPosition.Y + 192;

            uiManager.player2TextPosition.X = currentCameraLerpPosition.X + 274;
            uiManager.player2TextPosition.Y = currentCameraLerpPosition.Y + 192;

            uiManager.player1LifeRect.X = (int)currentCameraLerpPosition.X - 430;
            uiManager.player1LifeRect.Y = (int)currentCameraLerpPosition.Y + 192;
            uiManager.totalLifeRect1.X = (int)currentCameraLerpPosition.X - 430;
            uiManager.totalLifeRect1.Y = (int)currentCameraLerpPosition.Y + 192;

            uiManager.player2LifeRect.X = (int)currentCameraLerpPosition.X + 234;
            uiManager.player2LifeRect.Y = (int)currentCameraLerpPosition.Y + 192;
            uiManager.totalLifeRect2.X = (int)currentCameraLerpPosition.X + 234;
            uiManager.totalLifeRect2.Y = (int)currentCameraLerpPosition.Y + 192;

            uiManager.powerTextPosition.X = currentCameraLerpPosition.X;
            uiManager.powerTextPosition.Y = currentCameraLerpPosition.Y + 280;

            uiManager.powerBarPosition.X = (int)currentCameraLerpPosition.X - 412;
            uiManager.powerBarPosition.Y = (int)currentCameraLerpPosition.Y + 232;

            uiManager.powerBarX = uiManager.powerBarPosition.X;
            uiManager.powerBarY = uiManager.powerBarPosition.Y;

            uiManager.endOfPowerBar = uiManager.powerBarPosition.X + uiManager.POWER_BAR_WIDTH;

            uiManager.bulletDisplayRect.X = (int)currentCameraLerpPosition.X - 200;
            uiManager.bulletDisplayRect.Y = (int)currentCameraLerpPosition.Y - 400;

            uiManager.bulletTextPosition.X = (int)uiManager.bulletDisplayRect.X + 75;
            uiManager.bulletTextPosition.Y = (int)uiManager.bulletDisplayRect.Y + 30;

            uiManager.hubRect.X = (int)currentCameraLerpPosition.X - 465;
            uiManager.hubRect.Y = (int)currentCameraLerpPosition.Y + 160;

            if (uiManager.fullPower)
            {
                for (int i = 0; i < uiManager.numEmptyPowerBarPieces; i++)
                {
                    Rectangle tempPosition = uiManager.emptyPowerBarPosition[i];
                    tempPosition.X = (int)currentCameraLerpPosition.X - 412 + uiManager.POWER_BAR_WIDTH + (i * -8) - 8;
                    tempPosition.Y = (int)currentCameraLerpPosition.Y + 232;
                    uiManager.emptyPowerBarPosition[i] = tempPosition;
                }
            }
            else
            {
                for (int i = 0; i < uiManager.numEmptyPowerBarPieces; i++)
                {
                    Rectangle tempPosition = uiManager.emptyPowerBarPosition[i];
                    tempPosition.X = (int)currentCameraLerpPosition.X - 412 + (((100 - uiManager.numEmptyPowerBarPieces) * 8) + i * 8);
                    tempPosition.Y = (int)currentCameraLerpPosition.Y + 232;
                    uiManager.emptyPowerBarPosition[i] = tempPosition;
                }
            }

            currentTank.previousShotPosition = new Rectangle(uiManager.powerBarX + currentTank.previousShotX, uiManager.powerBarY, 99, 100);
        }

        private void takeTurn(GameTime time)
        {
            if (currentTank.turnTime > 0 & !shotFired)
            {
                if (currentTank.inventory.getShotCount(currentTank.type) < 1)
                    currentTank.type = currentTank.inventory.findNextShotType();

                if (currentTank.moveLimit > 0)
                {
                    currentTank.move(time);
                }
                
                if ((currentTank.tankPosition.X > 180 && currentTank.tankPosition.X < 468) || (currentTank.tankPosition.X > 1160 && currentTank.tankPosition.X < 1260))
                {
                    currentTank.rotateTurret(2, -1);
                }
                else if ((currentTank.tankPosition.X > 705 && currentTank.tankPosition.X < 950) || (currentTank.tankPosition.X > 1540 && currentTank.tankPosition.X < 1795))
                {
                    currentTank.rotateTurret(1, -2);
                }
                else
                {
                    currentTank.rotateTurret(Math.PI / 2, -Math.PI / 2);
                }
                
                if (screenManager.currentState == ScreenManager.GameState.PLAYER1)
                {
                    cameraFollow(player1Tank.getTankPos());
                }
                else if (screenManager.currentState == ScreenManager.GameState.PLAYER2)
                {
                    cameraFollow(player2Tank.getTankPos());
                }

                playerControls();

                currentTank.turnTime -= (float)time.ElapsedGameTime.TotalSeconds;
                screenManager.updateTime((int)currentTank.turnTime, (int)currentTank.moveLimit);

                if (currentTank.turnTime < 0 & !(chargingShot || chargingShot2))
                {
                    turnOver = true;
                }
            }

            else if (chargingShot || chargingShot2)
            {
                currentTank.shoot(power);
                chargingShot = false;
                chargingShot2 = false;
                shotFired = true;
                setPreviousShotLine();
            }

            if (shotFired & !shotCollided)
            {
                if (currentTank.firstShot)
                {
                    currentTank.firstShot = false;
                }

                cameraFollow(currentTank.bullet.getBulletPosition());

                if (currentTank.bullet.type == BulletType.ScatterShot)
                {
                    if((Keyboard.GetState().IsKeyDown(Keys.Space) ||controller.IsButtonDown(Buttons.RightTrigger)) &!
                        ((ScatterShot)currentTank.bullet).scattered)
                    {
                        ((ScatterShot)currentTank.bullet).scatter();
                    }
                }

                #region MissileDropCollision

                if (currentTank.bullet.type == BulletType.MissileDrop && ((MissileDrop)currentTank.bullet).targetLockedOn)
                {
                    Missile currentMissile = ((MissileDrop)currentTank.bullet).missile;

                    if (currentMissile.intersectPixels(player1Tank.getTankRect(), player1Tank.tankTextureData) && currentTank != player1Tank ||
                        currentMissile.intersectPixels(player2Tank.getTankRect(), player2Tank.tankTextureData) && currentTank != player2Tank ||
                        currentMissile.intersectPixels(background.layers[1].Sprites[0].Position, background.terrainTextureData) ||
                        currentMissile.outOfBounds)
                    {
                        currentMissile.bulletCollided();

                        shotCollided = true;
                        shotPause = 2.0f;
                    }

                    if (currentMissile.explosion != null)
                    {
                        if (currentTank != player1Tank)
                        {
                            if (currentMissile.explosion.collisionRectangle.Intersects(player1Tank.collisionRect) & !currentMissile.damageDealt)
                            {
                                player1Tank.takeDamage(currentMissile.damage);
                                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                                uiManager.floatingDamageText(currentMissile.damage.ToString(), currentMissile.getBulletPosition());
                                currentMissile.damageDealt = true;
                            }
                        }

                        if (currentTank != player2Tank)
                        {
                            if (currentMissile.explosion.collisionRectangle.Intersects(player2Tank.collisionRect) & !currentMissile.damageDealt)
                            {
                                player2Tank.takeDamage(currentMissile.damage);
                                GamePad.SetVibration(PlayerIndex.Two, 1.0f, 1.0f);
                                uiManager.floatingDamageText(currentMissile.damage.ToString(), currentMissile.getBulletPosition());
                                currentMissile.damageDealt = true;
                            }
                        }
                    }
                }

                #endregion MissileDropCollision

                #region ScatterShotCollision

                else if (currentTank.bullet.type == BulletType.ScatterShot && ((ScatterShot)currentTank.bullet).scattered)
                {
                    foreach (Shots s in ((ScatterShot)currentTank.bullet).scatterShots)
                    {
                        if (s.intersectPixels(player1Tank.getTankRect(), player1Tank.tankTextureData) && currentTank != player1Tank ||
                            s.intersectPixels(player2Tank.getTankRect(), player2Tank.tankTextureData) && currentTank != player2Tank ||
                            s.intersectPixels(background.layers[1].Sprites[0].Position, background.terrainTextureData) ||
                            s.outOfBounds)
                        {
                            s.bulletCollided();
                        }

                        if (s.explosion != null)
                        {
                            if (currentTank != player1Tank)
                            {
                                if (s.explosion.collisionRectangle.Intersects(player1Tank.collisionRect) & !s.damageDealt)
                                {
                                    player1Tank.takeDamage(s.damage);
                                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                                    uiManager.floatingDamageText(s.damage.ToString(), s.getBulletPosition());
                                    s.damageDealt = true;
                                }
                            }

                            if (currentTank != player2Tank)
                            {
                                if (s.explosion.collisionRectangle.Intersects(player2Tank.collisionRect) & !s.damageDealt)
                                {
                                    player2Tank.takeDamage(s.damage);
                                    GamePad.SetVibration(PlayerIndex.Two, 1.0f, 1.0f);
                                    uiManager.floatingDamageText(s.damage.ToString(), s.getBulletPosition());
                                    s.damageDealt = true;
                                }
                            }
                        }
                    }

                    bool scatterCollided = false;
                    bool scatterChecked = false;

                    for (int i = 0; i < ((ScatterShot)currentTank.bullet).scatterShots.Count; i++)
                    {
                        if (((ScatterShot)currentTank.bullet).scatterShots[i].exploded & !scatterChecked)
                        {
                            scatterCollided = true;
                        }
                        else
                        {
                            scatterCollided = false;
                            scatterChecked = true;
                        }
                    }

                    if (scatterCollided)
                    {
                        shotCollided = true;
                        shotPause = 2.0f;
                    }
                }

                #endregion ScatterShotCollision

                #region BasicCollision

                else if (currentTank.bullet.intersectPixels(player1Tank.getTankRect(), player1Tank.tankTextureData) && currentTank != player1Tank ||
                    currentTank.bullet.intersectPixels(player2Tank.getTankRect(), player2Tank.tankTextureData) && currentTank != player2Tank)
                {
                    shotCollided = true;
                    shotPause = 2.0f;
                }
                else if (currentTank.bullet.intersectPixels(background.layers[1].Sprites[0].Position, background.terrainTextureData))
                {
                    if (currentTank.bullet.type == BulletType.TeleportShot)
                    {
                        currentTank.teleportTank(currentTank.bullet.getBulletPosition());
                        currentTank.terrainMovement();
                        currentTank.bullet.damageDealt = true;
                    }

                    shotCollided = true;
                    shotPause = 1.0f;
                }
                else if (currentTank.bullet.outOfBounds)
                {
                    shotCollided = true;
                    shotPause = 1.0f;
                }

                #endregion BasicCollision

                if (shotCollided && currentTank.bullet.type == BulletType.MissileDrop &! currentTank.bullet.outOfBounds)
                {
                    MissileDrop drop = ((MissileDrop)currentTank.bullet);

                    if (!drop.targetLockedOn)
                    {
                        if (currentTank != player1Tank)
                        {
                            drop.sendTankData(player1Tank.getTankPos(), player1Tank.getTankRect().Width / 2,
                                player1Tank.tankPic.Width, player1Tank.tankPic.Height, player1Tank.collisionRect);
                        }
                        else if (currentTank != player2Tank)
                        {
                            drop.sendTankData(player2Tank.getTankPos(), player2Tank.getTankRect().Width / 2,
                                player2Tank.tankPic.Width, player2Tank.tankPic.Height, player2Tank.collisionRect);
                        }

                        if (drop.scanTarget())
                        {
                            shotCollided = false;
                            shotPause = 0;
                        }
                    }
                }
                else if (shotCollided)
                {
                    currentTank.bullet.bulletCollided();
                }

                if (currentTank.bullet.explosion != null)
                {
                    if (currentTank != player2Tank)
                    {
                        if (currentTank.bullet.explosion.collisionRectangle.Intersects(player2Tank.collisionRect) & !currentTank.bullet.damageDealt)
                        {
                            player2Tank.takeDamage(currentTank.bullet.damage);
                            GamePad.SetVibration(PlayerIndex.Two, 1.0f, 1.0f);
                            uiManager.floatingDamageText(currentTank.bullet.damage.ToString(), currentTank.bullet.getBulletPosition());
                            currentTank.bullet.damageDealt = true;
                        }
                    }

                    if (currentTank != player1Tank)
                    {
                        if (currentTank.bullet.explosion.collisionRectangle.Intersects(player1Tank.collisionRect) & !currentTank.bullet.damageDealt)
                        {
                            player1Tank.takeDamage(currentTank.bullet.damage);
                            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                            uiManager.floatingDamageText(currentTank.bullet.damage.ToString(), currentTank.bullet.getBulletPosition());
                            currentTank.bullet.damageDealt = true;
                        }
                    }
                }
            }
            else if (shotPause > 0)
            {
                shotPause -= (float)time.ElapsedGameTime.TotalSeconds;
                screenManager.updateTime((int)shotPause, (int)currentTank.moveLimit);

                if (shotPause < 0)
                {
                    turnOver = true;
                }
            }
            else if (turnOver)
            {
                gameRunning = false;
                turnsTaken++;
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
                uiManager.removeText();
                swapPlayers();
            }
        }

        public void playerControls()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                uiManager.calculatePower();
                chargingShot = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space) && chargingShot)
            {
                currentTank.shoot(power);
                chargingShot = false;
                shotFired = true;
                setPreviousShotLine();
            }

            //Controller Check for shooting
            if (controller.IsButtonDown(Buttons.RightTrigger))
            {
                uiManager.calculatePower();
                chargingShot2 = true;
            }
            if (controller.IsButtonUp(Buttons.RightTrigger) && chargingShot2)
            {
                currentTank.shoot(power);
                chargingShot2 = false;
                shotFired = true;
                setPreviousShotLine();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                currentTank.type = BulletType.BasicBullet;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                if (currentTank.inventory.getShotCount(BulletType.HeavyShot) > 0)
                    currentTank.type = BulletType.HeavyShot;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                if (currentTank.inventory.getShotCount(BulletType.ScatterShot) > 0)
                    currentTank.type = BulletType.ScatterShot;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                currentTank.type = BulletType.TeleportShot;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                currentTank.type = BulletType.MissileDrop;
            }

            //Swapping Weapons
            if (controller.IsButtonDown(Buttons.RightShoulder) & !rightSwap)
            {
                currentTank.type = currentTank.nextWeapon();
                rightSwap = true;
            }
            if (controller.IsButtonUp(Buttons.RightShoulder))
            {
                rightSwap = false;
            }
            if (controller.IsButtonDown(Buttons.LeftShoulder) & !leftSwap)
            {
                currentTank.type = currentTank.previousWeapon();
                leftSwap = true;
            }
            if (controller.IsButtonUp(Buttons.LeftShoulder))
            {
                leftSwap = false;
            }

            //Gives UIManager the Bullet Type for display
            uiManager.takeBulletType(currentTank.type, currentTank.inventory.getShotCount(currentTank.type));
        }

        public void setPreviousShotLine()
        {
            if (uiManager.emptyPowerBarPosition.Count == 0)
            {
                currentTank.previousShotX = uiManager.POWER_BAR_WIDTH - 49;
            }
            else
            {
                currentTank.previousShotX = (uiManager.POWER_BAR_WIDTH - (uiManager.numEmptyPowerBarPieces * 8)) - 49;
            }

            currentTank.showPreviousShotLine = false;
        }

        public void endTurn(Tank currentTank)
        {
            if (currentTank.tankPosition.X < 600)
            {
                camera.lookAt(new Vector2(600, player1Tank.getTankPos().Y + FIELD_HEIGHT_OFFSET));
                camera.Origin.X = GraphicsDevice.Viewport.Width / 2.0f;
            }

            if (currentTank.tankPosition.X > 1400)
            {
                camera.lookAt(new Vector2(1400, player2Tank.getTankPos().Y + FIELD_HEIGHT_OFFSET));
                camera.Origin.X = 1400;
            }

            player1Tank.takingTurn = false;
            player2Tank.takingTurn = false;
        }

        public void changeTurn()
        {
            if (screenManager.currentState == ScreenManager.GameState.PLAYER1 || debugGame)
            {
                currentTank = player1Tank;
                controller = GamePad.GetState(PlayerIndex.One);

                if (debugGame)
                {
                    player1Tank.inventory.debugShot();
                }
            }
            else if (screenManager.currentState == ScreenManager.GameState.PLAYER2)
            {
                currentTank = player2Tank;
                controller = GamePad.GetState(PlayerIndex.Two);
            }

            endTurn(currentTank);
            currentTank.takingTurn = true;
            currentTank.controller = controller;
            if (debugGame)
            {
                currentTank.moveLimit = 99;
                currentTank.turnTime = 99;
            }
            else
            {
                currentTank.moveLimit = 3;
                currentTank.turnTime = 20;
            }
            shotPause = 0;
            uiManager.resetPowerBar();
            uiManager.resetMove();
            shotFired = false;
            shotCollided = false;
            turnOver = false;

            if (turnsTaken > 0)
            {
                lerpCamera = true;
            }

            if(!currentTank.firstShot)
            {
                currentTank.showPreviousShotLine = true;
            }

            gameRunning = true;
        }

        private void swapPlayers()
        {
            if (debugGame)
            {
                screenManager.ChangeGameState(ScreenManager.GameState.PLAYER1);
            }
            else if (screenManager.currentState == ScreenManager.GameState.PLAYER1)
            {
                screenManager.ChangeGameState(ScreenManager.GameState.PLAYER2);
            }
            else if (screenManager.currentState == ScreenManager.GameState.PLAYER2)
            {
                screenManager.ChangeGameState(ScreenManager.GameState.PLAYER1);
            }
        }

        public bool isGameOver()
        {
            bool GameOver = false;
            if (player1Tank.health <= 0 || player2Tank.health <= 0)
                GameOver = true;

            if (player1Tank.health <= 0)
            {
                player1Tank.explode();
            }

            if (player2Tank.health <= 0)
            {
                player2Tank.explode();
            }

            return GameOver;
        }

        public void resetSetting()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                uiManager.resetHealth();
                player1Tank.resetPos(tankPos, (float)Math.PI / 2);
                player2Tank.resetPos(tankPos2, -(float)Math.PI / 2);
                player1Tank.inventory.resetWeapons();
                player2Tank.inventory.resetWeapons();
                player1Tank.showPreviousShotLine = false;
                player2Tank.showPreviousShotLine = false;
                player1Tank.tanksplosion = null;
                player2Tank.tanksplosion = null;
                player1Tank.explosionTimer = 1;
                player2Tank.explosionTimer = 1;
                powerUpManager.Initialize();
                screenManager.ChangeGameState(ScreenManager.GameState.START);
            }
        }

        private void tankCollision(Tank player1Tank, Tank player2Tank)
        {
            if (player1Tank.tankPosition.X <= player2Tank.tankPosition.X)
            {
                if (player1Tank.collisionRect.Intersects(player2Tank.collisionRect))
                {
                    player1Tank.collideRight = false;
                    player2Tank.collideLeft = false;
                }

                if (!(player1Tank.collisionRect.Intersects(player2Tank.collisionRect)))
                {
                    player1Tank.collideRight = true;
                    player2Tank.collideLeft = true;
                }
            }

            if (player1Tank.tankPosition.X >= player2Tank.tankPosition.X)
            {
                if (player1Tank.collisionRect.Intersects(player2Tank.collisionRect))
                {
                    player2Tank.collideRight = false;
                    player1Tank.collideLeft = false;
                }

                if (!(player1Tank.collisionRect.Intersects(player2Tank.collisionRect)))
                {
                    player1Tank.collideLeft = true;
                    player2Tank.collideRight = true;
                }
            }
        }

        public void startDebug()
        {
            debugGame = true;
            
        }

        public void noDebug()
        {
            debugGame = false;
            player1Tank.inventory.resetWeapons();
            player2Tank.inventory.resetWeapons();
        }
        
        public static Vector2 rotateVector(Vector2 originalVector, float angle)
        {
            return new Vector2((float)(Math.Cos(-angle) * originalVector.X - Math.Sin(-angle) * originalVector.Y),
                (float)(Math.Sin(-angle) * originalVector.X + Math.Cos(-angle) * originalVector.Y));
        }

        public static bool CircleRectangleIntersection(Circle circle, Rectangle rectangle)
        {
            Vector2 circleDistance = new Vector2(Math.Abs(circle.Center.X - rectangle.X),
                Math.Abs(circle.Center.Y - rectangle.Y));

            if (circleDistance.X > (rectangle.Width / 2 + circle.Radius) ||
                circleDistance.Y > (rectangle.Height / 2 + circle.Radius))
            {
                return false;
            }

            if (circleDistance.X <= (rectangle.Width / 2) ||
                circleDistance.Y <= (rectangle.Height / 2))
            {
                return true;
            }

            float cornerDistanceSquared = ((circleDistance.X - rectangle.Width / 2) * (circleDistance.X - rectangle.Width / 2) +
                (circleDistance.Y - rectangle.Height / 2) * (circleDistance.Y - rectangle.Height / 2));

            return (cornerDistanceSquared <= (circle.Radius * circle.Radius));
        }
    }
}
