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
    class MissleDrop : Bullet
    {
        Vector2 tankPosition;
        float distanceCheck;
        Missle missile = null;
        float missileOffset;

        public MissleDrop(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            Bullet basic = new Bullet(game); 

            this.position = bulletPosition;
            this.type = BulletType.MissileDrop;

            damage = 20;
            mass = 1.0f;

            angle = 0;
            speed = Vector2.Zero;
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/basicBullet");
        }

        public override void bulletCollided()
        {
            distanceCheck = ((tankPosition - this.position).Length());

            if (distanceCheck < 300)
            {
                missile = new Missle((TankGame)Game, new Vector2(tankPosition.X + missileOffset));
                Game.Components.Add(missile);
            }

            exploded = true;
        }

        public void TankPosition(Vector2 getTankPosition, float offset)
        {
            tankPosition = getTankPosition;
            missileOffset = offset;
        }

        public override void Update(GameTime gameTime)
        {
            if (missile != null)
            {
                missile.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (missile != null)
            {
                missile.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
