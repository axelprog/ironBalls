// Type: Microsoft.Xna.Framework.GameComponent
// Assembly: Microsoft.Xna.Framework.Game, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: c:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone\Microsoft.Xna.Framework.Game.dll

using System;

namespace Microsoft.Xna.Framework
{
    public class GameComponent : IGameComponent, IUpdateable, IDisposable
    {
        public GameComponent(Game game);
        public Game Game { get; }

        #region IDisposable Members

        public void Dispose();

        #endregion

        #region IGameComponent Members

        public virtual void Initialize();

        #endregion

        #region IUpdateable Members

        public virtual void Update(GameTime gameTime);
        public bool Enabled { get; set; }
        public int UpdateOrder { get; set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion

        ~GameComponent();
        protected virtual void Dispose(bool disposing);
        protected virtual void OnUpdateOrderChanged(object sender, EventArgs args);
        protected virtual void OnEnabledChanged(object sender, EventArgs args);
        public event EventHandler<EventArgs> Disposed;
    }
}
