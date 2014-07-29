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
    public class HeavyShot : Bullet
    {
        public HeavyShot(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            this.position = bulletPosition;
            this.type = BulletType.HeavyShot;

            damage = 75;
            mass = 2.0f;

            angle = 0;
            speed = Vector2.Zero;
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/HeavyShot");
        }

        public override void bulletCollided()
        {
            if (!outOfBounds)
            {
                if (explosion == null)
                {
                    explosion = new FlamePillar((TankGame)Game, new Vector2(position.X + (speed.X * 6), position.Y + (speed.Y * 6)));
                    explosion.AutoInitialize(Game.GraphicsDevice, Game.Content, TankGame.spriteBatch);

                    explosion.UpdateOrder = 100;
                    explosion.DrawOrder = 100;
                    explosion.Visible = true;

                    ((TankGame)Game).soundManager.heavyBullet.Play();
                }
            }

            exploded = true;
        }
    }
}
