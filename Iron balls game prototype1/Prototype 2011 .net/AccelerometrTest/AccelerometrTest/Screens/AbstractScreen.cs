using System.Collections.Generic;
using IronBalls.Components;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace IronBalls.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AbstractScreen : GameComponent
    {
        protected readonly List<AbstractGameComponent> Components = new List<AbstractGameComponent>();

        public AbstractScreen(Game game, ScreenManager manager)
            : base(game)
        {
            ScreenManager = manager;
        }

        private SpriteBatch SpriteBatch { get; set; }

        public ScreenType ScreenType { get; protected set; }

        protected ScreenManager ScreenManager;

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            SpriteBatch = DisplayService.Instance.GetCanvas();

            foreach (var gameComponent in Components)
            {
                gameComponent.Initialize();
            }

            base.Initialize();
        }

        public virtual void LoadContent()
        {
            foreach (var gameComponent in Components)
            {
                gameComponent.LoadContent();
            }
        }

        public virtual void UnloadContent()
        {
            foreach (var gameComponent in Components)
            {
                gameComponent.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var gameComponent in Components)
            {
                gameComponent.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (var gameComponent in Components)
            {
                gameComponent.Draw(gameTime);
            }
            SpriteBatch.End();
        }
    }
}
