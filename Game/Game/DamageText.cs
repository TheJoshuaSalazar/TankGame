using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game
{
    public class DamageText
    {
        public string damage;

        public Vector2 position;

        public float textTime = 3.0f;

        public DamageText(string damage, Vector2 position)
        {
            this.damage = damage;
            this.position = position;
        }
    }
}
