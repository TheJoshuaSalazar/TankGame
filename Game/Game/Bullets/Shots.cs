using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    public class Shots : Bullet
    {
        public Shots(TankGame game, Vector2 bulletPosition, Vector2 scatterDirection)
            : base(game)
        {
            this.position = bulletPosition;
            this.type = BulletType.ScatterShot;
            
            damage = 20;
            mass = .75f;

            angle = 0;
            speed = scatterDirection;
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/ScatterShots");
        }
    }
}
