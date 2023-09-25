using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace IronBalls.Services
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PhysicService
    {
        private World _world;
        private static PhysicService _instance;

#region Singletone

        public static PhysicService Instance
        {
            get { return _instance ?? (_instance = new PhysicService()); }
        }

        #endregion

        public PhysicService()
        {
            _world = new World(Vector2.UnitX * 10);    
        }

        public World World
        {
            get { return _world; }
        }

        public void SetGravity(Vector2 gravityVector)
        {
           
            _world.Gravity = gravityVector;
        }
    }
}
