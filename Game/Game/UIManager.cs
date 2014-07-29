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

namespace Game
{
    public class UIManager : DrawableGameComponent
    {
        TankGame game;

        public string powerUpString;
        bool powerOnScreen;

        SpriteFont font;
        SpriteFont bulletFont;
        SpriteFont DamagePowerFont;
        SpriteFont biggerFont; 

        public Rectangle player1LifeRect;
        public Rectangle player2LifeRect;

        public Rectangle player1MoveRect;
        public Rectangle player2MoveRect; 

        public Rectangle totalLifeRect1;
        public Rectangle totalLifeRect2;
        public Rectangle bulletDisplayRect;
        public Rectangle hubRect;

        public Vector2 player1TextPosition;
        public Vector2 player2TextPosition;
        public Vector2 bulletTextPosition;
        public Vector2 powerTextPosition;
        public Vector2 powerUpTextPosition;

        public List<DamageText> damageText;

        Texture2D player1LifeRectTexture;
        Texture2D player2LifeRectTexture;
        Texture2D player1MoveRectTexture;
        Texture2D player2MoveRectTexture; 
        Texture2D totalLifeRect1Texture;
        Texture2D totalLifeRect2Texture;
        Texture2D bulletDisplayTexture;
        Texture2D Hub;

        public int POWER_BAR_WIDTH = 800;
        public int POWER_BAR_HEIGHT = 40;
        public const int EMPTY_BAR_PIECE_WIDTH = 8;

        public int powerBarX;
        public int powerBarY;

        public Rectangle powerBarPosition;
        public List<Rectangle> emptyPowerBarPosition;

        public int endOfPowerBar;
        int currentEmptyPowerBar;
        public int numEmptyPowerBarPieces;

        public string bullet;
        
        public bool fullPower;

        KeyboardState keyState;

        SpriteBatch spriteBatch;

        const int MAX_HEALTH = 200;

        const float POWER_CHARGE = .05f;

