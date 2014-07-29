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
    public class Confetti : BasicParticleSystem
    {
        Random random = new Random();

        public Confetti(TankGame cGame, Vector2 position)
            : base(cGame)
        {
            this.position = new Vector3(position.X, position.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, random.Next(10, 400), random.Next(10, 400), "Textures/Pixel", cSpriteBatch);

            Name = "Confetti";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .4f;
            InitialProperties.LifetimeMax = 1f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = new Vector3(-random.Next(20, 400), random.Next(20, 400), 0);
            InitialProperties.VelocityMax = new Vector3(random.Next(20, 400), -random.Next(20, 400), 0);
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax = MathHelper.Pi;
            InitialProperties.StartWidthMin = 1;
            InitialProperties.StartWidthMax = 16;
            InitialProperties.StartHeightMin = 1;
            InitialProperties.StartHeightMax = 16;
            InitialProperties.EndWidthMin = 0;
            InitialProperties.EndWidthMax = 4;
            InitialProperties.EndHeightMin = 0;
            InitialProperties.EndHeightMax = 4;
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
            ParticleSystemEvents.LifetimeData.Lifetime = (float)random.NextDouble() + .5f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.2f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = random.Next(100, 2000);
            Emitter.PositionData.Position = position;
        }
    }
}