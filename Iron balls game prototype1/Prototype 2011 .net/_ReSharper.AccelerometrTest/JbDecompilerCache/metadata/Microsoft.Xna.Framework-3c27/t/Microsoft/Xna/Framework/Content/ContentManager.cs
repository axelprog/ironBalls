// Type: Microsoft.Xna.Framework.Content.ContentManager
// Assembly: Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: c:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone\Microsoft.Xna.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
    public class ContentManager : IDisposable
    {
        public ContentManager(IServiceProvider serviceProvider);
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory);
        public string RootDirectory { get; set; }
        public IServiceProvider ServiceProvider { get; }

        #region IDisposable Members

        public void Dispose();

        #endregion

        protected virtual void Dispose(bool disposing);
        public virtual void Unload();
        public virtual T Load<T>(string assetName);
        protected T ReadAsset<T>(string assetName, Action<IDisposable> recordDisposableObject);
        protected virtual Stream OpenStream(string assetName);
    }
}
