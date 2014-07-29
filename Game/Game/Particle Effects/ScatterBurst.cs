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
    public class ScatterBurst : BasicParticleSystem
    {
        public ScatterBurst(TankGame cGame, Vector2 newPosition)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 100, 100, "Textures/Scatter", cSpriteBatch);

            Name = "Scatter Burst";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .1f;
            InitialProperties.LifetimeMax = .4f;
            InitialProperties.PositionMin = new Vector3(-30, -30, 0);
            InitialProperties.PositionMax = new Vector3(30, 30, 0);
            InitialProperties.VelocityMin = new Vector3(0, 0, 0);
            InitialProperties.VelocityMax = new Vector3(0, 0, 0);
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = MathHelper.Pi;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 60;
            InitialProperties.StartWidthMax = 60;
            InitialProperties.StartHeightMin = 60;
            InitialProperties.StartHeightMax = 60;
            InitialProperties.EndWidthMin = 10;
            InitialProperties.EndWidthMax = 10;
            InitialProperties.EndHeightMin = 10;
            InitialProperties.EndHeightMax = 10;
            InitialProperties.StartColorMin = Color.White;
            InitialProperties.StartColorMax = Color.White;
            InitialProperties.EndColorMin = Color.White;
            InitialProperties.EndColorMax = Color.White;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = 1.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(0.1f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 1000;
            Emitter.PositionData.Position = position;
        }
    }
}