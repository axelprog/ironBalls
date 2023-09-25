using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Services
{
    class DisplayService
    {
        //private GraphicsDevice graphicsDevice;
        private readonly SpriteBatch _spriteBatch;

#region Singleton

        private static DisplayService _instance;

        public static DisplayService Instance
        {
            get { return _instance; }
        }
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _instance = new DisplayService(graphicsDevice);
        }
        private DisplayService(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        #endregion

        public GraphicsDevice GraphicsDevice { get; private set; }

        public SpriteBatch GetCanvas() {
            return _spriteBatch;
        }

        /// <summary>
        /// Immediately draws whatever is in the backbuffer onto the screen
        /// (normally this is done only after draw() method is completed)
        /// </summary>
        public void PushBackBufferToScreen() {
            _spriteBatch.GraphicsDevice.Present();
        }

        public int DisplayWidth
        {
            get { return GraphicsDevice.DisplayMode.Width; }
        }

        public int DisplayHeight
        {
            get { return GraphicsDevice.DisplayMode.Height; }
        }

    }
}
