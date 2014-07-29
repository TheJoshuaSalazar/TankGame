using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    class TeleportShot : Bullet
    {
        TeleportTrail trail;

        public TeleportShot(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            this.position = bulletPosition;
            this.type = BulletType.TeleportShot;

            damage = 10;
            mass = 1.0f;

            angle = 0;
            speed = Vector2.Zero;

            trail = new TeleportTrail(game, this.position);
            trail.AutoInitialize(game.GraphicsDevice, game.Content, TankGame.spriteBatch);

            trail.UpdateOrder = 100;
            trail.DrawOrder = 100;
            trail.Visible = true;
        }

        protected override void setImage()
        {
            bulletImage = Game.Content.Load<Texture2D>(@"Images/TeleportShot");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (trail != null)
            {
                trail.changeEmitterPosition(new Vector2(position.X, position.Y));

                trail.ParticleUpdate(gameTime);
            }
        }

        protected override void checkBounds()
        {
            if (position.X < 0 || position.X > 2048)
            {
                speed.X = -speed.X;
            }
        }

        public override void bulletCollided()
        {
            if (explosion == null)
            {
                explosion = new TeleportBlast((TankGame)Game, new Vector2(position.X + (bulletImage.Width / 2),
                    position.Y + (bulletImage.Height / 2) - 10));
                explosion.AutoInitialize(Game.GraphicsDevice, Game.Content, TankGame.spriteBatch);

                explosion.UpdateOrder = 100;
                explosion.DrawOrder = 100;
                explosion.Visible = true;

                ((TankGame)Game).soundManager.teleportBullet.Play();
            }

            if (trail != null)
            {
                trail.Dispose();
                trail = null;
            }

            exploded = true;
        }
    }
}
