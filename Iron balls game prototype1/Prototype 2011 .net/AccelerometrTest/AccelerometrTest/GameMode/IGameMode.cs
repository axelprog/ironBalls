using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Components;
using Microsoft.Xna.Framework;

namespace IronBalls.GameMode
{
    public enum GameModes
    {
        TronMode,  SurvivalMode
    }

    public interface IGameMode
    {

        void ExecuteGameLogic(OrbComponent orb);

        void OnColissionOrb(object sender, EventArgs e);

        IList<AbstractGameComponent> GetModeGeneratedComponents();

        void InitializeMode(Vector2 minFieldPoint, Vector2 maxFieldPoint);

        IList<BarrierParticleComponent> AddBarrier(GameTime time);

        event EventHandler OnGameOver;
    }
}
