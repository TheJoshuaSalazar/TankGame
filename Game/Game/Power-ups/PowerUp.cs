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
    public class PowerUp : DrawableGameComponent
    {
        TankGame game;
        public Vector2 powerUpPosition;
        //Rectangle powerUpRec;
        public Texture2D powerUpPic;
        int collisionOffset = 1; 

        public PowerUp(TankGame game, Vector2 pos, String type)
            : base(game)
        {
            this.game = game;
            this.powerUpPosition = pos;
            powerUpPic = Game.Content.Load<Texture2D>(@type);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(powerUpPic, powerUpPosition, Color.White);
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)powerUpPosition.X + collisionOffset,
                                (int)powerUpPosition.Y + collisionOffset,
                                powerUpPic.Width - (collisionOffset * 2),
                                powerUpPic.Height - (collisionOffset * 2));
            }
        }
    }
}
