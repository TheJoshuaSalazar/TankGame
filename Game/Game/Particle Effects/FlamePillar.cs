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
    public class FlamePillar : BasicParticleSystem
    {
        public FlamePillar(TankGame cGame, Vector2 newPosition)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 100, 100, "Textures/Flame", cSpriteBatch);

            Name = "Flame Pillar";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .1f;
            InitialProperties.LifetimeMax = .3f;
            InitialProperties.PositionMin = new Vector3(-20, -80, 0);
            InitialProperties.PositionMax = new Vector3(20, 30, 0);
            InitialProperties.VelocityMin = new Vector3(0, -600, 0);
            InitialProperties.VelocityMax = new Vector3(0, -100, 0);
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 60;
            InitialProperties.StartWidthMax = 60;
            InitialProperties.StartHeightMin = 60;
            InitialProperties.StartHeightMax = 60;
            InitialProperties.EndWidthMin = 0;
            InitialProperties.EndWidthMax = 0;
            InitialProperties.EndHeightMin = 0;
            InitialProperties.EndHeightMax = 0;
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
            ParticleSystemEvents.AddTimedEvent(0.6f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 1000;
            Emitter.PositionData.Position = position;
        }
    }
}