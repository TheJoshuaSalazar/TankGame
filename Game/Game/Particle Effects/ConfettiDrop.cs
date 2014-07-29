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
    public class ConfettiDrop : BasicParticleSystem
    {
        public ConfettiDrop(TankGame cGame, Vector2 position)
            : base(cGame)
        {
            this.position = new Vector3(position.X, position.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, (int)DPSFHelper.RandomNumberBetween(10, 400), (int)DPSFHelper.RandomNumberBetween(10, 400), "Textures/Pixel", cSpriteBatch);

            Name = "Confetti";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = 1;
            InitialProperties.LifetimeMax = 20;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = new Vector3(-20, 20, 0);
            InitialProperties.VelocityMax = new Vector3(20, 100, 0);
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 5;
            InitialProperties.StartWidthMax = 20;
            InitialProperties.StartHeightMin = 5;
            InitialProperties.StartHeightMax = 20;
            InitialProperties.EndWidthMin = 0;
            InitialProperties.EndWidthMax = 10;
            InitialProperties.EndHeightMin = 0;
            InitialProperties.EndHeightMax = 10;
            InitialProperties.StartColorMin = Color.Black;
            InitialProperties.StartColorMax = Color.White;
            InitialProperties.EndColorMin = Color.Black;
            InitialProperties.EndColorMax = Color.White;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = 20;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.6f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = DPSFHelper.RandomNumberBetween(100, 2000);
            Emitter.PositionData.Position = position;
        }
    }
}