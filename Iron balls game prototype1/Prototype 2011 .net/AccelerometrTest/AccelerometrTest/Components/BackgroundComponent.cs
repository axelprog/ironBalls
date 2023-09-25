using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BackgroundComponent : AbstractGameComponent
    {
        
        private Texture2D _background;
        private readonly Body _anchor;
        readonly IList<Body> _edges = new List<Body>();

        public Vector2 MinValue = new Vector2(16, 16);
        public Vector2 MaxValue = new Vector2(466, 784);


        public SpriteBatch SpriteBatch { get; set; }

        public BackgroundComponent(Game game)
            : base(game)
        {
            var borders = new Vertices(4)
                              {
                                  ConvertUnits.ToSimUnits(MinValue), 
                                  ConvertUnits.ToSimUnits(new Vector2(MinValue.X, MaxValue.Y)), 
                                  ConvertUnits.ToSimUnits(MaxValue), 
                                  ConvertUnits.ToSimUnits(new Vector2(MaxValue.X, MinValue.Y))
                              };

          
            _edges.Add(BodyFactory.CreateEdge(PhysicService.Instance.World, borders[0], borders[1], "border"));
            _edges.Add(BodyFactory.CreateEdge(PhysicService.Instance.World, borders[1], borders[2], "border"));
            _edges.Add(BodyFactory.CreateEdge(PhysicService.Instance.World, borders[2], borders[3], "border"));
            _edges.Add(BodyFactory.CreateEdge(PhysicService.Instance.World, borders[3], borders[0], "border"));
            //_anchor = BodyFactory.CreateLoopShape(PhysicService.Instance.World, borders, "border");
            foreach (var edge in _edges)
            {


                edge.CollisionCategories = Category.All;
                edge.CollidesWith = Category.All;
                edge.IsStatic = true;
                edge.BodyType = BodyType.Static;
                edge.Restitution = 0.3f;
                edge.Friction = 0.5f;
            }
        }

        public void Destroy()
        {
            foreach (var body in _edges)
            {
                body.Dispose();
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();

            base.Initialize();
        }

        public override void Draw (GameTime gameTime)
        {
            
            SpriteBatch.Draw(_background, Vector2.Zero, null, Color.White,
                0f, Vector2.Zero, 1f,SpriteEffects.None, 1);
            
        }

        public override void LoadContent()
        {
            _background = ContentLoaderService.Instance.LoadTexture("Textures/board");
        }

        public override void UnloadContent()
        {
            Destroy();
            ContentLoaderService.Instance.UnloadTexture("Textures/board");
        }
    }
}
