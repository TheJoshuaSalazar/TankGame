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
    public class DetectionWheel : BasicParticleSystem
    {
        string detectionWheel;
        float rotationSpeed;

        public DetectionWheel(TankGame cGame, Vector2 newPosition, string detectionWheel, float rotationSpeed)
            : base(cGame)
        {
            position = new Vector3(newPosition.X, newPosition.Y, 0);
            this.detectionWheel = detectionWheel;
            this.rotationSpeed = rotationSpeed;
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1, 1, "Textures/MissileLaunch/" + detectionWheel, cSpriteBatch);

            Name = "Detection Wheel";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = 1.0f;
            InitialProperties.LifetimeMax = 10.0f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = Vector3.Zero;
            InitialProperties.VelocityMax = Vector3.Zero;
            InitialProperties.RotationMin = 0.0f;
            InitialProperties.RotationMax = 0.0f;
            InitialProperties.RotationalVelocityMin = rotationSpeed;
            InitialProperties.RotationalVelocityMax = rotationSpeed;
            InitialProperties.StartWidthMin = 250;
            InitialProperties.StartWidthMax = 250;
            InitialProperties.StartHeightMin = 250;
            InitialProperties.StartHeightMax = 250;
            InitialProperties.EndWidthMin = 250;
            InitialProperties.EndWidthMax = 250;
            InitialProperties.EndHeightMin = 250;
            InitialProperties.EndHeightMax = 250;
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
            ParticleSystemEvents.AddTimedEvent(1.0f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 100;
            Emitter.PositionData.Position = position;
        }
    }
}