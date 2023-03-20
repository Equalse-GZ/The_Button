namespace Game.Data
{
    [System.Serializable]
    public class UserData : IData
    {
        public int ID;
        public string Login;
        public string Password;
        public string Icon;
        public PlayerData PlayerData;
        public ErrorData ErrorData;
        public int Place;
    }
}
