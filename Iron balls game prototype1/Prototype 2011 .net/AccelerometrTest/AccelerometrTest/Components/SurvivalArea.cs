using System;
using IronBalls.Extensions;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Components
{
    class SurvivalArea : AbstractGameComponent
    {
        private Texture2D _area;
        //private double _scale = 1f;
        private Vector2 _centerOffset;

        public SpriteBatch SpriteBatch { get; set; }


        public SurvivalArea(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();
            Position = new Vector2(DisplayService.Instance.DisplayHeight / 2f,
                DisplayService.Instance.DisplayHeight / 2f);
            Radius = 200;
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch.Draw(_area, Position, null, Color.Yellow,
                0f, _centerOffset, 1f, SpriteEffects.None, 0.7f);
        }

        public override void LoadContent()
        {
            InvalidateTexture();
            //ContentLoaderService.Instance.LoadTexture("Textures/Area");
        }

        public override void UnloadContent()
        {
            //ContentLoaderService.Instance.UnloadTexture("Textures/Area");
        }


        public Vector2 Position { get; set; }

        private int _radius;
        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                InvalidateTexture();
            }
        }

        private void InvalidateTexture()
        {
            _area = DrawPrimitivesHelper.CreateCircle(Radius, 3);
            _centerOffset = new Vector2(_area.Width / 2f, _area.Height / 2f);
        }
    }
}
