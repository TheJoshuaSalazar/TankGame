#region File Description
//===================================================================
// DefaultSpriteParticleSystemTemplate.cs
// 
// This file provides the template for creating a new Sprite Particle
// System that inherits from the Default Sprite Particle System.
//
// The spots that should be modified are marked with TODO statements.
//
// Copyright Daniel Schroeder 2008
//===================================================================
#endregion

#region Using Statements
using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Game
{
#if (WINDOWS)
    [Serializable]
#endif
    public class BasicBlast : BasicParticleSystem
    {
        public BasicBlast(TankGame cGame, Vector2 position) : base(cGame) 
        {
            this.position = new Vector3(position.X, position.Y, 0);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000, "Textures/Fire", cSpriteBatch);

            Name = "Basic Blast";

            LoadParticleSystem();
        }
    }
}
