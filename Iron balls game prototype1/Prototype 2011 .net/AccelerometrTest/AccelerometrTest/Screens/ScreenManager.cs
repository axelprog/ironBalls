using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IronBalls.Screens
{
    public enum ScreenType
    {
        MainGame,
        GameMenu
    }

    public class ScreenManager
    {
        private readonly IDictionary<ScreenType, AbstractScreen> _screens = new Dictionary<ScreenType, AbstractScreen>();

        private readonly Game _game;
       
        public ScreenManager(Game game)
        {
            _game = game;
            _screens.Add(ScreenType.MainGame, new GameScreen(game, this));
            _screens.Add(ScreenType.GameMenu, new MainMenuScreen(game, this));
            //TODO Create all game screens

            ChangeScreen(ScreenType.GameMenu);
        }

        public AbstractScreen CurrentScreen { get; set; }

        public void GameExit()
        {
            _game.Exit();
        }

        public void ChangeScreen(ScreenType typeScreen)
        {
            CurrentScreen = _screens[typeScreen];
        }

        public  void Initialize()
        {
            foreach (var gameScreen in _screens.Values)
            {
                gameScreen.Initialize();
            }
        }

        public virtual void LoadContent()
        {
            foreach (var gameScreen in _screens.Values)
            {
                gameScreen.LoadContent();
            }
        }

        public virtual void UnloadContent()
        {
            foreach (var gameScreen in _screens.Values)
            {
                gameScreen.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
            // Allows the game to exit
            if (InputState.Instance.IsNewButtonPress(Buttons.Back))
            {
                switch (CurrentScreen.ScreenType)
                {
                    case ScreenType.MainGame:
                        ChangeScreen(ScreenType.GameMenu);
                        break;
                    case ScreenType.GameMenu:
                        GameExit();
                        break;
                    default:
                        ChangeScreen(ScreenType.GameMenu);
                        break;
                }
            }
           
        }

        public void Draw(GameTime gameTime)
        {
            CurrentScreen.Draw(gameTime);
        }
    }
    
}
