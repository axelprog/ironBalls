using IronBalls.Screens;
using Microsoft.Xna.Framework;

namespace IronBalls.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class AbstractGameComponent : GameComponent
    {
        protected AbstractGameComponent(Game game) : base(game)
        {
           
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        

        public abstract void Draw(GameTime gameTime);

        public abstract void LoadContent();

        public abstract void UnloadContent();

        };
}
