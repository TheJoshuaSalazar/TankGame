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

namespace Game
{
    class MissileDrop : Bullet
    {
        TankGame game;
        public Missile missile = null;
        Vector2 tankPosition;
        Vector2 tankSize;
        Rectangle tankCollisionRectangle;
        float missileOffset;
        int sensorDistance = 125;
        public bool targetLockedOn = false;

        List<DetectionWheel> detect;
        TargetLocked target = null;
        Circle detectionCircle;

        public MissileDrop(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            this.game = game;

            this.position = bulletPosition;
            this.type = BulletType.MissileDrop;

            damage = 10;
            mass = 1.0f;

            angle = 0;
            speed = Vector2.Zero;

            detect = new List<DetectionWheel>();
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/Probe");
        }

        public override void Update(GameTime gameTime)
        {
            position += speed;

            if (speed.X >= 0)
            {
                angle += (float)Math.PI / 60;
            }
            else if(speed.X < 0)
            {
                angle -= (float)Math.PI / 60;
            }

            bulletTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            applyGravity();

            checkBounds();

            if (missile != null)
            {
                missile.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (missile != null)
            {
                missile.Draw();
            }

            base.Draw(gameTime);
        }

        public void sendTankData(Vector2 getTankPosition, float offset, int tankWidth, int tankHeight, Rectangle collisionRectangle)
        {
            tankPosition = getTankPosition;
            missileOffset = offset;
            tankSize.X = tankWidth;
            tankSize.Y = tankHeight;
            tankCollisionRectangle = collisionRectangle;
        }

        public bool scanTarget()
        {
            detectionCircle = new Circle(position, sensorDistance);

            if (TankGame.CircleRectangleIntersection(detectionCircle, tankCollisionRectangle))
            {
                missile = new Missile(game, new Vector2(tankPosition.X + missileOffset));
                Game.Components.Add(missile);

                target = new TargetLocked(game, new Vector2(tankPosition.X + tankSize.X / 2, tankPosition.Y + tankSize.Y / 2));
                target.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

                target.UpdateOrder = 100;
                target.DrawOrder = 100;
                target.Visible = true;

                targetLockedOn = true;
            }
            else
            {
                targetLockedOn = false;
            }

            bulletCollided();

            return targetLockedOn;
        }

        public override void bulletCollided()
        {
            Vector2 detectionPosition = new Vector2(position.X, position.Y);

            float rotationDirection = 0;

            if (speed.X > 0)
            {
                rotationDirection = (float)Math.PI;
            }
            else if (speed.X < 0)
            {
                rotationDirection = (float)Math.PI;
            }

            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionCore", -rotationDirection));
            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionWheel1", rotationDirection / 2));
            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionWheel2", -rotationDirection / 3));
            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionWheel3", rotationDirection / 4));
            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionWheel4", -rotationDirection / 2));
            detect.Add(new DetectionWheel(game, detectionPosition, "DetectionWheel5", rotationDirection));

            foreach (DetectionWheel d in detect)
            {
                d.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

                d.UpdateOrder = 100;
                d.DrawOrder = 100;
                d.Visible = true;
            }

            speed = Vector2.Zero;

            exploded = true;

            ((TankGame)Game).soundManager.missileScanning.Play();
        }
    }
}
