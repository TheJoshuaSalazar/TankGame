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
    public enum BulletType { NullBullet, BasicBullet, ScatterShot, HeavyShot, TeleportShot, MissileDrop, Missile }

    public class Bullet : DrawableGameComponent
    {
        protected Texture2D bulletImage;
        protected Vector2 position;
        protected Rectangle rectangle;
        protected float angle;
        protected Vector2 rotationOrigin;
        public Vector2 speed;
        public BulletType type { get; set; }
        public int damage { get; set; }
        public float mass { get; set; }
        protected float bulletTime = 0;

        public bool outOfBounds = false;
        public bool exploded = false;
        public bool damageDealt = false;
        bool played = false;

        public Color[] textureData;

        public BasicParticleSystem explosion;

        public Bullet(TankGame game)
            : base(game)
        {
            type = BulletType.NullBullet;
        }

        public Bullet(TankGame game, Vector2 bulletPosition)
            : base(game)
        {
            this.position = bulletPosition;
            this.type = BulletType.BasicBullet;

            damage = 50;
            mass = 1.0f;

            angle = 0;
            speed = Vector2.Zero;
        }

        protected override void LoadContent()
        {
            setImage();

            rectangle = new Rectangle((int)position.X, (int)position.Y, bulletImage.Width, bulletImage.Height);
            textureData = new Color[bulletImage.Width * bulletImage.Height];
            bulletImage.GetData(textureData);

            rotationOrigin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

            explosion = null;

            base.LoadContent();
        }

        protected virtual void setImage()
        {
            if (type == BulletType.BasicBullet)
            {
                bulletImage = Game.Content.Load<Texture2D>(@"Images/basicBullet");
            }
        }

        public override void Update(GameTime gameTime)
        {
            position += speed;

            angle = (float)Math.Atan2(speed.Y, speed.X);

            bulletTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            applyGravity();

            checkBounds();

            if (speed.Y <= 0 && !played)
            {
                ((TankGame)Game).soundManager.whistle.Play(1.0f,1.0f,0.0f);
                played = true;
            }

            base.Update(gameTime);
        }

        protected virtual void applyGravity()
        {
            if (bulletTime > .1f & !exploded)
            {
                speed.Y += TankGame.GRAVITY * mass;

                bulletTime = 0;
            }
        }

        protected virtual void checkBounds()
        {
            if (position.X < 0 || position.X > 2048)
            {
                outOfBounds = true;
            }
        }

        public virtual void Draw()
        {
            //TankGame.spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/DebugPixel"), collisionRect, Color.White);

            if (!exploded)
            {
                TankGame.spriteBatch.Draw(bulletImage, position, null, Color.White, angle, rotationOrigin, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
        
        public bool intersectPixels(Rectangle rectangle, Color[] data)
        {
            int top = Math.Max(this.rectangle.Top, rectangle.Top);
            int bottom = Math.Min(this.rectangle.Bottom, rectangle.Bottom);
            int left = Math.Max(this.rectangle.Left, rectangle.Left);
            int right = Math.Min(this.rectangle.Right, rectangle.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = this.textureData[(x - this.rectangle.Left) + (y - this.rectangle.Top) * this.rectangle.Width];
                    Color color2 = data[(x - rectangle.Left) + (y - rectangle.Top) * rectangle.Width];

                    if (color1.A != 0 && color2.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void bulletCollided()
        {
            if (!outOfBounds)
            {
                if (explosion == null)
                {
                    explosion = new BasicBlast((TankGame)Game, new Vector2(position.X + (speed.X * 6), position.Y + (speed.Y * 6)));
                    explosion.AutoInitialize(Game.GraphicsDevice, Game.Content, TankGame.spriteBatch);

                    explosion.UpdateOrder = 100;
                    explosion.DrawOrder = 100;
                    explosion.Visible = true;

                    ((TankGame)Game).soundManager.basicBullet.Play();
                }
            }

            exploded = true;
        }

        public Vector2 getBulletPosition()
        {
            return position;
        }
        
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)position.X - bulletImage.Width / 2,
                                (int)position.Y - bulletImage.Height / 2,
                                bulletImage.Width,
                                bulletImage.Height);
            }
        }
    }
}