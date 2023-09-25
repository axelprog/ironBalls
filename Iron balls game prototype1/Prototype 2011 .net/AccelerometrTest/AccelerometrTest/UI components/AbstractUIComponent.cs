using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Components;
using Microsoft.Xna.Framework;

namespace IronBalls.UI_components
{
    public abstract class AbstractUIComponent : AbstractGameComponent
    {
        protected AbstractUIComponent(Game game) : base(game)
        {
        }

        public Vector2 Position { get; set; }

        public bool Visible { get; set; }

       

    }
}
