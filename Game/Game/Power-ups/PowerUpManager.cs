using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class PowerUpManager :DrawableGameComponent
    {
        TankGame game;
        public List<PowerUp> powerList = new List<PowerUp>();
        PowerUp Health, HeavyShot, ScatterShot, ExtraMovement, ShieldPower;
        PowerUp ShieldLayer;
        Vector2 farLeftLocation, middleLeftLocation, rightLeftLocation,
                farRightLocation, middleRightLocation, leftRightLocation, 
                leftMiddleLocation, rightMiddleLocation, middleLocation;
        enum PowerUpEnum { Health, HeavyShot, ScatterShot, ExtraMovement, Shield}
        enum Location { Nolocation, Left, Right, Middle }

        Location lastLocation;
        bool shieldActive, play1HasShield, play2HasShield;

        UIManager uiManager;

        Rectangle shieldHealthRect;
        Texture2D shieldHealthTexture;
        int shieldHealth = 200;

        PowerUpEnum[] powerUpValues = {PowerUpEnum.Health, PowerUpEnum.HeavyShot,
                                       PowerUpEnum.ScatterShot, PowerUpEnum.ExtraMovement,
                                       PowerUpEnum.Shield };
        Location[] locationValues = { Location.Left, Location.Right, Location.Middle};

        Random random = new Random();

        public PowerUpManager(TankGame game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            uiManager = new UIManager(game);
            shieldActive = false;
            powerList.Clear();
            InitializeLocations();
            lastLocation = Location.Nolocation;
        }

        public void InitializeLocations()
        {
            farLeftLocation = new Vector2(520, 940);
            middleLeftLocation = new Vector2(600, 940);
            rightLeftLocation = new Vector2(680, 940);
            farRightLocation = new Vector2(1510, 882);
            middleRightLocation = new Vector2(1445, 882);
            leftRightLocation = new Vector2(1340, 882);
            leftMiddleLocation = new Vector2(1000, 825);
            middleLocation = new Vector2(1065, 825);
            rightMiddleLocation = new Vector2(1125, 825);
        }

        public void addPowerUps()
        {   
            Location randomLocation = locationValues[random.Next(locationValues.Length )];
            while (lastLocation == randomLocation)
            {
                randomLocation = locationValues[random.Next(locationValues.Length)];
            }

            SelectLocation(randomLocation);
            
            PowerUpEnum randomPower = powerUpValues[random.Next(powerUpValues.Length)];
            SelectPowerUp(randomPower);

            lastLocation = randomLocation;
        }

        public void Draw()
        {
            foreach (PowerUp item in powerList)
            {
                item.Draw(TankGame.spriteBatch);
            }

            if (shieldActive == true)
            {
                TankGame.spriteBatch.Draw(shieldHealthTexture, shieldHealthRect, Color.White);
            }
        }

        public void InitializePowerUps(Vector2 location)
        {
            Health = new PowerUp(game, location, "Images/HealthPackTexture");
            HeavyShot = new PowerUp(game, location, "Images/AmmoPackTexture");
            ScatterShot = new PowerUp(game, location, "Images/ScatterShotAmmo");
            ExtraMovement = new PowerUp(game, location, "Images/MovementPowerUp");
            ShieldPower = new PowerUp(game, location, "Images/ShieldPowerUp");
            shieldHealthTexture = Game.Content.Load<Texture2D>(@"Images/ShieldBar");
        }

        private void SelectLocation(Location location)
        {   
            Location randomLocation = locationValues[random.Next(locationValues.Length)];
            switch (location)
            {
                case Location.Left:
                    SelectLeftLocation(randomLocation);
                    break;

                case Location.Middle:
                    SelectMiddleLocation(randomLocation);
                    break;

                case Location.Right:
                    SelectRightLocation(randomLocation);
                    break;
            }
        }

        private void SelectLeftLocation(Location location)
        {
            switch (location)
            {
                case Location.Left:
                    InitializePowerUps(farLeftLocation);
                    break;

                case Location.Middle:
                    InitializePowerUps(middleLeftLocation);
                    break;

                case Location.Right:
                    InitializePowerUps(rightLeftLocation);
                    break;
            }
        }

        private void SelectRightLocation(Location location)
        {
            switch (location)
            {
                case Location.Left:
                    InitializePowerUps(leftRightLocation);
                    break;

                case Location.Middle:
                    InitializePowerUps(middleRightLocation);
                    break;

                case Location.Right:
                    InitializePowerUps(farRightLocation);
                    break;
            }
        }

        private void SelectMiddleLocation(Location location)
        {
            switch (location)
            {
                case Location.Left:
                    InitializePowerUps(leftMiddleLocation);
                    break;

                case Location.Middle:
                    InitializePowerUps(middleLocation);
                    break;

                case Location.Right:
                    InitializePowerUps(rightMiddleLocation);
                    break;
            }
        }

        private void SelectPowerUp(PowerUpEnum powerup)
        {
            switch (powerup)
            {
                case PowerUpEnum.Health:
                    powerList.Add(Health);
                    break;

                case PowerUpEnum.ScatterShot:
                    powerList.Add(ScatterShot);
                    break;

                case PowerUpEnum.HeavyShot:
                    powerList.Add(HeavyShot);
                    break;

                case PowerUpEnum.ExtraMovement:
                    powerList.Add(ExtraMovement);
                    break;

                case PowerUpEnum.Shield:
                    powerList.Add(ShieldPower);
                    break;
            }
        }

        public void PowerUpCollision(Tank tank, Tank otherTank)
        {
            for (int i = 0; i < powerList.Count; i++)
            {
                if (play1HasShield == true)
                {
                    shieldLogic(tank, otherTank, powerList[i]);
                }

                if (play2HasShield == true)
                {
                    shieldLogic(otherTank, tank, powerList[i]);
                }

                if (powerList.Count == 1)
                {
                    if (powerList[i].collisionRect.Intersects(tank.collisionRect) & (powerList[i] != ShieldLayer))
                    {
                        game.soundManager.powerUpPickUp.Play();
                        PowerUpEffect(powerList[i], tank);
                        powerList.RemoveAt(i);
                        if (shieldActive == true)
                            play1HasShield = true;
                    }
                }

                if (powerList.Count == 1)
                {
                    if (powerList[i].collisionRect.Intersects(otherTank.collisionRect) & (powerList[i] != ShieldLayer))
                    {
                        game.soundManager.powerUpPickUp.Play();
                        PowerUpEffect(powerList[i], otherTank);
                        powerList.RemoveAt(i);
                        if (shieldActive == true)
                            play2HasShield = true;
                    }
                }
            }
        }

        public void PowerUpEffect(PowerUp powerUp, Tank tank)
        {
            if (powerUp == Health)
            {
                game.uiManager.floatingPowerUpText("+100 Health", tank.getTankPos());
                tank.healTank(100);
            }
            if (powerUp == HeavyShot)
            {
                game.uiManager.floatingPowerUpText("Heavy Shot +1", tank.getTankPos());
                tank.inventory.incrementShot(BulletType.HeavyShot);
            }
            if (powerUp == ScatterShot)
            {
                game.uiManager.floatingPowerUpText("ScatterShot +1", tank.getTankPos());
                tank.inventory.incrementShot(BulletType.ScatterShot);
            }
            if (powerUp == ExtraMovement)
            {
                game.uiManager.floatingPowerUpText("Move Limit +3", tank.getTankPos());
                tank.moveLimit += 3;
            }
            if (powerUp == ShieldPower)
            {
                game.uiManager.floatingPowerUpText("Shield", tank.getTankPos());
                updateShield(tank.getTankPos());
            }
        }

        public void updateShield(Vector2 tankLocation)
        {
            //Add Shield
            shieldHealthRect = new Rectangle((int)tankLocation.X, (int)tankLocation.Y, 200, 20);
            ShieldLayer = new PowerUp(game, tankLocation, "Images/Shield");
            powerList.Add(ShieldLayer);
            shieldActive = true;
        }

        private void shieldLogic(Tank tank, Tank otherTank, PowerUp powerUp)
        {
            shieldHealthRect.X = (int)tank.getTankPos().X - 20;
            shieldHealthRect.Y = (int)tank.getTankPos().Y - 40;
            ShieldLayer.powerUpPosition = tank.getTankPos() - new Vector2(8, 20);
            if (powerUp.collisionRect.Intersects(otherTank.bullet.collisionRect))
            {
                otherTank.bullet.outOfBounds = true;
                if (otherTank.bullet.type == BulletType.TeleportShot)
                    otherTank.bullet.speed.X = -(otherTank.bullet.speed.X);

                shieldHealth -= otherTank.bullet.damage / 10;
                shieldHealthRect.Width = shieldHealth;
                game.soundManager.shieldHit.Play();

                if (shieldHealth <= 100)
                    ShieldLayer.powerUpPic = Game.Content.Load<Texture2D>(@"Images/BrokenShield");

                if (shieldHealth <= 0)
                {
                    game.soundManager.shieldBreaking.Play();
                    powerList.Remove(powerUp);
                    shieldActive = false;
                    play1HasShield = false;
                    play2HasShield = false;
                }
            }
        }
    }
}
