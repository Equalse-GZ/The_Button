namespace Game.Data 
{
    [System.Serializable]
    public class TeamData : IData
    {
        public string Name;
        public int MembersCount;
        public string InviteCode;
        public int Tickets;
        public ErrorData ErrorData;
        public string Role;
        public int Place;
    }
}
