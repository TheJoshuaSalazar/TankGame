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
using System.Diagnostics;
using DPSF;

namespace Game
{
    public class Tank : DrawableGameComponent
    {
        public enum TankState { Normal, Downhill, Uphill }

        TankState currentTankState;

        TankGame game;

        public Vector2 tankPosition;
        public Vector2 turretPosition;
        Vector2 turretOrigin;
        Vector2 originalTurretDirection;
        Vector2 turretDirection;

        Rectangle tankRect;
        Rectangle turretRect;

        public Texture2D tankPic;
        Texture2D turretPic;

        
        public int health;

        public float turnTime;
        public float moveLimit;
        const float speedLeft = -2;
        const float speedRight = 2;
        float turretAngle;

        int collisionOffset = 1;
        public bool collideLeft = true;
        public bool collideRight = true;
        
        KeyboardState keyState;

        public Color[] tankTextureData;

        public float tempSpeed = 4;

        public Bullet bullet;

        public BulletType type;

        public Inventory inventory;

        public GamePadState controller;

        public bool takingTurn = false;

        public int previousShotX = 0;
        public Rectangle previousShotPosition;
        Texture2D previousShotLine;
        public bool showPreviousShotLine = false;
        public bool firstShot = true;

        SmokeCloud damageClouds;
        List<Spark> sparks = new List<Spark>();

        double MAX;
        double MIN;

        bool showLightDamageClouds = false;
        bool showDamageClouds = false;
        bool showSevereClouds = false;
        bool showCritialClouds = false;
        bool showYourFateIsSealedClouds = false;

        bool recoil = false;
        bool resetRecoil = false;

        float timer;
        float shotPosition = 0.0f;

        const int dirtTrailOffset = 70;
        DirtTrail dirtTrail;

        public Tanksplosion tanksplosion = null;
        public float explosionTimer = 1;

        Puff shotPuff;

        Random random = new Random();


        public Tank(TankGame game, Vector2 tankPosition, float turretAngle)
            : base(game)
        { 
            this.game = game;
            this.tankPosition = tankPosition;
            turretPosition = new Vector2(tankPosition.X + 40, tankPosition.Y + 25);
            this.turretAngle = turretAngle;
            health = 200;
            turnTime = 0;
            moveLimit = 0;
            originalTurretDirection = new Vector2(0, 1);
            turretDirection = new Vector2((float)(Math.Cos(-turretAngle) * originalTurretDirection.X - Math.Sin(-turretAngle) * originalTurretDirection.Y),
                (float)(Math.Sin(-turretAngle) * originalTurretDirection.X + Math.Cos(-turretAngle) * originalTurretDirection.Y));
            bullet = new Bullet(game);
            type = BulletType.BasicBullet;
            inventory = new Inventory(game);
            damageClouds = null;
            timer = 0.0f;

            currentTankState = TankState.Normal;
        }

