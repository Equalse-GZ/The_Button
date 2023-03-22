namespace Game.Data
{
    [System.Serializable]
    public class GlobalData : IData
    {
        public UserData User;
        public TeamData[] TeamLeaders;
        public UserData[] PersonLeaders;
        public UserData[] TeamMembers;
        public BonusData[] Bonuses;
        public int UserPlace;
        public int TeamPlace;
    }
}