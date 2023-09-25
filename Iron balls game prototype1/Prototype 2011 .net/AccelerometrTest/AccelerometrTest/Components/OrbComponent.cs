using System;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IronBalls.Services;
using IronBalls.UserData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Components
{
    public class OrbComponent: AbstractGameComponent
    {
        private Texture2D _orb;
        private readonly Body _physicBody;
        private CircleShape _circleShape;
        private const float Scale = 0.5f;
        private Vector2 _centerOffset;

        public event EventHandler OnColission;

        private SpriteBatch SpriteBatch { get; set; }
        

        public OrbComponent(Game game) : base(game)
        {
            _physicBody = BodyFactory.CreateCircle(PhysicService.Instance.World, 
                ConvertUnits.ToSimUnits(35.75), 1f, Vector2.Zero, "orb");
            _physicBody.BodyType = BodyType.Dynamic;
            //circleShape = new CircleShape(ConvertUnits.ToSimUnits(75), 5f);
            _physicBody.Restitution = 0.3f;
            _physicBody.Friction = 0.3f;
            _physicBody.OnCollision += _physicBody_OnCollision;
            _physicBody.SleepingAllowed = false;
           
            
            // Fixture fixture = _physicBody.CreateFixture(circleShape);
        }

        public void Destroy()
        {
            _physicBody.Dispose();
        }

        bool _physicBody_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {

            if (OnColission != null)
                OnColission(this, null);
            return true;
        }

        public override void Initialize()
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();
            Position = new Vector2(DisplayService.Instance.DisplayWidth / 2f, DisplayService.Instance.DisplayHeight / 2f);
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            

            
            SpriteBatch.Draw(_orb,
                ConvertUnits.ToDisplayUnits(_physicBody.Position),
                null, Color.White, 0f, _centerOffset, Scale, SpriteEffects.None, 0.1f);
           
            
        }

        public override void LoadContent()
        {
            _orb = ContentLoaderService.Instance.LoadTexture("Textures/Orb");
            
            _centerOffset = new Vector2(_orb.Width / 2f, _orb.Height / 2f);

        }

        public override void UnloadContent()
        {
            Destroy();
            ContentLoaderService.Instance.UnloadTexture("Textures/Orb");
        }

        public Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(_physicBody.Position); }
            set 
            { 
                _physicBody.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        public int Radius
        {
            get { return (int)(_orb.Width / 2f * Scale); }
        }
    }
}
