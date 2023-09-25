using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IronBalls.Extensions;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Components
{
    public class BarrierParticleComponent : AbstractGameComponent
    {

        public Vector2 Position
        {
            get
            {
                return  ConvertUnits.ToDisplayUnits(_physicBody.Position);
            }
            set
            {
                _physicBody.Position = new Vector2(ConvertUnits.ToSimUnits(value.X), ConvertUnits.ToSimUnits(value.Y));
            }
        }

        public SpriteBatch SpriteBatch { get; set; }

        private readonly Body _physicBody;

        public Vector2 Size { get; private set; }

        private static readonly Random Random = new Random();
        private static int _typeNum;

        private Texture2D _edge;
        private Texture2D _rect;

        private readonly Vector2 _offset;

        public BarrierParticleComponent(Game game) : base(game)
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();

            Size = new Vector2(Random.Next(5, 80), Random.Next(5, 80));
            
            _physicBody = BodyFactory.CreateRectangle(PhysicService.Instance.World,
              ConvertUnits.ToSimUnits(Size.X), ConvertUnits.ToSimUnits(Size.Y), Random.Next(10),
              Vector2.Zero, "barrier");

            _typeNum++;
            //TODO replace with random generator
            switch (_typeNum % 2)
            {
                case 0:
                    _physicBody.BodyType = BodyType.Static;
                    break;
                case 1:
                    _physicBody.BodyType = BodyType.Dynamic;
                    break;
                default:
                    _physicBody.BodyType = BodyType.Static;
                    break;
            }
            
            //circleShape = new CircleShape(ConvertUnits.ToSimUnits(75), 5f);
            _physicBody.Restitution = 0.1f;
            _physicBody.Friction = 0.2f;
           // _physicBody.OnCollision += _physicBody_OnCollision;
            _physicBody.SleepingAllowed = false;


            Position = Vector2.One * 50;
            _offset = new Vector2(Size.X/2f, Size.Y/2f);
            InvalidateTextures();
        }

        public void Destroy()
        {
            _physicBody.Dispose();
        }

        public BarrierParticleComponent(Game game, Vector2 size, BodyType type)
            : base(game)
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();

            Size = size;

            _physicBody = BodyFactory.CreateRectangle(PhysicService.Instance.World,
              ConvertUnits.ToSimUnits(Size.X), ConvertUnits.ToSimUnits(Size.Y), Random.Next(10),
              Vector2.Zero, "barrier");

           _physicBody.BodyType = type;
           
            _physicBody.Restitution = 0.1f;
            _physicBody.Friction = 0.2f;
            _physicBody.SleepingAllowed = false;

            Position = Vector2.One * 50;
            _offset = new Vector2(Size.X / 2f, Size.Y / 2f);
            InvalidateTextures();
        }

        public void InvalidateTextures()
        {
            _rect = DrawPrimitivesHelper.CreateRectangleFill((int) Size.X, (int) Size.Y);
            _edge = DrawPrimitivesHelper.CreateRectangle((int) Size.X, (int) Size.Y, 2);
        }


        public override void Draw(GameTime gameTime)
        {
            
            SpriteBatch.Draw(_rect,
                Position, null, Color.DarkRed, _physicBody.Rotation, _offset, 
                Vector2.One, SpriteEffects.None, 0.5f);
            SpriteBatch.Draw(_edge,
                 Position, null, Color.Black, _physicBody.Rotation, _offset,
                 Vector2.One, SpriteEffects.None, 0.49f);
            
        }

        public override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {
            Destroy();
        }
    }
}
