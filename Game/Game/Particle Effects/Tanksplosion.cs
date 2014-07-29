using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
#if (WINDOWS)
    [Serializable]
#endif
    public class Tanksplosion : BasicParticleSystem
    {
        float currentRotation = 0;
        Vector2 originalVector = new Vector2(1, 0);

        public Tanksplosion(TankGame cGame, Vector2 position)
            : base(cGame)
        {
            this.position = new Vector3(position.X, position.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1000, 1000, "Textures/Flame", cSpriteBatch);

            Name = "Tanksplosion";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleProperties;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocity, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = 2.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.1f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 9999;
            Emitter.PositionData.Position = position;
        }

        public override void InitializeParticleProperties(DefaultSpriteParticle cParticle)
        {
            cParticle.Lifetime = DPSFHelper.RandomNumberBetween(1, 2);

            cParticle.Position = Emitter.PositionData.Position;

            cParticle.StartSize = 40;
            cParticle.EndSize = 10;

            cParticle.StartColor = Color.Yellow;
            cParticle.EndColor = Color.DarkRed;
        }

        public void UpdateParticleVelocity(DPSFDefaultBaseParticle cParticle, float fElapsedTimeInSeconds)
        {
            currentRotation += (float)Math.PI / 4;

            Vector2 rotatedRandomVector = TankGame.rotateVector(originalVector, currentRotation);

            cParticle.Velocity = new Vector3(rotatedRandomVector.X * 800, rotatedRandomVector.Y * 800, 0);
        }
    }
}