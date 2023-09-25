// Type: Microsoft.Xna.Framework.GraphicsDeviceManager
// Assembly: Microsoft.Xna.Framework.Game, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: c:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone\Microsoft.Xna.Framework.Game.dll

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    public class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {
        public static readonly int DefaultBackBufferHeight;
        public static readonly int DefaultBackBufferWidth;
        public GraphicsDeviceManager(Game game);
        public GraphicsProfile GraphicsProfile { get; set; }
        public bool IsFullScreen { get; set; }
        public bool PreferMultiSampling { get; set; }
        public SurfaceFormat PreferredBackBufferFormat { get; set; }
        public int PreferredBackBufferHeight { get; set; }
        public int PreferredBackBufferWidth { get; set; }
        public DepthFormat PreferredDepthStencilFormat { get; set; }
        public DisplayOrientation SupportedOrientations { get; set; }
        public bool SynchronizeWithVerticalRetrace { get; set; }

        #region IDisposable Members

        void IDisposable.Dispose();

        #endregion

        #region IGraphicsDeviceManager Members

        void IGraphicsDeviceManager.CreateDevice();
        bool IGraphicsDeviceManager.BeginDraw();
        void IGraphicsDeviceManager.EndDraw();

        #endregion

        #region IGraphicsDeviceService Members

        public GraphicsDevice GraphicsDevice { get; }
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        #endregion

        public void ApplyChanges();
        public void ToggleFullScreen();
        protected virtual GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice);
        protected virtual bool CanResetDevice(GraphicsDeviceInformation newDeviceInfo);
        protected virtual void RankDevices(List<GraphicsDeviceInformation> foundDevices);
        protected virtual void OnDeviceCreated(object sender, EventArgs args);
        protected virtual void OnDeviceDisposing(object sender, EventArgs args);
        protected virtual void OnDeviceReset(object sender, EventArgs args);
        protected virtual void OnDeviceResetting(object sender, EventArgs args);
        protected virtual void Dispose(bool disposing);
        protected virtual void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args);
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;
    }
}