        protected override void LoadContent()
        {
            tankPic = Game.Content.Load<Texture2D>(@"Images/tank");
            turretPic = Game.Content.Load<Texture2D>(@"Images/turret");
            previousShotLine = Game.Content.Load<Texture2D>(@"Images/PreviousShotLine");

            tankTextureData = new Color[tankPic.Width * tankPic.Height];
            tankPic.GetData(tankTextureData);

            turretRect = new Rectangle((int)turretPosition.X, (int)turretPosition.Y, turretPic.Width, turretPic.Height);
            turretOrigin = new Vector2(turretPic.Width / 2, turretPic.Height);

            tankRect = new Rectangle((int)tankPosition.X, (int)tankPosition.Y, tankPic.Width, tankPic.Height);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            
            tankRect.X = (int)tankPosition.X;
            tankRect.Y = (int)tankPosition.Y;

            tankPosition.Y += tempSpeed;
            
            if (bullet.type != BulletType.NullBullet)
            {
                bullet.Update(gameTime);
            }
            
            #region Damage Clouds & Sparks

            if (explosionTimer > 0)
            {
                if (health < 25 & !showYourFateIsSealedClouds)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = false;
                    showDamageClouds = false;
                    showSevereClouds = false;
                    showCritialClouds = false;
                    showYourFateIsSealedClouds = true;
                }
                else if (health >= 25 && health < 75 & !showCritialClouds)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = false;
                    showDamageClouds = false;
                    showSevereClouds = false;
                    showCritialClouds = true;
                    showYourFateIsSealedClouds = false;
                }
                else if (health >= 75 && health < 125 & !showSevereClouds)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = false;
                    showDamageClouds = false;
                    showSevereClouds = true;
                    showCritialClouds = false;
                    showYourFateIsSealedClouds = false;
                }
                else if (health >= 125 && health < 175 & !showDamageClouds)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = false;
                    showDamageClouds = true;
                    showSevereClouds = false;
                    showCritialClouds = false;
                    showYourFateIsSealedClouds = false;
                }
                else if (health >= 175 && health < 200 & !showLightDamageClouds)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = true;
                    showDamageClouds = false;
                    showSevereClouds = false;
                    showCritialClouds = false;
                    showYourFateIsSealedClouds = false;
                }
                else if (health >= 200)
                {
                    if (damageClouds != null)
                    {
                        damageClouds.Dispose();
                        damageClouds = null;
                    }

                    while (sparks.Count > 0)
                    {
                        sparks[0].Dispose();
                        sparks.RemoveAt(0);
                    }

                    showLightDamageClouds = false;
                    showDamageClouds = false;
                    showSevereClouds = false;
                    showCritialClouds = false;
                    showYourFateIsSealedClouds = false;
                }

                if (showLightDamageClouds && damageClouds == null)
                {
                    initializeDamageClouds(1, 1, Color.White);
                    initializeSpark(1, 5, 2.0f);
                }
                else if (showDamageClouds && damageClouds == null)
                {
                    initializeDamageClouds(5, 5, Color.LightGray);
                    initializeSpark(2, 5, 1.6f);
                }
                else if (showSevereClouds && damageClouds == null)
                {
                    initializeDamageClouds(10, 10, Color.Gray);
                    initializeSpark(3, 10, 1.2f);
                }
                else if (showCritialClouds && damageClouds == null)
                {
                    initializeDamageClouds(20, 20, Color.DarkGray);
                    initializeSpark(4, 15, .8f);
                }
                else if (showYourFateIsSealedClouds && damageClouds == null)
                {
                    initializeDamageClouds(200, 200, Color.DarkSlateGray);
                    initializeSpark(6, 20, .4f);
                }

                if (damageClouds != null)
                {
                    damageClouds.changeEmitterPosition(new Vector2(tankPosition.X + tankPic.Width / 2, tankPosition.Y + tankPic.Height / 2));
                    damageClouds.ParticleUpdate(gameTime);
                }

                 foreach(Spark s in sparks)
                 {
                     s.changeEmitterPosition(new Vector2(tankPosition.X + tankPic.Width / 2, tankPosition.Y + tankPic.Height / 2));
                 }
            }
            else
            {
                if (damageClouds != null)
                {
                    damageClouds.Dispose();
                    damageClouds = null;
                }

                while(sparks.Count > 0)
                {
                    sparks[0].Dispose();
                    sparks.RemoveAt(0);
                }
            }

            #endregion Damage Clouds & Sparks

            if (recoil)
            {
                tankRecoil();
            }

            if (tanksplosion != null)
            {
                explosionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public void checkAngle()
        {
            if (turretAngle < MIN)
            {
                turretAngle = (float)MIN;
            }
        }

        public void checkAngle2()
        {
            if (turretAngle > MAX)
            {
                turretAngle = (float)MAX;
            }
        }

        public void terrainMovement()
        {
            if ((tankPosition.X > 190 && tankPosition.X < 468) || (tankPosition.X > 1160 && tankPosition.X < 1260))
            {
                currentTankState = TankState.Downhill;

                checkAngle2();
                tankPic = Game.Content.Load<Texture2D>(@"Images/tankRotated1");
                
                if (keyState.IsKeyDown(Keys.A) || controller.ThumbSticks.Left.X < 0)
                {
                    if (collideLeft)
                    {
                        if (tankPosition.X > 0)
                        {
                            tankPosition.Y -= 1.2f;
                        }
                    }
                }
            }

            else if ((tankPosition.X > 705 && tankPosition.X < 950) || (tankPosition.X > 1538 && tankPosition.X < 1795))
            {
                currentTankState = TankState.Uphill;

                checkAngle();
                tankPic = Game.Content.Load<Texture2D>(@"Images/tankRotated2"); 

                if (keyState.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
                {
                    if (collideLeft)
                    {
                        if (tankPosition.X > 0)
                        {
                            tankPosition.Y -= 1.2f;
                        }
                    }
                }
            }

            else
            {
                tankPic = Game.Content.Load<Texture2D>(@"Images/tank");

                currentTankState = TankState.Normal;
            }

            tankRect.Width = tankPic.Width;
            tankRect.Height = tankPic.Height;
            tankTextureData = new Color[tankPic.Width * tankPic.Height];
            tankPic.GetData(tankTextureData);
        }
        
        public void Draw(SpriteBatch sprite)
        {
            if (explosionTimer > 0)
            {
                sprite.Draw(turretPic, turretPosition, null, Color.White, turretAngle, turretOrigin, 1.0f, SpriteEffects.None, 0.0f);
                sprite.Draw(tankPic, tankPosition, Color.White);
            }

            if (showPreviousShotLine)
            {
                TankGame.spriteBatch.Draw(previousShotLine, previousShotPosition, Color.White);
            }

            if (bullet.type != BulletType.NullBullet)
            {
                bullet.Draw();
            }
        }

        public void move(GameTime time)
        {
			terrainMovement();

            if (keyState.IsKeyDown(Keys.A) || controller.ThumbSticks.Left.X < 0)
            {
                if (collideLeft)
                {
                    if (tankPosition.X > 0)
                    {
                        tankPosition.X += speedLeft;

                        if (timer >= 15.0f)
                            ((TankGame)Game).soundManager.tankMove.Play();

                        if (currentTankState == TankState.Downhill)
                        {
                            makeDirtTrail(new Vector2(tankRect.Right - dirtTrailOffset - 5, tankRect.Bottom - 26), TankGame.rotateVector(new Vector2(-1, 1), -26.3f));
                        }
                        else if (currentTankState == TankState.Uphill)
                        {
                            makeDirtTrail(new Vector2(tankRect.Right - dirtTrailOffset + 10, tankRect.Bottom - 5), TankGame.rotateVector(new Vector2(-1, 1), .5f));
                        }
                        else if (currentTankState == TankState.Normal)
                        {
                            makeDirtTrail(new Vector2(tankRect.Right - dirtTrailOffset + 5, tankRect.Bottom), new Vector2(-1, 1));
                        }
                    }

                    moveLimit -= (float)time.ElapsedGameTime.TotalSeconds;
                }
            }

            if (keyState.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
            {
                if (collideRight)
                {
                    if (tankPosition.X < 1920)
                    {
                        tankPosition.X += speedRight;
                        
                        if (timer >= 15.0f)
                            ((TankGame)Game).soundManager.tankMove.Play();

                        if (currentTankState == TankState.Downhill)
                        {
                            makeDirtTrail(new Vector2(tankRect.Left + dirtTrailOffset - 5, tankRect.Bottom), TankGame.rotateVector(new Vector2(2, 1), -.95f));
                        }
                        else if (currentTankState == TankState.Uphill)
                        {
                            makeDirtTrail(new Vector2(tankRect.Left + dirtTrailOffset + 10, tankRect.Bottom - 25), TankGame.rotateVector(new Vector2(1, .7f), 19.8f));
                        }
                        else if (currentTankState == TankState.Normal)
                        {
                            makeDirtTrail(new Vector2(tankRect.Left + dirtTrailOffset + 5, tankRect.Bottom), new Vector2(1, 1));
                        }

                    }

                    moveLimit -= (float)time.ElapsedGameTime.TotalSeconds;
                }
            }

            if (timer >= 15.0f)
                timer = 0.0f;
            timer++;
        }

        public void makeDirtTrail(Vector2 position, Vector2 direction)
        {
            dirtTrail = new DirtTrail(game, position, direction);
            dirtTrail.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);
            dirtTrail.UpdateOrder = 100;
            dirtTrail.DrawOrder = 100;
            dirtTrail.Visible = true;
        }

        public void takeDamage(int damage)
        {
            while (damage >= 0)
            {
                this.health--;
                damage--;
            }
        }

        public void healTank(int heal)
        {
            if (this.health < 200)
                this.health += heal;
            else
                this.health = 200;
        }

        public void shoot(float power)
        {
            if (type != BulletType.BasicBullet)
            {
                if (inventory.getShotCount(type) <= 0)
                    type = inventory.findNextShotType();

                switch (type)
                {
                    case BulletType.HeavyShot:
                        bullet = new HeavyShot(game, new Vector2(turretPosition.X + turretPic.Width / 2, turretPosition.Y - turretPic.Height / 8));
                        break;

                    case BulletType.ScatterShot:
                        bullet = new ScatterShot(game, new Vector2(turretPosition.X + turretPic.Width / 2, turretPosition.Y - turretPic.Height / 8));
                        break;

                    case BulletType.TeleportShot:
                        bullet = new TeleportShot(game, new Vector2(turretPosition.X + turretPic.Width / 2, turretPosition.Y - turretPic.Height / 8));
                        break;

                    case BulletType.MissileDrop:
                        bullet = new MissileDrop(game, new Vector2(turretPosition.X + turretPic.Width / 2, turretPosition.Y - turretPic.Height / 8));
                        break;
                }
                
                inventory.decrementShot(type);
            }
            else
            {
                bullet = new Bullet(game, new Vector2(turretPosition.X + turretPic.Width/2 , turretPosition.Y - turretPic.Height/8 ));
            }

            bullet.speed = turretDirection * new Vector2(power, -power);
            ((TankGame)Game).soundManager.shoot.Play();
            Game.Components.Add(bullet);
            recoil = true;
            shotPosition = tankPosition.X;

            shotPuff = new Puff(game, new Vector2(turretPosition.X + turretPic.Width / 2, turretPosition.Y - turretPic.Height / 8), turretDirection, power);
            shotPuff.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            shotPuff.UpdateOrder = 100;
            shotPuff.DrawOrder = 100;
            shotPuff.Visible = true;
        }

        public void rotateTurret(double max, double min)
        {
            MAX = max;
            MIN = min;
            if (Keyboard.GetState().IsKeyDown(Keys.W) || controller.ThumbSticks.Right.X < 0 || controller.ThumbSticks.Right.Y > 0)
            {
                if (turretAngle > min)
                {
                    turretAngle -= 0.01f;

                    turretDirection = new Vector2((float)(Math.Cos(-turretAngle) * originalTurretDirection.X - Math.Sin(-turretAngle) * originalTurretDirection.Y),
                        (float)(Math.Sin(-turretAngle) * originalTurretDirection.X + Math.Cos(-turretAngle) * originalTurretDirection.Y));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) || controller.ThumbSticks.Right.X > 0 || controller.ThumbSticks.Right.Y < 0)
            {
                if (turretAngle < max)
                {
                    turretAngle += 0.01f;

                    turretDirection = new Vector2((float)(Math.Cos(-turretAngle) * originalTurretDirection.X - Math.Sin(-turretAngle) * originalTurretDirection.Y),
                        (float)(Math.Sin(-turretAngle) * originalTurretDirection.X + Math.Cos(-turretAngle) * originalTurretDirection.Y));
                }
            }
        }

        public void teleportTank(Vector2 teleportPosition)
        {
            tankPosition = new Vector2(teleportPosition.X - (tankRect.Width / 2) + 20, teleportPosition.Y - 40);
        }
        
        public void initializeDamageClouds(int maxParticles, int particlesPerSecond, Color cloudColor)
        {
            damageClouds = new SmokeCloud(game, new Vector2(tankPosition.X + tankPic.Width / 2, tankPosition.Y + tankPic.Height / 2),
                maxParticles, particlesPerSecond, cloudColor);
            damageClouds.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            damageClouds.UpdateOrder = 100;
            damageClouds.DrawOrder = 100;
            damageClouds.Visible = true;
        }

        public void initializeSpark(int numSparks, int maxParticles, float lifeTime)
        {
            for (int i = 0; i < numSparks; i++)
            {
                sparks.Add(new Spark(game, new Vector2(tankPosition.X + tankPic.Width / 2, tankPosition.Y + tankPic.Height / 2),
                    maxParticles, lifeTime));
                sparks[sparks.Count - 1].AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

                sparks[sparks.Count - 1].UpdateOrder = 100;
                sparks[sparks.Count - 1].DrawOrder = 100;
                sparks[sparks.Count - 1].Visible = true;
            }
        }

        public void explode()
        {
            tanksplosion = new Tanksplosion(game, new Vector2(tankPosition.X + tankRect.Width / 2,
                tankPosition.Y + tankRect.Height / 2));
            tanksplosion.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            tanksplosion.UpdateOrder = 100;
            tanksplosion.DrawOrder = 100;
            tanksplosion.Visible = true;
        }

        public void resetPos(Vector2 tankPosition, float turretAngle)
        {
            this.tankPosition = tankPosition;
            this.turretAngle = turretAngle;
            tankPic = Game.Content.Load<Texture2D>(@"Images/tank");
            turretPosition = new Vector2(tankPosition.X + 35, tankPosition.Y + 10);      
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)tankPosition.X + collisionOffset,
                                (int)tankPosition.Y + collisionOffset,
                                tankPic.Width - (collisionOffset * 2),
                                tankPic.Height - (collisionOffset * 2));
            }
        }

        public bool intersectPixels(Rectangle rectangle, Color[] data)
        {
            //find the bounds of the rectangle intersection
            int top = Math.Max(this.tankRect.Top, rectangle.Top);
            int bottom = Math.Min(this.tankRect.Bottom, rectangle.Bottom);
            int left = Math.Max(this.tankRect.Left, rectangle.Left);
            int right = Math.Min(this.tankRect.Right, rectangle.Right);

            //check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    //get the color of both pixels at this point
                    Color color1 = this.tankTextureData[(x - this.tankRect.Left) + (y - this.tankRect.Top) * this.tankRect.Width];
                    Color color2 = data[(x - rectangle.Left) + (y - rectangle.Top) * rectangle.Width];

                    //if both pixels are not completely transparent
                    if (color1.A != 0 && color2.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }       

        public Vector2 getTankPos()
        {
            return tankPosition;
        }

        public Rectangle getTankRect()
        {
            return tankRect;
        }

        public BulletType nextWeapon()
        {
            return inventory.nextWeaponType(type);
        }

        public BulletType previousWeapon()
        {
            return inventory.previousWeaponType(type);
        }

        public void recieveController(GamePadState playerController)
        {
            controller = playerController;
        }

        private void tankRecoil()
        {
            if (((TankGame)Game).currentTank.turretAngle > (float)Math.PI / 10000)
            {
                if (!resetRecoil)
                {
                    tankPosition.X -= 2;
                    if (tankPosition.X <= shotPosition - 30)
                        resetRecoil = true;
                }
                if (resetRecoil)
                {
                    tankPosition.X += 2;
                }
                if (tankPosition.X >= shotPosition)
                {
                    resetRecoil = false;
                    recoil = false;
                }
            }
            else if (((TankGame)Game).currentTank.turretAngle < (float)Math.PI / 10000)
            {
                if (!resetRecoil)
                {
                    tankPosition.X += 2;

                    if (tankPosition.X >= shotPosition + 30)
                        resetRecoil = true;
                }

                if (resetRecoil)
                {
                    tankPosition.X -= 2;
                }

                if (tankPosition.X <= shotPosition)
                {
                    resetRecoil = false;
                    recoil = false;
                }
            }
        }
    }
}
