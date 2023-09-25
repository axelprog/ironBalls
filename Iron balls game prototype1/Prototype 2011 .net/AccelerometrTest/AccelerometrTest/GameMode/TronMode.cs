using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using IronBalls.Components;
using IronBalls.Screens;
using IronBalls.UserData;
using Microsoft.Xna.Framework;

namespace IronBalls.GameMode
{
    public class TronMode : IGameMode
    {
        private readonly Random _random = new Random();
        private Vector2 _minFieldPoint, _maxFieldPoint;
        private readonly Game _game;
        private readonly SurvivalArea _area;

        private Vector2 _lastOrbPosition;
        private Vector2 _barrierPos;
        private Vector2 _lastBarrierPos;
        private float _widthBarrier;

        private bool _drawBarrier;

        private double _saveSeconds;


        public TronMode(Game game)
        {
            _game = game;
            _area = new SurvivalArea(game);
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

            if (_lastOrbPosition == Vector2.Zero)
                _lastOrbPosition = orb.Position;

            if (_barrierPos == Vector2.Zero)
                _barrierPos = orb.Position;


            if ((orb.Position - _lastOrbPosition).Length() > orb.Radius + orb.Radius / 3f)
            {
                _barrierPos = _lastOrbPosition;
                _lastOrbPosition = orb.Position;
                _drawBarrier = true;
                _widthBarrier = orb.Radius * 2 / 3f;
            }

            

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
            var components = new List<AbstractGameComponent> { _area };
            return components;
        }

        public void InitializeMode(Vector2 minFieldPoint, Vector2 maxFieldPoint)
        {
            _area.Radius = 200;
            _minFieldPoint = minFieldPoint;
            _maxFieldPoint = maxFieldPoint;
            UserProfile.Instance.RemainingTime = 10;
            UserProfile.Instance.CurrentPoints = 0;
            _lastOrbPosition = Vector2.Zero;
            _barrierPos = Vector2.Zero;
            _lastBarrierPos = Vector2.Zero;

        }

        public IList<BarrierParticleComponent> AddBarrier(GameTime time)
        {
            if (time.TotalGameTime.TotalSeconds - _saveSeconds >= 1)
            {
                _saveSeconds = time.TotalGameTime.TotalSeconds;
                UserProfile.Instance.RemainingTime--;
            }

            var barriers = new List<BarrierParticleComponent>();
            

            if(_drawBarrier)
            {
                _drawBarrier = false;
                if (_lastBarrierPos == Vector2.Zero)
                {
                    
                    barriers.Add(new BarrierParticleComponent(_game, Vector2.One*_widthBarrier, BodyType.Static)
                                     {
                                         Position = _barrierPos
                                     });
                    _lastBarrierPos = _barrierPos;
                }
                else
                {
                    var vector = _barrierPos -_lastBarrierPos;
                    while(vector.Length() > _widthBarrier)
                    {
                       
                        vector.Normalize();
                        barriers.Add(new BarrierParticleComponent(_game, Vector2.One*_widthBarrier, BodyType.Static)
                                         {
                                             Position = _lastBarrierPos + vector*_widthBarrier
                                         });
                        _lastBarrierPos = _lastBarrierPos + vector*_widthBarrier * 1.3f;
                        vector = _barrierPos -_lastBarrierPos;
                    }
                }
            }
            return barriers;
        }

        public event EventHandler OnGameOver;

        private void InvokeOnGameOver(EventArgs e)
        {
            EventHandler handler = OnGameOver;
            if (handler != null) handler(this, e);
        }
    }
}