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
    public class Missile : Bullet
    {
        public Missile(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            this.position = new Vector2(bulletPosition.X, 0);

            this.type = BulletType.Missile;

            damage = 60;
            mass = 1.0f;

            angle = 0;
            speed = Vector2.Zero;
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/Missile");
        }

        public override void bulletCollided()
        {
            if (!outOfBounds)
            {
                if (explosion == null)
                {
                    explosion = new Flare((TankGame)Game, new Vector2(position.X, position.Y));
                    explosion.AutoInitialize(Game.GraphicsDevice, Game.Content, TankGame.spriteBatch);

                    explosion.UpdateOrder = 100;
                    explosion.DrawOrder = 100;
                    explosion.Visible = true;

                    ((TankGame)Game).soundManager.missleDropBullet.Play();
                }
            }

            exploded = true;
        }
    }
}
