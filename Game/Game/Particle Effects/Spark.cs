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
    public class Spark : BasicParticleSystem
    {
        float offsetX = 0;
        float offsetY = 0;
        int maxParticles = 0;
        float lifeTime = 0;
        float currentRotation = 0;
        Vector2 originalVector = new Vector2(1, 0);

        public Spark(TankGame cGame, Vector2 position, int maxParticles, float lifeTime)
            : base(cGame)
        {
            offsetX = DPSFHelper.RandomNumberBetween(-20, 20);
            offsetY = DPSFHelper.RandomNumberBetween(-20, 20);
            this.position = new Vector3(position.X + offsetX, position.Y + offsetY, 0);
            this.maxParticles = maxParticles;
            this.lifeTime = lifeTime;
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, maxParticles, maxParticles, "Textures/Pixel", cSpriteBatch);

            Name = "Spark";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleProperties;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocityUsingFriction);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Repeat;
            ParticleSystemEvents.LifetimeData.Lifetime = lifeTime;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.1f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 9999;

            Emitter.PositionData.Position = position;
        }

        public override void InitializeParticleProperties(DefaultSpriteParticle cParticle)
        {
            cParticle.Lifetime = DPSFHelper.RandomNumberBetween(.1f, lifeTime / 2);

            cParticle.Position = Emitter.PositionData.Position;

            currentRotation += (float)Math.PI / 10;
            Vector2 rotatedRandomVector = TankGame.rotateVector(originalVector, currentRotation);

            cParticle.Velocity = new Vector3(rotatedRandomVector.X * 60, rotatedRandomVector.Y * 60, 0);

            cParticle.Size = DPSFHelper.RandomNumberBetween(1, 4);

            cParticle.Rotation = currentRotation;

            cParticle.StartColor = Color.Yellow;
            cParticle.EndColor = Color.Red;

            cParticle.Friction = DPSFHelper.RandomNumberBetween(50, 200);
        }

        public override void changeEmitterPosition(Vector2 newPosition)
        {
            Emitter.PositionData.Position = new Vector3(newPosition.X + offsetX, newPosition.Y + offsetY, 0);
        }
    }
}