        public UIManager(TankGame game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            powerUpString = "";
            powerOnScreen = false;

            player1LifeRect = new Rectangle((int)game.camera.Origin.X - 430, (int)game.camera.Origin.Y + 713, 200, 20);
            totalLifeRect1 = new Rectangle((int)game.camera.Origin.X - 430, (int)game.camera.Origin.Y + 713, 200, 20);
            player1MoveRect = new Rectangle((int)game.player1Tank.getTankPos().X, (int)game.player1Tank.getTankPos().Y + 100, 90, 20);

            player2LifeRect = new Rectangle((int)game.camera.Origin.X + 234, (int)game.camera.Origin.Y + 713, 200, 20);
            totalLifeRect2 = new Rectangle((int)game.camera.Origin.X + 234, (int)game.camera.Origin.Y + 713, 200, 20);
            player2MoveRect = new Rectangle((int)game.player2Tank.getTankPos().X, (int)game.player2Tank.getTankPos().Y + 100, 90, 20);

            player1TextPosition = new Vector2((int)game.camera.Origin.X - 395, (int)game.camera.Origin.Y + 713);
            player2TextPosition = new Vector2((int)game.camera.Origin.X + 274, (int)game.camera.Origin.Y + 713);

            bulletDisplayRect = new Rectangle((int)game.camera.Origin.X - 200, (int)game.camera.Origin.Y + 120, 400, 100);
            bulletTextPosition = new Vector2(bulletDisplayRect.X + 75, bulletDisplayRect.Y + 30);
            bullet = "BasicBullet";
            
            powerTextPosition = new Vector2((int)game.camera.Origin.X - 40, (int)game.camera.Origin.Y + 800);

            powerUpTextPosition = new Vector2(-100, -100);

            damageText = new List<DamageText>();

            powerBarX = (int)game.camera.Origin.X - 410;
            powerBarY = (int)game.camera.Origin.Y + 752;

            powerBarPosition = new Rectangle(powerBarX, powerBarY, POWER_BAR_WIDTH, POWER_BAR_HEIGHT);

            hubRect = new Rectangle((int)game.camera.Origin.X - 465, (int)game.camera.Origin.Y + 680, 935, 211);

            emptyPowerBarPosition = new List<Rectangle>();

            endOfPowerBar = powerBarPosition.X + POWER_BAR_WIDTH;

            currentEmptyPowerBar = POWER_BAR_WIDTH;
            numEmptyPowerBarPieces = 0;

            fullPower = false;

            for (int i = 0; i < POWER_BAR_WIDTH; i += EMPTY_BAR_PIECE_WIDTH)
            {
                emptyPowerBarPosition.Add(new Rectangle(i + powerBarX, powerBarY,
                    EMPTY_BAR_PIECE_WIDTH, POWER_BAR_HEIGHT));
                numEmptyPowerBarPieces++;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player1MoveRectTexture = Game.Content.Load<Texture2D>(@"Images/MoveBar");
            player2MoveRectTexture = Game.Content.Load<Texture2D>(@"Images/MoveBar");

            totalLifeRect2Texture = Game.Content.Load<Texture2D>(@"Images/EmptyLifeMeter");
            totalLifeRect1Texture = Game.Content.Load<Texture2D>(@"Images/EmptyLifeMeter");

            player1LifeRectTexture = Game.Content.Load<Texture2D>(@"Images/HealthBar");

            totalLifeRect2Texture = Game.Content.Load<Texture2D>(@"Images/EmptyLifeMeter");
            player2LifeRectTexture = Game.Content.Load<Texture2D>(@"Images/HealthBar");

            bulletDisplayTexture = Game.Content.Load<Texture2D>(@"Images/AmmoDisplay");

            Hub = Game.Content.Load<Texture2D>(@"Images/hub");

            font = Game.Content.Load<SpriteFont>(@"Fonts/font");
            biggerFont = Game.Content.Load<SpriteFont>(@"Fonts/LargeFont");
            bulletFont = Game.Content.Load<SpriteFont>(@"Fonts/BulletFont");
            DamagePowerFont = Game.Content.Load<SpriteFont>(@"Fonts/Damage-PowerFont");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            DebuggingHealth();

            if (damageText.Count > 0)
            {
                for (int i = 0; i < damageText.Count; i++)
                {
                    if (damageText[i].textTime >= 0)
                    {
                        damageText[i].textTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }

            //Moving MoveBars to Tank Pos.
            player1MoveRect.X = (int)game.player1Tank.getTankPos().X;
            player1MoveRect.Y = (int)game.player1Tank.getTankPos().Y + 75;
            player2MoveRect.X = (int)game.player2Tank.getTankPos().X;
            player2MoveRect.Y = (int)game.player2Tank.getTankPos().Y + 75;

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Hub, hubRect, Color.White);

            batch.Draw(totalLifeRect1Texture, totalLifeRect1, Color.White);
            batch.Draw(player1LifeRectTexture, player1LifeRect, Color.White);

            batch.Draw(player1MoveRectTexture, player1MoveRect, Color.White);
           
            batch.Draw(totalLifeRect2Texture, totalLifeRect2, Color.White);
            batch.Draw(player2LifeRectTexture, player2LifeRect, Color.White);

            batch.Draw(player2MoveRectTexture, player2MoveRect, Color.White);
            
            batch.DrawString(font, "Player 1 Health", player1TextPosition, Color.White);
            batch.DrawString(font, "Player 2 Health", player2TextPosition, Color.White);
            
            batch.Draw(bulletDisplayTexture, bulletDisplayRect, Color.White);
            batch.DrawString(bulletFont, bullet , bulletTextPosition, Color.Black);

            batch.DrawString(DamagePowerFont, powerUpString, powerUpTextPosition, Color.White);

            foreach (DamageText dT in damageText)
            {
                if(dT.textTime > 0)
                {
                    batch.DrawString(DamagePowerFont, dT.damage, dT.position, Color.White);
                }
            }

            batch.DrawString(font, "Power Bar", powerTextPosition, Color.White);
           
            batch.Draw(Game.Content.Load<Texture2D>(@"Images/PowerBar"), powerBarPosition, Color.White);

            foreach (var emptyBar in emptyPowerBarPosition)
            {
                TankGame.spriteBatch.Draw(Game.Content.Load<Texture2D>(@"Images/EmptyPowerBar"), emptyBar, Color.White);
            }
        }

        public void resetHealth()
        {
            game.player1Tank.health = MAX_HEALTH;
            game.player2Tank.health = MAX_HEALTH;
            player1LifeRect.Width = MAX_HEALTH;
            player2LifeRect.Width = MAX_HEALTH;
        }

        public void calculatePower()
        {
            if (!fullPower)
            {
                if (emptyPowerBarPosition.Count > 0)
                {
                    emptyPowerBarPosition.RemoveAt(0);
                    currentEmptyPowerBar -= EMPTY_BAR_PIECE_WIDTH;
                    numEmptyPowerBarPieces--;
                    ((TankGame)Game).power += POWER_CHARGE;
                }
                else
                {
                    currentEmptyPowerBar = 0;
                    fullPower = true;
                }
            }

            if (fullPower)
            {
                currentEmptyPowerBar += EMPTY_BAR_PIECE_WIDTH;
                emptyPowerBarPosition.Add(new Rectangle(endOfPowerBar - currentEmptyPowerBar,
                    powerBarY, EMPTY_BAR_PIECE_WIDTH, POWER_BAR_HEIGHT));
                numEmptyPowerBarPieces++;
                ((TankGame)Game).power -= POWER_CHARGE;

                if (currentEmptyPowerBar >= POWER_BAR_WIDTH)
                {
                    emptyPowerBarPosition.Reverse();
                    currentEmptyPowerBar = POWER_BAR_WIDTH;
                    fullPower = false;
                }
            }

            if (((TankGame)Game).power < POWER_CHARGE)
            {
                ((TankGame)Game).power = POWER_CHARGE;
            }
        }

        public void resetPowerBar()
        {
            numEmptyPowerBarPieces = 0;

            while (emptyPowerBarPosition.Count > 0)
            {
                emptyPowerBarPosition.RemoveAt(0);
            }

            for (int i = 0; i < POWER_BAR_WIDTH; i += EMPTY_BAR_PIECE_WIDTH)
            {
                emptyPowerBarPosition.Add(new Rectangle(i + powerBarX, powerBarY,
                    EMPTY_BAR_PIECE_WIDTH, POWER_BAR_HEIGHT));
                numEmptyPowerBarPieces++;
            }

            currentEmptyPowerBar = 0;
            ((TankGame)Game).power = 0;
            fullPower = false;
        }

        public void takeBulletType(BulletType type, int count)
        {
            bullet = type.ToString();
            if (count != 999)
                bullet += (" Ammo:" + count);
        }

        public void floatingPowerUpText(string power, Vector2 pos)
        {
            powerUpString = power;
            powerUpTextPosition = pos;
            powerOnScreen = true;
        }

        public void floatingDamageText(string damage, Vector2 pos)
        {
            damageText.Add(new DamageText(damage, pos));
        }

        public void moveFloatingText()
        {
            if (powerUpTextPosition.Y > powerUpTextPosition.Y - 50)
            {
                powerUpTextPosition.Y--;
            }

            foreach(DamageText dT in damageText)
            {
                dT.position.Y--;
            }
        }

        public void removeText()
        {
            powerUpTextPosition = new Vector2(-100, -100);

            while (damageText.Count > 0)
            {
                damageText.RemoveAt(damageText.Count - 1);
            }
        }       

        #region debugCode

        //DEBUG_________________________________________________________CODE

        public void DebuggingHealth()
        {
            //Debug to test Health Bar shrinkage.
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                debugDamage(1, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                debugDamage(2, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                debugDamage(1, 1);
                debugDamage(2, 1);
            }

            //Debug Heal Player
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                debugHeal(1, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                debugHeal(2, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                debugHeal(1, 10);
                debugHeal(2, 10);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                resetHealth();
            }

            //Logic to shrink move bar
            if (player1MoveRect.Width > game.player1Tank.moveLimit * 30 && game.player1Tank.moveLimit != 0)
            {
                player1MoveRect.Width -= 9;
            }

            if (player2MoveRect.Width > game.player2Tank.moveLimit * 30 && game.player2Tank.moveLimit != 0)
            {
                player2MoveRect.Width -= 9;
            }

            //Logic to shrink health bar
            if (player1LifeRect.Width > game.player1Tank.health && game.player1Tank.health != 0)
            {
                player1LifeRect.Width -= 1;
            }

            if (player2LifeRect.Width > game.player2Tank.health && game.player2Tank.health != 0)
            {
                player2LifeRect.Width -= 1;
            }

            //Logic to grow health bar
            while (player1LifeRect.Width < game.player1Tank.health && game.player1Tank.health <= 200)
            {
                player1LifeRect.Width += 1;
            }
            while (player2LifeRect.Width < game.player2Tank.health && game.player2Tank.health <= 200)
            {
                player2LifeRect.Width += 1;
            }
        }

        public void debugDamage(int playerNum, int damage)
        {
            if (playerNum == 1)
            {
                game.player1Tank.takeDamage(damage);
            }
            else if (playerNum == 2)
            {
                game.player2Tank.takeDamage(damage);
            }
        }

        public void debugHeal(int playerNum, int heal)
        {
            if (playerNum == 1)
            {
                game.player1Tank.healTank(heal);
            }
            else if (playerNum == 2)
            {
                game.player2Tank.healTank(heal);
            }
        }

        public void resetMove()
        {
            player1MoveRect.Width = 90;
            //player1MoveRect.X = (int)game.player1Tank.getTankPos().X;
            //player1MoveRect.Y = (int)game.player1Tank.getTankPos().Y + 75;

            player2MoveRect.Width = 90;
            //player2MoveRect.X = (int)game.player1Tank.getTankPos().X;
            //player2MoveRect.Y = (int)game.player1Tank.getTankPos().Y + 75;
        }

        #endregion
    }
}
