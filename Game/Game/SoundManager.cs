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
    public class SoundManager : DrawableGameComponent
    {
        Song background;
        Song intro;
        Song ending;
        
        public bool backgroundPlaying;
        public bool introPlaying;
        public bool endingPlaying;

        public SoundEffect tankMove;
        public SoundEffect basicBullet;
        public SoundEffect heavyBullet;
        public SoundEffect missleDropBullet;
        public SoundEffect teleportBullet;
        public SoundEffect whistle;
        public SoundEffect missileScanning;
        public SoundEffect shoot;
        public SoundEffect timeBeep;
        public SoundEffect powerUpPickUp;
        public SoundEffect shieldBreaking;
        public SoundEffect shieldHit;

        public SoundManager(TankGame game)
            : base(game)
        {
            backgroundPlaying = false;
            introPlaying = false;
        }

        protected override void  LoadContent()
        {
            background = Game.Content.Load<Song>(@"Audio/BattleMusic");
            intro = Game.Content.Load<Song>(@"Audio/IntroMusic");
            ending = Game.Content.Load<Song>("Audio/KidsCheering");

            basicBullet = Game.Content.Load<SoundEffect>(@"Audio/basicExplosion");
            missleDropBullet = Game.Content.Load<SoundEffect>(@"Audio/MissileExplosion");
            missileScanning = Game.Content.Load<SoundEffect>(@"Audio/Scanning");
            heavyBullet = Game.Content.Load<SoundEffect>(@"Audio/HeavyExplosion");
            teleportBullet = Game.Content.Load<SoundEffect>(@"Audio/Teleporting");
            whistle = Game.Content.Load<SoundEffect>(@"Audio/Whistle");
            tankMove = Game.Content.Load<SoundEffect>(@"Audio/tankMoving");
            shoot = Game.Content.Load<SoundEffect>(@"Audio/TankShoot");
            timeBeep = Game.Content.Load<SoundEffect>(@"Audio/TimeBeep");
            powerUpPickUp = Game.Content.Load<SoundEffect>(@"Audio/PowerUpPickUp");
            shieldBreaking = Game.Content.Load<SoundEffect>(@"Audio/ShieldBreaking");
            shieldHit = Game.Content.Load<SoundEffect>(@"Audio/ShieldHit");

            base.LoadContent();
        }

        public void playBackground()
        {
            MediaPlayer.Play(background);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            backgroundPlaying = true;
        }

        public void stopBackground()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            backgroundPlaying = false;
        }

        public void playIntro()
        {
            MediaPlayer.Play(intro);
            MediaPlayer.IsRepeating = true;
            introPlaying = true; 
        }

        public void stopIntro()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            introPlaying = false;
        }

        public void playEnd()
        {
            MediaPlayer.Play(ending);
            MediaPlayer.IsRepeating = true;
            endingPlaying = true;
        }

        public void stopEnd()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            endingPlaying = false;
        }
    }
}
