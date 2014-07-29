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
    public class Puff : BasicParticleSystem
    {
        Vector3 direction;
        float power;
        int maxParticles = 0;

        public Puff(TankGame cGame, Vector2 newPosition, Vector2 newDirection, float power)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
            direction = new Vector3(newDirection.X, newDirection.Y, 0);
            this.power = power;
            maxParticles = (int)power;
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, maxParticles, maxParticles, "Textures/SmokeCloud", cSpriteBatch);

            Name = "Puff";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = .2f;
            InitialProperties.LifetimeMax = .6f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = direction * new Vector3(power * 20, -power * 20, 0);
            InitialProperties.VelocityMax = direction * new Vector3(power * 100, -power * 100, 0);
            InitialProperties.RotationMin = (float)-Math.PI;
            InitialProperties.RotationMax = (float)Math.PI;
            InitialProperties.RotationalVelocityMin = 0;
            InitialProperties.RotationalVelocityMax = 0;
            InitialProperties.StartWidthMin = 5;
            InitialProperties.StartWidthMax = 10;
            InitialProperties.StartHeightMin = 5;
            InitialProperties.StartHeightMax = 10;
            InitialProperties.EndWidthMin = 30;
            InitialProperties.EndWidthMax = 40;
            InitialProperties.EndHeightMin = 30;
            InitialProperties.EndHeightMax = 40;
            InitialProperties.StartColorMin = Color.Gray;
            InitialProperties.StartColorMax = Color.Gray;
            InitialProperties.EndColorMin = Color.White;
            InitialProperties.EndColorMax = Color.White;
            InitialProperties.FrictionMin = 100;
            InitialProperties.FrictionMax = 200;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeInUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocityUsingFriction, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = .6f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(0.1f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 100;
            Emitter.PositionData.Position = position;
        }
    }
}