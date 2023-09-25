using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Components;
using IronBalls.Screens;
using IronBalls.UserData;
using Microsoft.Xna.Framework;

namespace IronBalls.GameMode
{
    public class SurvivalMode: IGameMode
    {
        private readonly Random _random = new Random();

        private Vector2 _minFieldPoint, _maxFieldPoint;
        private readonly SurvivalArea _area;
        private double _saveSeconds;

        private readonly Game _game;

        public SurvivalMode(Game game)
        {
            _game = game;
            _area = new SurvivalArea(game);
            
        }

        public event EventHandler OnGameOver;

        private void InvokeOnGameOver(EventArgs e)
        {
            EventHandler handler = OnGameOver;
            if (handler != null) handler(this, e);
        }

        public void ExecuteGameLogic(OrbComponent orb)
        {
            if (UserProfile.Instance.RemainingTime <= 0)
            {
                UserProfile.Instance.RemainingTime = 10;
                _area.Radius -= _area.Radius / 3;

                SetSurvivalAreaPos();

                if (_area.Radius <= orb.Radius)
                {
                   InvokeOnGameOver(null); 
                }
            }

            if ((orb.Position - _area.Position).Length() > _area.Radius)
            {
                UserProfile.Instance.CurrentPoints++;
            }
        }

        private void SetBarrierPos(BarrierParticleComponent barrier)
        {

            var posX = _random.Next((int)_minFieldPoint.X, (int)_maxFieldPoint.X);
            var posY = _random.Next((int)_minFieldPoint.Y, (int)_maxFieldPoint.Y);

            if (posX - barrier.Size.X / 2f <= _minFieldPoint.X)
                posX += (int)(barrier.Size.X / 2f + 2);

            if (posX + barrier.Size.X / 2f >= _maxFieldPoint.X)
                posX -= (int)(barrier.Size.X / 2f - 2);

            if (posY - barrier.Size.Y / 2f <= _minFieldPoint.Y)
                posY += (int)(barrier.Size.Y / 2f + 2);

            if (posY + barrier.Size.Y / 2f >= _maxFieldPoint.Y)
                posY -= (int)(barrier.Size.Y / 2f - 2);

            barrier.Position = new Vector2(posX, posY);
        }

        private void SetSurvivalAreaPos()
        {
            var posX = _random.Next((int)_minFieldPoint.X, (int)_maxFieldPoint.X);
            var posY = _random.Next((int)_minFieldPoint.Y, (int)_maxFieldPoint.Y);

            if (posX - _area.Radius <= _minFieldPoint.X)
                posX += _area.Radius + 2;

            if (posX + _area.Radius >= _maxFieldPoint.X)
                posX -= _area.Radius - 2;

            if (posY - _area.Radius <= _minFieldPoint.Y)
                posY += _area.Radius + 2;

            if (posY + _area.Radius >= _maxFieldPoint.Y)
                posY -= _area.Radius - 2;

            _area.Position = new Vector2(posX, posY);
        }

        public void OnColissionOrb(object sender, EventArgs e)
        {
            UserProfile.Instance.CurrentPoints++;
        }

        public IList<AbstractGameComponent> GetModeGeneratedComponents()
        {
            var components = new List<AbstractGameComponent> {_area};
            return components;
        }

        public void InitializeMode(Vector2 minFieldPoint, Vector2 maxFieldPoint)
        {
            _area.Radius = 200;
            _minFieldPoint = minFieldPoint;
            _maxFieldPoint = maxFieldPoint;
            UserProfile.Instance.RemainingTime = 10;
            UserProfile.Instance.CurrentPoints = 0;
           
        }

        

        public IList<BarrierParticleComponent> AddBarrier(GameTime time)
        {

            var returnList = new List<BarrierParticleComponent>();
            if (time.TotalGameTime.TotalSeconds - _saveSeconds >= 1)
            {
                _saveSeconds = time.TotalGameTime.TotalSeconds;
                UserProfile.Instance.RemainingTime--;
                if (UserProfile.Instance.RemainingTime % 2 == 0)
                {
                    var barrier = new BarrierParticleComponent(_game);
                    returnList.Add(barrier);
                    SetBarrierPos(barrier);
                }
            }

            return returnList;
        }

        
    }
}
