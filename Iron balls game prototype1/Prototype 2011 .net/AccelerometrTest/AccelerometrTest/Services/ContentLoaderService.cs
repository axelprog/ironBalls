using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Services
{
    class ContentLoaderService
    {
        public class AdvancedContentManager : ContentManager
        {
            public AdvancedContentManager(IServiceProvider serviceProvider)
                : base(serviceProvider)
            { }

            public override T Load<T>(string assetName)
            {
                return ReadAsset<T>(assetName, null);
            }
        }

        private readonly ContentManager _contentManager;

        private readonly Dictionary<string, SoundEffectInstance> _soundBank;
        private readonly Dictionary<string, Texture2D> _textureBank;
        private readonly Dictionary<string, SpriteFont> _fontBank;

        private readonly Dictionary<string, int> _contentUsing;



#region Singleton

        private static ContentLoaderService _instance;

        public static ContentLoaderService Instance
        {
            get
            {
                return _instance;
            }
        }
        public static void Initialize(ContentManager content)
        {
            _instance = new ContentLoaderService(content);
        }
        private ContentLoaderService(ContentManager content)
        {
            _contentManager = content;
            _soundBank = new Dictionary<string, SoundEffectInstance>();
            _textureBank = new Dictionary<string, Texture2D>();
            _fontBank = new Dictionary<string, SpriteFont>();
            _contentUsing = new Dictionary<string, int>();
        }

#endregion

        
#region Loading Methods

        public SoundEffectInstance LoadSound( string soundPath)
        {
            var soundKey = soundPath;
            
            if (_soundBank.ContainsKey(soundKey))
            {
                _contentUsing[soundKey]++;
                return _soundBank[soundKey];
            }

            SoundEffectInstance item = _contentManager.Load<SoundEffect>(soundPath).CreateInstance();
            _soundBank.Add(soundKey, item);
            _contentUsing.Add(soundKey, 1);
            return item;
        }

        public void UnloadSound(string soundPath) 
        {
            
            if (!_soundBank.ContainsKey(soundPath) ) 
                return;

            _contentUsing[soundPath]--;

            if(_contentUsing[soundPath] > 0)
                return;

            SoundEffectInstance item = _soundBank[soundPath];
            _soundBank.Remove(soundPath);
            _contentUsing.Remove(soundPath);
            item.Dispose();
        }

        public SoundEffectInstance GetSound(string soundPath)
        {
            return _soundBank[soundPath];
        }
        
        public Texture2D LoadTexture( string texturePath)
        {
            var textureKey = texturePath;

            if (_textureBank.ContainsKey(textureKey))
            {
                _contentUsing[textureKey]++;
                return _textureBank[textureKey];
            }

            var item = _contentManager.Load<Texture2D>(texturePath);

            _textureBank.Add(textureKey, item);
            _contentUsing.Add(textureKey, 1);
            
            return item;
        }

        public void UnloadTexture(string texturePath)
        {

            if (!_textureBank.ContainsKey(texturePath)) 
                return;

            _contentUsing[texturePath]--;
            if (_contentUsing[texturePath] > 0)
                return;

            Texture2D item = _textureBank[texturePath];
            _textureBank.Remove(texturePath);
            _contentUsing.Remove(texturePath);
            item.Dispose();
        }

        public SpriteFont LoadFont(string fontPath)
        {

            var fontKey = fontPath;

            if (_fontBank.ContainsKey(fontKey))
            {
                _contentUsing[fontKey]++;
                return _fontBank[fontKey];
            }

            var item = _contentManager.Load<SpriteFont>(fontPath);
            _fontBank.Add(fontKey, item);
            _contentUsing.Add(fontKey, 1);
            return item;
        }

        public void UnloadFont(string fontPath)
        {

            if (!_fontBank.ContainsKey(fontPath))
                return;

            _contentUsing[fontPath]--;
            if (_contentUsing[fontPath] > 0)
                return;

            _fontBank.Remove(fontPath);
            _contentUsing.Remove(fontPath);
        }
        #endregion
    }
}
