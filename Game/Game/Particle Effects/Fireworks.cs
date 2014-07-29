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
    public class Fireworks : BasicParticleSystem
    {
        float currentRotation = 0;
        Color startColor;
        Color endColor;

        public Fireworks(TankGame cGame, Vector2 position)
            : base(cGame)
        {
            this.position = new Vector3(position.X, position.Y, 0);

            switch ((int)DPSFHelper.RandomNumberBetween(0, 3))
            {
                case 0:
                    startColor = Color.Yellow;
                    endColor = Color.DarkRed;
                    break;

                case 1:
                    startColor = Color.LightCyan;
                    endColor = Color.Green;
                    break;

                case 2:
                    startColor = Color.LightBlue;
                    endColor = Color.DarkBlue;
                    break;

            }
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, (int)DPSFHelper.RandomNumberBetween(10, 400), (int)DPSFHelper.RandomNumberBetween(10, 400), "Textures/Pixel", cSpriteBatch);

            Name = "Fireworks";

            LoadParticleSystem();
        }

        public override void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleProperties;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);

            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = DPSFHelper.RandomNumberBetween(0.5f, 2.0f);
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(.2f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = DPSFHelper.RandomNumberBetween(100, 2000);
            Emitter.PositionData.Position = position;
        }

        public override void InitializeParticleProperties(DefaultSpriteParticle cParticle)
        {
            cParticle.Lifetime = DPSFHelper.RandomNumberBetween(.5f, 1);

            cParticle.Position = Emitter.PositionData.Position;

            float randomVelocity = DPSFHelper.RandomNumberBetween(-200, 200);
            currentRotation += (float)Math.PI / 20;
            Vector2 rotatedRandomVector = TankGame.rotateVector(new Vector2(randomVelocity, randomVelocity), currentRotation);

            cParticle.Velocity = new Vector3(rotatedRandomVector.X, rotatedRandomVector.Y, 0);

            cParticle.Width = DPSFHelper.RandomNumberBetween(2, 6);
            cParticle.Height = DPSFHelper.RandomNumberBetween(2, 12);

            cParticle.Rotation = currentRotation;

            //Vector3 sVelocityMin = new Vector3(-100, 25, -100);
            //Vector3 sVelocityMax = new Vector3(100, 50, 100);
            //cParticle.Velocity = DPSFHelper.RandomVectorBetweenTwoVectors(sVelocityMin, sVelocityMax);
            
            //cParticle.Velocity = Vector3.Transform(cParticle.Velocity, Emitter.OrientationData.Orientation);

            cParticle.StartColor = startColor;
            cParticle.EndColor = endColor;
        }
    }
}