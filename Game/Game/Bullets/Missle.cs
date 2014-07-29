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
    public class Missle : Bullet
    {
        public Missle(TankGame game, Vector2 bulletPosition)
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
            bulletImage = Game.Content.Load<Texture2D>(@"Images/basicBullet");
        }
    }
}
