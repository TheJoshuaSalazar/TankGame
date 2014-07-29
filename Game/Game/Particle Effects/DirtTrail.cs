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
    public class DirtTrail : BasicParticleSystem
    {
        Vector3 direction;

        public DirtTrail(TankGame cGame, Vector2 newPosition, Vector2 newDirection)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
            direction = new Vector3(newDirection.X, newDirection.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 20, 20, "Textures/Dirt", cSpriteBatch);

            Name = "Dirt Trail";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .1f;
            InitialProperties.LifetimeMax = .3f;
            InitialProperties.PositionMin = new Vector3(0, 0, 0);
            InitialProperties.PositionMax = new Vector3(0, 0, 0);
            InitialProperties.VelocityMin = direction * new Vector3(-200, -100, 0);
            InitialProperties.VelocityMax = direction *new Vector3(-50, -10, 0);
            InitialProperties.RotationMin = -MathHelper.Pi;
            InitialProperties.RotationMax = MathHelper.Pi;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 5;
            InitialProperties.StartWidthMax = 5;
            InitialProperties.StartHeightMin = 5;
            InitialProperties.StartHeightMax = 5;
            InitialProperties.EndWidthMin = 5;
            InitialProperties.EndWidthMax = 5;
            InitialProperties.EndHeightMin = 5;
            InitialProperties.EndHeightMax = 5;
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
            ParticleSystemEvents.LifetimeData.Lifetime = .4f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(0.1f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 2000;
            Emitter.PositionData.Position = position;
        }

        public override void ParticleUpdate(GameTime time)
        {
            base.ParticleUpdate(time);
        }

        public void StartDirtTrail(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {

        }

        public void StopDirtTrail(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {

        }
    }
}