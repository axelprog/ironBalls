using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace IronBalls.UI_components
{
    public enum ButtonState
    {
        Normal,
        Pressed
    }

    public class TexturedButton:AbstractUIComponent
    {
        public ButtonState State { get; set; }

        private Texture2D _buttonTexture;
        private Texture2D _buttonTexturePressed;

        private String _normalTextureName;
        private string _pressedTextureName;

        public TexturedButton(Game game, String normalTextureName, string pressedTextureName) : base(game)
        {
            _normalTextureName = normalTextureName;
            _pressedTextureName = pressedTextureName;
            Visible = true;
        }

        public event EventHandler OnClick;

        protected void InvokeClick()
        {
            var handler = OnClick;
            if (handler != null)
                    handler(this, null);
        }

        private bool PositionInButton(Vector2 position)
        {
            return Position.X <= position.X &&
                   position.X <= Position.X + _buttonTexture.Width
                   && Position.Y <= position.Y &&
                   position.Y <= Position.Y + _buttonTexture.Height;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            var spriteBatch = DisplayService.Instance.GetCanvas();

           
            
            switch (State)
            {
                case ButtonState.Normal:
                    spriteBatch.Draw(_buttonTexture, Position,
                                        Color.White);
                    break;

                case ButtonState.Pressed:
                    spriteBatch.Draw(_buttonTexturePressed, Position,
                                        Color.White);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

           
        }

        public override void LoadContent()
        {
            var loader = ContentLoaderService.Instance;
            _buttonTexture = loader.LoadTexture(_normalTextureName);
            _buttonTexturePressed = loader.LoadTexture(_pressedTextureName);
        }

        public override void UnloadContent()
        {
            var loader = ContentLoaderService.Instance;
            loader.UnloadTexture(_normalTextureName);
            loader.UnloadTexture(_pressedTextureName);
        }

        public override void Update(GameTime gameTime)
        {

            if (!Visible)
                return;

            var touchCollection = InputState.Instance.TouchState;

            if (touchCollection.Count == 0)
            {
                if (State == ButtonState.Pressed)
                {
                    State = ButtonState.Normal;
                    InvokeClick();
                }
            }
            else
                foreach (var touch in touchCollection)
                {
                    //TouchLocation prevLocation;
                    //var prevLocationGood = touch.TryGetPreviousLocation(out prevLocation);

                    if ((touch.State == TouchLocationState.Pressed) && PositionInButton(touch.Position))
                    {
                        State = ButtonState.Pressed;
                    }
                    if (State == ButtonState.Pressed && 
                        (touch.State == TouchLocationState.Released || !PositionInButton(touch.Position)))
                    {
                        State = ButtonState.Normal;
                        InvokeClick();
                    }
                } 
            base.Update(gameTime);
        }
    }
}
