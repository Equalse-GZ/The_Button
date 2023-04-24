namespace Game.Data
{
    [System.Serializable]
    public class GlobalDataRaw : IData
    {
        public UserData User;
        public string TeamLeaders;
        public string PersonLeaders;
        public string TeamMembers;
        public string Bonuses;
        public int UserPlace;
        public int TeamPlace;
        public GameUntilTime GameUntilTime;
    }
}