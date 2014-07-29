using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Game
{
    public class Inventory : Microsoft.Xna.Framework.GameComponent
    {
        int heavyShot;
        int scatterShot;
        int missileDropShot;

        public Inventory(TankGame game)
            : base(game)
        {
            heavyShot = 1;
            scatterShot = 1;
            missileDropShot = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public int getShotCount(BulletType type)
        {
            switch (type)
            {
                case BulletType.HeavyShot:
                    return heavyShot;
                case BulletType.ScatterShot:
                    return scatterShot;
                case BulletType.MissileDrop:
                    return missileDropShot;
                default:
                    return 999;
            }
        }

        public void decrementShot(BulletType type)
        {
            switch (type)
            {
                case BulletType.HeavyShot:
                    heavyShot--;
                    break;
                case BulletType.ScatterShot:
                    scatterShot--;
                    break;
                case BulletType.MissileDrop:
                    missileDropShot--;
                    break;
                default:
                    break;
            }
        }

        public void incrementShot(BulletType type)
        {
            switch (type)
            {
                case BulletType.HeavyShot:
                    heavyShot++;
                    break;
                case BulletType.ScatterShot:
                    scatterShot++;
                    break;
                case BulletType.MissileDrop:
                    missileDropShot++;
                    break;
                default:
                    break;
            }
        }

        public BulletType findNextShotType()
        {
            if (heavyShot > 0)
                return BulletType.HeavyShot;
            else if (scatterShot > 0)
                return BulletType.ScatterShot;
            else if (missileDropShot > 0)
                return BulletType.MissileDrop;
            else
                return BulletType.BasicBullet;
        }

        public BulletType nextWeaponType(BulletType bulletType)
        {
            //Returns Heavy Shot if passed type is Basic
            if (bulletType == BulletType.BasicBullet && heavyShot > 0)
                return BulletType.HeavyShot;
            //Returns Scatter Shot if type passed is Basic & Heavy Shot is out
            else if (bulletType == BulletType.BasicBullet && heavyShot < 1 && scatterShot > 0)
                return BulletType.ScatterShot;
            //Returns Teleport Shot if type passed is Basic & Heavy & Scatter are out
            else if (bulletType == BulletType.BasicBullet && heavyShot < 1 && scatterShot < 1)
                return BulletType.TeleportShot;
            //Returns Scatter Shot if type passed is Heavy
            if (bulletType == BulletType.HeavyShot && scatterShot > 0)
                return BulletType.ScatterShot;
            //Returns Teleport if passed type is Heavy & Scatter Shot is out
            else if (bulletType == BulletType.HeavyShot && scatterShot < 1)
                return BulletType.TeleportShot;
            //Return Teleport Shot if type passed is Scatter Shot
            if (bulletType == BulletType.ScatterShot)
                return BulletType.TeleportShot;
            //Returns MissileDrop Shot if type passed is Teleport Shot
            if (bulletType == BulletType.TeleportShot && missileDropShot > 0)
                return BulletType.MissileDrop;
            //Returns Basic Bullet if type passed it Teleport Shot & MissileDrop is out
            else if (bulletType == BulletType.TeleportShot && missileDropShot < 1)
                return BulletType.BasicBullet;
            //Returns Basic Bullet if type passed is MissileDrop Shot
            if (bulletType == BulletType.MissileDrop)
                return BulletType.BasicBullet;

            //All else fails, basic bullet...
            return BulletType.BasicBullet;
        }

        public BulletType previousWeaponType(BulletType bulletType)
        {
            //Returns MissileDrop Shot if passed type is Basic
            if (bulletType == BulletType.BasicBullet && missileDropShot > 0)
                return BulletType.MissileDrop;
            //Returns Teleport Shot if type passed it Basic & MissileDrop is out
            else if (bulletType == BulletType.BasicBullet && missileDropShot == 0)
                return BulletType.TeleportShot;
            //Returns Scatter Shot if passed type is Teleport
            if (bulletType == BulletType.TeleportShot && scatterShot > 0)
                return BulletType.ScatterShot;
            //Returns Heavy Shot if type passed is Teleport & Scatter Shot is out
            else if (bulletType == BulletType.TeleportShot && scatterShot == 0 && heavyShot > 0)
                return BulletType.HeavyShot;
            //Returns Basic Bullet if passed type is Teleport Shot & Scatter & Heavy is out.
            else if (bulletType == BulletType.TeleportShot && scatterShot == 0 && heavyShot == 0)
                return BulletType.BasicBullet;
            //Returns Basic Shot if type passed is Heavy
            if (bulletType == BulletType.HeavyShot)
                return BulletType.BasicBullet;
            //Return Heavy Shot if type passed is Scatter Shot
            if (bulletType == BulletType.ScatterShot && heavyShot > 0)
                return BulletType.HeavyShot;
            //Return Basic Shot if type passed is Scatter Shot & Heavy Shot is out
            else if (bulletType == BulletType.ScatterShot && heavyShot == 0)
                return BulletType.BasicBullet;
            //Returns Teleport Shot if type passed is MissileDrop
            if (bulletType == BulletType.MissileDrop)
                return BulletType.TeleportShot;

            //All else fails, basic bullet...
            return BulletType.BasicBullet;
        }

        public void resetWeapons()
        {
            heavyShot = 1;
            scatterShot = 1;
            missileDropShot = 1;
        }

        public void debugShot()
        {
            heavyShot = 999;
            scatterShot = 999;
            missileDropShot = 999;
        }
    }
}
