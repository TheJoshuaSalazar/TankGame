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
    public class Flare : BasicParticleSystem
    {
        public Flare(TankGame cGame, Vector2 newPosition)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 200, 200, "Textures/MissileLaunch/Flare", cSpriteBatch);

            Name = "Flare";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .1f;
            InitialProperties.LifetimeMax = .4f;
            InitialProperties.PositionMin = new Vector3(0, 25, 0);
            InitialProperties.PositionMax = new Vector3(0, 25, 0);
            InitialProperties.VelocityMin = new Vector3(-400, -600, 0);
            InitialProperties.VelocityMax = new Vector3(400, -100, 0);
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 80;
            InitialProperties.StartWidthMax = 80;
            InitialProperties.StartHeightMin = 80;
            InitialProperties.StartHeightMax = 80;
            InitialProperties.EndWidthMin = 0;
            InitialProperties.EndWidthMax = 0;
            InitialProperties.EndHeightMin = 0;
            InitialProperties.EndHeightMax = 0;
            InitialProperties.StartColorMin = Color.White;
            InitialProperties.StartColorMax = Color.White;
            InitialProperties.EndColorMin = Color.Red;
            InitialProperties.EndColorMax = Color.Red;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndQuickFadeOut, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = 1.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.6f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 1000;
            Emitter.PositionData.Position = position;
        }
    }
}