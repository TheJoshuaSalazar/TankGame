//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

//namespace Game
//{
//    class TankManager : Microsoft.Xna.Framework.DrawableGameComponent
//    {
//        SpriteBatch spriteBatch;
//        Tank player;
//        List<Tank> tankList = new List<Tank>();

//        public TankManager(TankGame game)
//            : base(game)
//        { }

//        public override void Initialize()
//        {
            
//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

//            player = new Tank(Game.Content.Load<Texture2D>(@"Images/tank"),
//                new Point(75, 75), new Point(0, 0), new Point(6, 8), 3,
//                new Vector2(4, 0), Vector2.Zero, 1);
                
//            base.LoadContent();
//        }

//        public override void Update(GameTime gameTime)
//        {
//            player.Update(gameTime, Game.Window.ClientBounds);

//            foreach (Tank s in tankList)
//            {
//                s.Update(gameTime, Game.Window.ClientBounds);

//                if (s.collisionRect.Intersects(player.collisionRect))
//                    Game.Exit();
//            }

//            base.Update(gameTime);
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

//            player.Draw(gameTime, spriteBatch);

//            foreach (Tank t in tankList)
//                t.Draw(gameTime, spriteBatch);

//            spriteBatch.End();

//            base.Draw(gameTime);
//        }
//    }
//}
