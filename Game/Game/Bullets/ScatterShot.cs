using System;
using System.Collections.Generic;
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
    public class ScatterShot : Bullet
    {
        public List<Shots> scatterShots;

        public bool scattered = false;

        Vector2 bulletDirection;
        Vector2 scatterDirection;

        float scatterAngle;
        float scatterTime = 0;

        public ScatterShot(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            Bullet basic = new Bullet(game);

            this.position = bulletPosition;
            this.type = BulletType.ScatterShot;

            angle = 0;

            scatterAngle = 0;

            damage = 20;
            mass = .75f;

            bulletDirection = new Vector2(1, 0);

            speed = Vector2.Zero;

            scatterShots = new List<Shots>();
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/ScatterShot");
        }

        public void scatter()
        {
            if (scatterTime >= 1)
            {
                damage = 10;

                for (int i = 0; i < 8; i++)
                {
                    scatterDirection = TankGame.rotateVector(bulletDirection, scatterAngle);

                    scatterShots.Add(new Shots((TankGame)Game, this.position,
                        scatterDirection * new Vector2(TankGame.GRAVITY * 10 + .25f, -TankGame.GRAVITY * 10 - .25f)));

                    scatterAngle += (float)Math.PI / 7;
 
                    Game.Components.Add(scatterShots[i]);
                }

                if (position.Y < 200)
                {
                    speed = new Vector2(0, TankGame.GRAVITY * mass * 40);
                }
                else if (position.Y < 800)
                {
                    speed = new Vector2(0, TankGame.GRAVITY * mass * 20);
                }
                else
                {
                    speed = Vector2.Zero;
                }

                scattered = true;

                bulletCollided();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (scatterTime < 1)
            {
                scatterTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            foreach (Shots s in scatterShots)
            {
                s.Update(gameTime);
            }
        }

        public override void Draw()
        {
            foreach (Shots s in scatterShots)
            {
                s.Draw();
            }

            base.Draw();
        }

        public override void bulletCollided()
        {
            if (!outOfBounds)
            {
                if (scattered)
                {
                    if (explosion == null)
                    {
                        explosion = new ScatterBurst((TankGame)Game, new Vector2(position.X + (speed.X * 6), position.Y + (speed.Y * 6)));
                        explosion.AutoInitialize(Game.GraphicsDevice, Game.Content, TankGame.spriteBatch);

                        explosion.UpdateOrder = 100;
                        explosion.DrawOrder = 100;
                        explosion.Visible = true;
                    }
                }

                else
                {
                    base.bulletCollided();
                }
            }

            exploded = true;
        }
    }
}
