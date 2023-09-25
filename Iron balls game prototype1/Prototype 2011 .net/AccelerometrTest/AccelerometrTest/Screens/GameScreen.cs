using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IronBalls.Components;
using IronBalls.GameMode;
using IronBalls.Services;
using IronBalls.UserData;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Screens
{
    class GameScreen: AbstractScreen
    {
        private double _saveSeconds;
        Accelerometer _accelSensor;

        private readonly Random _random = new Random();
        private OrbComponent _orb;
        private SurvivalArea _area;
        private BackgroundComponent _background;
        private SpriteFont _font;

        private IGameMode _currentMode;

        private readonly SurvivalMode _survivalMode;
        private readonly TronMode _tronMode;

        private List<BarrierParticleComponent> _barriers = new List<BarrierParticleComponent>();
        private double _lastDeleteTime;

        public GameScreen(Game game, ScreenManager manager) : base(game, manager)
        {
            ScreenType = ScreenType.MainGame;
            _background = new BackgroundComponent(game);
            Components.Add(_background);

            //_area = new SurvivalArea(game);
            //Components.Add(_area);

            _survivalMode = new SurvivalMode(game);
            _survivalMode.OnGameOver += _survivalMode_OnGameOver;
            //Components.AddRange(_survivalMode.GetModeGeneratedComponents());

            _tronMode = new TronMode(game);
            _tronMode.OnGameOver += _survivalMode_OnGameOver;
            //Components.AddRange(_tronMode.GetModeGeneratedComponents());

            _orb = new OrbComponent(game);
            //_orb.OnColission += _survivalMode.OnColissionOrb;
            Components.Add(_orb);

            _currentMode = _survivalMode;
        }

        void _survivalMode_OnGameOver(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(ScreenType.GameMenu);
        }

       

        public override void Initialize()
        {
            PhysicService.Instance.SetGravity(Vector2.Zero);

            _survivalMode.InitializeMode(_background.MinValue, _background.MaxValue);
            _tronMode.InitializeMode(_background.MinValue, _background.MaxValue);

            _accelSensor = new Accelerometer();
            _accelSensor.CurrentValueChanged += accelSensor_CurrentValueChanged;
            try
            {
                _accelSensor.Start();

            }
            catch
            {
                Debug.WriteLine("This exception is thrown in the emulator-which doesn't support an accelerometer.");
            }

            var initComponents = new List<AbstractGameComponent>();
            initComponents.AddRange(_survivalMode.GetModeGeneratedComponents());
            initComponents.AddRange(_tronMode.GetModeGeneratedComponents());

            foreach (var abstractGameComponent in initComponents)
            {
                abstractGameComponent.Initialize();
            }

            base.Initialize();
        }

        void accelSensor_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            PhysicService.Instance.SetGravity(new Vector2(e.SensorReading.Acceleration.X * 100f,
                -e.SensorReading.Acceleration.Y * 100f));
        }

        public override void LoadContent()
        {
            _font = ContentLoaderService.Instance.LoadFont("points");

            var initComponents = new List<AbstractGameComponent>();
            initComponents.AddRange(_survivalMode.GetModeGeneratedComponents());
            initComponents.AddRange(_tronMode.GetModeGeneratedComponents());

            foreach (var abstractGameComponent in initComponents)
            {
                abstractGameComponent.LoadContent();
            }

            base.LoadContent();

            
        }

        public override void UnloadContent()
        {
            ContentLoaderService.Instance.UnloadFont("points");

            var initComponents = new List<AbstractGameComponent>();
            initComponents.AddRange(_survivalMode.GetModeGeneratedComponents());
            initComponents.AddRange(_tronMode.GetModeGeneratedComponents());

            foreach (var abstractGameComponent in initComponents)
            {
                abstractGameComponent.UnloadContent();
            }

            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var spriteBatch = DisplayService.Instance.GetCanvas();
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Points: " + UserProfile.Instance.CurrentPoints, new Vector2(20, 16), Color.White);
            spriteBatch.DrawString(_font, "Time: " + UserProfile.Instance.RemainingTime, new Vector2(200, 730), Color.White);
            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            PhysicService.Instance.World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            ExecuteGameLogic(gameTime);
            base.Update(gameTime);
        }

        public void ChangeMode(GameModes gameMode)
        {
            if(_currentMode != null)
            {
                var removeComponents = _currentMode.GetModeGeneratedComponents();

                _orb.OnColission -= _currentMode.OnColissionOrb;

                foreach (var abstractGameComponent in removeComponents)
                {
                    Components.Remove(abstractGameComponent);
                }
                foreach (var abstractGameComponent in _barriers)
                {
                    abstractGameComponent.Destroy();
                    Components.Remove(abstractGameComponent);
                }
            }
                
            switch (gameMode)
            {
                case GameModes.TronMode:
                    _currentMode = _tronMode;
                    break;
                case GameModes.SurvivalMode:
                    _currentMode = _survivalMode;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("gameMode");
            }

            if (_currentMode != null)
            {
                
                _orb.OnColission += _currentMode.OnColissionOrb;
                _orb.Position = new Vector2(DisplayService.Instance.DisplayWidth / 2f,
                    DisplayService.Instance.DisplayHeight / 2f);
                _currentMode.InitializeMode(_background.MinValue, _background.MaxValue);
                Components.AddRange(_currentMode.GetModeGeneratedComponents());
            }
        }

        private void ExecuteGameLogic(GameTime gameTime)
        {

            if (_currentMode is TronMode && gameTime.TotalGameTime.TotalSeconds >5
                && _barriers !=null && _barriers.Count > 0
                && gameTime.TotalGameTime.TotalMilliseconds - _lastDeleteTime > 200)
            {
                var barrierParticleComponent = _barriers[0];
                _lastDeleteTime = gameTime.TotalGameTime.TotalMilliseconds;
                barrierParticleComponent.Destroy();
                _barriers.Remove(barrierParticleComponent);
                Components.Remove(barrierParticleComponent); 
            }
            
            _currentMode.ExecuteGameLogic(_orb);
            var barriers = _currentMode.AddBarrier(gameTime);
            foreach (var barrierParticleComponent in barriers)
            {
                if (_barriers != null) _barriers.Add(barrierParticleComponent);
                Components.Add(barrierParticleComponent);
            }
        }

        
    }
}
