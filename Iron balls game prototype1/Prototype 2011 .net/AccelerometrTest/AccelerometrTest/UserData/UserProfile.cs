namespace IronBalls.UserData
{
    class UserProfile
    {

#region singletone

        private static UserProfile _instance;

        public static UserProfile Instance
        {
            get { return _instance ?? (_instance = new UserProfile()); }
        }

        private UserProfile ()
        {
        }

#endregion

        public void Clear()
        {
            CurrentPoints = 0;
            RemainingTime = 10;
        }

        public int CurrentPoints { get; set; }

        public long RemainingTime { get; set; }
    }
}
