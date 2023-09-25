using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.GameMode;
using IronBalls.UI_components;
using Microsoft.Xna.Framework;

namespace IronBalls.Screens
{
    public class MainMenuScreen : AbstractScreen
    {
        private readonly TexturedButton _survivalButton;
        private readonly TexturedButton _tronButton;

        public MainMenuScreen(Game game, ScreenManager manager) : base(game, manager)
        {
            this.ScreenType = ScreenType.GameMenu;
            _survivalButton = new TexturedButton(game, "Textures/Buttons/survival_mode",
                                                 "Textures/Buttons/survival_mode_pressed")
                                  {
                                      Position = new Vector2(20, 10)
                                  };
            _survivalButton.OnClick += _survivalButton_OnClick;
            Components.Add(_survivalButton);

            _tronButton = new TexturedButton(game, "Textures/Buttons/tron_mode",
                                                 "Textures/Buttons/tron_mode_pressed")
            {
                Position = new Vector2(195, 10)
            };
            _tronButton.OnClick += _tronButton_OnClick;
            Components.Add(_tronButton);

        }

        private void _tronButton_OnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(ScreenType.MainGame);
            var gameScreen = ScreenManager.CurrentScreen as GameScreen;
            if (gameScreen != null)
                gameScreen.ChangeMode(GameModes.TronMode);
        }

        void _survivalButton_OnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(ScreenType.MainGame);
            var gameScreen = ScreenManager.CurrentScreen as GameScreen;
            if (gameScreen != null)
                gameScreen.ChangeMode(GameModes.SurvivalMode);
        }
    }
}
