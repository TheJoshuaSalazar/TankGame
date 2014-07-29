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
    public class TargetLocked : BasicParticleSystem
    {
        public TargetLocked(TankGame cGame, Vector2 newPosition)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1, 1, "Textures/MissileLaunch/TargetLocked", cSpriteBatch);

            Name = "Target Locked";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = 2.0f;
            InitialProperties.LifetimeMax = 2.0f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = Vector3.Zero;
            InitialProperties.VelocityMax = Vector3.Zero;
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = 0;
            InitialProperties.RotationalVelocityMax = 0;
            InitialProperties.StartWidthMin = 60;
            InitialProperties.StartWidthMax = 60;
            InitialProperties.StartHeightMin = 60;
            InitialProperties.StartHeightMax = 60;
            InitialProperties.EndWidthMin = 60;
            InitialProperties.EndWidthMax = 60;
            InitialProperties.EndHeightMin = 60;
            InitialProperties.EndHeightMax = 60;
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
            ParticleSystemEvents.LifetimeData.Lifetime = 2.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(1.0f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 10;
            Emitter.PositionData.Position = position;
        }
    }
}