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
    public class SmokeCloud : BasicParticleSystem
    {
        int maxParticles = 0;
        int particlesPerSecond = 0;
        Color cloudColor;

        public SmokeCloud(TankGame cGame, Vector2 newPosition, int maxParticles, int particlesPerSecond, Color cloudColor)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
            this.maxParticles = maxParticles;
            this.particlesPerSecond = particlesPerSecond;
            this.cloudColor = cloudColor;
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, maxParticles, maxParticles, "Textures/SmokeCloud", cSpriteBatch);

            Name = "Smoke Cloud";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .4f;
            InitialProperties.LifetimeMax = 1.0f;
            InitialProperties.PositionMin = new Vector3(-20, 0, 0);
            InitialProperties.PositionMax = new Vector3(20, 20, 0);
            InitialProperties.VelocityMin = new Vector3(0, -20, 0);
            InitialProperties.VelocityMax = new Vector3(0, -60, 0);
            InitialProperties.RotationMin = (float)-Math.PI / 8;
            InitialProperties.RotationMax = (float)Math.PI / 8;
            InitialProperties.RotationalVelocityMin = 0;
            InitialProperties.RotationalVelocityMax = 0;
            InitialProperties.StartWidthMin = 0;
            InitialProperties.StartWidthMax = 10;
            InitialProperties.StartHeightMin = 0;
            InitialProperties.StartHeightMax = 10;
            InitialProperties.EndWidthMin = 30;
            InitialProperties.EndWidthMax = 40;
            InitialProperties.EndHeightMin = 20;
            InitialProperties.EndHeightMax = 40;
            InitialProperties.StartColorMin = cloudColor;
            InitialProperties.StartColorMax = cloudColor;
            InitialProperties.EndColorMin = cloudColor;
            InitialProperties.EndColorMax = cloudColor;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeInUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Repeat;
            ParticleSystemEvents.LifetimeData.Lifetime = 1.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(0.6f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = particlesPerSecond;
            Emitter.PositionData.Position = position;
        }
    }
}